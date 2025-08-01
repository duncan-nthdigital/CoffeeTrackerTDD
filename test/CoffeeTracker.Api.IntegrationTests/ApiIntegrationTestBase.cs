using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Net;
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
        
        // Clear database before each test
        Factory.ClearDatabase();
    }

    /// <summary>
    /// Creates a valid test session ID (32 characters)
    /// </summary>
    protected static string CreateTestSessionId(string suffix)
    {
        // Create a 32-character session ID by padding the suffix
        var sessionId = $"test-session-{suffix}";
        if (sessionId.Length > 32)
        {
            sessionId = sessionId.Substring(0, 32);
        }
        else if (sessionId.Length < 32)
        {
            sessionId = sessionId.PadRight(32, '0');
        }
        return sessionId;
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
    /// Extracts the session ID from the HTTP response cookies
    /// </summary>
    protected string? ExtractSessionIdFromResponse(HttpResponseMessage response)
    {
        // First check if we can get it from the cookie container
        // We'll need to modify the base Client to use a CookieContainer
        
        // For now, let's try a simpler approach - get it from Set-Cookie headers
        if (response.Headers.TryGetValues("Set-Cookie", out var headerCookies))
        {
            foreach (var cookie in headerCookies)
            {
                if (cookie.StartsWith($"{CookieSessionName}="))
                {
                    var parts = cookie.Split(';');
                    var cookiePart = parts[0];
                    var sessionId = cookiePart.Substring(CookieSessionName.Length + 1);
                    return sessionId;
                }
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
        // Use only cookies for session management
        client.DefaultRequestHeaders.Add("Cookie", $"{CookieSessionName}={sessionId}");
        return client;
    }
    
    /// <summary>
    /// Creates a client with proper cookie support for session management
    /// </summary>
    protected HttpClient CreateClientWithCookieSupport()
    {
        var cookieContainer = new CookieContainer();
        var handler = new HttpClientHandler()
        {
            CookieContainer = cookieContainer,
            UseCookies = true
        };
        
        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };
        
        // Wire up to the test server
        var testServer = Factory.Server;
        client.BaseAddress = testServer.BaseAddress;
        
        return client;
    }
}
