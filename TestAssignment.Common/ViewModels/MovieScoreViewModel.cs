using TestAssignment.Common.Enumerations;

namespace TestAssignment.Common.ViewModels;

/// <summary>
/// Represents a view model for movie scores.
/// </summary>
public class MovieScoreViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MovieScoreViewModel"/> class.
    /// </summary>
    public MovieScoreViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieScoreViewModel"/> class.
    /// </summary>
    /// <param name="movieId">The ID of the movie.</param>
    /// <param name="score">The score of the movie.</param>
    public MovieScoreViewModel(int movieId, float score)
    {
        MovieId = movieId;
        Score = score;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieScoreViewModel"/> class.
    /// </summary>
    /// <param name="movieId">The ID of the movie.</param>
    /// <param name="movieName">The name of the movie.</param>
    /// <param name="score">The score of the movie.</param>
    public MovieScoreViewModel(int movieId, string movieName, float score)
    {
        MovieId = movieId;
        MovieName = movieName;
        Score = score;
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
    /// Gets or sets the score of the movie.
    /// </summary>
    public float Score { get; set; }
}