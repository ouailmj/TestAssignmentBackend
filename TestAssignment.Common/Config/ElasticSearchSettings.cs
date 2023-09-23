namespace TestAssignment.Configuration
{
    /// <summary>
    /// Represents settings for Elasticsearch configuration.
    /// </summary>
    public class ElasticSearchSettings
    {
        /// <summary>
        /// Gets or sets the URI of the Elasticsearch instance.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the name of the Elasticsearch index for movies.
        /// </summary>
        public string MovieIndex { get; set; }

        /// <summary>
        /// Gets or sets the file path to the source data file.
        /// </summary>
        public string FilePath { get; set; }
    }
}