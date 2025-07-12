using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.IntegrationTests.Utilities;

/// <summary>
/// Test data builder for coffee entries
/// </summary>
public static class TestDataBuilder
{
    /// <summary>
    /// Creates a coffee entry request with the specified properties
    /// </summary>
    public static CreateCoffeeEntryRequest CreateCoffeeEntryRequest(
        string coffeeType = "Latte", 
        string size = "Medium", 
        string? source = null,
        DateTime? timestamp = null)
    {
        return new CreateCoffeeEntryRequest
        {
            CoffeeType = coffeeType,
            Size = size,
            Source = source,
            Timestamp = timestamp
        };
    }
    
    /// <summary>
    /// Creates a coffee entry model for seeding test data
    /// </summary>
    public static CoffeeEntry CreateCoffeeEntryModel(
        string coffeeType = "Latte",
        string size = "Medium",
        string? source = null,
        DateTime? timestamp = null,
        string sessionId = "test-session-id")
    {
        return new CoffeeEntry
        {
            CoffeeType = coffeeType,
            Size = size,
            Source = source,
            Timestamp = timestamp ?? DateTime.UtcNow,
            SessionId = sessionId
        };
    }
    
    /// <summary>
    /// Creates multiple coffee entries with the same session ID
    /// </summary>
    public static List<CoffeeEntry> CreateMultipleCoffeeEntries(
        int count, 
        string sessionId = "test-session-id", 
        DateTime? baseDate = null)
    {
        var entries = new List<CoffeeEntry>();
        var date = baseDate ?? DateTime.UtcNow;
        
        var coffeeTypes = new[] { "Latte", "Espresso", "Cappuccino", "Americano", "Mocha" };
        var sizes = new[] { "Small", "Medium", "Large" };
        var sources = new[] { "Starbucks", "Costa", "Home", "Office", null };
        
        for (int i = 0; i < count; i++)
        {
            entries.Add(new CoffeeEntry
            {
                CoffeeType = coffeeTypes[i % coffeeTypes.Length],
                Size = sizes[i % sizes.Length],
                Source = sources[i % sources.Length],
                Timestamp = date.AddHours(i),
                SessionId = sessionId
            });
        }
        
        return entries;
    }
}
