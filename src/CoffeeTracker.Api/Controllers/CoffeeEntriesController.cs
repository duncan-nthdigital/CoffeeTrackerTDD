using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Services;
using CoffeeTracker.Api.Exceptions;

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
    private readonly ICoffeeService _coffeeService;
    private readonly ISessionService _sessionService;

    /// <summary>
    /// Initializes a new instance of the CoffeeEntriesController
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="coffeeService">Coffee service with business rules</param>
    /// <param name="sessionService">Session service for managing anonymous sessions</param>
    public CoffeeEntriesController(ILogger<CoffeeEntriesController> logger, ICoffeeService coffeeService, ISessionService sessionService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _coffeeService = coffeeService ?? throw new ArgumentNullException(nameof(coffeeService));
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateCoffeeEntry([FromBody] CreateCoffeeEntryRequest request)
    {
        try
        {
            _logger.LogInformation("Creating coffee entry: {CoffeeType} {Size}", request.CoffeeType, request.Size);
            
            // Get session ID from HttpContext.Items (set by middleware) or create new one
            var sessionIdFromItems = HttpContext.Items["SessionId"] as string;
            _logger.LogWarning("CONTROLLER: Session ID from HttpContext.Items: {SessionId}", sessionIdFromItems ?? "null");
            
            var sessionId = sessionIdFromItems ?? _sessionService.GetOrCreateSessionId(HttpContext);
            
            _logger.LogInformation("Processing request for session: {SessionId}", sessionId);
            
            var createdEntry = await _coffeeService.CreateCoffeeEntryAsync(request, sessionId);
            
            _logger.LogInformation("Coffee entry created with ID: {Id}", createdEntry.Id);
            
            return CreatedAtAction(nameof(GetCoffeeEntries), new { id = createdEntry.Id }, createdEntry);
        }
        catch (BusinessRuleViolationException ex)
        {
            _logger.LogWarning("Business rule violation: {RuleName} - {Message}", ex.RuleName, ex.Message);
            
            // Return more specific titles based on exception type
            var title = ex.RuleName switch
            {
                "DailyEntryLimit" => "Daily Entry Limit Exceeded",
                "DailyCaffeineLimit" => "Daily Caffeine Limit Exceeded", 
                "InvalidTimestamp" => "Invalid Timestamp",
                _ => "Business Rule Violation"
            };
            
            return UnprocessableEntity(new ProblemDetails
            {
                Title = title,
                Detail = ex.Message,
                Status = StatusCodes.Status422UnprocessableEntity,
                Type = "https://tools.ietf.org/html/rfc4918#section-11.2"
            });
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
            
            // Get session ID from HttpContext.Items (set by middleware) or create new one
            var sessionId = HttpContext.Items["SessionId"] as string ?? 
                           _sessionService.GetOrCreateSessionId(HttpContext);
            
            // Convert DateOnly to DateTime for ICoffeeService
            DateTime? dateTime = date?.ToDateTime(TimeOnly.MinValue);
            
            var entries = await _coffeeService.GetCoffeeEntriesAsync(sessionId, dateTime);
            
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
