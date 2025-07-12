using CoffeeTracker.Api.Exceptions;
using CoffeeTracker.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CoffeeTracker.Api.Tests.Middleware;

public class GlobalExceptionHandlerMiddlewareTests
{
    private readonly Mock<ILogger<GlobalExceptionHandlerMiddleware>> _loggerMock;
    private RequestDelegate _next; // Changed from readonly to allow reassignment in tests
    private readonly DefaultHttpContext _httpContext;
    private readonly MemoryStream _responseBody;

    public GlobalExceptionHandlerMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();
        
        _next = (httpContext) =>
        {
            return Task.FromException(new Exception("Test exception"));
        };
        
        _httpContext = new DefaultHttpContext();
        _responseBody = new MemoryStream();
        _httpContext.Response.Body = _responseBody;
        
        // Add correlation ID header
        _httpContext.TraceIdentifier = "test-correlation-id";
    }
    
    [Fact]
    public async Task InvokeAsync_WithValidationException_Returns400BadRequest()
    {
        // Arrange
        RequestDelegate next = (httpContext) => Task.FromException(new ValidationException("Validation failed"));
        var middleware = new GlobalExceptionHandlerMiddleware(next, _loggerMock.Object);
        
        // Act
        await middleware.InvokeAsync(_httpContext);
        
        // Assert
        _responseBody.Position = 0;
        var responseContent = await new StreamReader(_responseBody, Encoding.UTF8).ReadToEndAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.Equal(StatusCodes.Status400BadRequest, _httpContext.Response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Equal("Validation failed", problemDetails!.Detail);
        Assert.Equal("Validation Error", problemDetails.Title);
        Assert.Equal("https://tools.ietf.org/html/rfc7231#section-6.5.1", problemDetails.Type);
        Assert.Equal("test-correlation-id", _httpContext.Response.Headers["X-Correlation-ID"]);
    }
    
    [Fact]
    public async Task InvokeAsync_WithBusinessRuleViolationException_Returns422UnprocessableEntity()
    {
        // Arrange
        RequestDelegate next = (httpContext) => Task.FromException(
            new BusinessRuleViolationException("DailyEntryLimit", "Daily limit exceeded"));
        var middleware = new GlobalExceptionHandlerMiddleware(next, _loggerMock.Object);
        
        // Act
        await middleware.InvokeAsync(_httpContext);
        
        // Assert
        _responseBody.Position = 0;
        var responseContent = await new StreamReader(_responseBody, Encoding.UTF8).ReadToEndAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, _httpContext.Response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Equal("Daily limit exceeded", problemDetails!.Detail);
        Assert.Equal("Business Rule Violation", problemDetails.Title);
    }
    
    [Fact]
    public async Task InvokeAsync_WithSessionNotFoundException_Returns404NotFound()
    {
        // Arrange
        RequestDelegate next = (httpContext) => Task.FromException(
            new SessionNotFoundException("test-session"));
        var middleware = new GlobalExceptionHandlerMiddleware(next, _loggerMock.Object);
        
        // Act
        await middleware.InvokeAsync(_httpContext);
        
        // Assert
        _responseBody.Position = 0;
        var responseContent = await new StreamReader(_responseBody, Encoding.UTF8).ReadToEndAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.Equal(StatusCodes.Status404NotFound, _httpContext.Response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Contains("test-session", problemDetails!.Detail ?? string.Empty);
        Assert.Equal("Not Found", problemDetails.Title);
    }
    
    [Fact]
    public async Task InvokeAsync_WithGenericException_Returns500InternalServerError()
    {
        // Arrange
        RequestDelegate next = (httpContext) => Task.FromException(
            new InvalidOperationException("Something went wrong"));
        var middleware = new GlobalExceptionHandlerMiddleware(next, _loggerMock.Object);
        
        // Act
        await middleware.InvokeAsync(_httpContext);
        
        // Assert
        _responseBody.Position = 0;
        var responseContent = await new StreamReader(_responseBody, Encoding.UTF8).ReadToEndAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Equal("An unexpected error occurred. Please try again later.", problemDetails!.Detail);
        Assert.Equal("Internal Server Error", problemDetails.Title);
        Assert.DoesNotContain("Something went wrong", responseContent); // Should not expose internal details
    }
}
