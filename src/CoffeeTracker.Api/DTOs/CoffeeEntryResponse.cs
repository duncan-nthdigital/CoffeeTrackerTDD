using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CoffeeTracker.Api.DTOs;

/// <summary>
/// Response DTO for coffee entry operations
/// </summary>
public class CoffeeEntryResponse
{
    /// <summary>
    /// The unique identifier for the coffee entry
    /// </summary>
    /// <example>1</example>
    [JsonPropertyName("id")]
    [Description("The unique identifier for the coffee entry")]
    public int Id { get; set; }

    /// <summary>
    /// The type of coffee (e.g., Espresso, Latte, Cappuccino)
    /// </summary>
    /// <example>Latte</example>
    [JsonPropertyName("coffeeType")]
    [Description("The type of coffee")]
    public string CoffeeType { get; set; } = string.Empty;

    /// <summary>
    /// The size of the coffee (e.g., Small, Medium, Large)
    /// </summary>
    /// <example>Medium</example>
    [JsonPropertyName("size")]
    [Description("The size of the coffee")]
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// The source of the coffee (e.g., coffee shop name or "Home")
    /// </summary>
    /// <example>Starbucks</example>
    [JsonPropertyName("source")]
    [Description("The source where the coffee was obtained")]
    public string? Source { get; set; }

    /// <summary>
    /// The timestamp when the coffee was consumed
    /// </summary>
    /// <example>2025-07-10T10:30:00Z</example>
    [JsonPropertyName("timestamp")]
    [Description("The timestamp when the coffee was consumed")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The calculated caffeine amount in milligrams
    /// </summary>
    /// <example>80</example>
    [JsonPropertyName("caffeineAmount")]
    [Description("The calculated caffeine amount in milligrams")]
    public int CaffeineAmount { get; set; }

    /// <summary>
    /// The formatted timestamp for display (e.g., "2:30 PM")
    /// </summary>
    /// <example>2:30 PM</example>
    [JsonPropertyName("formattedTimestamp")]
    [Description("The formatted timestamp for display")]
    public string FormattedTimestamp { get; set; } = string.Empty;
}
