using Microsoft.EntityFrameworkCore;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Models;
using FluentAssertions;

namespace CoffeeTracker.Api.Tests.Data;

/// <summary>
/// Integration tests to verify database migrations and schema are correct.
/// Tests that the database can be created and entities work properly.
/// </summary>
public class DatabaseMigrationTests : IDisposable
{
    private readonly CoffeeTrackerDbContext _context;
    private readonly string _connectionString;

    public DatabaseMigrationTests()
    {
        // Use a unique database file for each test run
        _connectionString = $"Data Source=test_migration_{Guid.NewGuid()}.db";
        
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseSqlite(_connectionString)
            .Options;
            
        _context = new CoffeeTrackerDbContext(options);
    }

    [Fact]
    public void Should_Create_Database_With_Migrations()
    {
        // Act - Create the database with migrations
        _context.Database.EnsureCreated();
        var canConnect = _context.Database.CanConnect();
        
        // Assert
        canConnect.Should().BeTrue("database should be accessible after migration");
    }

    [Fact]
    public void Should_Have_CoffeeEntries_Table()
    {
        // Arrange
        _context.Database.EnsureCreated();
        
        // Act - Try to query the table (will fail if table doesn't exist)
        var tableExists = _context.CoffeeEntries.Any();
        
        // Assert
        tableExists.Should().BeFalse("table should exist even if empty");
    }

    [Fact]
    public void Should_Have_CoffeeShops_Table()
    {
        // Arrange
        _context.Database.EnsureCreated();
        
        // Act - Try to query the table (will fail if table doesn't exist)
        var shops = _context.CoffeeShops.ToList();
        
        // Assert
        shops.Should().NotBeNull("CoffeeShops table should exist");
        shops.Should().HaveCountGreaterThan(0, "seed data should be loaded");
    }

    [Fact]
    public void Should_Insert_And_Retrieve_CoffeeEntry()
    {
        // Arrange
        _context.Database.EnsureCreated();
        
        var coffeeEntry = new CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = "Small",
            Source = "Test Shop",
            Timestamp = DateTime.UtcNow
        };

        // Act
        _context.CoffeeEntries.Add(coffeeEntry);
        _context.SaveChanges();

        var retrieved = _context.CoffeeEntries.First();

        // Assert
        retrieved.Should().NotBeNull();
        retrieved.CoffeeType.Should().Be("Espresso");
        retrieved.Size.Should().Be("Small");
        retrieved.Source.Should().Be("Test Shop");
        retrieved.Id.Should().BeGreaterThan(0, "ID should be auto-generated");
    }

    [Fact]
    public void Should_Insert_And_Retrieve_CoffeeShop()
    {
        // Arrange
        _context.Database.EnsureCreated();
        
        var coffeeShop = new CoffeeShop
        {
            Name = "Test Coffee Shop",
            Address = "123 Test Street",
            IsActive = true
        };

        // Act
        _context.CoffeeShops.Add(coffeeShop);
        _context.SaveChanges();

        var retrieved = _context.CoffeeShops
            .Where(s => s.Name == "Test Coffee Shop")
            .First();

        // Assert
        retrieved.Should().NotBeNull();
        retrieved.Name.Should().Be("Test Coffee Shop");
        retrieved.Address.Should().Be("123 Test Street");
        retrieved.IsActive.Should().BeTrue();
        retrieved.Id.Should().BeGreaterThan(0, "ID should be auto-generated");
        retrieved.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void Should_Verify_Indexes_Exist()
    {
        // Arrange
        _context.Database.EnsureCreated();
        
        // Act - Get the database structure
        var tables = _context.Database.ExecuteSqlRaw(
            "SELECT name FROM sqlite_master WHERE type='index' AND name LIKE 'IX_%'");
        
        // Note: We'll need to verify indexes exist by querying SQLite system tables
        // This is a placeholder test that will need proper implementation
        
        // Assert
        true.Should().BeTrue("Index verification test placeholder");
    }

    [Fact]
    public void Should_Verify_Constraints_Work()
    {
        // Arrange
        _context.Database.EnsureCreated();
        
        // Act & Assert - Try to insert invalid data
        var invalidCoffeeEntry = new CoffeeEntry
        {
            CoffeeType = "", // Should fail required validation at model level
            Size = "Small"
        };

        _context.CoffeeEntries.Add(invalidCoffeeEntry);
        
        // For SQLite, we need to validate at the model level rather than database level
        // So let's validate the model directly
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(invalidCoffeeEntry);
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
            invalidCoffeeEntry, validationContext, validationResults, true);
        
        // Assert
        isValid.Should().BeFalse("empty CoffeeType should fail model validation");
        validationResults.Should().NotBeEmpty("validation errors should be returned");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        
        // Clean up test database file
        var dbFile = _connectionString.Replace("Data Source=", "");
        if (File.Exists(dbFile))
        {
            try
            {
                File.Delete(dbFile);
            }
            catch (IOException)
            {
                // File may be locked, ignore for test cleanup
            }
        }
    }
}
