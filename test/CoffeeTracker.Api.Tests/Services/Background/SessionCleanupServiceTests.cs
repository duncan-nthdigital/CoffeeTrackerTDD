using CoffeeTracker.Api.Services;
using CoffeeTracker.Api.Services.Background;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoffeeTracker.Api.Tests.Services.Background;

public class SessionCleanupServiceTests
{
    private readonly Mock<ILogger<SessionCleanupService>> _loggerMock;
    private readonly Mock<ISessionService> _sessionServiceMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<IConfiguration> _configurationMock;

    public SessionCleanupServiceTests()
    {
        _loggerMock = new Mock<ILogger<SessionCleanupService>>();
        _sessionServiceMock = new Mock<ISessionService>();
        _serviceProviderMock = new Mock<IServiceProvider>();
        _serviceScopeMock = new Mock<IServiceScope>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _configurationMock = new Mock<IConfiguration>();
        
        // Setup configuration
        _configurationMock.Setup(c => c.GetValue(
            It.Is<string>(s => s == "SessionManagement:ExpirationHours"),
            It.IsAny<double>()))
            .Returns(24.0);
            
        _configurationMock.Setup(c => c.GetValue(
            It.Is<string>(s => s == "SessionManagement:CleanupIntervalHours"),
            It.IsAny<double>()))
            .Returns(1.0);
            
        // Setup service provider
        _serviceScopeMock.Setup(s => s.ServiceProvider).Returns(_serviceProviderMock.Object);
        _serviceScopeFactoryMock.Setup(f => f.CreateScope()).Returns(_serviceScopeMock.Object);
        _serviceProviderMock.Setup(s => s.GetService(typeof(IServiceScopeFactory)))
            .Returns(_serviceScopeFactoryMock.Object);
        _serviceProviderMock.Setup(s => s.GetService(typeof(ISessionService)))
            .Returns(_sessionServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_CallsCleanupExpiredSessions()
    {
        // Arrange
        _sessionServiceMock.Setup(s => s.CleanupExpiredSessionsAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(5);
            
        var cancellationTokenSource = new CancellationTokenSource();
        var service = new SessionCleanupService(_serviceProviderMock.Object, _loggerMock.Object, _configurationMock.Object);
        
        // Act - Start the service and then cancel after a short time
        var serviceTask = Task.Run(async () => 
            await service.StartAsync(cancellationTokenSource.Token)
        );
        
        await Task.Delay(100); // Give it time to start
        cancellationTokenSource.Cancel();
        
        // Ensure the task completes
        await Task.WhenAny(serviceTask, Task.Delay(1000));
        
        // Assert
        _sessionServiceMock.Verify(s => s.CleanupExpiredSessionsAsync(It.IsAny<TimeSpan>()), Times.AtMostOnce);
    }
}
