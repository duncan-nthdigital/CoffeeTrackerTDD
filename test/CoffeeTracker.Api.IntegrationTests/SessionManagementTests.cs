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
    public async Task NewRequest_WithoutSessionCookie_GeneratesNewSession()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/coffee-entries");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var sessionId = ExtractSessionIdFromResponse(response);
        sessionId.Should().NotBeNull().And.NotBeEmpty();
    }
    
    [Fact]
    public async Task SubsequentRequests_WithSessionCookie_MaintainsSession()
    {
        // Arrange - Make initial request to get a session
        var initialResponse = await Client.GetAsync("/api/coffee-entries");
        initialResponse.EnsureSuccessStatusCode();
        
        var sessionId = ExtractSessionIdFromResponse(initialResponse);
        sessionId.Should().NotBeNull().And.NotBeEmpty();
        
        // Create a client with this session cookie
        var clientWithSession = CreateClientWithSession(sessionId!);
        
        // Create a coffee entry with this session
        var coffeeRequest = TestDataBuilder.CreateCoffeeEntryRequest(
            coffeeType: "Session Test Coffee",
            size: "Medium"
        );
        
        var createResponse = await clientWithSession.PostAsync("/api/coffee-entries", CreateJsonContent(coffeeRequest));
        createResponse.EnsureSuccessStatusCode();
        
        var entry = await createResponse.Content.ReadFromJsonAsync<CoffeeEntryResponse>(JsonOptions);
        entry.Should().NotBeNull();
        
        // Act - Make another request with the same client (same session)
        var subsequentResponse = await clientWithSession.GetAsync("/api/coffee-entries");
        subsequentResponse.EnsureSuccessStatusCode();
        
        // Assert
        var entries = await subsequentResponse.Content.ReadFromJsonAsync<List<CoffeeEntryResponse>>(JsonOptions);
        entries.Should().NotBeNull();
        entries.Should().HaveCount(1);
        entries![0].CoffeeType.Should().Be("Session Test Coffee");
        
        // Verify in database that entry has the expected session ID
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
        var savedEntry = await dbContext.CoffeeEntries.FindAsync(entry!.Id);
        savedEntry.Should().NotBeNull();
        savedEntry!.SessionId.Should().Be(sessionId);
    }
    
    [Fact]
    public async Task DifferentClients_GetDifferentSessions()
    {
        // Arrange & Act
        var response1 = await Client.GetAsync("/api/coffee-entries");
        var client2 = Factory.CreateClient();
        var response2 = await client2.GetAsync("/api/coffee-entries");
        
        // Assert
        var sessionId1 = ExtractSessionIdFromResponse(response1);
        var sessionId2 = ExtractSessionIdFromResponse(response2);
        
        sessionId1.Should().NotBeNull();
        sessionId2.Should().NotBeNull();
        sessionId1.Should().NotBe(sessionId2);
    }
    
    [Fact]
    public async Task ExpiredSession_CookieHasCorrectExpiry()
    {
        // Arrange & Act
        var initialResponse = await Client.GetAsync("/api/coffee-entries");
        initialResponse.EnsureSuccessStatusCode();
        
        // Assert
        var cookies = initialResponse.Headers.GetValues("Set-Cookie");
        var sessionCookie = cookies.First(c => c.StartsWith($"{CookieSessionName}="));
        
        // Check for Expires attribute in cookie
        sessionCookie.Should().Contain("Expires=");
        
        // Parse expiry time
        var expiryPart = sessionCookie.Split(';')
            .FirstOrDefault(p => p.Trim().StartsWith("Expires="))?.Trim();
        expiryPart.Should().NotBeNull();
        
        if (DateTime.TryParse(expiryPart!.Substring("Expires=".Length), out var expiryDate))
        {
            // Verify expiry is about 24 hours in the future
            var expectedExpiry = DateTime.UtcNow.AddHours(24);
            expiryDate.Should().BeCloseTo(expectedExpiry, TimeSpan.FromMinutes(5));
        }
        else
        {
            Assert.Fail("Could not parse cookie expiry date");
        }
    }
}
