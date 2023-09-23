using System.ComponentModel.DataAnnotations;

namespace TestAssignment.Common.ViewModels;

/// <summary>
/// Represents a view model for movie search options.
/// </summary>
public class SearchViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchViewModel"/> class.
    /// </summary>
    public SearchViewModel() { }

    /// <summary>
    /// Gets or sets the search query.
    /// </summary>
    public string Query { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of old search options to retain.
    /// </summary>
    public int MaxOldSearchOptions { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of suggested search results to retrieve.
    /// </summary>
    public int MaxSuggestedSearch { get; set; }
}