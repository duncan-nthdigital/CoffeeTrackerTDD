using System.ComponentModel.DataAnnotations;
using Xunit;
using FluentAssertions;
using CoffeeTracker.Api.DTOs;

namespace CoffeeTracker.Api.Tests.DTOs;

/// <summary>
/// Unit tests for GetCoffeeEntriesRequest DTO validation
/// </summary>
public class GetCoffeeEntriesRequestTests
{
    [Fact]
    public void Should_Be_Valid_When_Date_IsNull()
    {
        // Arrange
        var request = new GetCoffeeEntriesRequest
        {
            Date = null
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}
