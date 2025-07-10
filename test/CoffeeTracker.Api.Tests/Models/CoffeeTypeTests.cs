using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Tests.Models;

/// <summary>
/// Unit tests for CoffeeType enum and its extension methods
/// </summary>
public class CoffeeTypeTests
{
    [Fact]
    public void CoffeeType_Should_Have_All_Required_Coffee_Types()
    {
        // Arrange & Act
        var coffeeTypes = Enum.GetValues<CoffeeType>();
        
        // Assert
        Assert.Contains(CoffeeType.Espresso, coffeeTypes);
        Assert.Contains(CoffeeType.Americano, coffeeTypes);
        Assert.Contains(CoffeeType.Latte, coffeeTypes);
        Assert.Contains(CoffeeType.Cappuccino, coffeeTypes);
        Assert.Contains(CoffeeType.Mocha, coffeeTypes);
        Assert.Contains(CoffeeType.Macchiato, coffeeTypes);
        Assert.Contains(CoffeeType.FlatWhite, coffeeTypes);
        Assert.Contains(CoffeeType.BlackCoffee, coffeeTypes);
    }

    [Theory]
    [InlineData(CoffeeType.Espresso, 90)]
    [InlineData(CoffeeType.Americano, 120)]
    [InlineData(CoffeeType.Latte, 80)]
    [InlineData(CoffeeType.Cappuccino, 80)]
    [InlineData(CoffeeType.Mocha, 90)]
    [InlineData(CoffeeType.Macchiato, 120)]
    [InlineData(CoffeeType.FlatWhite, 130)]
    [InlineData(CoffeeType.BlackCoffee, 95)]
    public void GetBaseCaffeineContent_Should_Return_Correct_Caffeine_Amount(CoffeeType coffeeType, int expectedCaffeine)
    {
        // Act
        var actualCaffeine = coffeeType.GetBaseCaffeineContent();
        
        // Assert
        Assert.Equal(expectedCaffeine, actualCaffeine);
    }

    [Theory]
    [InlineData(CoffeeType.Espresso, CoffeeSize.Small, 72)] // 90 * 0.8
    [InlineData(CoffeeType.Espresso, CoffeeSize.Medium, 90)] // 90 * 1.0
    [InlineData(CoffeeType.Espresso, CoffeeSize.Large, 117)] // 90 * 1.3
    [InlineData(CoffeeType.Espresso, CoffeeSize.ExtraLarge, 144)] // 90 * 1.6
    [InlineData(CoffeeType.Americano, CoffeeSize.Small, 96)] // 120 * 0.8
    [InlineData(CoffeeType.FlatWhite, CoffeeSize.Large, 169)] // 130 * 1.3
    public void GetCaffeineContent_Should_Calculate_Correct_Amount_With_Size_Multiplier(
        CoffeeType coffeeType, CoffeeSize size, int expectedCaffeine)
    {
        // Act
        var actualCaffeine = coffeeType.GetCaffeineContent(size);
        
        // Assert
        Assert.Equal(expectedCaffeine, actualCaffeine);
    }

    [Theory]
    [InlineData(CoffeeType.Espresso, "Espresso")]
    [InlineData(CoffeeType.Americano, "Americano")]
    [InlineData(CoffeeType.Latte, "Latte")]
    [InlineData(CoffeeType.Cappuccino, "Cappuccino")]
    [InlineData(CoffeeType.Mocha, "Mocha")]
    [InlineData(CoffeeType.Macchiato, "Macchiato")]
    [InlineData(CoffeeType.FlatWhite, "Flat White")]
    [InlineData(CoffeeType.BlackCoffee, "Black Coffee")]
    public void GetDisplayName_Should_Return_User_Friendly_Name(CoffeeType coffeeType, string expectedDisplayName)
    {
        // Act
        var actualDisplayName = coffeeType.GetDisplayName();
        
        // Assert
        Assert.Equal(expectedDisplayName, actualDisplayName);
    }
}
