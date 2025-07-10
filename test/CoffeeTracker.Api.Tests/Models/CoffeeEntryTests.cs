using System.ComponentModel.DataAnnotations;

namespace CoffeeTracker.Api.Tests.Models;

/// <summary>
/// Unit tests for the CoffeeEntry domain model
/// </summary>
public class CoffeeEntryTests
{
    [Fact]
    public void Constructor_Should_Set_Timestamp_To_UtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;
        
        // Act
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry();
        
        // Assert
        var afterCreation = DateTime.UtcNow;
        Assert.True(coffeeEntry.Timestamp >= beforeCreation && coffeeEntry.Timestamp <= afterCreation);
    }

    [Fact]
    public void CoffeeType_Should_Be_Required()
    {
        // Arrange
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = null!,
            Size = "Medium",
            SessionId = "test-session"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(coffeeEntry, new ValidationContext(coffeeEntry), validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(coffeeEntry.CoffeeType)));
    }

    [Fact]
    public void CoffeeType_Should_Not_Exceed_50_Characters()
    {
        // Arrange
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = new string('a', 51), // 51 characters
            Size = "Medium",
            SessionId = "test-session"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(coffeeEntry, new ValidationContext(coffeeEntry), validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(coffeeEntry.CoffeeType)));
    }

    [Fact]
    public void Size_Should_Be_Required()
    {
        // Arrange
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = null!,
            SessionId = "test-session"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(coffeeEntry, new ValidationContext(coffeeEntry), validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(coffeeEntry.Size)));
    }

    [Fact]
    public void Source_Should_Not_Exceed_100_Characters()
    {
        // Arrange
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = "Medium",
            Source = new string('a', 101), // 101 characters
            SessionId = "test-session"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(coffeeEntry, new ValidationContext(coffeeEntry), validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(coffeeEntry.Source)));
    }

    [Fact]
    public void Source_Should_Be_Optional()
    {
        // Arrange
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = "Medium",
            Source = null,
            SessionId = "test-session-123"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(coffeeEntry, new ValidationContext(coffeeEntry), validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Should_Calculate_Caffeine_Amount_For_Espresso_Medium()
    {
        // Arrange & Act
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = "Medium",
            SessionId = "test-session"
        };

        // Assert
        Assert.Equal(90, coffeeEntry.CaffeineAmount); // Espresso base 90mg * 1.0 multiplier
    }

    [Theory]
    [InlineData("Espresso", "Small", 72)]    // 90 * 0.8
    [InlineData("Espresso", "Large", 117)]   // 90 * 1.3
    [InlineData("Americano", "Medium", 120)] // 120 * 1.0
    [InlineData("Latte", "Large", 104)]      // 80 * 1.3
    [InlineData("UnknownType", "Medium", 80)] // Default base * 1.0
    [InlineData("Espresso", "UnknownSize", 90)] // 90 * default 1.0
    public void Should_Calculate_Correct_Caffeine_Amount(string coffeeType, string size, int expectedCaffeine)
    {
        // Arrange & Act
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = coffeeType,
            Size = size,
            SessionId = "test-session"
        };

        // Assert
        Assert.Equal(expectedCaffeine, coffeeEntry.CaffeineAmount);
    }

    [Fact]
    public void Should_Create_Valid_CoffeeEntry_With_All_Properties()
    {
        // Arrange & Act
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = "Latte",
            Size = "Large",
            Source = "Starbucks",
            SessionId = "test-session"
        };

        // Assert
        Assert.True(coffeeEntry.Id == 0); // Default value
        Assert.Equal("Latte", coffeeEntry.CoffeeType);
        Assert.Equal("Large", coffeeEntry.Size);
        Assert.Equal("Starbucks", coffeeEntry.Source);
        Assert.True(coffeeEntry.Timestamp <= DateTime.UtcNow);
        Assert.Equal(104, coffeeEntry.CaffeineAmount); // 80 * 1.3
    }

    [Fact]
    public void Validation_Should_Pass_For_Valid_CoffeeEntry()
    {
        // Arrange
        var coffeeEntry = new CoffeeTracker.Api.Models.CoffeeEntry
        {
            CoffeeType = "Cappuccino",
            Size = "Medium",
            Source = "Local Cafe",
            SessionId = "test-session-123"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(coffeeEntry, new ValidationContext(coffeeEntry), validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }
}
