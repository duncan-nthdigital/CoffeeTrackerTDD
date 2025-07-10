using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Tests.Models;

/// <summary>
/// Unit tests for CoffeeSize enum and its extension methods
/// </summary>
public class CoffeeSizeTests
{
    [Fact]
    public void CoffeeSize_Should_Have_All_Required_Coffee_Sizes()
    {
        // Arrange & Act
        var coffeeSizes = Enum.GetValues<CoffeeSize>();
        
        // Assert
        Assert.Contains(CoffeeSize.Small, coffeeSizes);
        Assert.Contains(CoffeeSize.Medium, coffeeSizes);
        Assert.Contains(CoffeeSize.Large, coffeeSizes);
        Assert.Contains(CoffeeSize.ExtraLarge, coffeeSizes);
    }

    [Theory]
    [InlineData(CoffeeSize.Small, 0.8)]
    [InlineData(CoffeeSize.Medium, 1.0)]
    [InlineData(CoffeeSize.Large, 1.3)]
    [InlineData(CoffeeSize.ExtraLarge, 1.6)]
    public void GetSizeMultiplier_Should_Return_Correct_Multiplier(CoffeeSize size, double expectedMultiplier)
    {
        // Act
        var actualMultiplier = size.GetSizeMultiplier();
        
        // Assert
        Assert.Equal(expectedMultiplier, actualMultiplier, 1); // 1 decimal place precision
    }

    [Theory]
    [InlineData(CoffeeSize.Small, "Small")]
    [InlineData(CoffeeSize.Medium, "Medium")]
    [InlineData(CoffeeSize.Large, "Large")]
    [InlineData(CoffeeSize.ExtraLarge, "Extra Large")]
    public void GetDisplayName_Should_Return_User_Friendly_Name(CoffeeSize size, string expectedDisplayName)
    {
        // Act
        var actualDisplayName = size.GetDisplayName();
        
        // Assert
        Assert.Equal(expectedDisplayName, actualDisplayName);
    }
}
