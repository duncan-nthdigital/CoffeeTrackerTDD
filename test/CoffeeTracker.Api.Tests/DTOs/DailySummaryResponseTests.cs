using Xunit;
using FluentAssertions;
using CoffeeTracker.Api.DTOs;

namespace CoffeeTracker.Api.Tests.DTOs;

/// <summary>
/// Unit tests for DailySummaryResponse DTO
/// </summary>
public class DailySummaryResponseTests
{
    [Fact]
    public void Should_Have_AverageCaffeinePerEntry_Property()
    {
        // Arrange
        var response = new DailySummaryResponse();

        // Act
        var hasProperty = typeof(DailySummaryResponse).GetProperty("AverageCaffeinePerEntry");

        // Assert
        hasProperty.Should().NotBeNull();
        hasProperty!.PropertyType.Should().Be(typeof(decimal));
    }
}
