namespace TestAssignment.Common.Entities;

/// <summary>
/// Represents a movie view interaction entity.
/// </summary>
public class MovieView
{
    /// <summary>
    /// Gets or sets the unique identifier for the movie view.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user's identifier associated with the movie view.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the movie that was viewed.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Gets or sets the name of the movie that was viewed.
    /// </summary>
    public string MovieName { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the movie view occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the related user entity for this movie view.
    /// </summary>
    public User User { get; set; } // Navigation property to User entity
}
