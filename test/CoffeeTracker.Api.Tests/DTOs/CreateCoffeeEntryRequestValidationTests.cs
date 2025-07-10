using CoffeeTracker.Api.DTOs;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.Tests.DTOs;

/// <summary>
/// Unit tests for DTO validation attributes
/// </summary>
public class CreateCoffeeEntryRequestValidationTests
{
    [Fact]
    public void Should_Be_Valid_When_RequiredFieldsProvided()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void Should_Be_Valid_When_AllFieldsProvided()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Americano",
            Size = "Large",
            Source = "Starbucks",
            Timestamp = DateTime.UtcNow
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Should_Be_Invalid_When_CoffeeType_IsNullOrWhitespace(string? coffeeType)
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = coffeeType!,
            Size = "Medium"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().HaveCount(1);
        validationResults[0].ErrorMessage.Should().Be("Coffee type is required");
        validationResults[0].MemberNames.Should().Contain("CoffeeType");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Should_Be_Invalid_When_Size_IsNullOrWhitespace(string? size)
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = size!
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().HaveCount(1);
        validationResults[0].ErrorMessage.Should().Be("Size is required");
        validationResults[0].MemberNames.Should().Contain("Size");
    }

    [Fact]
    public void Should_Be_Invalid_When_CoffeeType_ExceedsMaxLength()
    {
        // Arrange
        var longCoffeeType = new string('A', 51); // Exceeds 50 character limit
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = longCoffeeType,
            Size = "Medium"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().HaveCount(1);
        validationResults[0].ErrorMessage.Should().Be("Coffee type cannot exceed 50 characters");
        validationResults[0].MemberNames.Should().Contain("CoffeeType");
    }

    [Fact]
    public void Should_Be_Invalid_When_Source_ExceedsMaxLength()
    {
        // Arrange
        var longSource = new string('B', 101); // Exceeds 100 character limit
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium",
            Source = longSource
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().HaveCount(1);
        validationResults[0].ErrorMessage.Should().Be("Source cannot exceed 100 characters");
        validationResults[0].MemberNames.Should().Contain("Source");
    }

    [Fact]
    public void Should_Be_Valid_When_Source_IsAtMaxLength()
    {
        // Arrange
        var maxLengthSource = new string('C', 100); // Exactly 100 characters
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Espresso",
            Size = "Small",
            Source = maxLengthSource
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void Should_Be_Valid_When_CoffeeType_IsAtMaxLength()
    {
        // Arrange
        var maxLengthCoffeeType = new string('D', 50); // Exactly 50 characters
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = maxLengthCoffeeType,
            Size = "Large"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void Should_Have_Multiple_Validation_Errors_When_Multiple_Fields_Invalid()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "", // Required field empty
            Size = "", // Required field empty
            Source = new string('E', 101) // Exceeds max length
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().HaveCount(3);
        validationResults.Should().Contain(r => r.MemberNames.Contains("CoffeeType"));
        validationResults.Should().Contain(r => r.MemberNames.Contains("Size"));
        validationResults.Should().Contain(r => r.MemberNames.Contains("Source"));
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}
