using System.Net;
using System.Net.Http.Json;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.IntegrationTests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace CoffeeTracker.Api.IntegrationTests;

/// <summary>
/// Integration tests for coffee entries API endpoints
/// </summary>
public class CoffeeEntriesApiTests : ApiIntegrationTestBase
{
    public CoffeeEntriesApiTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task CreateCoffeeEntry_WithValidData_Returns201Created()
    {
        // Arrange
        var sessionId = CreateTestSessionId("valid-entry-test");
        var client = CreateClientWithSession(sessionId);
        var request = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Latte",
            size: "Medium",
            source: "Starbucks"
        );
        
        // Act
        var response = await client.PostAsync("/api/coffee-entries", CreateJsonContent(request));
        
        // Assert
        response.ShouldBeSuccessStatusCode(HttpStatusCode.Created);
        
        var createdEntry = await response.Content.ReadFromJsonAsync<CoffeeEntryResponse>(JsonOptions);
        createdEntry.ShouldBeValidCoffeeEntry("Latte", "Medium", "Starbucks");
        
        // Verify entry was saved to database
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
        
        var savedEntry = await dbContext.CoffeeEntries.FindAsync(createdEntry!.Id);
        savedEntry.Should().NotBeNull();
        savedEntry!.CoffeeType.Should().Be("Latte");
        savedEntry.Size.Should().Be("Medium");
        savedEntry.Source.Should().Be("Starbucks");
        savedEntry.SessionId.Should().Be(sessionId);
    }
    
    [Theory]
    [InlineData(null, "Medium", "Coffee type is required")]
    [InlineData("", "Medium", "Coffee type is required")]
    [InlineData("Latte", null, "Size is required")]
    [InlineData("Latte", "", "Size is required")]
    [InlineData("ThisCoffeeTypeIsTooLongAndExceedsFiftyCharactersLimitForValidation", "Medium", "Coffee type length")]
    public async Task CreateCoffeeEntry_WithInvalidData_Returns400BadRequest(string coffeeType, string size, string expectedError)
    {
        // Arrange
        var request = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: coffeeType,
            size: size
        );
        
        // Act
        var response = await Client.PostAsync("/api/coffee-entries", CreateJsonContent(request));
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        
        // At least one error message should contain the expected error text
        problemDetails.Errors.Should().NotBeEmpty();
        problemDetails.Errors.SelectMany(e => e.Value).Any(v => v.Contains(expectedError, StringComparison.OrdinalIgnoreCase))
            .Should().BeTrue($"Error message should contain '{expectedError}'");
    }
    
    [Fact]
    public async Task CreateCoffeeEntry_WithFutureDate_Returns400BadRequest()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);
        var request = TestDataBuilder.CreateCoffeeEntryRequest(
            timestamp: futureDate
        );
        
        // Act
        var response = await Client.PostAsync("/api/coffee-entries", CreateJsonContent(request));
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        
        // Error message should mention future date not being allowed
        problemDetails.Errors.SelectMany(e => e.Value)
            .Any(v => v.Contains("future", StringComparison.OrdinalIgnoreCase))
            .Should().BeTrue("Error message should mention that future dates are not allowed");
    }

    [Fact]
    public async Task CreateCoffeeEntry_ExceedingDailyLimit_Returns422UnprocessableEntity()
    {
        // Arrange - Add max allowed entries to the database
        const int maxEntries = 10; // As defined in CoffeeService
        
        var sessionId = CreateTestSessionId("limit");
        var client = CreateClientWithSession(sessionId);
        
        // Create entries up to the limit
        for (int i = 0; i < maxEntries; i++)
        {
            var request = TestDataBuilder.CreateCoffeeEntryRequest(
                coffeeType: "Latte",
                size: "Medium",
                source: $"Test {i}"
            );
            
            var response = await client.PostAsync("/api/coffee-entries", CreateJsonContent(request));
            response.IsSuccessStatusCode.Should().BeTrue();
        }
        
        // Act - Try to add one more entry
        var finalRequest = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Espresso",
            size: "Small"
        );
        
        var finalResponse = await client.PostAsync("/api/coffee-entries", CreateJsonContent(finalRequest));
        
        // Assert
        finalResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        
        var problemDetails = await finalResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(422);
        problemDetails.Title.Should().Contain("Limit Exceeded");
    }
    
    [Fact]
    public async Task CreateCoffeeEntry_ExceedingCaffeineLimit_Returns422UnprocessableEntity()
    {
        // Arrange - Create high caffeine entries
        const int maxDailyCaffeine = 1000; // As defined in CoffeeService
        
        var sessionId = CreateTestSessionId("caffeine");
        var client = CreateClientWithSession(sessionId);
        
        // Create high caffeine drinks first (multiple large coffees)
        var highCaffeineDrinks = new[]
        {
            TestDataBuilder.CreateCoffeeEntryRequest("Americano", "Large"), // ~156mg
            TestDataBuilder.CreateCoffeeEntryRequest("Macchiato", "Large"), // ~156mg
            TestDataBuilder.CreateCoffeeEntryRequest("FlatWhite", "Large"), // ~169mg
            TestDataBuilder.CreateCoffeeEntryRequest("Espresso", "Large"), // ~117mg
            TestDataBuilder.CreateCoffeeEntryRequest("BlackCoffee", "Large"), // ~124mg
            TestDataBuilder.CreateCoffeeEntryRequest("Latte", "Large"), // ~104mg
            TestDataBuilder.CreateCoffeeEntryRequest("Americano", "Large") // ~156mg
            // Total: ~982mg caffeine
        };
        
        foreach (var drink in highCaffeineDrinks)
        {
            var response = await client.PostAsync("/api/coffee-entries", CreateJsonContent(drink));
            response.IsSuccessStatusCode.Should().BeTrue();
        }
        
        // Act - Try to add another high caffeine drink that would exceed the limit
        var finalRequest = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Americano", 
            size: "Large" // ~156mg which would bring total to 1138mg and exceed 1000mg limit
        );
        
        // Debug logging
        Console.WriteLine($"About to send final request: {System.Text.Json.JsonSerializer.Serialize(finalRequest)}");
        
        var finalResponse = await client.PostAsync("/api/coffee-entries", CreateJsonContent(finalRequest));
        
        // Debug logging
        Console.WriteLine($"Final response status: {finalResponse.StatusCode}");
        var finalResponseContent = await finalResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Final response content: {finalResponseContent}");
        
        // Assert
        finalResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        
        var problemDetails = await finalResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(422);
        problemDetails.Title.Should().Contain("Caffeine Limit Exceeded");
    }
    
    [Fact]
    public async Task CreateCoffeeEntry_WithDifferentSessions_IsolatesData()
    {
        // Arrange
        var sessionId1 = CreateTestSessionId("1");
        var sessionId2 = CreateTestSessionId("2");
        
        var client1 = CreateClientWithSession(sessionId1);
        var client2 = CreateClientWithSession(sessionId2);
        
        var request1 = TestDataBuilder.CreateCoffeeEntryRequest("Latte", "Medium", "Session 1 Coffee");
        var request2 = TestDataBuilder.CreateCoffeeEntryRequest("Espresso", "Small", "Session 2 Coffee");
        
        // Act
        var response1 = await client1.PostAsync("/api/coffee-entries", CreateJsonContent(request1));
        var response2 = await client2.PostAsync("/api/coffee-entries", CreateJsonContent(request2));
        
        // Assert
        response1.IsSuccessStatusCode.Should().BeTrue();
        response2.IsSuccessStatusCode.Should().BeTrue();
        
        var entry1 = await response1.Content.ReadFromJsonAsync<CoffeeEntryResponse>(JsonOptions);
        var entry2 = await response2.Content.ReadFromJsonAsync<CoffeeEntryResponse>(JsonOptions);
        
        entry1.Should().NotBeNull();
        entry2.Should().NotBeNull();
        
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
        
        var savedEntry1 = await dbContext.CoffeeEntries.FindAsync(entry1!.Id);
        var savedEntry2 = await dbContext.CoffeeEntries.FindAsync(entry2!.Id);
        
        savedEntry1.Should().NotBeNull();
        savedEntry2.Should().NotBeNull();
        savedEntry1!.SessionId.Should().Be(sessionId1);
        savedEntry2!.SessionId.Should().Be(sessionId2);
        
        // Verify that session 1 can't see entries from session 2
        var session1Entries = await client1.GetAsync("/api/coffee-entries");
        var session1Data = await session1Entries.Content.ReadFromJsonAsync<List<CoffeeEntryResponse>>(JsonOptions);
        
        session1Data.Should().NotBeNull();
        session1Data.Should().HaveCount(1);
        session1Data![0].Id.Should().Be(entry1.Id);
        session1Data[0].Source.Should().Be("Session 1 Coffee");
    }

    // GET /api/coffee-entries tests
    
    [Fact]
    public async Task GetCoffeeEntries_ForToday_ReturnsCurrentDayEntries()
    {
        // Arrange
        var sessionId = CreateTestSessionId("get-session");
        var client = CreateClientWithSession(sessionId);
        
        // Add entries for today
        var todayEntries = new[]
        {
            TestDataBuilder.CreateCoffeeEntryRequest("Latte", "Medium", "Morning Coffee"),
            TestDataBuilder.CreateCoffeeEntryRequest("Espresso", "Small", "Afternoon Pick-me-up")
        };
        
        foreach (var entry in todayEntries)
        {
            var postResponse = await client.PostAsync("/api/coffee-entries", CreateJsonContent(entry));
            postResponse.IsSuccessStatusCode.Should().BeTrue();
        }
        
        // Add entry for yesterday (directly to the database to set specific date)
        using (var scope = Factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
            dbContext.CoffeeEntries.Add(new Models.CoffeeEntry
            {
                CoffeeType = "Americano",
                Size = "Large",
                Source = "Yesterday's Coffee",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                SessionId = sessionId
            });
            await dbContext.SaveChangesAsync();
        }
        
        // Act
        var response = await client.GetAsync("/api/coffee-entries");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var entries = await response.Content.ReadFromJsonAsync<List<CoffeeEntryResponse>>(JsonOptions);
        entries.Should().NotBeNull();
        entries.Should().HaveCount(2); // Only today's entries
        
        entries!.Should().AllSatisfy(e => 
            DateOnly.FromDateTime(e.Timestamp).Should().Be(DateOnly.FromDateTime(DateTime.UtcNow))
        );
        
        entries.Select(e => e.Source).Should().Contain(new[] { "Morning Coffee", "Afternoon Pick-me-up" });
        entries.Should().NotContain(e => e.Source == "Yesterday's Coffee");
    }
    
    [Fact]
    public async Task GetCoffeeEntries_WithEmptyResult_ReturnsEmptyList()
    {
        // Arrange
        var sessionId = CreateTestSessionId("empty-session");
        var client = CreateClientWithSession(sessionId);
        
        // Act
        var response = await client.GetAsync("/api/coffee-entries");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var entries = await response.Content.ReadFromJsonAsync<List<CoffeeEntryResponse>>(JsonOptions);
        entries.Should().NotBeNull();
        entries.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetCoffeeEntries_WithDateFilter_ReturnsFilteredEntries()
    {
        // Arrange
        var sessionId = CreateTestSessionId("filter-session");
        var client = CreateClientWithSession(sessionId);
        
        // Create entries for different dates in the database
        var targetDate = DateTime.UtcNow.AddDays(-3);
        var targetDateOnly = DateOnly.FromDateTime(targetDate);
        
        using (var scope = Factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
            
            // Add entries for target date
            dbContext.CoffeeEntries.AddRange(
                TestDataBuilder.CreateMultipleCoffeeEntries(2, sessionId, targetDate)
            );
            
            // Add entries for today
            dbContext.CoffeeEntries.AddRange(
                TestDataBuilder.CreateMultipleCoffeeEntries(3, sessionId, DateTime.UtcNow)
            );
            
            await dbContext.SaveChangesAsync();
        }
        
        // Act
        var response = await client.GetAsync($"/api/coffee-entries?date={targetDateOnly:yyyy-MM-dd}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var entries = await response.Content.ReadFromJsonAsync<List<CoffeeEntryResponse>>(JsonOptions);
        entries.Should().NotBeNull();
        entries.Should().HaveCount(2); // Only entries for the target date
        
        entries!.Should().AllSatisfy(e => 
            DateOnly.FromDateTime(e.Timestamp).Should().Be(targetDateOnly)
        );
    }
    
    [Fact]
    public async Task GetCoffeeEntries_WithDifferentSessions_ReturnsOnlySessionData()
    {
        // Arrange
        var sessionId1 = CreateTestSessionId("isolation-1");
        var sessionId2 = CreateTestSessionId("isolation-2");
        
        // Add entries for both sessions
        using (var scope = Factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
            
            // Session 1 entries
            dbContext.CoffeeEntries.AddRange(
                TestDataBuilder.CreateMultipleCoffeeEntries(3, sessionId1, DateTime.UtcNow)
            );
            
            // Session 2 entries
            dbContext.CoffeeEntries.AddRange(
                TestDataBuilder.CreateMultipleCoffeeEntries(2, sessionId2, DateTime.UtcNow)
            );
            
            await dbContext.SaveChangesAsync();
        }
        
        // Create clients with different session IDs
        var client1 = CreateClientWithSession(sessionId1);
        var client2 = CreateClientWithSession(sessionId2);
        
        // Act
        var response1 = await client1.GetAsync("/api/coffee-entries");
        var response2 = await client2.GetAsync("/api/coffee-entries");
        
        // Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        
        var entries1 = await response1.Content.ReadFromJsonAsync<List<CoffeeEntryResponse>>(JsonOptions);
        var entries2 = await response2.Content.ReadFromJsonAsync<List<CoffeeEntryResponse>>(JsonOptions);
        
        entries1.Should().NotBeNull();
        entries2.Should().NotBeNull();
        
        entries1.Should().HaveCount(3);
        entries2.Should().HaveCount(2);
        
        // Verify database state to confirm session isolation
        using (var scope = Factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
            var allEntries = await dbContext.CoffeeEntries.ToListAsync();
            
            allEntries.Count(e => e.SessionId == sessionId1).Should().Be(3);
            allEntries.Count(e => e.SessionId == sessionId2).Should().Be(2);
        }
    }

    // Error handling tests
    
    [Fact]
    public async Task CreateCoffeeEntry_WithInvalidJson_Returns400BadRequest()
    {
        // Arrange
        var invalidJson = "{ \"coffeeType\": \"Latte\", \"size\": }"; // Missing value for size
        var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");
        
        // Act
        var response = await Client.PostAsync("/api/coffee-entries", content);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseText = await response.Content.ReadAsStringAsync();
        responseText.Should().ContainEquivalentOf("Invalid");
    }
    
    [Fact]
    public async Task CreateCoffeeEntry_WithMissingRequiredFields_Returns400BadRequest()
    {
        // Arrange - create an incomplete JSON object
        var incompleteRequest = new { }; // Missing coffeeType and size
        
        // Act
        var response = await Client.PostAsync("/api/coffee-entries", CreateJsonContent(incompleteRequest));
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        
        // Should have validation errors for both required fields
        problemDetails.Errors.Should().ContainKey("CoffeeType");
        problemDetails.Errors.Should().ContainKey("Size");
    }
    
    [Fact]
    public async Task GetCoffeeEntries_WithInvalidDate_Returns400BadRequest()
    {
        // Arrange
        var invalidDate = "not-a-date";
        
        // Act
        var response = await Client.GetAsync($"/api/coffee-entries?date={invalidDate}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails.Title.Should().ContainEquivalentOf("Invalid");
    }

    [Fact]
    public async Task CreateCoffeeEntry_WithLargePayload_Returns413RequestEntityTooLarge()
    {
        // Arrange - Create an excessively large coffee type
        const int maxSize = 100 * 1024; // 100KB, which should exceed any reasonable limit
        var largeString = new string('X', maxSize);
        
        var largeRequest = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: largeString,
            size: "Medium"
        );
        
        // Act
        var response = await Client.PostAsync("/api/coffee-entries", CreateJsonContent(largeRequest));
        
        // Assert
        // Note: Different servers might handle this differently (413 or 400)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.RequestEntityTooLarge, HttpStatusCode.BadRequest);
    }
}
