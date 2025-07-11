using System.ComponentModel.DataAnnotations;
using Xunit;
using FluentAssertions;
using CoffeeTracker.Api.DTOs;

namespace CoffeeTracker.Api.Tests.DTOs;

/// <summary>
/// Unit tests for CreateCoffeeEntryRequest DTO validation
/// </summary>
public class CreateCoffeeEntryRequestTests
{
    [Fact]
    public void Should_Require_CoffeeType()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = null!,
            Size = "Medium"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().Contain(vr => vr.MemberNames.Contains("CoffeeType"));
    }

    [Fact]
    public void Should_Require_Size()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = null!
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().Contain(vr => vr.MemberNames.Contains("Size"));
    }

    [Fact]
    public void Should_Validate_Source_MaxLength()
    {
        // Arrange
        var longSource = new string('x', 101); // 101 characters, exceeds limit of 100
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium",
            Source = longSource
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().Contain(vr => vr.MemberNames.Contains("Source"));
    }

    [Fact]
    public void Should_Validate_CoffeeType_EnumValue()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "InvalidCoffeeType",
            Size = "Medium"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().Contain(vr => vr.MemberNames.Contains("CoffeeType"));
    }

    [Fact]
    public void Should_Validate_Size_EnumValue()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "InvalidSize"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().Contain(vr => vr.MemberNames.Contains("Size"));
    }

    [Fact]
    public void Should_Not_Allow_Future_Timestamp()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium",
            Timestamp = DateTime.UtcNow.AddDays(1) // Future date
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().Contain(vr => vr.MemberNames.Contains("Timestamp"));
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}
