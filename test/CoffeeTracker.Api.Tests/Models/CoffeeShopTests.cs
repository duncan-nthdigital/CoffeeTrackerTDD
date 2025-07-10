using System.ComponentModel.DataAnnotations;
using CoffeeTracker.Api.Models;
using FluentAssertions;
using Xunit;

namespace CoffeeTracker.Api.Tests.Models;

/// <summary>
/// Unit tests for the CoffeeShop domain model.
/// Verifies validation rules, business logic, and data annotations.
/// </summary>
public class CoffeeShopTests
{
    [Fact]
    public void Constructor_Should_Set_CreatedAt_To_UtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;
        
        // Act
        var coffeeShop = new CoffeeShop();
        
        // Assert
        var afterCreation = DateTime.UtcNow;
        coffeeShop.CreatedAt.Should().BeOnOrAfter(beforeCreation);
        coffeeShop.CreatedAt.Should().BeOnOrBefore(afterCreation);
        coffeeShop.CreatedAt.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void Constructor_Should_Set_IsActive_To_True_By_Default()
    {
        // Act
        var coffeeShop = new CoffeeShop();
        
        // Assert
        coffeeShop.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData("Starbucks")]
    [InlineData("Local Coffee House")]
    [InlineData("Home")]
    public void Name_Should_Accept_Valid_Names(string name)
    {
        // Arrange
        var coffeeShop = new CoffeeShop { Name = name };
        var context = new ValidationContext(coffeeShop);
        var results = new List<ValidationResult>();
        
        // Act
        var isValid = Validator.TryValidateObject(coffeeShop, context, results, true);
        
        // Assert
        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Name_Should_Require_Non_Empty_Value(string? name)
    {
        // Arrange
        var coffeeShop = new CoffeeShop { Name = name! };
        var context = new ValidationContext(coffeeShop);
        var results = new List<ValidationResult>();
        
        // Act
        var isValid = Validator.TryValidateObject(coffeeShop, context, results, true);
        
        // Assert
        isValid.Should().BeFalse();
        results.Should().ContainSingle(r => r.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Name_Should_Reject_Value_Longer_Than_100_Characters()
    {
        // Arrange
        var longName = new string('A', CoffeeShop.NameMaxLength + 1);
        var coffeeShop = new CoffeeShop { Name = longName };
        var context = new ValidationContext(coffeeShop);
        var results = new List<ValidationResult>();
        
        // Act
        var isValid = Validator.TryValidateObject(coffeeShop, context, results, true);
        
        // Assert
        isValid.Should().BeFalse();
        results.Should().ContainSingle(r => r.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Name_Should_Accept_Value_With_Exactly_100_Characters()
    {
        // Arrange
        var maxLengthName = new string('A', CoffeeShop.NameMaxLength);
        var coffeeShop = new CoffeeShop { Name = maxLengthName };
        var context = new ValidationContext(coffeeShop);
        var results = new List<ValidationResult>();
        
        // Act
        var isValid = Validator.TryValidateObject(coffeeShop, context, results, true);
        
        // Assert
        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData("123 Main St, Anytown USA")]
    [InlineData("")]
    [InlineData(null)]
    public void Address_Should_Accept_Optional_Values(string? address)
    {
        // Arrange
        var coffeeShop = new CoffeeShop 
        { 
            Name = "Test Shop",
            Address = address 
        };
        var context = new ValidationContext(coffeeShop);
        var results = new List<ValidationResult>();
        
        // Act
        var isValid = Validator.TryValidateObject(coffeeShop, context, results, true);
        
        // Assert
        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }

    [Fact]
    public void Address_Should_Reject_Value_Longer_Than_200_Characters()
    {
        // Arrange
        var longAddress = new string('A', CoffeeShop.AddressMaxLength + 1);
        var coffeeShop = new CoffeeShop 
        { 
            Name = "Test Shop",
            Address = longAddress 
        };
        var context = new ValidationContext(coffeeShop);
        var results = new List<ValidationResult>();
        
        // Act
        var isValid = Validator.TryValidateObject(coffeeShop, context, results, true);
        
        // Assert
        isValid.Should().BeFalse();
        results.Should().ContainSingle(r => r.MemberNames.Contains("Address"));
    }

    [Fact]
    public void Address_Should_Accept_Value_With_Exactly_200_Characters()
    {
        // Arrange
        var maxLengthAddress = new string('A', CoffeeShop.AddressMaxLength);
        var coffeeShop = new CoffeeShop 
        { 
            Name = "Test Shop",
            Address = maxLengthAddress 
        };
        var context = new ValidationContext(coffeeShop);
        var results = new List<ValidationResult>();
        
        // Act
        var isValid = Validator.TryValidateObject(coffeeShop, context, results, true);
        
        // Assert
        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }

    [Fact]
    public void Constants_Should_Have_Expected_Values()
    {
        // Assert
        CoffeeShop.NameMaxLength.Should().Be(100);
        CoffeeShop.AddressMaxLength.Should().Be(200);
    }
}
