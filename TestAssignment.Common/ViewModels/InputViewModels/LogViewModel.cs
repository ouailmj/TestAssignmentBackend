using TestAssignment.Common.Enumerations;

namespace TestAssignment.Common.ViewModels.InputViewModels;

/// <summary>
/// Represents a view model for logging movie actions.
/// </summary>
public class LogViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogViewModel"/> class.
    /// </summary>
    public LogViewModel() { }

    /// <summary>
    /// Gets or sets the keyword associated with the action.
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// Gets or sets the ID of the movie related to the action.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Gets or sets the name of the movie related to the action.
    /// </summary>
    public string MovieName { get; set; }

    /// <summary>
    /// Gets or sets the type of movie action (e.g., click or view).
    /// </summary>
    public MovieActionType Action { get; set; }
}