using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Models;
using CoffeeTracker.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoffeeTracker.Api.Tests.Services;

public class SessionServiceTests
{
    private readonly Mock<ILogger<SessionService>> _loggerMock;
    private readonly CoffeeTrackerDbContext _dbContext;

    public SessionServiceTests()
    {
        _loggerMock = new Mock<ILogger<SessionService>>();
        
        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: $"CoffeeTrackerTest_{Guid.NewGuid()}")
            .Options;
            
        _dbContext = new CoffeeTrackerDbContext(options);
    }

    [Fact]
    public void GetOrCreateSessionId_WithNoCookie_GeneratesNewSessionId()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var cookieCollection = new Mock<IRequestCookieCollection>();
        cookieCollection.Setup(c => c.TryGetValue(It.IsAny<string>(), out It.Ref<string?>.IsAny))
            .Returns(false);
            
        var responseCookies = new Mock<IResponseCookies>();
        responseCookies.Setup(c => c.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()));
        
        httpContext.Request.Cookies = cookieCollection.Object;
        
        // Create a custom HttpContext with our mocked cookies
        var mockHttpContext = new Mock<HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        var mockResponse = new Mock<HttpResponse>();
        
        // Setup request with the cookie collection
        mockRequest.Setup(r => r.Cookies).Returns(cookieCollection.Object);
        mockRequest.Setup(r => r.IsHttps).Returns(false);
        
        // Setup response with the response cookies
        mockResponse.Setup(r => r.Cookies).Returns(responseCookies.Object);
        
        // Link the mocks to the HttpContext
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
        mockHttpContext.Setup(c => c.Response).Returns(mockResponse.Object);
        
        var service = new SessionService(_dbContext, _loggerMock.Object);
        
        // Act
        var sessionId = service.GetOrCreateSessionId(mockHttpContext.Object);
        
        // Assert
        Assert.NotNull(sessionId);
        Assert.Equal(32, sessionId.Length);
        responseCookies.Verify(c => c.Append(
            It.Is<string>(s => s == "coffee-session"),
            It.Is<string>(s => s == sessionId),
            It.IsAny<CookieOptions>()), Times.Once);
    }
    
    [Fact]
    public void GetOrCreateSessionId_WithExistingValidCookie_ReturnsCookieValue()
    {
        // Arrange
        var existingSessionId = "abcdef1234567890abcdef1234567890"; // 32 chars
        
        var cookieCollection = new Mock<IRequestCookieCollection>();
        cookieCollection.Setup(c => c.TryGetValue(It.Is<string>(s => s == "coffee-session"), out existingSessionId))
            .Returns(true);
            
        var mockHttpContext = new Mock<HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        
        // Setup request with the cookie collection
        mockRequest.Setup(r => r.Cookies).Returns(cookieCollection.Object);
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
        
        var service = new SessionService(_dbContext, _loggerMock.Object);
        
        // Act
        var sessionId = service.GetOrCreateSessionId(mockHttpContext.Object);
        
        // Assert
        Assert.Equal(existingSessionId, sessionId);
    }
    
    [Fact]
    public void IsSessionValid_WithValidSessionId_ReturnsTrue()
    {
        // Arrange
        var service = new SessionService(_dbContext, _loggerMock.Object);
        var validSessionId = "abcdef1234567890abcdef1234567890"; // 32 chars
        
        // Act
        var isValid = service.IsSessionValid(validSessionId);
        
        // Assert
        Assert.True(isValid);
    }
    
    [Fact]
    public void IsSessionValid_WithInvalidSessionId_ReturnsFalse()
    {
        // Arrange
        var service = new SessionService(_dbContext, _loggerMock.Object);
        
        // Act & Assert - Empty
        Assert.False(service.IsSessionValid(string.Empty));
        
        // Act & Assert - Too short
        Assert.False(service.IsSessionValid("tooShort"));
        
        // Act & Assert - Too long
        Assert.False(service.IsSessionValid("thisIsMuchTooLongToBeAValidSessionIdentifier1234567890"));
    }
    
    [Fact]
    public async Task CleanupExpiredSessionsAsync_RemovesOnlyExpiredEntries()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var oldEntries = new List<CoffeeEntry>
        {
            new() { CoffeeType = "Espresso", Size = "Medium", SessionId = "session1", Timestamp = now.AddHours(-36) },
            new() { CoffeeType = "Latte", Size = "Large", SessionId = "session2", Timestamp = now.AddHours(-25) }
        };
        
        var newEntries = new List<CoffeeEntry>
        {
            new() { CoffeeType = "Cappuccino", Size = "Small", SessionId = "session3", Timestamp = now.AddHours(-12) },
            new() { CoffeeType = "Mocha", Size = "Medium", SessionId = "session4", Timestamp = now.AddHours(-1) }
        };
        
        await _dbContext.CoffeeEntries.AddRangeAsync(oldEntries);
        await _dbContext.CoffeeEntries.AddRangeAsync(newEntries);
        await _dbContext.SaveChangesAsync();
        
        var service = new SessionService(_dbContext, _loggerMock.Object);
        
        // Act
        var removedCount = await service.CleanupExpiredSessionsAsync(TimeSpan.FromHours(24));
        
        // Assert
        Assert.Equal(2, removedCount);
        Assert.Equal(2, await _dbContext.CoffeeEntries.CountAsync());
        
        // Verify remaining entries are the non-expired ones
        var remaining = await _dbContext.CoffeeEntries.ToListAsync();
        Assert.Collection(remaining,
            entry => Assert.Equal("session3", entry.SessionId),
            entry => Assert.Equal("session4", entry.SessionId)
        );
    }
}
