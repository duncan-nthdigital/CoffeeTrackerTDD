using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.Validation;

/// <summary>
/// Validates that caffeine amount doesn't exceed a maximum daily limit
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class MaxDailyCaffeineAttribute : ValidationAttribute
{
    /// <summary>
    /// The maximum allowed daily caffeine in milligrams
    /// </summary>
    public int MaxCaffeine { get; }

    /// <summary>
    /// Initializes a new instance of the MaxDailyCaffeineAttribute class
    /// </summary>
    /// <param name="maxCaffeine">The maximum allowed caffeine in mg</param>
    public MaxDailyCaffeineAttribute(int maxCaffeine)
    {
        if (maxCaffeine <= 0)
        {
            throw new ArgumentException("Maximum caffeine must be greater than zero", nameof(maxCaffeine));
        }

        MaxCaffeine = maxCaffeine;
        ErrorMessage = $"Caffeine amount exceeds the maximum allowed daily limit of {maxCaffeine}mg";
    }

    /// <summary>
    /// Validates the value against the maximum caffeine limit
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Success if valid, error message otherwise</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        // For simple property validation, check if the value is an integer
        if (value is int caffeineAmount)
        {
            if (caffeineAmount <= MaxCaffeine)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName ?? string.Empty });
        }

        // For class validation, check if the object has a property named CaffeineAmount
        var property = validationContext.ObjectType.GetProperty("CaffeineAmount");
        if (property != null)
        {
            var caffeineValue = property.GetValue(validationContext.ObjectInstance);
            if (caffeineValue is int caffeine && caffeine <= MaxCaffeine)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
