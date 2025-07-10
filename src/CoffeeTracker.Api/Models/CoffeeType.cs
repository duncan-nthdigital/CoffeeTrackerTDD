using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.Models;

/// <summary>
/// Represents different types of coffee with their base caffeine content
/// </summary>
public enum CoffeeType
{
    /// <summary>
    /// Espresso - concentrated coffee with 90mg base caffeine
    /// </summary>
    [Display(Name = "Espresso")]
    Espresso,

    /// <summary>
    /// Americano - espresso with hot water, 120mg base caffeine
    /// </summary>
    [Display(Name = "Americano")]
    Americano,

    /// <summary>
    /// Latte - espresso with steamed milk, 80mg base caffeine
    /// </summary>
    [Display(Name = "Latte")]
    Latte,

    /// <summary>
    /// Cappuccino - espresso with steamed milk and foam, 80mg base caffeine
    /// </summary>
    [Display(Name = "Cappuccino")]
    Cappuccino,

    /// <summary>
    /// Mocha - espresso with chocolate and steamed milk, 90mg base caffeine
    /// </summary>
    [Display(Name = "Mocha")]
    Mocha,

    /// <summary>
    /// Macchiato - espresso with a dollop of steamed milk, 120mg base caffeine
    /// </summary>
    [Display(Name = "Macchiato")]
    Macchiato,

    /// <summary>
    /// Flat White - espresso with microfoam milk, 130mg base caffeine
    /// </summary>
    [Display(Name = "Flat White")]
    FlatWhite,

    /// <summary>
    /// Black Coffee - regular drip coffee, 95mg base caffeine
    /// </summary>
    [Display(Name = "Black Coffee")]
    BlackCoffee
}

/// <summary>
/// Extension methods for CoffeeType enum
/// </summary>
public static class CoffeeTypeExtensions
{
    /// <summary>
    /// Gets the base caffeine content in milligrams for the coffee type
    /// </summary>
    /// <param name="coffeeType">The coffee type</param>
    /// <returns>Base caffeine content in milligrams</returns>
    public static int GetBaseCaffeineContent(this CoffeeType coffeeType)
    {
        return coffeeType switch
        {
            CoffeeType.Espresso => 90,
            CoffeeType.Americano => 120,
            CoffeeType.Latte => 80,
            CoffeeType.Cappuccino => 80,
            CoffeeType.Mocha => 90,
            CoffeeType.Macchiato => 120,
            CoffeeType.FlatWhite => 130,
            CoffeeType.BlackCoffee => 95,
            _ => throw new ArgumentOutOfRangeException(nameof(coffeeType), coffeeType, "Unknown coffee type")
        };
    }

    /// <summary>
    /// Gets the caffeine content in milligrams for the coffee type adjusted by size
    /// </summary>
    /// <param name="coffeeType">The coffee type</param>
    /// <param name="size">The coffee size</param>
    /// <returns>Calculated caffeine content in milligrams</returns>
    public static int GetCaffeineContent(this CoffeeType coffeeType, CoffeeSize size)
    {
        var baseCaffeine = coffeeType.GetBaseCaffeineContent();
        var multiplier = size.GetSizeMultiplier();
        return (int)Math.Round(baseCaffeine * multiplier);
    }

    /// <summary>
    /// Gets the user-friendly display name for the coffee type
    /// </summary>
    /// <param name="coffeeType">The coffee type</param>
    /// <returns>Display name</returns>
    public static string GetDisplayName(this CoffeeType coffeeType)
    {
        return EnumExtensions.GetDisplayName(coffeeType);
    }
}
