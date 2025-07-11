using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoffeeTracker.Api.IntegrationTests;

/// <summary>
/// Base class for API integration tests with common setup
/// </summary>
[Collection("Integration Tests")]
public class ApiIntegrationTestBase
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected const string CookieSessionName = "coffee-session";

    public ApiIntegrationTestBase(CustomWebApplicationFactory factory)
    {
        // Use the custom factory directly
        Factory = factory;
        
        // Don't disable auto-redirect as it can interfere with cookie handling
        Client = Factory.CreateClient();
    }

    /// <summary>
    /// Creates a test coffee entry request
    /// </summary>
    protected static StringContent CreateJsonContent(object content)
    {
        return new StringContent(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            "application/json");
    }
    
    /// <summary>
    /// Extracts the session ID from the HTTP response cookies or headers
    /// </summary>
    protected string? ExtractSessionIdFromResponse(HttpResponseMessage response)
    {
        // First try to get from X-Session-Id header (for testing)
        if (response.Headers.TryGetValues("X-Session-Id", out var sessionHeaders))
        {
            return sessionHeaders.FirstOrDefault();
        }
        
        // Fallback to cookies
        if (!response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            return null;
        }

        foreach (var cookie in cookies)
        {
            if (cookie.StartsWith($"{CookieSessionName}="))
            {
                // Extract the session ID from the cookie string
                var parts = cookie.Split(';');
                var cookiePart = parts[0];
                var sessionId = cookiePart.Substring(CookieSessionName.Length + 1);
                return sessionId;
            }
        }

        return null;
    }

    /// <summary>
    /// Creates a client with a specific session ID
    /// </summary>
    protected HttpClient CreateClientWithSession(string sessionId)
    {
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"{CookieSessionName}={sessionId}");
        return client;
    }
}
