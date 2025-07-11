using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CoffeeTracker.Api.DTOs;

/// <summary>
/// Request DTO for getting coffee entries
/// </summary>
public class GetCoffeeEntriesRequest
{
    /// <summary>
    /// Optional date to filter coffee entries (defaults to today if not provided)
    /// </summary>
    /// <example>2025-07-10</example>
    [JsonPropertyName("date")]
    [Description("Optional date to filter coffee entries")]
    public DateTime? Date { get; set; }
}
