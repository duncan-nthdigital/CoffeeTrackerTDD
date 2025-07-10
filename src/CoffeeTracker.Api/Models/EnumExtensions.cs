using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CoffeeTracker.Api.Models;

/// <summary>
/// Utility methods for working with enums
/// </summary>
internal static class EnumExtensions
{
    /// <summary>
    /// Gets the display name for an enum value using the DisplayAttribute
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <param name="enumValue">The enum value</param>
    /// <returns>Display name from DisplayAttribute or enum name if not found</returns>
    internal static string GetDisplayName<T>(T enumValue) where T : Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                              .FirstOrDefault() as DisplayAttribute;
        return attribute?.Name ?? enumValue.ToString();
    }
}
