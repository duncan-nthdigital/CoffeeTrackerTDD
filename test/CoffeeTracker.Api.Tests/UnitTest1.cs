using Microsoft.EntityFrameworkCore;
using CoffeeTracker.Api.Data;

namespace CoffeeTracker.Api.Tests;

public class CoffeeTrackerDbContextTests
{
    [Fact]
    public void DbContext_Should_Be_Created_Successfully()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act & Assert
        using var context = new CoffeeTrackerDbContext(options);
        Assert.NotNull(context);
    }
    
    [Fact]
    public void DbContext_Should_Ensure_Database_Can_Be_Created()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new CoffeeTrackerDbContext(options);
        var canConnect = context.Database.EnsureCreated();

        // Assert
        Assert.True(canConnect);
    }
}
