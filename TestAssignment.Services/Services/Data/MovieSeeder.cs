using Nest;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Exceptions;
using TestAssignment.Common.Extensions;
using TestAssignment.Common.Interfaces;
using TestAssignment.Configuration;

namespace TestAssignment.Services.Services.Data;

/// <summary>
/// Service responsible for indexing movie data into Elasticsearch.
/// Create the index if it doens't exist and insert datas to it if no data found
/// </summary>
public class MovieSeeder : IDataSeeder
{
    private readonly IElasticSearchClientProvider _elasticSearchClientProvider;
    private readonly ElasticSearchSettings _appSettings;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MovieSeeder"/> class.
    /// </summary>
    /// <param name="clientProvider">Elasticsearch client provider.</param>
    /// <param name="appSettings">Elasticsearch settings.</param>
    public MovieSeeder(IElasticSearchClientProvider clientProvider, ElasticSearchSettings appSettings)
    {
        _elasticSearchClientProvider = clientProvider;
        _appSettings = appSettings;
    }

    /// <summary>
    /// Indexes movies into Elasticsearch.
    /// Create the index if it doens't exist and insert datas to it if no data found
    /// </summary>
    public async Task SeedDataAsync()
    {
        try
        {
            // get elastic client
            var elasticClient = _elasticSearchClientProvider.GetElasticClient();
            
            // Check if the movies index exists; if not, create it
            if (!IndexExists(elasticClient, _appSettings.MovieIndex)) {
                CreateIndex(elasticClient, _appSettings.MovieIndex);
            }
            
            // Check if the index already contains data
            long documentCount = GetDocumentCount(elasticClient, _appSettings.MovieIndex);
            if (documentCount > 0) { Console.WriteLine($"Index '{_appSettings.MovieIndex}' already contains {documentCount} documents."); }
            else
            {
                // Read the data from the file
                string data = await File.ReadAllTextAsync(_appSettings.FilePath);
                // Parse movies from the data
                List<Movie> movies = data.ParseMovieDatas();
                // Bulk index the movies into Elasticsearch
                BulkIndexMovies(elasticClient, _appSettings.MovieIndex, movies);
            }
        }
        catch (Exception ex)
        {
            throw new ElasticSearchException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Checks if the specified Elasticsearch index exists.
    /// </summary>
    /// <param name="client">Elasticsearch client instance.</param>
    /// <param name="indexName">Name of the index to check.</param>
    /// <returns><c>true</c> if the index exists; otherwise, <c>false</c>.</returns>
    private bool IndexExists(ElasticClient client, string indexName)
    {
        var indexExistsResponse = client.Indices.Exists(indexName);
        return indexExistsResponse.Exists;
    }
    
    /// <summary>
    /// Retrieves the document count in the specified Elasticsearch index.
    /// </summary>
    /// <param name="client">Elasticsearch client instance.</param>
    /// <param name="indexName">Name of the index to count documents in.</param>
    /// <returns>The number of documents in the index.</returns>
    private long GetDocumentCount(ElasticClient client, string indexName)
    {
        var countResponse = client.Count<Movie>(c => c.Index(indexName));
        return countResponse.IsValid ? countResponse.Count : 0;
    }

    /// <summary>
    /// Creates an Elasticsearch index with custom settings for movie data.
    /// The search is configured to show results even if the words are misspelled or stemming (plural /singular handling “”)
    /// </summary>
    /// <param name="client">Elasticsearch client instance.</param>
    /// <param name="indexName">Name of the index to create.</param>
    private void CreateIndex(ElasticClient client, string indexName)
    {
        var createIndexResponse = client.Indices.Create(indexName, c => c
            .Settings(s => s
                .Analysis(a => a
                    .TokenFilters(tf => tf
                        .Stemmer("english_stemmer", stemmer => stemmer.Language("english"))
                    )
                    .Analyzers(an => an
                        .Custom("custom_english", ca => ca
                            .Tokenizer("standard")
                            .Filters("lowercase", "english_stemmer")
                        )
                    )
                )
            )
            .Map<Movie>(m => m
                .AutoMap()
                .Properties(p => p
                    .Text(t => t
                        .Name(n => n.Title)
                        .Analyzer("custom_english")
                    )
                )
            )
        );

        if (!createIndexResponse.IsValid)
        {
            Console.WriteLine("Index creation failed: " + createIndexResponse.ServerError);
            throw new ElasticSearchException("Index creation failed!");
        }
        else
        {
            Console.WriteLine($"Index '{indexName}' created successfully.");
        }
    }
    
    
    /// <summary>
    /// Bulk indexes a list of movies into the specified Elasticsearch index.
    /// </summary>
    /// <param name="client">Elasticsearch client instance.</param>
    /// <param name="indexName">Name of the index to bulk index into.</param>
    /// <param name="movies">List of movies to be indexed.</param>
    private void BulkIndexMovies(ElasticClient client, string indexName, List<Movie> movies)
    {
        var bulkDescriptor = new BulkDescriptor();
        foreach (var movie in movies)
        {
            bulkDescriptor.Index<Movie>(op => op
                .Index(indexName)
                .Document(movie)
            );
        }

        var bulkResponse = client.Bulk(bulkDescriptor);
        if (!bulkResponse.IsValid && bulkResponse.Errors)
        {
            Console.WriteLine($"Error: {bulkResponse.ServerError}");

            foreach (var item in bulkResponse.Items)
            {
                if (item.Error != null)
                {
                    Console.WriteLine($"Error for operation {item.Operation}");
                    Console.WriteLine($"Error reason: {item.Error.Reason}");
                    Console.WriteLine($"Error type: {item.Error.Type}");
                }
            }

            throw new ElasticSearchException("Bulk indexing failed!");
        }
        
        Console.WriteLine("Bulk indexing completed successfully.");
    }
}