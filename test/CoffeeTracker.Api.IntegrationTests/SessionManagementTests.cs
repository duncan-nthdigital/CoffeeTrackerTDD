using System.Net.Http.Json;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.IntegrationTests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoffeeTracker.Api.IntegrationTests;

/// <summary>
/// Integration tests for session management
/// </summary>
public class SessionManagementTests : ApiIntegrationTestBase
{
    public SessionManagementTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task NewRequest_WithoutSessionCookie_CreatesNewSessionAndWorks()
    {
        // Arrange & Act - Test that requests without session cookies work
        var response = await Client.GetAsync("/api/coffee-entries");
        
        // Assert - Should work (creates new session internally)
        response.EnsureSuccessStatusCode();
        var coffeeEntries = await response.Content.ReadFromJsonAsync<CoffeeEntryResponse[]>();
        coffeeEntries.Should().NotBeNull().And.BeEmpty();
    }
    
    [Fact]
    public async Task SubsequentRequests_WithSameSessionId_MaintainSessionData()
    {
        // Arrange - Use a predefined session ID for both requests
        var sessionId = CreateTestSessionId("same-session-data-test00000");
        var client = CreateClientWithSession(sessionId);
        
        // Act 1 - Create a coffee entry
        var createRequest = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Latte",
            size: "Medium"
        );
        var createResponse = await client.PostAsJsonAsync("/api/coffee-entries", createRequest);
        
        // Assert - Entry created successfully
        createResponse.EnsureSuccessStatusCode();
        var createdEntry = await createResponse.Content.ReadFromJsonAsync<CoffeeEntryResponse>();
        createdEntry.Should().NotBeNull();
        
        // Act 2 - Create a new client with the same session ID
        var client2 = CreateClientWithSession(sessionId);
        var getResponse = await client2.GetAsync("/api/coffee-entries");
        
        // Assert - Should find the created entry (same session)
        getResponse.EnsureSuccessStatusCode();
        var retrievedEntries = await getResponse.Content.ReadFromJsonAsync<CoffeeEntryResponse[]>();
        retrievedEntries.Should().NotBeNull().And.HaveCount(1);
        retrievedEntries![0].Should().BeEquivalentTo(createdEntry, options => options.Excluding(e => e.Id));
    }
    
    [Fact]
    public async Task DifferentSessionIds_IsolateSessionData()
    {
        // Arrange - Create clients with different predefined session IDs
        var sessionId1 = CreateTestSessionId("session-isolation-1000000000000");
        var sessionId2 = CreateTestSessionId("session-isolation-2000000000000");
        var client1 = CreateClientWithSession(sessionId1);
        var client2 = CreateClientWithSession(sessionId2);
        
        // Act 1 - Create a coffee entry with client 1
        var request1 = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Latte",
            size: "Medium"
        );
        var response1 = await client1.PostAsJsonAsync("/api/coffee-entries", request1);
        response1.EnsureSuccessStatusCode();
        
        // Act 2 - Create a different coffee entry with client 2
        var request2 = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Espresso",
            size: "Small"
        );
        var response2 = await client2.PostAsJsonAsync("/api/coffee-entries", request2);
        response2.EnsureSuccessStatusCode();
        
        // Act 3 - Get entries for each session
        var getResponse1 = await client1.GetAsync("/api/coffee-entries");
        var getResponse2 = await client2.GetAsync("/api/coffee-entries");
        
        // Assert - Each session should only see their own data
        getResponse1.EnsureSuccessStatusCode();
        getResponse2.EnsureSuccessStatusCode();
        
        var entries1 = await getResponse1.Content.ReadFromJsonAsync<CoffeeEntryResponse[]>();
        var entries2 = await getResponse2.Content.ReadFromJsonAsync<CoffeeEntryResponse[]>();
        
        entries1.Should().NotBeNull().And.HaveCount(1);
        entries2.Should().NotBeNull().And.HaveCount(1);
        
        entries1![0].CoffeeType.Should().Be("Latte");
        entries2![0].CoffeeType.Should().Be("Espresso");
    }
    
    [Fact]
    public async Task PredefinedSessionId_WorksWithSessionCookie()
    {
        // Arrange - Create a client with a predefined session ID
        var predefinedSessionId = CreateTestSessionId("predefined-1234567890");
        var client = CreateClientWithSession(predefinedSessionId);
        
        // Act 1 - Create a coffee entry
        var createRequest = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Americano",
            size: "Large"
        );
        var createResponse = await client.PostAsJsonAsync("/api/coffee-entries", createRequest);
        
        // Assert - Entry created successfully
        createResponse.EnsureSuccessStatusCode();
        var createdEntry = await createResponse.Content.ReadFromJsonAsync<CoffeeEntryResponse>();
        createdEntry.Should().NotBeNull();
        
        // Act 2 - Verify in database that the entry has the expected session ID
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
        var savedEntry = await dbContext.CoffeeEntries.FindAsync(createdEntry!.Id);
        
        // Assert - Entry should be saved with the predefined session ID
        savedEntry.Should().NotBeNull();
        savedEntry!.SessionId.Should().Be(predefinedSessionId);
        savedEntry.CoffeeType.Should().Be("Americano");
    }
}
