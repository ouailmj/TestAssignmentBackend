namespace TestAssignment.Common.Entities;

/// <summary>
/// Represents a movie click interaction entity.
/// </summary>
public class MovieClick
{
    /// <summary>
    /// Gets or sets the unique identifier for the movie click.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user's identifier associated with the movie click.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the movie that was clicked.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Gets or sets the name of the movie that was clicked.
    /// </summary>
    public string MovieName { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the movie click occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the related user entity for this movie click.
    /// </summary>
    public User User { get; set; } // Navigation property to User entity
}
