using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.Models;

/// <summary>
/// Represents a coffee entry for anonymous coffee tracking
/// </summary>
public class CoffeeEntry
{
    #region Constants
    
    /// <summary>
    /// Maximum length for coffee type field
    /// </summary>
    public const int CoffeeTypeMaxLength = 50;
    
    /// <summary>
    /// Maximum length for source field
    /// </summary>
    public const int SourceMaxLength = 100;
    
    /// <summary>
    /// Default caffeine amount for unknown coffee types
    /// </summary>
    private const int DefaultCaffeineAmount = 80;
    
    /// <summary>
    /// Default size multiplier for unknown sizes
    /// </summary>
    private const double DefaultSizeMultiplier = 1.0;
    
    #endregion

    #region Properties
    
    /// <summary>
    /// Gets or sets the unique identifier for the coffee entry
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the type of coffee (e.g., Espresso, Latte, Cappuccino)
    /// </summary>
    [Required]
    [StringLength(CoffeeTypeMaxLength)]
    public string CoffeeType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the coffee (e.g., Small, Medium, Large)
    /// </summary>
    [Required]
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source of the coffee (e.g., coffee shop name or "Home")
    /// </summary>
    [StringLength(SourceMaxLength)]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the coffee was consumed
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets the calculated caffeine amount based on coffee type and size
    /// </summary>
    public int CaffeineAmount => CalculateCaffeineAmount();

    #endregion
    
    #region Constructor

    #endregion
    
    #region Constructor
    
    /// <summary>
    /// Initializes a new instance of the CoffeeEntry class with current UTC timestamp
    /// </summary>
    public CoffeeEntry()
    {
        Timestamp = DateTime.UtcNow;
    }

    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Calculates the caffeine amount based on coffee type and size
    /// </summary>
    /// <returns>The caffeine amount in milligrams</returns>
    private int CalculateCaffeineAmount()
    {
        var baseCaffeine = GetBaseCaffeineAmount(CoffeeType);
        var sizeMultiplier = GetSizeMultiplier(Size);
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
            _ => DefaultCaffeineAmount
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
            _ => DefaultSizeMultiplier
        };
    }

    #endregion
}
