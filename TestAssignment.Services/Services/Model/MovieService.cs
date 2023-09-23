using Microsoft.EntityFrameworkCore;
using TestAssignment.Common.Contexts;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Enumerations;
using TestAssignment.Common.Exceptions;
using TestAssignment.Common.Interfaces;
using TestAssignment.Common.ViewModels;

namespace TestAssignment.Services.Services.Model;


/// <summary>
/// Service responsible for managing movie-related data and interactions.
/// </summary>
public class MovieService : IMovieService
{
    private readonly InteractionDbContext _interactionDbContext;

    public MovieService(InteractionDbContext interactionDbContext)
    {
        _interactionDbContext = interactionDbContext;
    }

    /// <summary>
    /// Retrieves a list of movie IDs viewed by a user with the specified ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of movie IDs viewed by the user.</returns>
    public async Task<List<int>> GetViewedMovieIdsByUserIdAsync(string userId)
    {
        try
        {
            return await _interactionDbContext.MovieViews
                .Where(mv => mv.UserId == userId)
                .Select(mv => mv.MovieId)
                .Distinct()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InternalServerException("Error when loading datas", ex);
        }
    }
    
    /// <summary>
    /// Retrieves a list of movies with the count of views, excluding those already viewed by the current user.
    /// </summary>
    /// <param name="movieIdsViewedByCurrentUser">A list of movie IDs already viewed by the current user.</param>
    /// <returns>A list of MovieCountViewModel containing movie ID, count of views, and action type.</returns>
    public async Task<List<MovieCountViewModel>> GetBestViewedMoviesWithCountOfViewsAndNotViewedByCurrentUser(List<int> movieIdsViewedByCurrentUser)
    {
        try
        {
            var movieViews = await _interactionDbContext.MovieViews
                .Where(m => !movieIdsViewedByCurrentUser.Contains(m.MovieId))
                .ToListAsync();

            return movieViews.GroupBy(mv => mv.MovieId)
                .Select(group => new MovieCountViewModel(group.Key, group.Count(), MovieActionType.ActionView))
                .Take(20)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new InternalServerException("Error when loading datas", ex);
        }
    }
    
    /// <summary>
    /// Retrieves a list of movies with the count of clicks, excluding those already viewed by the current user.
    /// </summary>
    /// <param name="movieIdsViewedByCurrentUser">A list of movie IDs already viewed by the current user.</param>
    /// <returns>A list of MovieCountViewModel containing movie ID, count of clicks, and action type.</returns>
    public async Task<List<MovieCountViewModel>> GetBestClickedMoviesWithCountOfClicksAndNotViewedByCurrentUser(List<int> movieIdsViewedByCurrentUser)
    {
        try
        {
            var movieClicks = await _interactionDbContext.MovieClicks
                .Where(mv => !movieIdsViewedByCurrentUser.Contains(mv.MovieId))
                .ToListAsync();
        
            return movieClicks.GroupBy(mc => mc.MovieId)
                .Select(group => new  MovieCountViewModel(group.Key, group.Count(), MovieActionType.ActionClick))
                .OrderByDescending(item => item.Count)
                .Take(20)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new InternalServerException("Error when loading datas", ex);
        }
    }
    
    /// <summary>
    /// Retrieves a list of most clicked movies based on the provided keyword and count.
    /// </summary>
    /// <param name="keyword">The keyword to filter movie names by.</param>
    /// <param name="count">The maximum number of movies to retrieve.</param>
    /// <returns>A list of MovieCountViewModel containing movie ID, movie name, count of clicks, and action type.</returns>
    public async Task<List<MovieCountViewModel>> GetMostClickedMoviesAsync(string keyword, int count)
    {
        try
        {
            IQueryable<MovieClick> query = _interactionDbContext.MovieClicks;

            if (!string.IsNullOrEmpty(keyword))
            {
                // Filter by keyword (if provided)
                keyword = keyword.Trim();
                query = query.Where(search => search.MovieName != keyword);
            }

            // Group and aggregate data
            var groupedData = await query
                .GroupBy(mc => new { mc.MovieId, mc.MovieName })
                .ToListAsync();

            // Project data to view models
            var movieCountViewModels = groupedData
                .Select(group => new MovieCountViewModel(
                    group.Key.MovieId,
                    group.Key.MovieName,
                    group.Count(),
                    MovieActionType.ActionClick))
                .OrderByDescending(movie => movie.Count)
                .Take(count)
                .ToList();

            return movieCountViewModels;
        }
        catch (Exception ex)
        {
            throw new InternalServerException("Error when loading datas", ex);
        }
    }

