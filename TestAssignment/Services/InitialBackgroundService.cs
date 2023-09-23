using TestAssignment.Services.Services.Data;

namespace TestAssignment.Services
{
    /// <summary>
    /// A background service for seeding initial data into the application.
    /// This Service create dummy datas for Movie Search / click / views tables
    /// And create Movie Index in Elastic search and copy datas into database
    /// </summary>
    public class InitialBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialBackgroundService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
        public InitialBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes the background service logic.
        /// </summary>
        /// <param name="stoppingToken">A cancellation token that is triggered when the background service should stop.</param>
        /// <returns>A task representing the background service's operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Resolve the UserSeeder service to seed user data
                    var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
                    await userSeeder.SeedDataAsync();

                    // Resolve the InteractionSeeder service to seed interaction data (e.g., clicks, views, searches)
                    var interactionSeeder = scope.ServiceProvider.GetRequiredService<InteractionSeeder>();
                    await interactionSeeder.SeedDataAsync();

                    // Resolve the MovieSeeder service to seed movie data
                    var movieSeeder = scope.ServiceProvider.GetRequiredService<MovieSeeder>();
                    await movieSeeder.SeedDataAsync();
                }

                // Delay for a specific interval before running again (e.g., every hour)
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}