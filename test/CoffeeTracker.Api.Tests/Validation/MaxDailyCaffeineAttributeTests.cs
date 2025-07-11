using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoffeeTracker.Api.Validation;
using Xunit;

namespace CoffeeTracker.Api.Tests.Validation;

public class MaxDailyCaffeineAttributeTests
{
    [Fact]
    public void IsValid_WhenCaffeineUnderLimit_ReturnsTrue()
    {
        // Arrange
        var attribute = new MaxDailyCaffeineAttribute(500);
        var model = new TestModelWithCaffeine { CaffeineAmount = 400 };
        var validationContext = new ValidationContext(model);
        
        // Act
        var result = attribute.GetValidationResult(model, validationContext);
        
        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }
    
    [Fact]
    public void IsValid_WhenCaffeineEqualToLimit_ReturnsTrue()
    {
        // Arrange
        var attribute = new MaxDailyCaffeineAttribute(500);
        var model = new TestModelWithCaffeine { CaffeineAmount = 500 };
        var validationContext = new ValidationContext(model);
        
        // Act
        var result = attribute.GetValidationResult(model, validationContext);
        
        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }
    
    [Fact]
    public void IsValid_WhenCaffeineOverLimit_ReturnsFalse()
    {
        // Arrange
        var attribute = new MaxDailyCaffeineAttribute(500);
        var model = new TestModelWithCaffeine { CaffeineAmount = 501 };
        var validationContext = new ValidationContext(model);
        
        // Act
        var result = attribute.GetValidationResult(model, validationContext);
        
        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("exceeds the maximum allowed", result?.ErrorMessage ?? string.Empty);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WhenMaxCaffeineInvalid_ThrowsArgumentException(int maxCaffeine)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new MaxDailyCaffeineAttribute(maxCaffeine));
    }
    
    private class TestModelWithCaffeine
    {
        public int CaffeineAmount { get; set; }
    }
}
