using Microsoft.EntityFrameworkCore;
using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Data;

/// <summary>
/// Entity Framework DbContext for the Coffee Tracker application.
/// Provides data access for coffee tracking functionality including
/// coffee entries for anonymous users.
/// </summary>
public class CoffeeTrackerDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the CoffeeTrackerDbContext class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public CoffeeTrackerDbContext(DbContextOptions<CoffeeTrackerDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet for coffee entries.
    /// Provides access to coffee consumption records for anonymous users.
    /// </summary>
    public DbSet<CoffeeEntry> CoffeeEntries { get; set; } = null!;

    /// <summary>
    /// Configures the entity models and their relationships using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureCoffeeEntry(modelBuilder);
    }

    /// <summary>
    /// Configures the CoffeeEntry entity with appropriate constraints and indexes.
    /// </summary>
    /// <param name="modelBuilder">The model builder to configure.</param>
    private static void ConfigureCoffeeEntry(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoffeeEntry>(entity =>
        {
            // Primary key configuration
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            
            // Required fields configuration
            entity.Property(e => e.CoffeeType)
                .IsRequired()
                .HasMaxLength(CoffeeEntry.CoffeeTypeMaxLength)
                .HasComment("Type of coffee consumed (e.g., Espresso, Latte)");
                
            entity.Property(e => e.Size)
                .IsRequired()
                .HasComment("Size of the coffee (e.g., Small, Medium, Large)");
                
            entity.Property(e => e.Timestamp)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')")
                .HasComment("When the coffee was consumed (UTC)");
                
            // Optional fields configuration
            entity.Property(e => e.Source)
                .HasMaxLength(CoffeeEntry.SourceMaxLength)
                .HasComment("Source of the coffee (e.g., coffee shop name)");
                
            // Performance indexes
            entity.HasIndex(e => e.Timestamp)
                .HasDatabaseName("IX_CoffeeEntries_Timestamp")
                .HasFilter(null) // Ensure SQLite compatibility
                .HasAnnotation("Description", "Index for time-based queries and analytics");
                
            // Exclude computed properties from database mapping
            entity.Ignore(e => e.CaffeineAmount);
        });
    }
}
