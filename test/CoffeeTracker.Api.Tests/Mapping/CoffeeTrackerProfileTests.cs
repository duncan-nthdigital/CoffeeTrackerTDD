using AutoMapper;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Mapping;
using CoffeeTracker.Api.Models;
using FluentAssertions;
using Xunit;

namespace CoffeeTracker.Api.Tests.Mapping;

/// <summary>
/// Unit tests for AutoMapper profile mappings
/// </summary>
public class CoffeeTrackerProfileTests
{
    private readonly IMapper _mapper;

    public CoffeeTrackerProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
            cfg.AddProfile<CoffeeTrackerProfile>());
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void Should_Map_CoffeeEntry_To_CoffeeEntryResponse()
    {
        // Arrange
        var coffeeEntry = new CoffeeEntry
        {
            Id = 1,
            CoffeeType = "Latte",
            Size = "Medium",
            Source = "Starbucks",
            Timestamp = DateTime.UtcNow,
            SessionId = "session123"
        };

        // Act
        var response = _mapper.Map<CoffeeEntryResponse>(coffeeEntry);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(coffeeEntry.Id);
        response.CoffeeType.Should().Be(coffeeEntry.CoffeeType);
        response.Size.Should().Be(coffeeEntry.Size);
        response.Source.Should().Be(coffeeEntry.Source);
        response.Timestamp.Should().Be(coffeeEntry.Timestamp);
        response.CaffeineAmount.Should().Be(coffeeEntry.CaffeineAmount);
        response.FormattedTimestamp.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Map_CreateCoffeeEntryRequest_To_CoffeeEntry()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Cappuccino",
            Size = "Large",
            Source = "Local Café",
            Timestamp = DateTime.UtcNow.AddMinutes(-30)
        };

        // Act
        var coffeeEntry = _mapper.Map<CoffeeEntry>(request);

        // Assert
        coffeeEntry.Should().NotBeNull();
        coffeeEntry.CoffeeType.Should().Be(request.CoffeeType);
        coffeeEntry.Size.Should().Be(request.Size);
        coffeeEntry.Source.Should().Be(request.Source);
        coffeeEntry.Id.Should().Be(0); // Should be ignored in mapping
        coffeeEntry.SessionId.Should().BeEmpty(); // Should be ignored in mapping, default to empty string
        // Timestamp should be ignored in mapping, not set from request
    }

    [Fact]
    public void Should_Format_Timestamp_Correctly_In_Response()
    {
        // Arrange
        var testDate = new DateTime(2025, 1, 15, 14, 30, 45, DateTimeKind.Utc);
        var coffeeEntry = new CoffeeEntry
        {
            Id = 42,
            CoffeeType = "Mocha",
            Size = "Small",
            Source = "Coffee Bean",
            Timestamp = testDate,
            SessionId = "test-session-456"
        };

        // Act
        var response = _mapper.Map<CoffeeEntryResponse>(coffeeEntry);

        // Assert
        response.FormattedTimestamp.Should().Be("2025-01-15 14:30:45");
    }

    [Fact]
    public void Should_Have_Valid_Configuration()
    {
        // Arrange & Act
        var configuration = new MapperConfiguration(cfg =>
            cfg.AddProfile<CoffeeTrackerProfile>());

        // Assert
        configuration.AssertConfigurationIsValid();
    }
}
