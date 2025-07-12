using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Models;
using CoffeeTracker.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoffeeTracker.Api.Tests.Services;

public class SessionManagementTests
{
    private readonly Mock<ILogger<CoffeeEntryService>> _loggerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CoffeeTrackerDbContext _dbContext;
    private readonly HttpContext _httpContext;
    private readonly string _testSessionId = "test-session-id-123456789012345678901234";

    public SessionManagementTests()
    {
        _loggerMock = new Mock<ILogger<CoffeeEntryService>>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _httpContext = new DefaultHttpContext();
        
        // Set up the session ID in the HTTP context
        _httpContext.Items["SessionId"] = _testSessionId;
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(_httpContext);
        
        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: $"CoffeeTrackerTest_{Guid.NewGuid()}")
            .Options;
            
        _dbContext = new CoffeeTrackerDbContext(options);
    }

    private CoffeeEntryService CreateCoffeeEntryService()
    {
        var coffeeService = new CoffeeService(_dbContext, new Mock<ILogger<CoffeeService>>().Object);
        return new CoffeeEntryService(coffeeService, _loggerMock.Object, _httpContextAccessorMock.Object);
    }
    
    [Fact]
    public async Task CreateCoffeeEntryAsync_AddsSessionIdToEntry()
    {
        // Arrange
        var service = CreateCoffeeEntryService();
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium",
            Source = "Home"
        };
        
        // Act
        var response = await service.CreateCoffeeEntryAsync(request);
        
        // Assert
        var entry = await _dbContext.CoffeeEntries.FirstOrDefaultAsync();
        Assert.NotNull(entry);
        Assert.Equal(_testSessionId, entry.SessionId);
    }
    
    [Fact]
    public async Task GetCoffeeEntriesAsync_OnlyReturnsEntriesForCurrentSession()
    {
        // Arrange
        var service = CreateCoffeeEntryService();
        
        // Add entries with different session IDs
        await _dbContext.CoffeeEntries.AddRangeAsync(new[]
        {
            new CoffeeEntry { CoffeeType = "Latte", Size = "Medium", SessionId = _testSessionId, Timestamp = DateTime.UtcNow },
            new CoffeeEntry { CoffeeType = "Espresso", Size = "Small", SessionId = _testSessionId, Timestamp = DateTime.UtcNow },
            new CoffeeEntry { CoffeeType = "Cappuccino", Size = "Large", SessionId = "different-session-id", Timestamp = DateTime.UtcNow }
        });
        await _dbContext.SaveChangesAsync();
        
        // Act
        var entries = await service.GetCoffeeEntriesAsync();
        
        // Assert
        Assert.Equal(2, entries.Count());
        Assert.All(entries, e => Assert.Contains(e.CoffeeType, new[] { "Latte", "Espresso" }));
    }
    
    [Fact]
    public async Task CreateCoffeeEntryAsync_WithNoSessionId_ThrowsInvalidOperation()
    {
        // Arrange
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext());
        
        var service = CreateCoffeeEntryService();
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium"
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await service.CreateCoffeeEntryAsync(request)
        );
    }
    
    [Fact]
    public async Task GetCoffeeEntriesAsync_WithNoSessionId_ThrowsInvalidOperation()
    {
        // Arrange
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext());
        
        var service = CreateCoffeeEntryService();
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await service.GetCoffeeEntriesAsync()
        );
    }
}
