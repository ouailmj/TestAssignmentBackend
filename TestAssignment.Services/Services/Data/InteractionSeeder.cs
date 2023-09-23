using TestAssignment.Common.Config;
using TestAssignment.Common.Contexts;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Interfaces;
using TestAssignment.Data;

namespace TestAssignment.Services.Services.Data;

/// <summary>
/// Service responsible for seeding interaction data into the InteractionDbContext.
/// Allow to create click/view/search dummy datas
/// </summary>
public class InteractionSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly InteractionDbContext _interactionDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionSeeder"/> class.
    /// </summary>
    /// <param name="applicationDbContext">The Application DbContext for user-related data.</param>
    /// <param name="interactionDbContext">The Interaction DbContext for interaction data.</param>
    public InteractionSeeder(ApplicationDbContext applicationDbContext, InteractionDbContext interactionDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _interactionDbContext = interactionDbContext;
    }

    /// <summary>
    /// Seed interaction data into the InteractionDbContext.
    /// </summary>
    public async Task SeedDataAsync()
    {
        // getting the user with UserName user1
        User? user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == $"{Constants.DUMMY_USERNAME_PREFIX}1");
        // init dummy movies names
        List<String> dummyMovieNames = Constants.DUMMY_MOVIES_NAMES;

        // create dummy movie clicks
        // associating them to user and movie name
        if (!_interactionDbContext.MovieClicks.Any())
        {
            List<MovieClick> movieClicks = GenerateDummyMovieClicks(user, dummyMovieNames);
            _interactionDbContext.MovieClicks.AddRange(movieClicks);
        }

        // create dummy movie views
        // associating them to user and movie name
        if (!_interactionDbContext.MovieViews.Any())
        {
            List<MovieView> movieViews = GenerateDummyMovieViews(user, dummyMovieNames);
            _interactionDbContext.MovieViews.AddRange(movieViews);
        }

        // create dummy movie searchs
        // associating them to user and movie name
        if (!_interactionDbContext.MovieSearches.Any())
        {
            List<MovieSearch> movieSearches = GenerateDummyMovieSearches(user, dummyMovieNames);
            _interactionDbContext.MovieSearches.AddRange(movieSearches);
        }

        _interactionDbContext.SaveChanges();
    }
    
    private List<MovieClick> GenerateDummyMovieClicks(User user, List<string> dummyMovieNames)
    {
        List<MovieClick> movieClicks = new List<MovieClick>();
        // Create 10 dummy clicks
        for (int i = 1; i <= 10; i++)
        {
            var movieClick = new MovieClick
            {
                UserId = user.Id,
                MovieId = i,
                MovieName = dummyMovieNames[i - 1],
                Timestamp = DateTime.Now
            };
            movieClicks.Add(movieClick);
        }

        return movieClicks;
    }
    
    private List<MovieView> GenerateDummyMovieViews(User user, List<string> dummyMovieNames)
    {
        List<MovieView> movieViews = new List<MovieView>();
        // Create 10 dummy views
        for (int i = 1; i <= 10; i++)
        {
            var movieView = new MovieView
            {
                UserId = user.Id,
                MovieId = i * 2,
                MovieName = dummyMovieNames[2 * i - 1],
                Timestamp = DateTime.Now
            };
            movieViews.Add(movieView);
        }

        return movieViews;
    }

    private List<MovieSearch> GenerateDummyMovieSearches(User user, List<string> dummyMovieNames)
    {
        int keywordIndex = 0;
        List<MovieSearch> movieSearches = new List<MovieSearch>();
        // Create 10 dummy searches
        for (int i = 1; i <= 10; i++)
        {
            var movieSearch = new MovieSearch
            {
                UserId = user.Id,
                Keyword = dummyMovieNames[keywordIndex],
                Timestamp = DateTime.Now
            };
            keywordIndex = (keywordIndex + 1) % dummyMovieNames.Count;
            movieSearches.Add(movieSearch);
        }

        return movieSearches;
    }
}