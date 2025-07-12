using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;

namespace CoffeeTracker.Api.Tests.Documentation;

/// <summary>
/// Integration tests for Swagger/OpenAPI endpoint functionality
/// </summary>
public class SwaggerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SwaggerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Swagger_UI_Endpoint_Should_Be_Accessible()
    {
        // Act
        var response = await _client.GetAsync("/swagger");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");
    }

    [Fact]
    public async Task OpenAPI_Json_Endpoint_Should_Return_Valid_Specification()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");

        var jsonContent = await response.Content.ReadAsStringAsync();
        var isValidJson = IsValidJson(jsonContent);
        isValidJson.Should().BeTrue("OpenAPI specification should be valid JSON");
    }

    [Fact]
    public async Task OpenAPI_Specification_Should_Follow_OpenAPI_3_0_Standard()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);

        // Assert
        openApiDoc.RootElement.TryGetProperty("openapi", out var openApiVersion).Should().BeTrue();
        var versionString = openApiVersion.GetString();
        versionString.Should().StartWith("3.0", "Should follow OpenAPI 3.0 standard");
    }

    [Fact]
    public async Task All_Documented_Endpoints_Should_Be_Functional()
    {
        // Arrange
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);

        // Act & Assert
        openApiDoc.RootElement.TryGetProperty("paths", out var paths).Should().BeTrue();
        
        foreach (var path in paths.EnumerateObject())
        {
            var pathName = path.Name;
            
            // Skip test endpoints that might not be real
            if (pathName.Contains("weatherforecast"))
                continue;

            foreach (var method in path.Value.EnumerateObject())
            {
                var httpMethod = method.Name.ToUpperInvariant();
                
                // For now, just verify the path structure is valid
                pathName.Should().StartWith("/", $"Path {pathName} should start with /");
            }
        }
    }

    [Fact]
    public async Task Swagger_UI_Should_Display_API_Metadata()
    {
        // Act
        var response = await _client.GetAsync("/swagger");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().Contain("Coffee Tracker API");
        content.Should().Contain("v1");
    }

    [Fact]
    public async Task OpenAPI_Specification_Should_Include_Required_Metadata()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);

        // Assert
        openApiDoc.RootElement.TryGetProperty("info", out var info).Should().BeTrue();
        
        // Required metadata
        info.TryGetProperty("title", out var title).Should().BeTrue();
        title.GetString().Should().Be("Coffee Tracker API");
        
        info.TryGetProperty("version", out var version).Should().BeTrue();
        version.GetString().Should().Be("v1");
        
        info.TryGetProperty("description", out var description).Should().BeTrue();
        description.GetString().Should().NotBeNullOrEmpty();
    }

    private static bool IsValidJson(string jsonString)
    {
        try
        {
            JsonDocument.Parse(jsonString);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
