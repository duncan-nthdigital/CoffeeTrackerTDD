using CoffeeTracker.Api.Middleware;
using CoffeeTracker.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoffeeTracker.Api.Tests.Middleware;

public class AnonymousSessionMiddlewareTests
{
    private readonly Mock<ILogger<AnonymousSessionMiddleware>> _loggerMock;
    private readonly Mock<ISessionService> _sessionServiceMock;
    private readonly RequestDelegate _nextDelegate;

    public AnonymousSessionMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<AnonymousSessionMiddleware>>();
        _sessionServiceMock = new Mock<ISessionService>();
        
        _nextDelegate = (HttpContext context) => Task.CompletedTask;
    }

    [Fact]
    public async Task InvokeAsync_SetsSessionIdInContext()
    {
        // Arrange
        var testSessionId = "test-session-id";
        var httpContext = new DefaultHttpContext();
        
        _sessionServiceMock.Setup(s => s.GetOrCreateSessionId(httpContext))
            .Returns(testSessionId);
            
        var middleware = new AnonymousSessionMiddleware(_nextDelegate, _loggerMock.Object);
        
        // Act
        await middleware.InvokeAsync(httpContext, _sessionServiceMock.Object);
        
        // Assert
        Assert.True(httpContext.Items.ContainsKey("SessionId"));
        Assert.Equal(testSessionId, httpContext.Items["SessionId"]);
        
        // Verify session service was called
        _sessionServiceMock.Verify(s => s.GetOrCreateSessionId(httpContext), Times.Once);
    }
    
    [Fact]
    public async Task InvokeAsync_SessionServiceThrowsException_CallsNextMiddleware()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var wasCalled = false;
        
        _sessionServiceMock.Setup(s => s.GetOrCreateSessionId(httpContext))
            .Throws<Exception>();
            
        var next = new RequestDelegate(context =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        });
        
        var middleware = new AnonymousSessionMiddleware(next, _loggerMock.Object);
        
        // Act
        await middleware.InvokeAsync(httpContext, _sessionServiceMock.Object);
        
        // Assert
        Assert.True(wasCalled);
    }
    
    [Fact]
    public void UseAnonymousSession_AddsMiddlewareToAppBuilder()
    {
        // Arrange
        var appBuilderMock = new Mock<IApplicationBuilder>();
        appBuilderMock.Setup(x => x.UseMiddleware<AnonymousSessionMiddleware>())
            .Returns(appBuilderMock.Object);
        
        // Act
        var result = AnonymousSessionMiddlewareExtensions.UseAnonymousSession(appBuilderMock.Object);
        
        // Assert
        appBuilderMock.Verify(x => x.UseMiddleware<AnonymousSessionMiddleware>(), Times.Once);
        Assert.Same(appBuilderMock.Object, result);
    }
}
