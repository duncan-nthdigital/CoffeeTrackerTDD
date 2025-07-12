using System.Diagnostics;
using System.Net.Http.Json;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.IntegrationTests.Utilities;
using CoffeeTracker.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoffeeTracker.Api.IntegrationTests;

/// <summary>
/// Performance tests for API endpoints
/// </summary>
public class PerformanceTests : ApiIntegrationTestBase
{
    private const int PerformanceThresholdMs = 500; // 500 milliseconds maximum response time
    
    public PerformanceTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateCoffeeEntry_CompletesWithin500Milliseconds()
    {
        // Arrange
        var request = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Latte",
            size: "Medium",
            source: "Starbucks"
        );
        
        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await Client.PostAsync("/api/coffee-entries", CreateJsonContent(request));
        stopwatch.Stop();
        
        // Assert
        response.EnsureSuccessStatusCode();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(PerformanceThresholdMs);
    }
    
    [Fact]
    public async Task GetCoffeeEntries_WithLargeDataset_CompletesWithin500Milliseconds()
    {
        // Arrange
        const int entryCount = 100;
        var sessionId = CreateTestSessionId("perf");
        var client = CreateClientWithSession(sessionId);
        
        // Array of valid coffee types
        var validCoffeeTypes = new[] { "Espresso", "Americano", "Latte", "Cappuccino", "Mocha" };
        
        // Create a large dataset in the database
        using (var scope = Factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
            var testEntries = new List<CoffeeEntry>();
            
            // Create entries for today
            for (int i = 0; i < entryCount; i++)
            {
                testEntries.Add(new CoffeeEntry
                {
                    CoffeeType = validCoffeeTypes[i % validCoffeeTypes.Length],
                    Size = i % 3 == 0 ? "Small" : i % 3 == 1 ? "Medium" : "Large",
                    Source = i % 4 == 0 ? "Home" : $"Coffee Shop {i % 4}",
                    SessionId = sessionId,
                    Timestamp = DateTime.UtcNow.AddMinutes(-i) // Spread throughout today
                });
            }
            
            dbContext.CoffeeEntries.AddRange(testEntries);
            await dbContext.SaveChangesAsync();
        }
        
        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/api/coffee-entries");
        stopwatch.Stop();
        
        // Assert
        response.EnsureSuccessStatusCode();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(PerformanceThresholdMs);
        
        var entries = await response.Content.ReadFromJsonAsync<List<CoffeeEntryResponse>>(JsonOptions);
        entries.Should().NotBeNull();
        entries.Should().HaveCount(entryCount);
    }
    
    [Fact]
    public async Task ConcurrentRequests_HandleMultipleSessionsCorrectly()
    {
        // Arrange
        const int concurrentSessions = 10;
        var tasks = new List<Task<HttpResponseMessage>>();
        var sessionIds = new List<string>();
        
        // Create multiple clients with different session IDs
        // Array of valid coffee types
        var validCoffeeTypes = new[] { "Espresso", "Americano", "Latte", "Cappuccino", "Mocha", "Macchiato", "FlatWhite", "BlackCoffee" };
        
        for (int i = 0; i < concurrentSessions; i++)
        {
            var sessionId = CreateTestSessionId($"conc-{i}");
            sessionIds.Add(sessionId);
            var client = CreateClientWithSession(sessionId);
            
            var request = TestDataBuilder.CreateCoffeeEntryRequest(
                coffeeType: validCoffeeTypes[i % validCoffeeTypes.Length],
                size: i % 3 == 0 ? "Small" : i % 3 == 1 ? "Medium" : "Large",
                source: $"Shop {i}"
            );
            
            // Add task to post a coffee entry
            tasks.Add(client.PostAsync("/api/coffee-entries", CreateJsonContent(request)));
        }
        
        // Act
        var stopwatch = Stopwatch.StartNew();
        var results = await Task.WhenAll(tasks);
        stopwatch.Stop();
        
        // Assert
        // Calculate average time per request
        var averageTimePerRequest = stopwatch.ElapsedMilliseconds / (double)concurrentSessions;
        averageTimePerRequest.Should().BeLessThan(PerformanceThresholdMs);
        
        // Check that all responses were successful
        foreach (var response in results)
        {
            response.EnsureSuccessStatusCode();
        }
        
        // Verify each session got the correct entry in database
        using (var scope = Factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
            var entries = await dbContext.CoffeeEntries.ToListAsync();
            
            entries.Should().HaveCount(concurrentSessions);
            
            for (int i = 0; i < concurrentSessions; i++)
            {
                var sessionId = sessionIds[i];
                var entry = entries.FirstOrDefault(e => e.SessionId == sessionId);
                entry.Should().NotBeNull($"Entry for session {sessionId} should exist");
                entry!.CoffeeType.Should().Be(validCoffeeTypes[i % validCoffeeTypes.Length]);
                entry.Source.Should().Be($"Shop {i}");
            }
        }
    }
}
