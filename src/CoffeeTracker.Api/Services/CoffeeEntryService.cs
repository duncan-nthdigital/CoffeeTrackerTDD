using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Models;
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

    /// <summary>
    /// Initializes a new instance of the CoffeeEntryService
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger instance</param>
    public CoffeeEntryService(CoffeeTrackerDbContext context, ILogger<CoffeeEntryService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        _logger.LogInformation("Creating coffee entry: {CoffeeType} {Size}", request.CoffeeType, request.Size);

        var coffeeEntry = new CoffeeEntry
        {
            CoffeeType = request.CoffeeType,
            Size = request.Size,
            Source = request.Source,
            Timestamp = request.Timestamp ?? DateTime.UtcNow
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

        _logger.LogInformation("Getting coffee entries for date: {Date}", filterDate);

        var entries = await _context.CoffeeEntries
            .Where(e => e.Timestamp >= startDate && e.Timestamp <= endDate)
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
}
