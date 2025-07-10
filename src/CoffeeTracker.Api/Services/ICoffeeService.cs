using CoffeeTracker.Api.DTOs;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service interface for coffee operations with session management and business rules
/// </summary>
public interface ICoffeeService
{
    /// <summary>
    /// Creates a new coffee entry for the specified session
    /// </summary>
    /// <param name="request">The coffee entry creation request</param>
    /// <param name="sessionId">The anonymous session identifier</param>
    /// <returns>The created coffee entry response</returns>
    Task<CoffeeEntryResponse> CreateCoffeeEntryAsync(CreateCoffeeEntryRequest request, string sessionId);

    /// <summary>
    /// Gets coffee entries for a specific session and optional date
    /// </summary>
    /// <param name="sessionId">The anonymous session identifier</param>
    /// <param name="date">The date to filter entries (optional, defaults to today)</param>
    /// <returns>List of coffee entries for the specified session and date</returns>
    Task<IEnumerable<CoffeeEntryResponse>> GetCoffeeEntriesAsync(string sessionId, DateTime? date = null);

    /// <summary>
    /// Gets daily summary including total entries and caffeine for a session
    /// </summary>
    /// <param name="sessionId">The anonymous session identifier</param>
    /// <param name="date">The date to get summary for (optional, defaults to today)</param>
    /// <returns>Daily summary for the specified session and date</returns>
    Task<DailySummaryResponse> GetDailySummaryAsync(string sessionId, DateTime? date = null);
}