    /// <summary>
    /// Retrieves a list of most viewed movies based on the provided keyword and count.
    /// </summary>
    /// <param name="keyword">The keyword to filter movie names by.</param>
    /// <param name="count">The maximum number of movies to retrieve.</param>
    /// <returns>A list of MovieCountViewModel containing movie ID, movie name, count of views, and action type.</returns>
    public async Task<List<MovieCountViewModel>> GetMostViewedMoviesAsync(string keyword, int count)
    {
        try
        {
            IQueryable<MovieView> query = _interactionDbContext.MovieViews;

            if (!string.IsNullOrEmpty(keyword))
            {
                // Filter by keyword (if provided)
                keyword = keyword.Trim();
                query = query.Where(search => search.MovieName != keyword);
            }

            // Group and aggregate data
            var groupedData = await query
                .GroupBy(mc => new { mc.MovieId, mc.MovieName })
                .ToListAsync();

            // Project data to view models
            var movieCountViewModels = groupedData
                .Select(group => new MovieCountViewModel(
                    group.Key.MovieId,
                    group.Key.MovieName,
                    group.Count(),
                    MovieActionType.ActionView))
                .OrderByDescending(movie => movie.Count)
                .Take(count)
                .ToList();

            return movieCountViewModels;
        }
        catch (Exception ex)
        {
            throw new InternalServerException("Error when loading datas", ex);
        }
    }

    /// <summary>
    /// Retrieves the last user searches by keyword and user ID, limiting the result to a specified count.
    /// </summary>
    /// <param name="keyword">The keyword to filter searches by.</param>
    /// <param name="userId">The ID of the user whose searches are retrieved.</param>
    /// <param name="count">The maximum number of searches to retrieve.</param>
    /// <returns>A list of MovieSearch containing the last user searches matching the keyword and user ID.</returns>
    public async Task<List<MovieSearch>> GetLastUserSearchesByKeyWordAsync(string keyword, string userId, int count)
    {
        try
        {
            // gettint the datas related to current user 
            IQueryable<MovieSearch> query = _interactionDbContext.MovieSearches
                .Where(search => search.UserId == userId);

            // if keyword is not null, search by keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                // Filter by keyword (if provided)
                keyword = keyword.Trim();
                query = query.Where(search => search.Keyword.StartsWith(keyword));
            }
        
            // return the last ones
            return await query
                .OrderByDescending(search => search.Timestamp)
                .Take(count) // Limit the result to a reasonable number
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InternalServerException("Error when loading datas", ex);
        }
    }
    
    /// <summary>
    /// Logs a movie-related action (search, click, or view) by a user.
    /// </summary>
    /// <param name="keyword">The keyword used in the action (applicable for searches).</param>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <param name="movieId">The ID of the movie associated with the action.</param>
    /// <param name="movieName">The name of the movie associated with the action.</param>
    /// <param name="actionType">The type of movie-related action (search, click, or view).</param>
    public async Task LogMovieAction(string keyword, string userId, int movieId, string movieName, MovieActionType actionType)
    {
        try
        {
            // depending on the action type creating the actions logs
            switch (actionType)
            {
                case MovieActionType.ActionSearch:
                    _interactionDbContext.MovieSearches.Add(new MovieSearch()
                    {
                        Keyword = keyword,
                        UserId = userId,
                        Timestamp = DateTime.Now
                    });
                    break;
                case MovieActionType.ActionClick:
                    _interactionDbContext.MovieClicks.Add(new MovieClick()
                    {
                        MovieName = movieName,
                        MovieId = movieId,
                        UserId = userId,
                        Timestamp = DateTime.Now
                    });
                    break;
                case MovieActionType.ActionView:
                    _interactionDbContext.MovieViews.Add(new MovieView()
                    {
                        MovieName = movieName,
                        MovieId = movieId,
                        UserId = userId,
                        Timestamp = DateTime.Now
                    });
                    break;
                default:
                    break;
            };
            _interactionDbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while logging the movie action.", ex);
        }
    }

}