using TestAssignment.Common.Entities;
using TestAssignment.Common.Enumerations;
using TestAssignment.Common.ViewModels;

namespace TestAssignment.Common.Interfaces;

/// <summary>
/// Defines the contract for a movie service responsible for movie-related operations.
/// </summary>
public interface IMovieService
{
    /// <summary>
    /// Gets a list of viewed movie IDs by the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task representing the asynchronous operation and returning a list of viewed movie IDs.</returns>
    Task<List<int>> GetViewedMovieIdsByUserIdAsync(string userId);

    /// <summary>
    /// Gets the best viewed movies with the count of views and those not viewed by the current user.
    /// </summary>
    /// <param name="movieIdsViewedByCurrentUser">The list of movie IDs viewed by the current user.</param>
    /// <returns>A task representing the asynchronous operation and returning a list of MovieCountViewModel.</returns>
    Task<List<MovieCountViewModel>> GetBestViewedMoviesWithCountOfViewsAndNotViewedByCurrentUser(
        List<int> movieIdsViewedByCurrentUser);

    /// <summary>
    /// Gets the best clicked movies with the count of clicks and those not viewed by the current user.
    /// </summary>
    /// <param name="movieIdsViewedByCurrentUser">The list of movie IDs viewed by the current user.</param>
    /// <returns>A task representing the asynchronous operation and returning a list of MovieCountViewModel.</returns>
    Task<List<MovieCountViewModel>> GetBestClickedMoviesWithCountOfClicksAndNotViewedByCurrentUser(
        List<int> movieIdsViewedByCurrentUser);

    /// <summary>
    /// Gets the most clicked movies based on a keyword and count.
    /// </summary>
    /// <param name="keyword">The keyword to search for.</param>
    /// <param name="count">The number of movies to retrieve.</param>
    /// <returns>A task representing the asynchronous operation and returning a list of MovieCountViewModel.</returns>
    Task<List<MovieCountViewModel>> GetMostClickedMoviesAsync(string keyword, int count);

    /// <summary>
    /// Gets the most viewed movies based on a keyword and count.
    /// </summary>
    /// <param name="keyword">The keyword to search for.</param>
    /// <param name="count">The number of movies to retrieve.</param>
    /// <returns>A task representing the asynchronous operation and returning a list of MovieCountViewModel.</returns>
    Task<List<MovieCountViewModel>> GetMostViewedMoviesAsync(string keyword, int count);

    /// <summary>
    /// Gets the last user searches by keyword and count.
    /// </summary>
    /// <param name="keyword">The keyword to search for.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="count">The number of searches to retrieve.</param>
    /// <returns>A task representing the asynchronous operation and returning a list of MovieSearch entities.</returns>
    Task<List<MovieSearch>> GetLastUserSearchesByKeyWordAsync(string keyword, string userId, int count);

    /// <summary>
    /// Logs a movie action for a user.
    /// </summary>
    /// <param name="keyword">The keyword associated with the action.</param>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <param name="movieId">The ID of the movie related to the action.</param>
    /// <param name="movieName">The name of the movie related to the action.</param>
    /// <param name="actionType">The type of movie action (e.g., click or view).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task LogMovieAction(string keyword, string userId, int movieId, string movieName, MovieActionType actionType);
}