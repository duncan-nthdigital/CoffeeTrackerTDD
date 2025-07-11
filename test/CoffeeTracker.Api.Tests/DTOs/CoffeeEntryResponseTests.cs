using Xunit;
using FluentAssertions;
using CoffeeTracker.Api.DTOs;

namespace CoffeeTracker.Api.Tests.DTOs;

/// <summary>
/// Unit tests for CoffeeEntryResponse DTO
/// </summary>
public class CoffeeEntryResponseTests
{
    [Fact]
    public void Should_Have_FormattedTimestamp_Property()
    {
        // Arrange
        var response = new CoffeeEntryResponse();

        // Act
        var hasProperty = typeof(CoffeeEntryResponse).GetProperty("FormattedTimestamp");

        // Assert
        hasProperty.Should().NotBeNull();
        hasProperty!.PropertyType.Should().Be(typeof(string));
    }
}
