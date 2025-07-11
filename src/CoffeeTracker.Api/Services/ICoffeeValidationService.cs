using CoffeeTracker.Api.DTOs;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service for validating coffee-related operations
/// </summary>
public interface ICoffeeValidationService
{
    /// <summary>
    /// Validates the creation of a new coffee entry
    /// </summary>
    /// <param name="request">The create coffee entry request</param>
    /// <param name="sessionId">The session ID</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task ValidateCreateCoffeeEntryAsync(CreateCoffeeEntryRequest request, string sessionId);
    
    /// <summary>
    /// Validates daily limits for coffee entries
    /// </summary>
    /// <param name="sessionId">The session ID</param>
    /// <param name="date">The date to validate</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task ValidateDailyLimitsAsync(string sessionId, DateTime date);
    
    /// <summary>
    /// Validates that a session exists
    /// </summary>
    /// <param name="sessionId">The session ID to validate</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task ValidateSessionAsync(string sessionId);
}
