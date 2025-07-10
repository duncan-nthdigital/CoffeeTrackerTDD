using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Exceptions;
using CoffeeTracker.Api.Models;
using CoffeeTracker.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CoffeeTracker.Api.Tests.Services;

public class CoffeeServiceTests : IDisposable
{
    private readonly DbContextOptions<CoffeeTrackerDbContext> _dbOptions;
    private readonly Mock<ILogger<CoffeeService>> _mockLogger;
    private readonly CoffeeTrackerDbContext _context;
    private readonly CoffeeService _service;

    public CoffeeServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CoffeeTrackerDbContext(_dbOptions);
        _mockLogger = new Mock<ILogger<CoffeeService>>();
        _service = new CoffeeService(_context, _mockLogger.Object);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateCoffeeEntryAsync_Should_Create_Entry_With_SessionId()
    {
        // Arrange
        var sessionId = "test-session-123";
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium",
            Source = "Test Cafe"
        };

        // Act
        var result = await _service.CreateCoffeeEntryAsync(request, sessionId);

        // Assert
        result.Should().NotBeNull();
        result.CoffeeType.Should().Be(request.CoffeeType);
        result.Size.Should().Be(request.Size);
        result.Source.Should().Be(request.Source);
        result.Id.Should().BeGreaterThan(0);
        result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task CreateCoffeeEntryAsync_Should_Throw_When_Daily_Entry_Limit_Exceeded()
    {
        // Arrange
        var sessionId = "test-session-limit";
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Espresso",
            Size = "Small"
        };

        // Create 10 entries (the daily limit)
        for (int i = 0; i < 10; i++)
        {
            await _service.CreateCoffeeEntryAsync(request, sessionId);
        }

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DailyEntryLimitExceededException>(
            () => _service.CreateCoffeeEntryAsync(request, sessionId));

