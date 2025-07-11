using Microsoft.AspNetCore.Http;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service interface for managing anonymous user sessions
/// </summary>
public interface ISessionService
{
    /// <summary>
    /// Gets an existing session ID from the HttpContext or creates a new one
    /// </summary>
    /// <param name="context">The current HttpContext</param>
    /// <returns>The session ID</returns>
    string GetOrCreateSessionId(HttpContext context);
    
    /// <summary>
    /// Checks if a session ID is valid
    /// </summary>
    /// <param name="sessionId">The session ID to validate</param>
    /// <returns>True if the session is valid, otherwise false</returns>
    bool IsSessionValid(string sessionId);
    
    /// <summary>
    /// Cleans up expired anonymous session data
    /// </summary>
    /// <returns>The number of records cleaned up</returns>
    Task<int> CleanupExpiredSessionsAsync(TimeSpan expirationPeriod);
}
