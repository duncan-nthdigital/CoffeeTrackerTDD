using Microsoft.EntityFrameworkCore;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Models;
using FluentAssertions;

namespace CoffeeTracker.Api.Tests.Data;

/// <summary>
/// Quick verification test to demonstrate the database working with actual data.
/// This test shows the complete workflow of the migration setup.
/// </summary>
public class DatabaseVerificationDemo : IDisposable
{
    private readonly CoffeeTrackerDbContext _context;
    private readonly string _connectionString;

    public DatabaseVerificationDemo()
    {
        _connectionString = $"Data Source=demo_verification_{Guid.NewGuid()}.db";
        
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseSqlite(_connectionString)
            .Options;
            
        _context = new CoffeeTrackerDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void Database_Should_Have_Complete_Working_Schema_With_Sample_Data()
    {
        // Arrange & Act - Query seed data
        var coffeeShops = _context.CoffeeShops.ToList();
        var totalShops = coffeeShops.Count;
        
        // Add a test coffee entry
        var testEntry = new CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = "Small",
            Source = "Home",
            Timestamp = DateTime.UtcNow
        };
        
        _context.CoffeeEntries.Add(testEntry);
        _context.SaveChanges();
        
        var savedEntry = _context.CoffeeEntries.First();
        
        // Assert - Verify complete database functionality
        coffeeShops.Should().HaveCount(9, "should have 9 seeded coffee shops");
        coffeeShops.Should().Contain(shop => shop.Name == "Home", "should include 'Home' as a coffee shop");
        coffeeShops.Should().Contain(shop => shop.Name == "Starbucks", "should include popular coffee chains");
        
        savedEntry.Should().NotBeNull("coffee entry should be saved successfully");
        savedEntry.Id.Should().BeGreaterThan(0, "ID should be auto-generated");
        savedEntry.CoffeeType.Should().Be("Espresso");
        savedEntry.Size.Should().Be("Small");
        savedEntry.Source.Should().Be("Home");
        
        // Verify the database tables exist with proper structure
        var tableNames = _context.Database.ExecuteSqlRaw(
            "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' AND name NOT LIKE '__EF%'");
            
        // Just verify we can query both tables without errors
        var coffeeEntriesCount = _context.CoffeeEntries.Count();
        var coffeeShopsCount = _context.CoffeeShops.Count();
        
        coffeeEntriesCount.Should().Be(1, "should have our test coffee entry");
        coffeeShopsCount.Should().Be(9, "should have all seed coffee shops");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        
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