        exception.CurrentCount.Should().Be(10);
        exception.MaxAllowed.Should().Be(10);
    }

    [Fact]
    public async Task CreateCoffeeEntryAsync_Should_Throw_When_Daily_Caffeine_Limit_Exceeded()
    {
        // Arrange
        var sessionId = "test-session-caffeine";
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Espresso",
            Size = "Large" // This should have high caffeine content
        };

        // Act & Assert - This should fail because we haven't implemented caffeine calculation yet
        await Assert.ThrowsAsync<DailyCaffeineLimitExceededException>(async () =>
        {
            // Try to add many high-caffeine entries
            for (int i = 0; i < 20; i++)
            {
                await _service.CreateCoffeeEntryAsync(request, sessionId);
            }
        });
    }

    [Fact]
    public async Task CreateCoffeeEntryAsync_Should_Throw_When_Future_Date_Provided()
    {
        // Arrange
        var sessionId = "test-session-future";
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium",
            Timestamp = DateTime.UtcNow.AddDays(1) // Future date
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidTimestampException>(
            () => _service.CreateCoffeeEntryAsync(request, sessionId));
    }

    [Fact]
    public async Task GetCoffeeEntriesAsync_Should_Return_Only_Session_Entries()
    {
        // Arrange
        var sessionId1 = "session-1";
        var sessionId2 = "session-2";
        
        var request1 = new CreateCoffeeEntryRequest { CoffeeType = "Latte", Size = "Medium" };
        var request2 = new CreateCoffeeEntryRequest { CoffeeType = "Cappuccino", Size = "Large" };

        await _service.CreateCoffeeEntryAsync(request1, sessionId1);
        await _service.CreateCoffeeEntryAsync(request2, sessionId2);

        // Act
        var session1Entries = await _service.GetCoffeeEntriesAsync(sessionId1);
        var session2Entries = await _service.GetCoffeeEntriesAsync(sessionId2);

        // Assert
        session1Entries.Should().HaveCount(1);
        session1Entries.First().CoffeeType.Should().Be("Latte");

        session2Entries.Should().HaveCount(1);
        session2Entries.First().CoffeeType.Should().Be("Cappuccino");
    }

    [Fact]
    public async Task GetCoffeeEntriesAsync_Should_Filter_By_Date()
    {
        // Arrange
        var sessionId = "session-date-filter";
        var now = DateTime.UtcNow;
        
        // Create two entries on different dates but within retention period
        var today = now.Date;
        var recentDate = now.AddHours(-18).Date; // 18 hours ago (different date, within 24h retention)

        var todayRequest = new CreateCoffeeEntryRequest 
        { 
            CoffeeType = "Latte", 
            Size = "Medium",
            Timestamp = today.AddHours(10) // Today at 10 AM
        };
        
        var recentRequest = new CreateCoffeeEntryRequest 
        { 
            CoffeeType = "Espresso", 
            Size = "Small",
            Timestamp = now.AddHours(-18) // 18 hours ago (different date but within retention)
        };

        await _service.CreateCoffeeEntryAsync(todayRequest, sessionId);
        await _service.CreateCoffeeEntryAsync(recentRequest, sessionId);

        // Act
        var todayEntries = await _service.GetCoffeeEntriesAsync(sessionId, today);
        var recentEntries = await _service.GetCoffeeEntriesAsync(sessionId, recentDate);

        // Assert
        todayEntries.Should().HaveCount(1);
        todayEntries.First().CoffeeType.Should().Be("Latte");

        recentEntries.Should().HaveCount(1);
        recentEntries.First().CoffeeType.Should().Be("Espresso");
    }

    [Fact]
    public async Task GetDailySummaryAsync_Should_Calculate_Totals_Correctly()
    {
        // Arrange
        var sessionId = "session-summary";
        var request1 = new CreateCoffeeEntryRequest { CoffeeType = "Espresso", Size = "Small" };
        var request2 = new CreateCoffeeEntryRequest { CoffeeType = "Latte", Size = "Medium" };

        await _service.CreateCoffeeEntryAsync(request1, sessionId);
        await _service.CreateCoffeeEntryAsync(request2, sessionId);

        // Act
        var summary = await _service.GetDailySummaryAsync(sessionId);

        // Assert
        summary.Should().NotBeNull();
        summary.TotalEntries.Should().Be(2);
        summary.TotalCaffeineMilligrams.Should().BeGreaterThan(0);
        summary.Entries.Should().HaveCount(2);
        summary.Date.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
    }

    [Fact]
    public async Task GetDailySummaryAsync_Should_Set_Limit_Flags_Correctly()
    {
        // Arrange
        var sessionId = "session-limits";
        var request = new CreateCoffeeEntryRequest { CoffeeType = "Espresso", Size = "Small" };

        // Create entries near the limit
        for (int i = 0; i < 9; i++)
        {
            await _service.CreateCoffeeEntryAsync(request, sessionId);
        }

        // Act
        var summary = await _service.GetDailySummaryAsync(sessionId);

        // Assert
        summary.Should().NotBeNull();
        summary.TotalEntries.Should().Be(9);
        summary.HasReachedDailyLimit.Should().BeFalse(); // Not yet at limit of 10
        
        // Add one more to reach the limit
        await _service.CreateCoffeeEntryAsync(request, sessionId);
        var limitSummary = await _service.GetDailySummaryAsync(sessionId);
        limitSummary.HasReachedDailyLimit.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task CreateCoffeeEntryAsync_Should_Throw_ArgumentException_For_Invalid_SessionId(string? sessionId)
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateCoffeeEntryAsync(request, sessionId!));
    }

    [Fact]
    public async Task Service_Should_Cleanup_Old_Anonymous_Entries()
    {
        // Arrange
        var sessionId = "old-session";
        var oldTimestamp = DateTime.UtcNow.AddHours(-25); // Older than 24 hours

        // Directly insert old entry to simulate expired data
        var oldEntry = new CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = "Small",
            SessionId = sessionId,
            Timestamp = oldTimestamp,
            Source = "Old Cafe"
        };

        _context.CoffeeEntries.Add(oldEntry);
        await _context.SaveChangesAsync();

        // Act - Creating a new entry should trigger cleanup
        var request = new CreateCoffeeEntryRequest { CoffeeType = "Latte", Size = "Medium" };
        await _service.CreateCoffeeEntryAsync(request, sessionId);

        // Assert - Old entry should be cleaned up
        var allEntries = await _context.CoffeeEntries.Where(e => e.SessionId == sessionId).ToListAsync();
        allEntries.Should().HaveCount(1);
        allEntries.First().CoffeeType.Should().Be("Latte");
    }
}
