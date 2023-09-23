using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAssignment.Common.Entities;

namespace TestAssignment.Data;

/// <summary>
/// Represents the application's database context that inherits from IdentityDbContext for user management.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User>
{
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The DbContext options for configuring the database context.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}