using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Services;

namespace CoffeeTracker.Api.Controllers;

/// <summary>
/// Controller for managing coffee entries for anonymous users
/// </summary>
[ApiController]
[Route("api/coffee-entries")]
[Produces("application/json")]
public class CoffeeEntriesController : ControllerBase
{
    private readonly ILogger<CoffeeEntriesController> _logger;
    private readonly ICoffeeEntryService _coffeeEntryService;

    /// <summary>
    /// Initializes a new instance of the CoffeeEntriesController
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="coffeeEntryService">Coffee entry service</param>
    public CoffeeEntriesController(ILogger<CoffeeEntriesController> logger, ICoffeeEntryService coffeeEntryService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _coffeeEntryService = coffeeEntryService ?? throw new ArgumentNullException(nameof(coffeeEntryService));
    }

    /// <summary>
    /// Creates a new coffee entry for anonymous user
    /// </summary>
    /// <param name="request">The coffee entry creation request containing coffee type, size, and optional source</param>
    /// <returns>The created coffee entry with calculated caffeine amount</returns>
    /// <response code="201">Returns the newly created coffee entry</response>
    /// <response code="400">If the request is invalid or validation fails</response>
    [HttpPost]
    [ProducesResponseType(typeof(CoffeeEntryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCoffeeEntry([FromBody] CreateCoffeeEntryRequest request)
    {
        try
        {
            _logger.LogInformation("Creating coffee entry: {CoffeeType} {Size}", request.CoffeeType, request.Size);
            
            var createdEntry = await _coffeeEntryService.CreateCoffeeEntryAsync(request);
            
            _logger.LogInformation("Coffee entry created with ID: {Id}", createdEntry.Id);
            
            return CreatedAtAction(nameof(GetCoffeeEntries), new { id = createdEntry.Id }, createdEntry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating coffee entry");
            return Problem("An error occurred while creating the coffee entry", statusCode: 500);
        }
    }

    /// <summary>
    /// Gets coffee entries for a specific date
    /// </summary>
    /// <param name="date">The date to filter entries in ISO 8601 format (optional, defaults to today)</param>
    /// <returns>List of coffee entries for the specified date</returns>
    /// <response code="200">Returns the list of coffee entries for the specified date</response>
    /// <response code="400">If the date parameter is invalid</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CoffeeEntryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCoffeeEntries([FromQuery] DateOnly? date = null)
    {
        try
        {
            _logger.LogInformation("Getting coffee entries for date: {Date}", date?.ToString() ?? "today");
            
            var entries = await _coffeeEntryService.GetCoffeeEntriesAsync(date);
            
            _logger.LogInformation("Retrieved {Count} coffee entries", entries.Count());
            
            return Ok(entries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving coffee entries");
            return Problem("An error occurred while retrieving coffee entries", statusCode: 500);
        }
    }
}
