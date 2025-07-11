using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Exceptions;
using CoffeeTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service implementation for coffee operations with session management and business rules
/// </summary>
public class CoffeeService : ICoffeeService
{
    private readonly CoffeeTrackerDbContext _context;
    private readonly ILogger<CoffeeService> _logger;

    private const int MaxDailyEntries = 10;
    private const int MaxDailyCaffeineMilligrams = 1000;
    private const int DataRetentionHours = 24;

    /// <summary>
    /// Initializes a new instance of the CoffeeService class
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="logger">The logger instance</param>
    public CoffeeService(CoffeeTrackerDbContext context, ILogger<CoffeeService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CoffeeEntryResponse> CreateCoffeeEntryAsync(CreateCoffeeEntryRequest request, string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("Session ID cannot be null or empty", nameof(sessionId));
        }

        _logger.LogInformation("Creating coffee entry for session {SessionId}", sessionId);

        // Clean up old entries first
        await CleanupOldEntriesAsync(sessionId);

        // Validate timestamp
        var entryTimestamp = request.Timestamp ?? DateTime.UtcNow;
        if (entryTimestamp > DateTime.UtcNow)
        {
            throw new InvalidTimestampException(entryTimestamp);
        }

        var today = DateOnly.FromDateTime(entryTimestamp);

        // Check daily entry limit
        var todayEntriesCount = await _context.CoffeeEntries
            .Where(e => e.SessionId == sessionId && 
                       DateOnly.FromDateTime(e.Timestamp) == today)
            .CountAsync();

        if (todayEntriesCount >= MaxDailyEntries)
        {
            throw new DailyEntryLimitExceededException(todayEntriesCount, MaxDailyEntries);
        }

        // Calculate caffeine for the new entry
        var newEntryCaffeine = CalculateCaffeineAmount(request.CoffeeType, request.Size);

        // Check daily caffeine limit - get entries and calculate caffeine client-side
        var todayEntries = await _context.CoffeeEntries
            .Where(e => e.SessionId == sessionId && 
                       DateOnly.FromDateTime(e.Timestamp) == today)
            .ToListAsync();

        var todayCaffeine = todayEntries.Sum(e => e.CaffeineAmount);

        if (todayCaffeine + newEntryCaffeine > MaxDailyCaffeineMilligrams)
        {
            throw new DailyCaffeineLimitExceededException(
                todayCaffeine, 
                newEntryCaffeine, 
                MaxDailyCaffeineMilligrams);
        }

        // Create the coffee entry
        var coffeeEntry = new CoffeeEntry
        {
            CoffeeType = request.CoffeeType,
            Size = request.Size,
            Source = request.Source,
            SessionId = sessionId,
            Timestamp = entryTimestamp
        };

        _context.CoffeeEntries.Add(coffeeEntry);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created coffee entry {EntryId} for session {SessionId}", 
            coffeeEntry.Id, sessionId);

        return MapToResponse(coffeeEntry);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CoffeeEntryResponse>> GetCoffeeEntriesAsync(string sessionId, DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("Session ID cannot be null or empty", nameof(sessionId));
        }

        _logger.LogInformation("Retrieving coffee entries for session {SessionId}, date: {Date}", 
            sessionId, date);

        // Clean up old entries first
        await CleanupOldEntriesAsync(sessionId);

        var query = _context.CoffeeEntries
            .Where(e => e.SessionId == sessionId);

        if (date.HasValue)
        {
            var filterDate = DateOnly.FromDateTime(date.Value);
            query = query.Where(e => DateOnly.FromDateTime(e.Timestamp) == filterDate);
        }
        else
        {
            // Default to today if no date specified
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            query = query.Where(e => DateOnly.FromDateTime(e.Timestamp) == today);
        }

        var entries = await query
            .OrderBy(e => e.Timestamp)
            .ToListAsync();

