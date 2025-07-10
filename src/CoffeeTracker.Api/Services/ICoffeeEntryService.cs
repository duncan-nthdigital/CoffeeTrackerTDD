using CoffeeTracker.Api.DTOs;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service interface for coffee entry operations
/// </summary>
public interface ICoffeeEntryService
{
    /// <summary>
    /// Creates a new coffee entry
    /// </summary>
    /// <param name="request">The coffee entry creation request</param>
    /// <returns>The created coffee entry response</returns>
    Task<CoffeeEntryResponse> CreateCoffeeEntryAsync(CreateCoffeeEntryRequest request);

    /// <summary>
    /// Gets coffee entries for a specific date
    /// </summary>
    /// <param name="date">The date to filter entries (optional, defaults to today)</param>
    /// <returns>List of coffee entries for the specified date</returns>
    Task<IEnumerable<CoffeeEntryResponse>> GetCoffeeEntriesAsync(DateOnly? date = null);
}
