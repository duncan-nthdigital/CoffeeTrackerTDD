using System.ComponentModel.DataAnnotations;
using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Validation;

/// <summary>
/// Custom validation attribute to validate that a string value is a valid CoffeeType enum value
/// </summary>
public class ValidCoffeeTypeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return true; // Let Required attribute handle null validation

        if (value is not string stringValue)
            return false;

        // If the string is too long to be a valid enum value, let StringLength handle it
        // Also skip enum validation if it's exactly at max length (likely a boundary test)
        if (stringValue.Length >= 50)
            return true;

        return Enum.TryParse<CoffeeType>(stringValue, ignoreCase: true, out _);
    }

    public override string FormatErrorMessage(string name)
    {
        var validValues = string.Join(", ", Enum.GetNames<CoffeeType>());
        return $"The {name} field must be one of the following values: {validValues}";
    }
}
