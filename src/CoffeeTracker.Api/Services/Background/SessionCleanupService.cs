namespace CoffeeTracker.Api.Services.Background;

/// <summary>
/// Background service that periodically cleans up expired anonymous session data
/// </summary>
public class SessionCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SessionCleanupService> _logger;
    private readonly TimeSpan _sessionExpiration;
    private readonly TimeSpan _cleanupInterval;

    /// <summary>
    /// Initializes a new instance of the SessionCleanupService class
    /// </summary>
    /// <param name="serviceProvider">Service provider for creating scoped services</param>
    /// <param name="logger">Logger instance</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="environment">Hosting environment</param>
    public SessionCleanupService(
        IServiceProvider serviceProvider,
        ILogger<SessionCleanupService> logger,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        // Get configuration values or use defaults
        // Default session expiration: 24 hours
        // Default cleanup interval: 1 hour
        _sessionExpiration = TimeSpan.FromHours(
            configuration.GetValue<double>("SessionManagement:ExpirationHours", 24));
        _cleanupInterval = TimeSpan.FromHours(
            configuration.GetValue<double>("SessionManagement:CleanupIntervalHours", 1));
    }

    /// <summary>
    /// Executes the background task
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Session cleanup service started with expiration period: {ExpirationPeriod}, cleanup interval: {CleanupInterval}",
            _sessionExpiration,
            _cleanupInterval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Wait for the next cleanup interval
                await Task.Delay(_cleanupInterval, stoppingToken);
                
                // Perform cleanup
                await CleanupExpiredSessionsAsync();
            }
            catch (OperationCanceledException)
            {
                // Service is shutting down, just break the loop
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cleaning up expired sessions");
                
                // Wait a bit before retrying
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        
        _logger.LogInformation("Session cleanup service stopped");
    }

    /// <summary>
    /// Cleans up expired anonymous session data
    /// </summary>
    private async Task CleanupExpiredSessionsAsync()
    {
        _logger.LogInformation("Running scheduled cleanup of expired session data");

        // Create a scope to get scoped services
        using var scope = _serviceProvider.CreateScope();
        var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

        // Clean up expired sessions
        var removedCount = await sessionService.CleanupExpiredSessionsAsync(_sessionExpiration);
        
        _logger.LogInformation("Session cleanup completed. Removed {Count} expired entries", removedCount);
    }
}
