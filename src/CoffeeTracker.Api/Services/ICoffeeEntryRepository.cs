using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Repository for coffee entry operations
/// </summary>
public interface ICoffeeEntryRepository
{
    /// <summary>
    /// Checks if a session exists
    /// </summary>
    /// <param name="sessionId">The session ID to check</param>
    /// <returns>True if the session exists, false otherwise</returns>
    Task<bool> SessionExistsAsync(string sessionId);
    
    /// <summary>
    /// Gets all coffee entries for a session on a specific date
    /// </summary>
    /// <param name="sessionId">The session ID</param>
    /// <param name="date">The date</param>
    /// <returns>A list of coffee entries</returns>
    Task<List<CoffeeEntry>> GetCoffeeEntriesBySessionAndDateAsync(string sessionId, DateTime date);
}
