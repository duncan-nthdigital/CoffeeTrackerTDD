using System.Net;
using System.Net.Http.Json;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.IntegrationTests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CoffeeTracker.Api.IntegrationTests;

/// <summary>
/// Integration tests for API error handling
/// </summary>
public class ErrorHandlingTests : ApiIntegrationTestBase
{
    public ErrorHandlingTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task InvalidJson_Returns400WithProblemDetails()
    {
        // Arrange
        var invalidJson = "{ \"coffeeType\": \"Latte\", \"size\": }"; // Missing value
        var content = new StringContent(invalidJson, System.Text.Encoding.UTF8, "application/json");
        
        // Act
        var response = await Client.PostAsync("/api/coffee-entries", content);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        // Problem details should have status code and proper format
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails.Title.Should().NotBeNullOrEmpty();
        problemDetails.Type.Should().NotBeNullOrEmpty();
        
        // Response should have correct content type
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
    }

    [Fact]
    public async Task ValidationError_Returns400WithValidationDetails()
    {
        // Arrange
        var invalidRequest = new { CoffeeType = "" }; // Missing size, empty coffee type
        
        // Act
        var response = await Client.PostAsync("/api/coffee-entries", CreateJsonContent(invalidRequest));
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        // Should return validation problem details
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails.Errors.Should().NotBeEmpty();
        
        // Should contain field-specific error messages
        problemDetails.Errors.Should().ContainKey("CoffeeType");
        problemDetails.Errors.Should().ContainKey("Size");
    }

    [Fact]
    public async Task RouteNotFound_Returns404WithProblemDetails()
    {
        // Arrange & Act
        var response = await Client.GetAsync("/api/non-existent-route");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        // Response format should be consistent
        var responseText = await response.Content.ReadAsStringAsync();
        responseText.Should().Contain("404");
    }
}
