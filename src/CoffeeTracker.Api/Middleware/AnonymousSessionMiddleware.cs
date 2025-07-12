using CoffeeTracker.Api.Services;

namespace CoffeeTracker.Api.Middleware;

/// <summary>
/// Middleware that handles anonymous session management
/// </summary>
public class AnonymousSessionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AnonymousSessionMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the AnonymousSessionMiddleware class
    /// </summary>
    /// <param name="next">Next middleware in the pipeline</param>
    /// <param name="logger">Logger instance</param>
    public AnonymousSessionMiddleware(RequestDelegate next, ILogger<AnonymousSessionMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Process an HTTP request
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="sessionService">The session service</param>
    public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
    {
        if (sessionService == null)
            throw new ArgumentNullException(nameof(sessionService));

        _logger.LogDebug("Anonymous session middleware processing request");

        try
        {
            // Get or create a session ID for this request
            var sessionId = sessionService.GetOrCreateSessionId(context);
            
            // Store the session ID in the HTTP context items for use in controllers/services
            context.Items["SessionId"] = sessionId;
            
            _logger.LogDebug("Session ID {SessionId} set in HTTP context items", sessionId);
        }
        catch (Exception ex)
        {
            // Log the error but continue processing the request
            _logger.LogError(ex, "Error in anonymous session middleware");
        }

        // Continue processing the request
        await _next(context);
    }
}

/// <summary>
/// Extension methods for the AnonymousSessionMiddleware
/// </summary>
public static class AnonymousSessionMiddlewareExtensions
{
    /// <summary>
    /// Adds the anonymous session middleware to the request pipeline
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder</returns>
    public static IApplicationBuilder UseAnonymousSession(this IApplicationBuilder app)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));
            
        return app.UseMiddleware<AnonymousSessionMiddleware>();
    }
}
