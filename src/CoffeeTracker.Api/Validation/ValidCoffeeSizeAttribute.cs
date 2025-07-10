using System.ComponentModel.DataAnnotations;
using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Validation;

/// <summary>
/// Custom validation attribute to validate that a string value is a valid CoffeeSize enum value
/// </summary>
public class ValidCoffeeSizeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return true; // Let Required attribute handle null validation

        if (value is not string stringValue)
            return false;

        return Enum.TryParse<CoffeeSize>(stringValue, ignoreCase: true, out _);
    }

    public override string FormatErrorMessage(string name)
    {
        var validValues = string.Join(", ", Enum.GetNames<CoffeeSize>());
        return $"The {name} field must be one of the following values: {validValues}";
    }
}
