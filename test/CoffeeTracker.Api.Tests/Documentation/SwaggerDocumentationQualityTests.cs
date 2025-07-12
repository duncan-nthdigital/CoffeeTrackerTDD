using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;

namespace CoffeeTracker.Api.Tests.Documentation;

/// <summary>
/// Tests that verify the quality and completeness of the Swagger/OpenAPI documentation
/// </summary>
public class SwaggerDocumentationQualityTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SwaggerDocumentationQualityTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task OpenAPI_Specification_Should_Include_Comprehensive_Endpoint_Documentation()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Verify paths exist
        openApiDoc.RootElement.TryGetProperty("paths", out var paths).Should().BeTrue();
        paths.EnumerateObject().Should().NotBeEmpty("API should have documented endpoints");
        
        // Look for coffee entries endpoints
        var pathsDict = paths.EnumerateObject().ToDictionary(p => p.Name, p => p.Value);
        pathsDict.Should().ContainKey("/api/coffee-entries", "Should document coffee entries endpoint");
        
        // Verify POST endpoint documentation
        if (pathsDict.TryGetValue("/api/coffee-entries", out var coffeeEntriesPath))
        {
            coffeeEntriesPath.TryGetProperty("post", out var postOperation).Should().BeTrue();
            
            // Verify operation has summary and description
            postOperation.TryGetProperty("summary", out var summary).Should().BeTrue();
            summary.GetString().Should().NotBeNullOrEmpty();
            
            postOperation.TryGetProperty("description", out var description).Should().BeTrue();
            description.GetString().Should().NotBeNullOrEmpty();
            
            // Verify request body schema
            postOperation.TryGetProperty("requestBody", out var requestBody).Should().BeTrue();
            requestBody.TryGetProperty("content", out var content_).Should().BeTrue();
            content_.TryGetProperty("application/json", out var jsonContent).Should().BeTrue();
            jsonContent.TryGetProperty("schema", out var schema).Should().BeTrue();
            
            // Verify response documentation
            postOperation.TryGetProperty("responses", out var responses).Should().BeTrue();
            responses.TryGetProperty("201", out var response201).Should().BeTrue();
            responses.TryGetProperty("400", out var response400).Should().BeTrue();
            responses.TryGetProperty("422", out var response422).Should().BeTrue();
        }
    }

    [Fact]
    public async Task OpenAPI_Specification_Should_Include_Schema_Definitions()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);

        // Assert
        openApiDoc.RootElement.TryGetProperty("components", out var components).Should().BeTrue();
        components.TryGetProperty("schemas", out var schemas).Should().BeTrue();
        
        var schemaNames = schemas.EnumerateObject().Select(s => s.Name).ToList();
        
        // Verify important DTOs are documented
        schemaNames.Should().Contain("CreateCoffeeEntryRequest", "Request DTO should be documented");
        schemaNames.Should().Contain("CoffeeEntryResponse", "Response DTO should be documented");
        schemaNames.Should().Contain("ProblemDetails", "Error response should be documented");
    }

    [Fact]
    public async Task OpenAPI_Specification_Should_Include_String_Validation_Documentation()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();

        // Assert - Check that string properties with validation are properly documented
        content.Should().Contain("coffeeType", "Coffee type property should be documented");
        content.Should().Contain("maxLength", "String length validation should be documented");
        content.Should().Contain("example", "Examples should be provided for properties");
    }

    [Fact]
    public async Task Swagger_Configuration_Should_Enable_Professional_Features()
    {
        // Act
        var response = await _client.GetAsync("/swagger");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().Contain("Coffee Tracker API v1 Documentation", "Should have professional title");
        content.Should().Contain("swagger-ui", "Should load Swagger UI");
        content.Should().Contain("v1", "Should display version information");
        
        // Check that the page loads successfully with proper structure
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("<!DOCTYPE html>", "Should return valid HTML");
    }

    [Fact]
    public async Task OpenAPI_Specification_Should_Follow_Best_Practices()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);

        // Assert basic structure
        openApiDoc.RootElement.TryGetProperty("openapi", out var openApiVersion).Should().BeTrue();
        openApiVersion.GetString().Should().StartWith("3.0");
        
        openApiDoc.RootElement.TryGetProperty("info", out var info).Should().BeTrue();
        info.TryGetProperty("title", out var title).Should().BeTrue();
        info.TryGetProperty("version", out var version).Should().BeTrue();
        info.TryGetProperty("description", out var description).Should().BeTrue();
        info.TryGetProperty("contact", out var contact).Should().BeTrue();
        
        // Verify contact information is complete
        contact.TryGetProperty("name", out var contactName).Should().BeTrue();
        contact.TryGetProperty("email", out var contactEmail).Should().BeTrue();
        
        contactName.GetString().Should().NotBeNullOrEmpty();
        contactEmail.GetString().Should().NotBeNullOrEmpty();
        contactEmail.GetString().Should().Contain("@", "Should be a valid email format");
    }

    [Fact]
    public async Task Swagger_Annotations_Should_Be_Properly_Applied()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();

        // Assert that Swagger annotations are working
        content.Should().Contain("operationId", "Should include operation IDs from SwaggerOperation attributes");
        content.Should().Contain("CreateCoffeeEntry", "Should include operation ID for create endpoint");
        content.Should().Contain("GetCoffeeEntries", "Should include operation ID for get endpoint");
    }
}
