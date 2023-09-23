using Microsoft.EntityFrameworkCore;
using TestAssignment.Common.Entities;

namespace TestAssignment.Common.Contexts;

/// <summary>
/// Represents the database context for interaction-related entities.
/// </summary>
public class InteractionDbContext : DbContext
{
    
    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionDbContext"/> class.
    /// </summary>
    /// <param name="options">The DbContext options for configuring the database context.</param>
    public InteractionDbContext(DbContextOptions<InteractionDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the DbSet for storing MovieClick entities.
    /// </summary>
    public DbSet<MovieClick> MovieClicks { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the DbSet for storing MovieView entities.
    /// </summary>
    public DbSet<MovieView> MovieViews { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the DbSet for storing MovieSearch entities.
    /// </summary>
    public DbSet<MovieSearch> MovieSearches { get; set; } = null!;

    /// <summary>
    /// Configures the relationships between entities in the database model.
    /// </summary>
    /// <param name="modelBuilder">The model builder used for configuring the database model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<MovieView>()
            .HasOne(mv => mv.User)
            .WithMany() // Assuming User can have multiple MovieView
            .HasForeignKey(mv => mv.UserId);
        
        modelBuilder.Entity<MovieClick>()
            .HasOne(mc => mc.User)
            .WithMany() // Assuming User can have multiple MovieClicks
            .HasForeignKey(mc => mc.UserId);
        
        modelBuilder.Entity<MovieSearch>()
            .HasOne(ms => ms.User)
            .WithMany() // Assuming User can have multiple MovieSearch
            .HasForeignKey(ms => ms.UserId);
    }
}