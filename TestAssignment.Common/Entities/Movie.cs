namespace TestAssignment.Common.Entities
{
    /// <summary>
    /// Represents a movie entity with basic information.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Gets or sets the unique identifier of the movie.
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
}