namespace TestAssignment.Common.ViewModels.OutputViewModels;

/// <summary>
/// Represents the result of a movie search operation.
/// </summary>
public class SearchMovieResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchMovieResult"/> class.
    /// </summary>
    public SearchMovieResult() { }

    /// <summary>
    /// Gets or sets the list of last searched keywords.
    /// </summary>
    public List<string> LastSearchedKeywords { get; set; }

    /// <summary>
    /// Gets or sets the list of recommended keywords.
    /// </summary>
    public List<string> RecommendedKeywords { get; set; }

    /// <summary>
    /// Gets or sets the list of searched movies.
    /// </summary>
    public List<MovieViewModel> SearchedMovies { get; set; }
}