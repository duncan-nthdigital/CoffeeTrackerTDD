using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Exceptions;
using CoffeeTracker.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CoffeeTracker.Api.Services;

/// <summary>
/// Service for validating coffee-related operations
/// </summary>
public class CoffeeValidationService : ICoffeeValidationService
{
    private readonly ICoffeeEntryRepository _repository;
    private const int MaxDailyEntries = 10;
    private const int MaxDailyCaffeine = 1000; // mg

    /// <summary>
    /// Initializes a new instance of the CoffeeValidationService class
    /// </summary>
    /// <param name="repository">The coffee entry repository</param>
    public CoffeeValidationService(ICoffeeEntryRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Validates the creation of a new coffee entry
    /// </summary>
    /// <param name="request">The create coffee entry request</param>
    /// <param name="sessionId">The session ID</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task ValidateCreateCoffeeEntryAsync(CreateCoffeeEntryRequest request, string sessionId)
    {
        // Validate session exists
        await ValidateSessionAsync(sessionId);
        
        // Validate request data
        ValidateCoffeeTypeAndSize(request);
        ValidateTimestamp(request.Timestamp);
        
        // Validate business rules
        var date = request.Timestamp?.Date ?? DateTime.UtcNow.Date;
        await ValidateDailyLimitsAsync(sessionId, date);
    }
    
    /// <summary>
    /// Validates daily limits for coffee entries
    /// </summary>
    /// <param name="sessionId">The session ID</param>
    /// <param name="date">The date to validate</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task ValidateDailyLimitsAsync(string sessionId, DateTime date)
    {
        var dailyEntries = await _repository.GetCoffeeEntriesBySessionAndDateAsync(sessionId, date);
        
        // Check entry count limit
        if (dailyEntries.Count >= MaxDailyEntries)
        {
            throw new BusinessRuleViolationException("DailyEntryLimit", 
                $"Daily entry limit exceeded. Current: {dailyEntries.Count}, Maximum allowed: {MaxDailyEntries}");
        }
        
        // Check caffeine limit
        int totalCaffeine = dailyEntries.Sum(e => e.CaffeineMg);
        
        if (totalCaffeine >= MaxDailyCaffeine)
        {
            throw new DailyCaffeineLimitExceededException(totalCaffeine, 0, MaxDailyCaffeine);
        }
        
        // The average caffeine content of a typical coffee is around 100-150mg
        // Assuming the new entry would add an average of 100mg, check if that would exceed the limit
        const int estimatedAdditionalCaffeine = 100;
        
        if (totalCaffeine + estimatedAdditionalCaffeine > MaxDailyCaffeine)
        {
            throw new DailyCaffeineLimitExceededException(
                totalCaffeine, 
                estimatedAdditionalCaffeine, 
                MaxDailyCaffeine);
        }
    }
    
    /// <summary>
    /// Validates that a session exists
    /// </summary>
    /// <param name="sessionId">The session ID to validate</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task ValidateSessionAsync(string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
        {
            throw new SessionNotFoundException("empty", "Session ID cannot be empty");
        }
        
        bool sessionExists = await _repository.SessionExistsAsync(sessionId);
        
        if (!sessionExists)
        {
            throw new SessionNotFoundException(sessionId);
        }
    }
    
    private void ValidateCoffeeTypeAndSize(CreateCoffeeEntryRequest request)
    {
        // Validate coffee type
        if (!Enum.TryParse<CoffeeType>(request.CoffeeType, out _))
        {
            throw new ValidationException($"Invalid coffee type: {request.CoffeeType}");
        }
        
        // Validate coffee size
        if (!Enum.TryParse<CoffeeSize>(request.Size, out _))
        {
            throw new ValidationException($"Invalid coffee size: {request.Size}");
        }
    }
    
    private void ValidateTimestamp(DateTime? timestamp)
    {
        if (timestamp.HasValue)
        {
            // Check for future dates
            if (timestamp.Value > DateTime.UtcNow)
            {
                throw new ValidationException($"Timestamp cannot be in the future: {timestamp.Value}");
            }
            
            // Check for unreasonable past dates (e.g., more than 30 days ago)
            if (timestamp.Value < DateTime.UtcNow.AddDays(-30))
            {
                throw new ValidationException($"Timestamp is too far in the past: {timestamp.Value}");
            }
        }
    }
}
