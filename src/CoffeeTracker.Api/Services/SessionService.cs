using System.Security.Cryptography;
using CoffeeTracker.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service for managing anonymous user sessions
/// </summary>
public class SessionService : ISessionService
{
    private const string SessionCookieName = "coffee-session";
    private const int SessionIdLength = 32; // 32 characters
    private readonly CoffeeTrackerDbContext _dbContext;
    private readonly ILogger<SessionService> _logger;
    
    /// <summary>
    /// Initializes a new instance of the SessionService class
    /// </summary>
    /// <param name="dbContext">Database context</param>
    /// <param name="logger">Logger instance</param>
    public SessionService(CoffeeTrackerDbContext dbContext, ILogger<SessionService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets an existing session ID from the HttpContext or creates a new one
    /// </summary>
    /// <param name="context">The current HttpContext</param>
    /// <returns>The session ID</returns>
    public string GetOrCreateSessionId(HttpContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
            
        // Try to get session ID from cookie
        if (context.Request.Cookies.TryGetValue(SessionCookieName, out var existingSessionId) && 
            !string.IsNullOrEmpty(existingSessionId) &&
            IsSessionValid(existingSessionId))
        {
            _logger.LogDebug("Using existing session ID: {SessionId}", existingSessionId);
            return existingSessionId;
        }
        
        // Generate a new session ID
        var newSessionId = GenerateSecureSessionId();
        
        // Set the session cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddHours(24), // 24-hour expiration
            SameSite = SameSiteMode.Lax,
            Secure = context.Request.IsHttps // Use secure cookies for HTTPS requests
        };
        
        context.Response.Cookies.Append(SessionCookieName, newSessionId, cookieOptions);
        
        _logger.LogInformation("Created new session ID: {SessionId}", newSessionId);
        
        return newSessionId;
    }
    
    /// <summary>
    /// Checks if a session ID is valid
    /// </summary>
    /// <param name="sessionId">The session ID to validate</param>
    /// <returns>True if the session is valid, otherwise false</returns>
    public bool IsSessionValid(string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId) || sessionId.Length != SessionIdLength)
        {
            _logger.LogWarning("Invalid session ID format: {SessionId}", sessionId);
            return false;
        }
        
        // In a more complex implementation, we might check against a sessions table
        // For now, we just validate the format
        return true;
    }
    
    /// <summary>
    /// Cleans up expired anonymous session data
    /// </summary>
    /// <param name="expirationPeriod">The period after which data is considered expired</param>
    /// <returns>The number of records cleaned up</returns>
    public async Task<int> CleanupExpiredSessionsAsync(TimeSpan expirationPeriod)
    {
        var cutoffDate = DateTime.UtcNow.Subtract(expirationPeriod);
        _logger.LogInformation("Cleaning up anonymous session data older than {CutoffDate}", cutoffDate);
        
        var expiredEntries = await _dbContext.CoffeeEntries
            .Where(e => e.Timestamp < cutoffDate)
            .ToListAsync();
            
        if (expiredEntries.Any())
        {
            _dbContext.CoffeeEntries.RemoveRange(expiredEntries);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Removed {Count} expired coffee entries", expiredEntries.Count);
            return expiredEntries.Count;
        }
        
        _logger.LogInformation("No expired coffee entries found");
        return 0;
    }
    
    /// <summary>
    /// Generates a cryptographically secure random session ID
    /// </summary>
    /// <returns>A random 32-character alphanumeric string</returns>
    private static string GenerateSecureSessionId()
    {
        // Generate random bytes
        var randomBytes = new byte[16]; // 16 bytes = 32 hex characters
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        
        // Convert bytes to a hex string
        return string.Concat(randomBytes.Select(b => b.ToString("x2")));
    }
}