        return entries.Select(MapToResponse);
    }

    /// <inheritdoc />
    public async Task<DailySummaryResponse> GetDailySummaryAsync(string sessionId, DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("Session ID cannot be null or empty", nameof(sessionId));
        }

        var summaryDate = date ?? DateTime.UtcNow;
        var summaryDateOnly = DateOnly.FromDateTime(summaryDate);

        _logger.LogInformation("Generating daily summary for session {SessionId}, date: {Date}", 
            sessionId, summaryDateOnly);

        // Clean up old entries first
        await CleanupOldEntriesAsync(sessionId);

        var entries = await _context.CoffeeEntries
            .Where(e => e.SessionId == sessionId && 
                       DateOnly.FromDateTime(e.Timestamp) == summaryDateOnly)
            .OrderBy(e => e.Timestamp)
            .ToListAsync();

        var totalCaffeine = entries.Sum(e => e.CaffeineAmount);
        var totalEntries = entries.Count;
        var averageCaffeinePerEntry = totalEntries > 0 ? (decimal)totalCaffeine / totalEntries : 0;

        return new DailySummaryResponse
        {
            Date = summaryDateOnly.ToDateTime(TimeOnly.MinValue),
            TotalEntries = totalEntries,
            TotalCaffeine = totalCaffeine,
            Entries = entries.Select(MapToResponse).ToList(),
            AverageCaffeinePerEntry = averageCaffeinePerEntry
        };
    }

    /// <summary>
    /// Cleans up old anonymous entries (older than 24 hours)
    /// </summary>
    /// <param name="sessionId">The session ID to clean up entries for</param>
    private async Task CleanupOldEntriesAsync(string sessionId)
    {
        var cutoffTime = DateTime.UtcNow.AddHours(-DataRetentionHours);

        var oldEntries = await _context.CoffeeEntries
            .Where(e => e.SessionId == sessionId && e.Timestamp < cutoffTime)
            .ToListAsync();

        if (oldEntries.Any())
        {
            _context.CoffeeEntries.RemoveRange(oldEntries);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Cleaned up {Count} old entries for session {SessionId}", 
                oldEntries.Count, sessionId);
        }
    }

    /// <summary>
    /// Calculates the caffeine amount based on coffee type and size
    /// </summary>
    /// <param name="coffeeType">The type of coffee</param>
    /// <param name="size">The size of the coffee</param>
    /// <returns>The caffeine amount in milligrams</returns>
    private static int CalculateCaffeineAmount(string coffeeType, string size)
    {
        var baseCaffeine = GetBaseCaffeineAmount(coffeeType);
        var sizeMultiplier = GetSizeMultiplier(size);
        return (int)(baseCaffeine * sizeMultiplier);
    }

    /// <summary>
    /// Gets the base caffeine amount for a given coffee type
    /// </summary>
    /// <param name="coffeeType">The type of coffee</param>
    /// <returns>Base caffeine amount in milligrams</returns>
    private static int GetBaseCaffeineAmount(string coffeeType)
    {
        return coffeeType switch
        {
            "Espresso" => 90,
            "Americano" => 120,
            "Latte" => 80,
            "Cappuccino" => 80,
            "Mocha" => 90,
            "Macchiato" => 120,
            "FlatWhite" => 130,
            "BlackCoffee" => 95,
            _ => 80 // Default caffeine amount
        };
    }

    /// <summary>
    /// Gets the size multiplier for a given coffee size
    /// </summary>
    /// <param name="size">The size of the coffee</param>
    /// <returns>Size multiplier</returns>
    private static double GetSizeMultiplier(string size)
    {
        return size switch
        {
            "Small" => 0.8,
            "Medium" => 1.0,
            "Large" => 1.3,
            "ExtraLarge" => 1.6,
            _ => 1.0 // Default size multiplier
        };
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
            CaffeineAmount = entry.CaffeineAmount,
            FormattedTimestamp = entry.Timestamp.ToString("h:mm tt")
        };
    }
}
