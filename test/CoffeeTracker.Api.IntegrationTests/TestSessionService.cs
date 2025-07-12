using CoffeeTracker.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeTracker.Api.IntegrationTests
{
    /// <summary>
    /// Test-specific session service that can use predetermined session IDs for testing
    /// </summary>
    public class TestSessionService : ISessionService
    {
        private readonly ILogger<TestSessionService> _logger;
        private readonly Dictionary<string, string> _cookieToSessionMap = new();

        public TestSessionService(ILogger<TestSessionService> logger)
        {
            _logger = logger;
        }

        public string GetOrCreateSessionId(HttpContext context)
        {
            // Debug logging to understand what's being received
            _logger.LogWarning("TestSessionService: Processing request with {CookieCount} cookies", context.Request.Cookies.Count);
            foreach (var cookie in context.Request.Cookies)
            {
                _logger.LogWarning("TestSessionService: Cookie '{Key}' = '{Value}'", cookie.Key, cookie.Value);
            }
            
            // Debug headers too
            _logger.LogWarning("TestSessionService: Processing request with {HeaderCount} headers", context.Request.Headers.Count);
            if (context.Request.Headers.TryGetValue("X-Session-Id", out var sessionHeaders))
            {
                _logger.LogWarning("TestSessionService: Found X-Session-Id header with values: {Values}", string.Join(", ", sessionHeaders.ToArray()));
            }
            else
            {
                _logger.LogWarning("TestSessionService: No X-Session-Id header found");
            }

            // Check if there's already a session ID set for testing
            if (context.Items.TryGetValue("SessionId", out var sessionIdFromItems) && sessionIdFromItems is string sessionId)
            {
                _logger.LogInformation("Using session ID from test context: {SessionId}", sessionId);
                return sessionId;
            }

            // Check for existing cookie FIRST (this is what tests use)
            if (context.Request.Cookies.TryGetValue("coffee-session", out var cookieSessionId))
            {
                _logger.LogWarning("TestSessionService: Found coffee-session cookie with value '{SessionId}'", cookieSessionId);
                if (!string.IsNullOrEmpty(cookieSessionId) && IsValidSessionId(cookieSessionId))
                {
                    _logger.LogInformation("Using existing session ID from cookie: {SessionId}", cookieSessionId);
                    context.Items["SessionId"] = cookieSessionId;
                    
                    try
                    {
                        context.Response.Headers["X-Session-Id"] = cookieSessionId;
                    }
                    catch (InvalidOperationException)
                    {
                        _logger.LogWarning("Could not set response header - headers already sent");
                    }
                    
                    return cookieSessionId;
                }
                else
                {
                    _logger.LogWarning("TestSessionService: Invalid session ID from cookie: '{SessionId}' (length: {Length})", cookieSessionId, cookieSessionId?.Length ?? 0);
                }
            }
            else
            {
                _logger.LogWarning("TestSessionService: No SessionId cookie found");
            }

            // Check for X-Session-Id header (backup option)
            if (context.Request.Headers.TryGetValue("X-Session-Id", out var headerSessionId))
            {
                var sessionIdValue = headerSessionId.First();
                if (!string.IsNullOrEmpty(sessionIdValue) && IsValidSessionId(sessionIdValue))
                {
                    _logger.LogInformation("Using session ID from X-Session-Id header: {SessionId}", sessionIdValue);
                    context.Items["SessionId"] = sessionIdValue;
                    
                    // Set response header for test verification
                    try
                    {
                        context.Response.Headers["X-Session-Id"] = sessionIdValue;
                    }
                    catch (InvalidOperationException)
                    {
                        _logger.LogWarning("Could not set response header - headers already sent");
                    }
                    
                    return sessionIdValue;
                }
            }

            // Generate new session ID
            var newSessionId = GenerateSessionId();
            _logger.LogInformation("Created new session ID: {SessionId}", newSessionId);
            
            context.Items["SessionId"] = newSessionId;
            
            // Set cookie and header
            try
            {
                context.Response.Cookies.Append("coffee-session", newSessionId, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(30)
                });
                
                context.Response.Headers["X-Session-Id"] = newSessionId;
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning("Could not set cookie/header - headers already sent");
            }

            return newSessionId;
        }

        public bool IsSessionValid(string sessionId)
        {
            return IsValidSessionId(sessionId);
        }

        public Task<int> CleanupExpiredSessionsAsync(TimeSpan expirationPeriod)
        {
            // In test environment, we don't need to clean up sessions
            return Task.FromResult(0);
        }

        private static string GenerateSessionId()
        {
            return Guid.NewGuid().ToString("N"); // 32 character string
        }

        private static bool IsValidSessionId(string sessionId)
        {
            // More flexible validation for testing - allow test session IDs of various lengths
            return !string.IsNullOrEmpty(sessionId) && sessionId.Length >= 8 && 
                   sessionId.All(c => char.IsAsciiLetterOrDigit(c) || c == '-');
        }
    }
}
