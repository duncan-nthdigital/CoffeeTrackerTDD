using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CoffeeTracker.Api.DTOs;

/// <summary>
/// Response DTO for daily coffee consumption summary
/// </summary>
public class DailySummaryResponse
{
    /// <summary>
    /// The date for this summary
    /// </summary>
    /// <example>2025-07-10</example>
    [JsonPropertyName("date")]
    [Description("The date for this summary")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Total number of coffee entries for the day
    /// </summary>
    /// <example>5</example>
    [JsonPropertyName("totalEntries")]
    [Description("Total number of coffee entries for the day")]
    public int TotalEntries { get; set; }

    /// <summary>
    /// Total caffeine consumed in milligrams
    /// </summary>
    /// <example>450</example>
    [JsonPropertyName("totalCaffeine")]
    [Description("Total caffeine consumed in milligrams")]
    public int TotalCaffeine { get; set; }

    /// <summary>
    /// List of coffee entries for the day
    /// </summary>
    [JsonPropertyName("entries")]
    [Description("List of coffee entries for the day")]
    public List<CoffeeEntryResponse> Entries { get; set; } = new List<CoffeeEntryResponse>();

    /// <summary>
    /// Average caffeine per entry for the day
    /// </summary>
    /// <example>90.0</example>
    [JsonPropertyName("averageCaffeinePerEntry")]
    [Description("Average caffeine per entry for the day")]
    public decimal AverageCaffeinePerEntry { get; set; }
}
