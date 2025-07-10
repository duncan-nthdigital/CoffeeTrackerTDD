using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.Models;

/// <summary>
/// Represents different coffee sizes with their caffeine multipliers
/// </summary>
public enum CoffeeSize
{
    /// <summary>
    /// Small size - 0.8x caffeine multiplier
    /// </summary>
    [Display(Name = "Small")]
    Small,

    /// <summary>
    /// Medium size - 1.0x caffeine multiplier (standard)
    /// </summary>
    [Display(Name = "Medium")]
    Medium,

    /// <summary>
    /// Large size - 1.3x caffeine multiplier
    /// </summary>
    [Display(Name = "Large")]
    Large,

    /// <summary>
    /// Extra Large size - 1.6x caffeine multiplier
    /// </summary>
    [Display(Name = "Extra Large")]
    ExtraLarge
}

/// <summary>
/// Extension methods for CoffeeSize enum
/// </summary>
public static class CoffeeSizeExtensions
{
    /// <summary>
    /// Size multiplier constants for performance and maintainability
    /// </summary>
    private static class SizeMultipliers
    {
        public const double Small = 0.8;
        public const double Medium = 1.0;
        public const double Large = 1.3;
        public const double ExtraLarge = 1.6;
    }

    /// <summary>
    /// Gets the size multiplier for calculating caffeine content
    /// </summary>
    /// <param name="size">The coffee size</param>
    /// <returns>Multiplier value</returns>
    public static double GetSizeMultiplier(this CoffeeSize size)
    {
        return size switch
        {
            CoffeeSize.Small => SizeMultipliers.Small,
            CoffeeSize.Medium => SizeMultipliers.Medium,
            CoffeeSize.Large => SizeMultipliers.Large,
            CoffeeSize.ExtraLarge => SizeMultipliers.ExtraLarge,
            _ => throw new ArgumentOutOfRangeException(nameof(size), size, "Unknown coffee size")
        };
    }

    /// <summary>
    /// Gets the user-friendly display name for the coffee size
    /// </summary>
    /// <param name="size">The coffee size</param>
    /// <returns>Display name</returns>
    public static string GetDisplayName(this CoffeeSize size)
    {
        return EnumExtensions.GetDisplayName(size);
    }
}
