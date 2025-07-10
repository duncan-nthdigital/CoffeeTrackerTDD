namespace CoffeeTracker.Api.DTOs;

/// <summary>
/// Response DTO for daily coffee consumption summary
/// </summary>
public class DailySummaryResponse
{
    /// <summary>
    /// The date for this summary
    /// </summary>
    /// <example>2024-01-15</example>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Total number of coffee entries for the day
    /// </summary>
    /// <example>5</example>
    public int TotalEntries { get; set; }

    /// <summary>
    /// Total caffeine consumed in milligrams
    /// </summary>
    /// <example>450</example>
    public int TotalCaffeineMilligrams { get; set; }

    /// <summary>
    /// List of coffee entries for the day
    /// </summary>
    public IEnumerable<CoffeeEntryResponse> Entries { get; set; } = new List<CoffeeEntryResponse>();

    /// <summary>
    /// Whether the daily limit has been reached
    /// </summary>
    /// <example>false</example>
    public bool HasReachedDailyLimit { get; set; }

    /// <summary>
    /// Whether the caffeine limit has been reached
    /// </summary>
    /// <example>false</example>
    public bool HasReachedCaffeineLimit { get; set; }
}
