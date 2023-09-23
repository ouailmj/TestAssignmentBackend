using TestAssignment.Common.Config;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Exceptions;
using TestAssignment.Common.Interfaces;
using TestAssignment.Common.ViewModels.OutputViewModels;
using TestAssignment.Configuration;

namespace TestAssignment.Services.Services.ElasticSearch;

/// <summary>
/// Service responsible for interacting with Elasticsearch to search for movies.
/// </summary>
public class ElasticSearchService : IElasticSearchService
{
    private readonly IElasticSearchClientProvider _elasticSearchClientProvider;
    private readonly ElasticSearchSettings _appSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElasticSearchService"/> class.
    /// </summary>
    /// <param name="clientProvider">The provider for the Elasticsearch client.</param>
    /// <param name="appSettings">The Elasticsearch settings.</param>
    public ElasticSearchService(IElasticSearchClientProvider clientProvider, ElasticSearchSettings appSettings)
    {
        _elasticSearchClientProvider = clientProvider ?? throw new ArgumentNullException(nameof(clientProvider));
        _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
    }

    /// <summary>
    /// Searches for movies by a list of movie IDs.
    /// </summary>
    /// <param name="movieIdsToSearch">The list of movie IDs to search for.</param>
    /// <returns>A list of <see cref="MovieViewModel"/> containing search results.</returns>
    /// <exception cref="ElasticSearchException">Thrown when an error occurs while searching.</exception>
    public async Task<List<MovieViewModel>> SearchMoviesByIds(List<int> movieIdsToSearch)
    {
        try
        {
            // getting the elasticsearch client
            var elasticClient = _elasticSearchClientProvider.GetElasticClient();
            // getting the search response
            var searchResponse = await elasticClient.SearchAsync<Movie>(s => s
                .Index(_appSettings.MovieIndex)
                .Query(q => q
                    .Terms(t => t
                        .Field(f => f.Id)
                        .Terms(movieIdsToSearch)
                    )
                ).Size(Constants.MAX_NUMBER_ELASTICSEARCH_RECORDS)
            );

            // if response not valid return exception
            if (!searchResponse.IsValid)
            {
                throw new ElasticSearchException("An error occurred while searching.");
            }

            // else return datas formated using the view model
            return searchResponse.Documents
                .Select(d => new MovieViewModel(d.Id, d.Title, d.Year, d.Genres))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new ElasticSearchException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Searches for movies by a keyword in their titles.
    /// </summary>
    /// <param name="keyword">The keyword to search for in movie titles.</param>
    /// <returns>A list of <see cref="MovieViewModel"/> containing search results.</returns>
    /// <exception cref="ElasticSearchException">Thrown when an error occurs while searching.</exception>
    public async Task<List<MovieViewModel>> SearchMoviesByKeyword(string keyword)
    {
        try
        {
            // getting the client
            var elasticClient = _elasticSearchClientProvider.GetElasticClient();
            // getting the response
            var searchResponse = await elasticClient.SearchAsync<Movie>(s => s
                .Index(_appSettings.MovieIndex)
                .Query(q => q
                    .MatchPhrasePrefix(m => m
                        .Field(fld => fld.Title)
                        .Query(keyword)
                    )
                ).Size(20)
            );

            // if response is not valid
            if (!searchResponse.IsValid)
            {
                throw new ElasticSearchException("An error occurred while searching.");
            }

            // else return the response in a DTO
            return searchResponse.Documents
                .Select(d => new MovieViewModel(d.Id, d.Title, d.Year, d.Genres))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new ElasticSearchException(ex.Message, ex);
        }
    }
}