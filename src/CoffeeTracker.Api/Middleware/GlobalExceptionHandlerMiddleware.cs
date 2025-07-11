using CoffeeTracker.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CoffeeTracker.Api.Middleware;

/// <summary>
/// Middleware for handling global exceptions and converting them to appropriate HTTP responses
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the GlobalExceptionHandlerMiddleware class
    /// </summary>
    /// <param name="logger">The logger</param>
    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="next">The next delegate in the pipeline</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.TraceIdentifier;
        context.Response.Headers.Append("X-Correlation-ID", correlationId);
        context.Response.ContentType = "application/problem+json";

        var problemDetails = exception switch
        {
            ValidationException validationEx => HandleValidationException(validationEx, context),
            BusinessRuleViolationException businessRuleEx => HandleBusinessRuleException(businessRuleEx, context),
            SessionNotFoundException sessionNotFoundEx => HandleSessionNotFoundException(sessionNotFoundEx, context),
            _ => HandleGenericException(exception, context)
        };

        // Add correlation ID to the problem details extensions
        if (problemDetails.Extensions == null)
        {
            problemDetails.Extensions = new Dictionary<string, object?>();
        }
        problemDetails.Extensions["correlationId"] = correlationId;

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }

    private ProblemDetails HandleValidationException(ValidationException exception, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Instance = context.Request.Path
        };
    }

    private ProblemDetails HandleBusinessRuleException(BusinessRuleViolationException exception, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        
        return new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Business Rule Violation",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Instance = context.Request.Path,
            Extensions = new Dictionary<string, object?>
            {
                { "ruleName", exception.RuleName }
            }
        };
    }

    private ProblemDetails HandleSessionNotFoundException(SessionNotFoundException exception, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        
        return new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Instance = context.Request.Path,
            Extensions = new Dictionary<string, object?>
            {
                { "sessionId", exception.SessionId }
            }
        };
    }

    private ProblemDetails HandleGenericException(Exception exception, HttpContext context)
    {
        _logger.LogError(exception, "Unhandled exception");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred. Please try again later.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Instance = context.Request.Path
        };
    }
}
