using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.Validation;

/// <summary>
/// Custom validation attribute to validate that a DateTime is not in the future
/// </summary>
public class NotFutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return true; // Optional field

        if (value is not DateTime dateTime)
            return false;

        return dateTime <= DateTime.UtcNow;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The {name} field cannot be a future date";
    }
}
