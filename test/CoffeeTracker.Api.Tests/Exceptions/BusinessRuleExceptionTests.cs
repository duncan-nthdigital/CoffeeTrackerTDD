using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Exceptions;
using FluentAssertions;
using Xunit;

namespace CoffeeTracker.Api.Tests.Exceptions;

public class BusinessRuleExceptionTests
{
    [Fact]
    public void DailyEntryLimitExceededException_Should_Set_Properties_Correctly()
    {
        // Arrange
        var currentCount = 10;
        var maxAllowed = 10;

        // Act
        var exception = new DailyEntryLimitExceededException(currentCount, maxAllowed);

        // Assert
        exception.CurrentCount.Should().Be(currentCount);
        exception.MaxAllowed.Should().Be(maxAllowed);
        exception.RuleName.Should().Be("DailyEntryLimit");
        exception.Message.Should().Contain("Daily entry limit exceeded");
        exception.Message.Should().Contain("Current: 10");
        exception.Message.Should().Contain("Maximum allowed: 10");
    }

    [Fact]
    public void DailyCaffeineLimitExceededException_Should_Set_Properties_Correctly()
    {
        // Arrange
        var currentCaffeine = 800;
        var additionalCaffeine = 300;
        var maxAllowed = 1000;

        // Act
        var exception = new DailyCaffeineLimitExceededException(currentCaffeine, additionalCaffeine, maxAllowed);

        // Assert
        exception.CurrentCaffeine.Should().Be(currentCaffeine);
        exception.AdditionalCaffeine.Should().Be(additionalCaffeine);
        exception.MaxAllowed.Should().Be(maxAllowed);
        exception.RuleName.Should().Be("DailyCaffeineLimit");
        exception.Message.Should().Contain("Daily caffeine limit would be exceeded");
        exception.Message.Should().Contain("Current: 800mg");
        exception.Message.Should().Contain("Adding: 300mg");
        exception.Message.Should().Contain("Maximum allowed: 1000mg");
    }

    [Fact]
    public void InvalidTimestampException_Should_Set_Properties_Correctly()
    {
        // Arrange
        var timestamp = DateTime.UtcNow.AddDays(1);

        // Act
        var exception = new InvalidTimestampException(timestamp);

        // Assert
        exception.Timestamp.Should().Be(timestamp);
        exception.RuleName.Should().Be("InvalidTimestamp");
        exception.Message.Should().Contain("Invalid timestamp provided");
        exception.Message.Should().Contain("Future dates are not allowed");
    }
}
