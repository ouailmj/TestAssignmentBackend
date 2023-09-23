using Nest;

namespace TestAssignment.Common.Interfaces;


// <summary>
/// Defines the contract for an ElasticSearch client provider.
/// </summary>
public interface IElasticSearchClientProvider
{
    /// <summary>
    /// Gets an instance of the ElasticSearch client.
    /// </summary>
    /// <returns>An ElasticClient instance.</returns>
    ElasticClient GetElasticClient();
}