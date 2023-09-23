using Microsoft.AspNetCore.Identity;
using TestAssignment.Common.Config;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Interfaces;
using TestAssignment.Data;

namespace TestAssignment.Services.Services.Data;

/// <summary>
/// Service responsible for seeding user data into the ApplicationDbContext.
/// </summary>
public class UserSeeder : IDataSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;
        
    /// <summary>
    /// Initializes a new instance of the <see cref="UserSeeder"/> class.
    /// </summary>
    /// <param name="userManager">The user manager for creating user entities.</param>
    /// <param name="applicationDbContext">The Application DbContext for user-related data.</param>
    public UserSeeder(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
    }

    /// <summary>
    /// Seed user data into the ApplicationDbContext.
    /// </summary>
    public async Task SeedDataAsync()
    {
        // Check if there are any users in the database
        if (!_applicationDbContext.Users.Any())
        {
            // Create 10 dummy users
            for (int i = 1; i <= 10; i++)
            {
                var user = new User
                {
                    UserName = $"{Constants.DUMMY_USERNAME_PREFIX}{i}",
                    Email = $"{Constants.DUMMY_USERNAME_PREFIX}{i}{Constants.DUMMY_EMAIL_SUFFIX}",
                    Name = $"{Constants.DUMMY_USERNAME_PREFIX} {i}",
                    Age = 25 + i,
                    Gender = i % 2 == 0 ? "Male" : "Female"
                };
                // Create the user with a default password
                IdentityResult result = await _userManager.CreateAsync(user, Constants.DUMMY_USER_PASSWORD);
            }
        }
    }
}