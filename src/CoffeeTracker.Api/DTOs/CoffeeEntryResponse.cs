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
    public int Id { get; set; }

    /// <summary>
    /// The type of coffee (e.g., Espresso, Latte, Cappuccino)
    /// </summary>
    /// <example>Latte</example>
    public string CoffeeType { get; set; } = string.Empty;

    /// <summary>
    /// The size of the coffee (e.g., Small, Medium, Large)
    /// </summary>
    /// <example>Medium</example>
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// The source of the coffee (e.g., coffee shop name or "Home")
    /// </summary>
    /// <example>Starbucks</example>
    public string? Source { get; set; }

    /// <summary>
    /// The timestamp when the coffee was consumed
    /// </summary>
    /// <example>2025-07-10T10:30:00Z</example>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The calculated caffeine amount in milligrams
    /// </summary>
    /// <example>80</example>
    public int CaffeineAmount { get; set; }
}
