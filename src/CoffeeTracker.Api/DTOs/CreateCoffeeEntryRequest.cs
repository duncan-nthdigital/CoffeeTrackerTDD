using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.DTOs;

/// <summary>
/// Request DTO for creating a new coffee entry
/// </summary>
public class CreateCoffeeEntryRequest
{
    /// <summary>
    /// The type of coffee (e.g., Espresso, Latte, Cappuccino)
    /// </summary>
    /// <example>Latte</example>
    [Required(ErrorMessage = "Coffee type is required")]
    [StringLength(50, ErrorMessage = "Coffee type cannot exceed 50 characters")]
    public string CoffeeType { get; set; } = string.Empty;

    /// <summary>
    /// The size of the coffee (e.g., Small, Medium, Large)
    /// </summary>
    /// <example>Medium</example>
    [Required(ErrorMessage = "Size is required")]
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Optional source of the coffee (e.g., coffee shop name or "Home")
    /// </summary>
    /// <example>Starbucks</example>
    [StringLength(100, ErrorMessage = "Source cannot exceed 100 characters")]
    public string? Source { get; set; }

    /// <summary>
    /// Optional timestamp when the coffee was consumed (defaults to current time if not provided)
    /// </summary>
    /// <example>2025-07-10T10:30:00Z</example>
    public DateTime? Timestamp { get; set; }
}
