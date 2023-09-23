using Nest;
using TestAssignment.Common.Interfaces;
using TestAssignment.Configuration;

namespace TestAssignment.Services.Providers;

/// <summary>
/// Provider for creating and managing an Elasticsearch client instance.
/// </summary>
public class ElasticSearchClientProvider : IElasticSearchClientProvider
{
    
    private readonly ElasticSearchSettings _appSettings;
    private Lazy<ElasticClient> _lazyClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElasticSearchClientProvider"/> class.
    /// </summary>
    /// <param name="appSettings">Elasticsearch settings.</param>
    public ElasticSearchClientProvider(ElasticSearchSettings appSettings)
    {
        _appSettings = appSettings;
        _lazyClient = new Lazy<ElasticClient>(InitializeElasticClient);
    }

    /// <summary>
    /// Gets an instance of the Elasticsearch client.
    /// </summary>
    /// <returns>An instance of the Elasticsearch client.</returns>
    public ElasticClient GetElasticClient()
    {
        return _lazyClient.Value;
    }

    /// <summary>
    /// Initialize the ElasticSearch client
    /// </summary>
    /// <returns>An instance of the Elasticsearch client.</returns>
    private ElasticClient InitializeElasticClient()
    {
        // Create connection settings with URI and optional configurations.
        var settings = new ConnectionSettings(_appSettings.Uri).DisableDirectStreaming();

        // Create and return a new Elasticsearch client instance.
        return new ElasticClient(settings);
    }
}