using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service for managing coffee entries - delegates to CoffeeService for business logic
/// </summary>
public class CoffeeEntryService : ICoffeeEntryService
{
    private readonly ICoffeeService _coffeeService;
    private readonly ILogger<CoffeeEntryService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the CoffeeEntryService (primary constructor for DI)
    /// </summary>
    /// <param name="coffeeService">Coffee service for business logic</param>
    /// <param name="logger">Logger instance</param>
    /// <param name="httpContextAccessor">HTTP context accessor for session management</param>
    public CoffeeEntryService(
        ICoffeeService coffeeService,
        ILogger<CoffeeEntryService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _coffeeService = coffeeService ?? throw new ArgumentNullException(nameof(coffeeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    


    /// <summary>
    /// Creates a new coffee entry - delegates to CoffeeService
    /// </summary>
    /// <param name="request">The coffee entry creation request</param>
    /// <returns>The created coffee entry response</returns>
    public async Task<CoffeeEntryResponse> CreateCoffeeEntryAsync(CreateCoffeeEntryRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        // Get the session ID from the HTTP context
        var sessionId = GetSessionId();
        if (string.IsNullOrEmpty(sessionId))
        {
            _logger.LogError("No session ID available for creating coffee entry");
            throw new InvalidOperationException("No session ID available");
        }

        _logger.LogInformation("CoffeeEntryService delegating to CoffeeService for creating coffee entry: {CoffeeType} {Size}", 
            request.CoffeeType, request.Size);

        // Delegate to the comprehensive CoffeeService which handles all business logic
        return await _coffeeService.CreateCoffeeEntryAsync(request, sessionId);
    }

    /// <summary>
    /// Gets coffee entries for a specific date - delegates to CoffeeService
    /// </summary>
    /// <param name="date">The date to filter entries (optional, defaults to today)</param>
    /// <returns>List of coffee entries for the specified date</returns>
    public async Task<IEnumerable<CoffeeEntryResponse>> GetCoffeeEntriesAsync(DateOnly? date = null)
    {
        // Get the session ID from the HTTP context
        var sessionId = GetSessionId();
        if (string.IsNullOrEmpty(sessionId))
        {
            _logger.LogError("No session ID available for getting coffee entries");
            throw new InvalidOperationException("No session ID available");
        }

        // Convert DateOnly to DateTime if provided
        DateTime? dateTime = date?.ToDateTime(TimeOnly.MinValue);

        _logger.LogInformation("CoffeeEntryService delegating to CoffeeService for getting coffee entries for date: {Date}", 
            date ?? DateOnly.FromDateTime(DateTime.UtcNow));

        // Delegate to the comprehensive CoffeeService which handles all business logic
        return await _coffeeService.GetCoffeeEntriesAsync(sessionId, dateTime);
    }

    /// <summary>
    /// Gets the current session ID from the HTTP context
    /// </summary>
    /// <returns>The session ID or null if not available</returns>
    private string? GetSessionId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null || !httpContext.Items.TryGetValue("SessionId", out var sessionId))
        {
            return null;
        }
        
        return sessionId?.ToString();
    }
}
