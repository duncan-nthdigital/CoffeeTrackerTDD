using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Repository for coffee entry operations using Entity Framework Core
/// </summary>
public class CoffeeEntryRepository : ICoffeeEntryRepository
{
    private readonly CoffeeTrackerDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the CoffeeEntryRepository class
    /// </summary>
    /// <param name="dbContext">The database context</param>
    public CoffeeEntryRepository(CoffeeTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets all coffee entries for a session on a specific date
    /// </summary>
    /// <param name="sessionId">The session ID</param>
    /// <param name="date">The date</param>
    /// <returns>A list of coffee entries</returns>
    public async Task<List<CoffeeEntry>> GetCoffeeEntriesBySessionAndDateAsync(string sessionId, DateTime date)
    {
        return await _dbContext.CoffeeEntries
            .Where(e => e.SessionId == sessionId)
            .Where(e => e.Timestamp.Date == date.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Checks if a session exists
    /// </summary>
    /// <param name="sessionId">The session ID to check</param>
    /// <returns>True if the session exists, false otherwise</returns>
    public async Task<bool> SessionExistsAsync(string sessionId)
    {
        return await _dbContext.Sessions
            .AnyAsync(s => s.SessionId == sessionId);
    }
}
