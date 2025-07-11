using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Models;
using CoffeeTracker.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoffeeTracker.Api.Tests.Services;

/// <summary>
/// Unit tests for CoffeeEntryService
/// </summary>
public class CoffeeEntryServiceTests
{
    private readonly Mock<ILogger<CoffeeEntryService>> _mockLogger;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly string _testSessionId = "test-session-id-123456789012345678901234";

    public CoffeeEntryServiceTests()
    {
        _mockLogger = new Mock<ILogger<CoffeeEntryService>>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        
        // Set up the HTTP context with a session ID
        var httpContext = new DefaultHttpContext();
        httpContext.Items["SessionId"] = _testSessionId;
        _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(httpContext);
    }

    [Fact]
    public async Task CreateCoffeeEntryAsync_Should_Create_And_Return_CoffeeEntry()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium"
        };

        using var context = new CoffeeTrackerDbContext(options);
        var service = new CoffeeEntryService(context, _mockLogger.Object, _mockHttpContextAccessor.Object);

        // Act
        var result = await service.CreateCoffeeEntryAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.CoffeeType.Should().Be("Latte");
        result.Size.Should().Be("Medium");
        result.CaffeineAmount.Should().Be(80); // Based on CoffeeEntry calculation logic
        result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));

        // Verify it was saved to database
        var savedEntry = await context.CoffeeEntries.FindAsync(result.Id);
        savedEntry.Should().NotBeNull();
        savedEntry!.CoffeeType.Should().Be("Latte");
        savedEntry.Size.Should().Be("Medium");
    }

    [Fact]
    public async Task CreateCoffeeEntryAsync_Should_Create_Entry_With_Optional_Fields()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var customTimestamp = DateTime.UtcNow.AddHours(-2);
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Americano",
            Size = "Large",
            Source = "Local Coffee Shop",
            Timestamp = customTimestamp
        };

        using var context = new CoffeeTrackerDbContext(options);
        var service = new CoffeeEntryService(context, _mockLogger.Object, _mockHttpContextAccessor.Object);

        // Act
        var result = await service.CreateCoffeeEntryAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.CoffeeType.Should().Be("Americano");
        result.Size.Should().Be("Large");
        result.Source.Should().Be("Local Coffee Shop");
        result.Timestamp.Should().Be(customTimestamp);
        result.CaffeineAmount.Should().Be(156); // 120 * 1.3 for Large Americano
    }

    [Fact]
    public async Task CreateCoffeeEntryAsync_Should_Throw_ArgumentNullException_When_Request_IsNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new CoffeeTrackerDbContext(options);
        var service = new CoffeeEntryService(context, _mockLogger.Object, _mockHttpContextAccessor.Object);

        // Act
        Func<Task> act = async () => await service.CreateCoffeeEntryAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("request");
    }

    [Fact]
    public async Task GetCoffeeEntriesAsync_Should_Return_Entries_For_Today_When_No_Date_Specified()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new CoffeeTrackerDbContext(options);
        
        // Add test data
        var today = DateTime.UtcNow.Date;
        var todayEntries = new List<CoffeeEntry>
        {
            new() { CoffeeType = "Latte", Size = "Medium", Timestamp = today.AddHours(9), SessionId = _testSessionId },
            new() { CoffeeType = "Espresso", Size = "Small", Timestamp = today.AddHours(14), SessionId = _testSessionId }
        };
        
        var yesterdayEntry = new CoffeeEntry 
        { 
            CoffeeType = "Americano", 
            Size = "Large", 
            Timestamp = today.AddDays(-1).AddHours(10),
            SessionId = _testSessionId
        };

        context.CoffeeEntries.AddRange(todayEntries);
        context.CoffeeEntries.Add(yesterdayEntry);
        await context.SaveChangesAsync();

        var service = new CoffeeEntryService(context, _mockLogger.Object, _mockHttpContextAccessor.Object);

        // Act
        var result = await service.GetCoffeeEntriesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(entry => 
            entry.Timestamp.Date.Should().Be(today.Date));
        result.Should().BeInAscendingOrder(entry => entry.Timestamp);
    }

    [Fact]
    public async Task GetCoffeeEntriesAsync_Should_Return_Entries_For_Specified_Date()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new CoffeeTrackerDbContext(options);
        
        var targetDate = DateTime.Today.AddDays(-5);
        var targetDateOnly = DateOnly.FromDateTime(targetDate);
        
        // Add test data for different dates
        var entries = new List<CoffeeEntry>
        {
            new() { CoffeeType = "Latte", Size = "Medium", Timestamp = targetDate.AddHours(9), SessionId = _testSessionId },
            new() { CoffeeType = "Espresso", Size = "Small", Timestamp = targetDate.AddHours(14), SessionId = _testSessionId },
            new() { CoffeeType = "Americano", Size = "Large", Timestamp = DateTime.Today.AddHours(10), SessionId = _testSessionId }
        };

        context.CoffeeEntries.AddRange(entries);
        await context.SaveChangesAsync();

        var service = new CoffeeEntryService(context, _mockLogger.Object, _mockHttpContextAccessor.Object);

        // Act
        var result = await service.GetCoffeeEntriesAsync(targetDateOnly);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(entry => 
            DateOnly.FromDateTime(entry.Timestamp).Should().Be(targetDateOnly));
        result.Should().BeInAscendingOrder(entry => entry.Timestamp);
    }

    [Fact]
    public async Task GetCoffeeEntriesAsync_Should_Return_Empty_List_When_No_Entries_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new CoffeeTrackerDbContext(options);
        var service = new CoffeeEntryService(context, _mockLogger.Object, _mockHttpContextAccessor.Object);

        var futureDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));

        // Act
        var result = await service.GetCoffeeEntriesAsync(futureDate);

        // Assert
        result.Should().BeEmpty();
    }
}
