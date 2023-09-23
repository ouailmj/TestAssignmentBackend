using TestAssignment.Common.Enumerations;

namespace TestAssignment.Common.ViewModels;

/// <summary>
/// Represents a view model for movie counts and actions.
/// </summary>
public class MovieCountViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MovieCountViewModel"/> class.
    /// </summary>
    public MovieCountViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieCountViewModel"/> class.
    /// </summary>
    /// <param name="movieId">The ID of the movie.</param>
    /// <param name="movieName">The name of the movie.</param>
    /// <param name="count">The count of the movie action.</param>
    /// <param name="actionType">The type of movie action.</param>
    public MovieCountViewModel(int movieId, string movieName, int count, MovieActionType actionType)
    {
        MovieId = movieId;
        MovieName = movieName;
        Count = count;
        ActionType = actionType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieCountViewModel"/> class.
    /// </summary>
    /// <param name="movieId">The ID of the movie.</param>
    /// <param name="count">The count of the movie action.</param>
    /// <param name="actionType">The type of movie action.</param>
    public MovieCountViewModel(int movieId, int count, MovieActionType actionType)
    {
        MovieId = movieId;
        Count = count;
        ActionType = actionType;
    }

    /// <summary>
    /// Gets or sets the ID of the movie.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Gets or sets the name of the movie.
    /// </summary>
    public string MovieName { get; set; }

    /// <summary>
    /// Gets or sets the count of the movie action.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Gets or sets the type of movie action.
    /// </summary>
    public MovieActionType ActionType { get; set; }
}