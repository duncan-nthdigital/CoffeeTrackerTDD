using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Exceptions;
using CoffeeTracker.Api.Models;
using CoffeeTracker.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoffeeTracker.Api.Tests.Services;

public class CoffeeValidationServiceTests
{
    private readonly Mock<ICoffeeEntryRepository> _repositoryMock;
    private readonly ICoffeeValidationService _validationService;

    public CoffeeValidationServiceTests()
    {
        _repositoryMock = new Mock<ICoffeeEntryRepository>();
        _validationService = new CoffeeValidationService(_repositoryMock.Object);
    }
    
    [Fact]
    public async Task ValidateSessionAsync_WithValidSessionId_DoesNotThrow()
    {
        // Arrange
        var sessionId = "valid-session";
        _repositoryMock.Setup(r => r.SessionExistsAsync(sessionId))
            .ReturnsAsync(true);
            
        // Act & Assert
        await _validationService.ValidateSessionAsync(sessionId);
    }
    
    [Fact]
    public async Task ValidateSessionAsync_WithInvalidSessionId_ThrowsSessionNotFoundException()
    {
        // Arrange
        var sessionId = "invalid-session";
        _repositoryMock.Setup(r => r.SessionExistsAsync(sessionId))
            .ReturnsAsync(false);
            
        // Act & Assert
        await Assert.ThrowsAsync<SessionNotFoundException>(() => 
            _validationService.ValidateSessionAsync(sessionId));
    }
    
    [Fact]
    public async Task ValidateDailyLimitsAsync_WithinLimits_DoesNotThrow()
    {
        // Arrange
        var sessionId = "test-session";
        var date = DateTime.UtcNow.Date;
        
        var entries = new List<CoffeeEntry>
        {
            new() { SessionId = sessionId, CaffeineMg = 200, Timestamp = date.AddHours(9) },
            new() { SessionId = sessionId, CaffeineMg = 150, Timestamp = date.AddHours(12) }
        };
        
        _repositoryMock.Setup(r => r.GetCoffeeEntriesBySessionAndDateAsync(sessionId, date))
            .ReturnsAsync(entries);
            
        // Act & Assert
        await _validationService.ValidateDailyLimitsAsync(sessionId, date);
    }
    
    [Fact]
    public async Task ValidateDailyLimitsAsync_ExceedingEntryCount_ThrowsBusinessRuleViolationException()
    {
        // Arrange
        var sessionId = "test-session";
        var date = DateTime.UtcNow.Date;
        
        var entries = Enumerable.Range(1, 10).Select(i => new CoffeeEntry 
        { 
            SessionId = sessionId, 
            CaffeineMg = 100, 
            Timestamp = date.AddHours(i) 
        }).ToList();
        
        _repositoryMock.Setup(r => r.GetCoffeeEntriesBySessionAndDateAsync(sessionId, date))
            .ReturnsAsync(entries);
            
        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessRuleViolationException>(() => 
            _validationService.ValidateDailyLimitsAsync(sessionId, date));
            
        Assert.Equal("DailyEntryLimit", exception.RuleName);
        Assert.Contains("limit exceeded", exception.Message);
    }
    
    [Fact]
    public async Task ValidateDailyLimitsAsync_ExceedingCaffeineLimit_ThrowsDailyCaffeineLimitExceededException()
    {
        // Arrange
        var sessionId = "test-session";
        var date = DateTime.UtcNow.Date;
        
        var entries = new List<CoffeeEntry>
        {
            new() { SessionId = sessionId, CaffeineMg = 600, Timestamp = date.AddHours(9) },
            new() { SessionId = sessionId, CaffeineMg = 500, Timestamp = date.AddHours(12) }
        };
        
        _repositoryMock.Setup(r => r.GetCoffeeEntriesBySessionAndDateAsync(sessionId, date))
            .ReturnsAsync(entries);
            
        // Act & Assert
        var exception = await Assert.ThrowsAsync<DailyCaffeineLimitExceededException>(() => 
            _validationService.ValidateDailyLimitsAsync(sessionId, date));
            
        Assert.Contains("caffeine limit", exception.Message.ToLower());
    }
    
    [Fact]
    public async Task ValidateCreateCoffeeEntryAsync_ValidRequest_DoesNotThrow()
    {
        // Arrange
        var sessionId = "test-session";
        var date = DateTime.UtcNow.Date;
        
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Espresso",
            Size = "Medium",
            Timestamp = date.AddHours(9)
        };
        
        var entries = new List<CoffeeEntry>
        {
            new() { SessionId = sessionId, CaffeineMg = 200, Timestamp = date.AddHours(9) }
        };
        
        _repositoryMock.Setup(r => r.GetCoffeeEntriesBySessionAndDateAsync(sessionId, It.IsAny<DateTime>()))
            .ReturnsAsync(entries);
            
        _repositoryMock.Setup(r => r.SessionExistsAsync(sessionId))
            .ReturnsAsync(true);
            
        // Act & Assert
        await _validationService.ValidateCreateCoffeeEntryAsync(request, sessionId);
    }
    
    [Fact]
    public async Task ValidateCreateCoffeeEntryAsync_InvalidTimestamp_ThrowsValidationException()
    {
        // Arrange
        var sessionId = "test-session";
        
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Espresso",
            Size = "Medium",
            Timestamp = DateTime.UtcNow.AddDays(1) // Future date
        };
        
        _repositoryMock.Setup(r => r.SessionExistsAsync(sessionId))
            .ReturnsAsync(true);
            
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _validationService.ValidateCreateCoffeeEntryAsync(request, sessionId));
    }
    
    [Fact]
    public async Task ValidateCreateCoffeeEntryAsync_InvalidCoffeeType_ThrowsValidationException()
    {
        // Arrange
        var sessionId = "test-session";
        
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "InvalidType", // Invalid coffee type
            Size = "Medium",
            Timestamp = DateTime.UtcNow
        };
        
        _repositoryMock.Setup(r => r.SessionExistsAsync(sessionId))
            .ReturnsAsync(true);
            
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _validationService.ValidateCreateCoffeeEntryAsync(request, sessionId));
    }
    
    [Fact]
    public async Task ValidateCreateCoffeeEntryAsync_InvalidSize_ThrowsValidationException()
    {
        // Arrange
        var sessionId = "test-session";
        
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Espresso",
            Size = "InvalidSize", // Invalid size
            Timestamp = DateTime.UtcNow
        };
        
        _repositoryMock.Setup(r => r.SessionExistsAsync(sessionId))
            .ReturnsAsync(true);
            
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _validationService.ValidateCreateCoffeeEntryAsync(request, sessionId));
    }
}

// Define the repository interface for use in tests
public interface ICoffeeEntryRepository
{
    Task<bool> SessionExistsAsync(string sessionId);
    Task<List<CoffeeEntry>> GetCoffeeEntriesBySessionAndDateAsync(string sessionId, DateTime date);
}
