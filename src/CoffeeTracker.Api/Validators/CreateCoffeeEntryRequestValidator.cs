using FluentValidation;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Validators;

/// <summary>
/// FluentValidation validator for CreateCoffeeEntryRequest
/// </summary>
public class CreateCoffeeEntryRequestValidator : AbstractValidator<CreateCoffeeEntryRequest>
{
    public CreateCoffeeEntryRequestValidator()
    {
        RuleFor(x => x.CoffeeType)
            .NotEmpty().WithMessage("Coffee type is required")
            .MaximumLength(50).WithMessage("Coffee type cannot exceed 50 characters")
            .Must(BeValidCoffeeType).WithMessage("Invalid coffee type");

        RuleFor(x => x.Size)
            .NotEmpty().WithMessage("Size is required")
            .Must(BeValidSize).WithMessage("Invalid coffee size");

        RuleFor(x => x.Source)
            .MaximumLength(100).WithMessage("Source cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Source));

        RuleFor(x => x.Timestamp)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Timestamp cannot be in the future")
            .When(x => x.Timestamp.HasValue);
    }

    private bool BeValidCoffeeType(string coffeeType)
    {
        return Enum.TryParse<CoffeeType>(coffeeType, ignoreCase: true, out _);
    }

    private bool BeValidSize(string size)
    {
        return Enum.TryParse<CoffeeSize>(size, ignoreCase: true, out _);
    }
}
