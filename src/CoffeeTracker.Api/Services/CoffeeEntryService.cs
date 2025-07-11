using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service for managing coffee entries
/// </summary>
public class CoffeeEntryService : ICoffeeEntryService
{
    private readonly CoffeeTrackerDbContext _context;
    private readonly ILogger<CoffeeEntryService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the CoffeeEntryService
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger instance</param>
    /// <param name="httpContextAccessor">HTTP context accessor</param>
    public CoffeeEntryService(
        CoffeeTrackerDbContext context, 
        ILogger<CoffeeEntryService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    /// Creates a new coffee entry
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

        _logger.LogInformation("Creating coffee entry: {CoffeeType} {Size} for session: {SessionId}", 
            request.CoffeeType, request.Size, sessionId);

        var coffeeEntry = new CoffeeEntry
        {
            CoffeeType = request.CoffeeType,
            Size = request.Size,
            Source = request.Source,
            Timestamp = request.Timestamp ?? DateTime.UtcNow,
            SessionId = sessionId
        };

        _context.CoffeeEntries.Add(coffeeEntry);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Coffee entry created with ID: {Id}", coffeeEntry.Id);

        return MapToResponse(coffeeEntry);
    }

    /// <summary>
    /// Gets coffee entries for a specific date
    /// </summary>
    /// <param name="date">The date to filter entries (optional, defaults to today)</param>
    /// <returns>List of coffee entries for the specified date</returns>
    public async Task<IEnumerable<CoffeeEntryResponse>> GetCoffeeEntriesAsync(DateOnly? date = null)
    {
        var filterDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var startDate = filterDate.ToDateTime(TimeOnly.MinValue);
        var endDate = filterDate.ToDateTime(TimeOnly.MaxValue);

        // Get the session ID from the HTTP context
        var sessionId = GetSessionId();
        if (string.IsNullOrEmpty(sessionId))
        {
            _logger.LogError("No session ID available for getting coffee entries");
            throw new InvalidOperationException("No session ID available");
        }

        _logger.LogInformation("Getting coffee entries for date: {Date} and session: {SessionId}", 
            filterDate, sessionId);

        var entries = await _context.CoffeeEntries
            .Where(e => e.Timestamp >= startDate && e.Timestamp <= endDate && e.SessionId == sessionId)
            .OrderBy(e => e.Timestamp)
            .ToListAsync();

        _logger.LogInformation("Found {Count} coffee entries for date: {Date}", entries.Count, filterDate);

        return entries.Select(MapToResponse);
    }

    /// <summary>
    /// Maps a CoffeeEntry entity to a CoffeeEntryResponse DTO
    /// </summary>
    /// <param name="entry">The coffee entry entity</param>
    /// <returns>The coffee entry response DTO</returns>
    private static CoffeeEntryResponse MapToResponse(CoffeeEntry entry)
    {
        return new CoffeeEntryResponse
        {
            Id = entry.Id,
            CoffeeType = entry.CoffeeType,
            Size = entry.Size,
            Source = entry.Source,
            Timestamp = entry.Timestamp,
            CaffeineAmount = entry.CaffeineAmount
        };
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
