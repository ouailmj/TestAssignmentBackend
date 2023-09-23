namespace TestAssignment.Common.Entities;

/// <summary>
/// Represents a movie search interaction entity.
/// </summary>
public class MovieSearch
{
    /// <summary>
    /// Gets or sets the unique identifier for the movie search.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user's identifier associated with the movie search.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the keyword used in the movie search.
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the movie search occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the related user entity for this movie search.
    /// </summary>
    public User User { get; set; } // Navigation property to User entity
}
