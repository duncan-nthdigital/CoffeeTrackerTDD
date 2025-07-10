using CoffeeTracker.Api.Controllers;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoffeeTracker.Api.Tests.Controllers;

/// <summary>
/// Unit tests for CoffeeEntriesController
/// </summary>
public class CoffeeEntriesControllerTests
{
    private readonly Mock<ILogger<CoffeeEntriesController>> _mockLogger;
    private readonly Mock<ICoffeeEntryService> _mockCoffeeEntryService;

    public CoffeeEntriesControllerTests()
    {
        _mockLogger = new Mock<ILogger<CoffeeEntriesController>>();
        _mockCoffeeEntryService = new Mock<ICoffeeEntryService>();
    }

    [Fact]
    public void Should_Create_Controller_Instance()
    {
        // Arrange & Act
        var controller = new CoffeeEntriesController(_mockLogger.Object, _mockCoffeeEntryService.Object);

        // Assert
        controller.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateCoffeeEntry_Should_Return_CreatedResult_When_ValidRequest()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium"
        };

        var expectedResponse = new CoffeeEntryResponse
        {
            Id = 1,
            CoffeeType = "Latte",
            Size = "Medium",
            Timestamp = DateTime.UtcNow,
            CaffeineAmount = 80
        };

        _mockCoffeeEntryService
            .Setup(s => s.CreateCoffeeEntryAsync(request))
            .ReturnsAsync(expectedResponse);

        var controller = new CoffeeEntriesController(_mockLogger.Object, _mockCoffeeEntryService.Object);

        // Act
        var result = await controller.CreateCoffeeEntry(request);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        var responseValue = createdResult.Value.Should().BeOfType<CoffeeEntryResponse>().Subject;
        responseValue.Should().BeEquivalentTo(expectedResponse);
        
        _mockCoffeeEntryService.Verify(s => s.CreateCoffeeEntryAsync(request), Times.Once);
    }

    [Fact]
    public async Task CreateCoffeeEntry_Should_Return_CreatedResult_When_ValidRequestWithOptionalFields()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Americano",
            Size = "Large",
            Source = "Starbucks",
            Timestamp = DateTime.UtcNow.AddMinutes(-30)
        };

        var expectedResponse = new CoffeeEntryResponse
        {
            Id = 2,
            CoffeeType = "Americano",
            Size = "Large",
            Source = "Starbucks",
            Timestamp = request.Timestamp.Value,
            CaffeineAmount = 156
        };

        _mockCoffeeEntryService
            .Setup(s => s.CreateCoffeeEntryAsync(request))
            .ReturnsAsync(expectedResponse);

        var controller = new CoffeeEntriesController(_mockLogger.Object, _mockCoffeeEntryService.Object);

        // Act
        var result = await controller.CreateCoffeeEntry(request);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        var responseValue = createdResult.Value.Should().BeOfType<CoffeeEntryResponse>().Subject;
        responseValue.Should().BeEquivalentTo(expectedResponse);
        
        _mockCoffeeEntryService.Verify(s => s.CreateCoffeeEntryAsync(request), Times.Once);
    }

    [Fact]
    public async Task CreateCoffeeEntry_Should_Return_ProblemDetails_When_ServiceThrows()
    {
        // Arrange
        var request = new CreateCoffeeEntryRequest
        {
            CoffeeType = "Latte",
            Size = "Medium"
        };

        _mockCoffeeEntryService
            .Setup(s => s.CreateCoffeeEntryAsync(request))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var controller = new CoffeeEntriesController(_mockLogger.Object, _mockCoffeeEntryService.Object);

        // Act
        var result = await controller.CreateCoffeeEntry(request);

        // Assert
        var problemResult = result.Should().BeOfType<ObjectResult>().Subject;
        problemResult.StatusCode.Should().Be(500);
        problemResult.Value.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task GetCoffeeEntries_Should_Return_OkResult_When_Called()
    {
        // Arrange
        var expectedEntries = new List<CoffeeEntryResponse>
        {
            new()
            {
                Id = 1,
                CoffeeType = "Latte",
                Size = "Medium",
                Timestamp = DateTime.UtcNow,
                CaffeineAmount = 80
            }
        };

        _mockCoffeeEntryService
            .Setup(s => s.GetCoffeeEntriesAsync(null))
            .ReturnsAsync(expectedEntries);

        var controller = new CoffeeEntriesController(_mockLogger.Object, _mockCoffeeEntryService.Object);

        // Act
        var result = await controller.GetCoffeeEntries(null);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var responseValue = okResult.Value.Should().BeAssignableTo<IEnumerable<CoffeeEntryResponse>>().Subject;
        responseValue.Should().BeEquivalentTo(expectedEntries);
        
        _mockCoffeeEntryService.Verify(s => s.GetCoffeeEntriesAsync(null), Times.Once);
    }

    [Fact]
    public async Task GetCoffeeEntries_Should_Return_OkResult_When_CalledWithSpecificDate()
    {
        // Arrange
        var specificDate = DateOnly.FromDateTime(DateTime.Today);
        var expectedEntries = new List<CoffeeEntryResponse>
        {
            new()
            {
                Id = 1,
                CoffeeType = "Espresso",
                Size = "Small",
                Timestamp = DateTime.Today.AddHours(9),
                CaffeineAmount = 72
            }
        };

        _mockCoffeeEntryService
            .Setup(s => s.GetCoffeeEntriesAsync(specificDate))
            .ReturnsAsync(expectedEntries);

        var controller = new CoffeeEntriesController(_mockLogger.Object, _mockCoffeeEntryService.Object);

        // Act
        var result = await controller.GetCoffeeEntries(specificDate);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var responseValue = okResult.Value.Should().BeAssignableTo<IEnumerable<CoffeeEntryResponse>>().Subject;
        responseValue.Should().BeEquivalentTo(expectedEntries);
        
        _mockCoffeeEntryService.Verify(s => s.GetCoffeeEntriesAsync(specificDate), Times.Once);
    }

    [Fact]
    public async Task GetCoffeeEntries_Should_Return_EmptyList_When_NoEntriesFound()
    {
        // Arrange
        var emptyList = new List<CoffeeEntryResponse>();

        _mockCoffeeEntryService
            .Setup(s => s.GetCoffeeEntriesAsync(null))
            .ReturnsAsync(emptyList);

        var controller = new CoffeeEntriesController(_mockLogger.Object, _mockCoffeeEntryService.Object);

        // Act
        var result = await controller.GetCoffeeEntries(null);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var responseValue = okResult.Value.Should().BeAssignableTo<IEnumerable<CoffeeEntryResponse>>().Subject;
        responseValue.Should().BeEmpty();
        
        _mockCoffeeEntryService.Verify(s => s.GetCoffeeEntriesAsync(null), Times.Once);
    }

    [Fact]
    public async Task GetCoffeeEntries_Should_Return_ProblemDetails_When_ServiceThrows()
    {
        // Arrange
        _mockCoffeeEntryService
            .Setup(s => s.GetCoffeeEntriesAsync(null))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var controller = new CoffeeEntriesController(_mockLogger.Object, _mockCoffeeEntryService.Object);

        // Act
        var result = await controller.GetCoffeeEntries(null);

        // Assert
        var problemResult = result.Should().BeOfType<ObjectResult>().Subject;
        problemResult.StatusCode.Should().Be(500);
        problemResult.Value.Should().BeOfType<ProblemDetails>();
    }
}
