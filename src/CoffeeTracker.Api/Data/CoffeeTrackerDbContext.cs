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
    /// Gets or sets the DbSet for coffee shops.
    /// Provides access to coffee shop records for tracking coffee sources.
    /// </summary>
    public DbSet<CoffeeShop> CoffeeShops { get; set; } = null!;

    /// <summary>
    /// Configures the entity models and their relationships using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureCoffeeEntry(modelBuilder);
        ConfigureCoffeeShop(modelBuilder);
        SeedCoffeeShops(modelBuilder);
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

    /// <summary>
    /// Configures the CoffeeShop entity with appropriate constraints and indexes.
    /// </summary>
    /// <param name="modelBuilder">The model builder to configure.</param>
    private static void ConfigureCoffeeShop(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoffeeShop>(entity =>
        {
            // Primary key configuration
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            
            // Required fields configuration
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(CoffeeShop.NameMaxLength)
                .HasComment("Name of the coffee shop");
                
            // Optional fields configuration
            entity.Property(e => e.Address)
                .HasMaxLength(CoffeeShop.AddressMaxLength)
                .HasComment("Address of the coffee shop");
                
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasComment("Whether the coffee shop is active");
                
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')")
                .HasComment("When the coffee shop record was created (UTC)");
                
            // Performance indexes
            entity.HasIndex(e => e.Name)
                .HasDatabaseName("IX_CoffeeShops_Name")
                .HasFilter(null) // Ensure SQLite compatibility
                .HasAnnotation("Description", "Index for coffee shop name searches");
                
            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_CoffeeShops_IsActive")
                .HasFilter(null) // Ensure SQLite compatibility
                .HasAnnotation("Description", "Index for filtering active coffee shops");
        });
    }

    /// <summary>
    /// Seeds the database with sample coffee shop data.
    /// </summary>
    /// <param name="modelBuilder">The model builder to configure.</param>
    private static void SeedCoffeeShops(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        var seedData = new[]
        {
            new CoffeeShop { Id = 1, Name = "Home", Address = null, IsActive = true, CreatedAt = seedDate },
            new CoffeeShop { Id = 2, Name = "Starbucks", Address = "Multiple Locations", IsActive = true, CreatedAt = seedDate },
            new CoffeeShop { Id = 3, Name = "Dunkin' Donuts", Address = "Multiple Locations", IsActive = true, CreatedAt = seedDate },
            new CoffeeShop { Id = 4, Name = "Local Coffee House", Address = "123 Main Street", IsActive = true, CreatedAt = seedDate },
            new CoffeeShop { Id = 5, Name = "Peet's Coffee", Address = "456 Oak Avenue", IsActive = true, CreatedAt = seedDate },
            new CoffeeShop { Id = 6, Name = "The Coffee Bean & Tea Leaf", Address = "789 Elm Street", IsActive = true, CreatedAt = seedDate },
            new CoffeeShop { Id = 7, Name = "Blue Bottle Coffee", Address = "321 Pine Road", IsActive = true, CreatedAt = seedDate },
            new CoffeeShop { Id = 8, Name = "Tim Hortons", Address = "654 Maple Drive", IsActive = true, CreatedAt = seedDate },
            new CoffeeShop { Id = 9, Name = "Costa Coffee", Address = "987 Cedar Lane", IsActive = true, CreatedAt = seedDate }
        };

        modelBuilder.Entity<CoffeeShop>().HasData(seedData);
    }
}
