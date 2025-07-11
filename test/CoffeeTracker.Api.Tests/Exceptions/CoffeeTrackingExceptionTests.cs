using CoffeeTracker.Api.Exceptions;
using System;
using Xunit;

namespace CoffeeTracker.Api.Tests.Exceptions;

public class CoffeeTrackingExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_SetsMessageProperty()
    {
        // Arrange & Act
        var exception = new CoffeeTrackingException("Test message");

        // Assert
        Assert.Equal("Test message", exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsProperties()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new CoffeeTrackingException("Test message", innerException);

        // Assert
        Assert.Equal("Test message", exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }
}

public class ValidationExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_SetsMessageProperty()
    {
        // Arrange & Act
        var exception = new ValidationException("Test validation message");

        // Assert
        Assert.Equal("Test validation message", exception.Message);
        Assert.IsType<CoffeeTrackingException>(exception);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsProperties()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new ValidationException("Test validation message", innerException);

        // Assert
        Assert.Equal("Test validation message", exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }
}

public class SessionNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithSessionId_SetsProperties()
    {
        // Arrange & Act
        var sessionId = "test-session-id";
        var exception = new SessionNotFoundException(sessionId);

        // Assert
        Assert.Equal(sessionId, exception.SessionId);
        Assert.Contains(sessionId, exception.Message);
        Assert.IsType<CoffeeTrackingException>(exception);
    }

    [Fact]
    public void Constructor_WithSessionIdAndCustomMessage_SetsProperties()
    {
        // Arrange & Act
        var sessionId = "test-session-id";
        var message = "Custom message";
        var exception = new SessionNotFoundException(sessionId, message);

        // Assert
        Assert.Equal(sessionId, exception.SessionId);
        Assert.Equal(message, exception.Message);
    }
}
