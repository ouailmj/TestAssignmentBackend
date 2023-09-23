namespace TestAssignment.Common.ViewModels.OutputViewModels;

/// <summary>
/// Represents a view model for movie information.
/// </summary>
public class MovieViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MovieViewModel"/> class.
    /// </summary>
    /// <param name="id">The ID of the movie.</param>
    /// <param name="title">The title of the movie.</param>
    /// <param name="year">The release year of the movie.</param>
    /// <param name="genres">An array of genres associated with the movie.</param>
    public MovieViewModel(int id, string title, int year, string[] genres)
    {
        Id = id;
        Title = title;
        Year = year;
        Genres = genres;
    }

    /// <summary>
    /// Gets or sets the ID of the movie.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the movie.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the release year of the movie.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets an array of genres associated with the movie.
    /// </summary>
    public string[] Genres { get; set; }
}