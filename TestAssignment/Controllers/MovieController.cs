using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestAssignment.Common.Contexts;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Exceptions;
using TestAssignment.Common.Extensions;
using TestAssignment.Common.Interfaces;
using TestAssignment.Common.ViewModels;
using TestAssignment.Common.ViewModels.InputViewModels;
using TestAssignment.Common.ViewModels.OutputViewModels;

namespace TestAssignment.Controllers;

/// <summary>
/// Controller responsible for managing movie-related actions.
/// eg: getting movie recommentations / getting search suggestions / log of view/click/search actions / fetching search datas
/// </summary>
[Route("api/movies")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMovieService _movieService;
    private readonly IElasticSearchService _elasticSearchService;
    
    /// <summary> Initializes a new instance of the <see cref="MovieController"/> class. </summary>
    /// <param name="authService">The authentication service for user identification.</param>
    /// <param name="movieService">The service for movie-related operations.</param>
    /// <param name="elasticSearchService">The service for Elasticsearch queries.</param>
    public MovieController(IAuthService authService, IMovieService movieService, IElasticSearchService elasticSearchService)
    {
        _authService = authService;
        _movieService = movieService;
        _elasticSearchService = elasticSearchService;
    }
    
    /// <summary>
    /// Called when to home page is initialized the first time and retrieves a list of recommended movies for the current user based on view and click history.
    /// This use LTR to bring the best movies to watch based on all the users clicks and all the movies viewed.
    /// The movies viewed by current user doesn't show in the recommendations
    /// This insure that suggestions/recommendations of movies to the current user are new
    /// These suggestions doesn't include search, the User can search for viewed movies at any moment 
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the list of recommended movies if successful.
    /// Returns 401 Unauthorized if authentication fails.
    /// Returns 500 Internal Server Error if an Elasticsearch query or data error occurs.
    /// Returns 400 Bad Request for other exceptions.
    /// </returns>
    [HttpGet("recommendations")]
    public async Task<IActionResult> FetchInitialMoviesRecomandations()
    {
        try
        {
            // getting the user id
            // return Authorization exception if token is incorrect or user not defined
            string userId = _authService.GetUserId(Request.Headers);

            // retrieve list of movies viewed by current user.
            // this list is the list of movies to be ignored and not to be shown in the recommendations.
            List<int> movieIdsViewedByCurrentUser = await _movieService.GetViewedMovieIdsByUserIdAsync(userId);

            // Get the 20 most viewed and clicked movies (with the count of their views) in 2 separate lists
            // These movies shouldn't have been seen by the current user 
            // using DTO objects to handle full data transition, including count of views and clicks of movies
            var moviesViewedWithCount = await _movieService.GetBestViewedMoviesWithCountOfViewsAndNotViewedByCurrentUser(movieIdsViewedByCurrentUser);
            var moviesClickedWithCount = await _movieService.GetBestClickedMoviesWithCountOfClicksAndNotViewedByCurrentUser(movieIdsViewedByCurrentUser);

            // After collecting the list of clicks/views of movies, combine the list and calculate a total score to rate those movies
            // The rating is calculated by this formula <<< SCORE = TOTAL_VIEWS_OF_MOVIE * VIEWFACTOR + TOTAL_CLICK_OF_MOVIE * CLICKFACTOR >>>
            // The factors are 5 per view and 1 per click
            var combinedMoviesWithScore = moviesViewedWithCount
                .Concat(moviesClickedWithCount)
                .CalculateTotalScore(5, 1);
            // Get the top 20 viewed movies by total score
            var movieIdsToSearch = combinedMoviesWithScore.GetTopMovieIds(20);
            
            // Get the movies from Elasticsearch based the IDs
            var moviesViewModel = movieIdsToSearch.IsNullOrEmpty() ? new List<MovieViewModel>() : (await _elasticSearchService.SearchMoviesByIds(movieIdsToSearch));
            
            return Ok(moviesViewModel);
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ElasticSearchException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
   
    /// <summary>
    /// Searches for movies based on the provided search criteria and user information.
    /// THis method use LTR and show old search terms / recomendations of movies if no keyword provided / movies sorted by SCORE 
    /// </summary>
    /// <param name="model">A <see cref="SearchViewModel"/> containing search parameters.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the result of the search, including last searched keywords,
    /// suggested movies, and recommended keywords.
    /// Returns 401 Unauthorized if authentication fails.
    /// Returns 500 Internal Server Error if an Elasticsearch query or data error occurs.
    /// Returns 400 Bad Request for other exceptions.
    /// </returns>
    [HttpPost("search")]
    public async Task<IActionResult> SearchMovies([FromBody] SearchViewModel model)
    {
        try
        {
            // init the result view model
            SearchMovieResult result = new SearchMovieResult();

            // getting the user id
            // return Authorization exception if token is incorrect or user not defined
            string userId = _authService.GetUserId(Request.Headers);

            // Retrieve most recent 5 searches of the current user and that have the same keyword passed to request
            List<MovieSearch> lastUserSearches = await _movieService.GetLastUserSearchesByKeyWordAsync(model.Query, userId, model.MaxOldSearchOptions);
            // Select only the keyword datas 
            result.LastSearchedKeywords = lastUserSearches.Select(ms => ms.Keyword).ToList();
            
            // Logic to retrieve the most clicked movies based on the keyword
            var mostClickedMovies = await _movieService.GetMostClickedMoviesAsync(model.Query, model.MaxSuggestedSearch - lastUserSearches.Count);
            // Logic to retrieve the most viewed movies based on the keyword
            var mostViewedMovies = await _movieService.GetMostViewedMoviesAsync(model.Query, model.MaxSuggestedSearch - lastUserSearches.Count);
            
            // After collecting the list of clicks/views of movies, combine the list and calculate a total score to rate those movies
            // The rating is calculated by this formula <<< SCORE = TOTAL_VIEWS_OF_MOVIE * VIEWFACTOR + TOTAL_CLICK_OF_MOVIE * CLICKFACTOR >>>
            // The factors are 5 per view and 1 per click
            var combinedMovies = mostClickedMovies.Concat(mostViewedMovies);
            var moviesWithTotalScore = combinedMovies.CalculateTotalScore(5, 1);
            
            // if keyword is not null or empty, then get movies from elasticsearch and order them by score of clicks/views 
            if (!string.IsNullOrEmpty(model.Query))
            {

                // Getting the movies from elastic search 
                List<MovieViewModel> moviesByKeyword = await _elasticSearchService.SearchMoviesByKeyword(model.Query);
                
                // Getting the best movies rated from clicks/views
                var bestMovies = moviesWithTotalScore.GetTopMovies(model.MaxSuggestedSearch - lastUserSearches.Count);
                
                // Get all the Elastic search movies scored by click/view
                // sort the list to give the best movies
                var moviesSorted = moviesByKeyword.SortExistingMoviesByScore(bestMovies);
                // The list is not sorted, take only few elements for it to show in movie suggestions
                var topMovies = moviesSorted.Take(model.MaxSuggestedSearch - lastUserSearches.Count).ToList();
                result.SearchedMovies = topMovies;
                
            }
            else
            {
                // if keyword is null or empty, then give suggestion of best movies
                var bestMovieNames = moviesWithTotalScore.GetTopMovieNames(10 - lastUserSearches.Count);
                result.RecommendedKeywords = bestMovieNames;
            }
            
            return Ok(result);
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ElasticSearchException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Logs user actions in database (search, view, click).
    /// </summary>
    /// <param name="model">A <see cref="LogViewModel"/> containing information about the user's action.</param>
    /// <returns>
    /// An HTTP response indicating the success of the action logging.
    /// Returns 401 Unauthorized if authentication fails.
    /// Returns 500 Internal Server Error if an Data query error occurs.
    /// Returns 400 Bad Request for other exceptions.
    /// </returns>
    [HttpPost("search/log")]
    public async Task<IActionResult> LogAction([FromBody] LogViewModel model)
    {
        try
        {
            // getting the user id
            // return Authorization exception if token is incorrect or user not defined
            string userId = _authService.GetUserId(Request.Headers);
            
            // log the action (search/view/click) based on action type
            await _movieService.LogMovieAction(model.Keyword, userId, model.MovieId, model.MovieName, model.Action);
            
            return Ok();
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Retrieves a list of movies based on the provided search keyword.
    /// </summary>
    /// <param name="model">A <see cref="SearchViewModel"/> containing the search query.</param>
    /// <returns>
    /// An HTTP response containing a list of <see cref="MovieViewModel"/> objects that match the search query.
    /// Returns 401 Unauthorized if authentication fails.
    /// Returns 500 Internal Server Error if an Elasticsearch query error occurs.
    /// Returns 400 Bad Request for other exceptions.
    /// </returns>
    [HttpPost()]
    public async Task<IActionResult> FetchMoviesByKeyword([FromBody] SearchViewModel model)
    {
        try
        {
            // getting the user id
            // return Authorization exception if token is incorrect or user not defined
            string userId = _authService.GetUserId(Request.Headers);
            
            // Get Movie list by keyword
            List<MovieViewModel> movies = await _elasticSearchService.SearchMoviesByKeyword(model.Query);
            
            return Ok(movies);
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ElasticSearchException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
    /// <summary>
    /// Get a movie by movieId.
    /// </summary>
    /// <param name="id">A <see cref="int"/> containing the id of the movie.</param>
    /// <returns>
    /// An HTTP response containing a list of <see cref="MovieViewModel"/> objects that match the search query.
    /// Returns 401 Unauthorized if authentication fails.
    /// Returns 500 Internal Server Error if an Elasticsearch query error occurs.
    /// Returns 400 Bad Request for other exceptions.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(int id)
    {
        try
        {
            // getting the user id
            // return Authorization exception if token is incorrect or user not defined
            string userId = _authService.GetUserId(Request.Headers);
            
            // Get Movie list by keyword
            List<MovieViewModel> movies = await _elasticSearchService.SearchMoviesByIds(new List<int>() {id});
            
            return Ok(movies);
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ElasticSearchException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}