namespace TestAssignment.Common.Interfaces;

/// <summary>
/// Defines the contract for a data seeding service responsible for populating data in a database.
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    /// Seeds data asynchronously into a database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SeedDataAsync();
}
