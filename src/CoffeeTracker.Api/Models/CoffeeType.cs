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
    /// Base caffeine content constants in milligrams for performance and maintainability
    /// </summary>
    private static class BaseCaffeineContent
    {
        public const int Espresso = 90;
        public const int Americano = 120;
        public const int Latte = 80;
        public const int Cappuccino = 80;
        public const int Mocha = 90;
        public const int Macchiato = 120;
        public const int FlatWhite = 130;
        public const int BlackCoffee = 95;
    }

    /// <summary>
    /// Gets the base caffeine content in milligrams for the coffee type
    /// </summary>
    /// <param name="coffeeType">The coffee type</param>
    /// <returns>Base caffeine content in milligrams</returns>
    public static int GetBaseCaffeineContent(this CoffeeType coffeeType)
    {
        return coffeeType switch
        {
            CoffeeType.Espresso => BaseCaffeineContent.Espresso,
            CoffeeType.Americano => BaseCaffeineContent.Americano,
            CoffeeType.Latte => BaseCaffeineContent.Latte,
            CoffeeType.Cappuccino => BaseCaffeineContent.Cappuccino,
            CoffeeType.Mocha => BaseCaffeineContent.Mocha,
            CoffeeType.Macchiato => BaseCaffeineContent.Macchiato,
            CoffeeType.FlatWhite => BaseCaffeineContent.FlatWhite,
            CoffeeType.BlackCoffee => BaseCaffeineContent.BlackCoffee,
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
