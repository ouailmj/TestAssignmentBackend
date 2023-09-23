using TestAssignment.Common.ViewModels.OutputViewModels;

namespace TestAssignment.Common.Interfaces;


/// <summary>
/// Defines the contract for an ElasticSearch service for searching movies.
/// </summary>
public interface IElasticSearchService
{
    /// <summary>
    /// Searches for movies by a list of movie IDs.
    /// </summary>
    /// <param name="movieIdsToSearch">The list of movie IDs to search for.</param>
    /// <returns>A list of MovieViewModel representing the searched movies.</returns>
    Task<List<MovieViewModel>> SearchMoviesByIds(List<int> movieIdsToSearch);
    
    /// <summary>
    /// Searches for movies by a keyword.
    /// </summary>
    /// <param name="keywordToSearch">The keyword to search for.</param>
    /// <returns>A list of MovieViewModel representing the searched movies.</returns>
    Task<List<MovieViewModel>> SearchMoviesByKeyword(string keywordToSearch);
}