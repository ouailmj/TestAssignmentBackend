using System.Text.RegularExpressions;
using TestAssignment.Common.Config;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Enumerations;
using TestAssignment.Common.ViewModels;
using TestAssignment.Common.ViewModels.OutputViewModels;

namespace TestAssignment.Common.Extensions;

/// <summary>
/// Provides extension methods for working with movie-related data and calculations.
/// </summary>
public static class MovieExtensions
{
    /// <summary>
    /// Calculates the total score for movies based on their interactions and specified factors.
    /// </summary>
    /// <param name="source">An IEnumerable of MovieCountViewModel containing interaction data for movies.</param>
    /// <param name="factorViewed">The factor to apply to views in the score calculation.</param>
    /// <param name="factorClicked">The factor to apply to clicks in the score calculation.</param>
    /// <returns>An IEnumerable of MovieScoreViewModel containing calculated scores for movies.</returns>
    public static IEnumerable<MovieScoreViewModel> CalculateTotalScore(
        this IEnumerable<MovieCountViewModel> source, int factorViewed, int factorClicked)
    {
        return source
            .GroupBy(movie => new { movie.MovieId, movie.MovieName })
            .Select(group => new MovieScoreViewModel
            {
                MovieId = group.Key.MovieId,
                MovieName = group.Key.MovieName,
                Score = group.Sum(movie =>
                    movie.ActionType == MovieActionType.ActionView ? factorViewed * movie.Count : 0) +
                        group.Sum(movie =>
                            movie.ActionType == MovieActionType.ActionClick ? factorClicked * movie.Count : 0)
            });
    }

    /// <summary>
    /// Gets the top-rated movies based on their calculated scores.
    /// </summary>
    /// <param name="source">An IEnumerable of MovieScoreViewModel containing calculated scores for movies.</param>
    /// <param name="topCount">The number of top-rated movies to retrieve.</param>
    /// <returns>A List of MovieScoreViewModel representing the top-rated movies.</returns>
    public static List<MovieScoreViewModel> GetTopMovies(
        this IEnumerable<MovieScoreViewModel> source, int topCount)
    {
        return source
            .OrderByDescending(movie => movie.Score)
            .Take(topCount)
            .ToList();
    }

    /// <summary>
    /// Gets the IDs of the top-rated movies based on their calculated scores.
    /// </summary>
    /// <param name="source">An IEnumerable of MovieScoreViewModel containing calculated scores for movies.</param>
    /// <param name="topCount">The number of top-rated movies to retrieve.</param>
    /// <returns>A List of integers representing the IDs of the top-rated movies.</returns>
    public static List<int> GetTopMovieIds(
        this IEnumerable<MovieScoreViewModel> source, int topCount)
    {
        return source
            .GetTopMovies(topCount)
            .Select(movie => movie.MovieId)
            .ToList();
    }

    /// <summary>
    /// Gets the names of the top-rated movies based on their calculated scores.
    /// </summary>
    /// <param name="source">An IEnumerable of MovieScoreViewModel containing calculated scores for movies.</param>
    /// <param name="topCount">The number of top-rated movies to retrieve.</param>
    /// <returns>A List of strings representing the names of the top-rated movies.</returns>
    public static List<string> GetTopMovieNames(
        this IEnumerable<MovieScoreViewModel> source, int topCount)
    {
        return source
            .GetTopMovies(topCount)
            .Select(movie => movie.MovieName)
            .ToList();
    }

    /// <summary>
    /// Sorts existing movies by score, based on the provided list of best movies with scores.
    /// </summary>
    /// <param name="source">An IEnumerable of MovieViewModel representing existing movies.</param>
    /// <param name="bestMovies">A List of MovieScoreViewModel containing the best movies with scores.</param>
    /// <returns>A List of MovieViewModel representing existing movies sorted by score.</returns>
    public static List<MovieViewModel> SortExistingMoviesByScore(
        this IEnumerable<MovieViewModel> source, List<MovieScoreViewModel> bestMovies)
    {
        var moviesSorted = from movie in source
                           join bestMovie in bestMovies
                           on movie.Id equals bestMovie.MovieId into temp
                           from bestMovie in temp.DefaultIfEmpty(new MovieScoreViewModel())
                           orderby bestMovie.Score descending
                           select new MovieViewModel(movie.Id, movie.Title, movie.Year, movie.Genres);

        return moviesSorted.ToList();
    }
}
