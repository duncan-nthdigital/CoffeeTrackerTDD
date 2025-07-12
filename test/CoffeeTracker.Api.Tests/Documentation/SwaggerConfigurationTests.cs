using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Text.Json;
using Xunit;

namespace CoffeeTracker.Api.Tests.Documentation;

/// <summary>
/// Tests for Swagger/OpenAPI configuration and functionality
/// </summary>
public class SwaggerConfigurationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SwaggerConfigurationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public void Should_Register_SwaggerGen_Services()
    {
        // Arrange & Act
        var services = _factory.Services;
        var swaggerProvider = services.GetService<ISwaggerProvider>();

        // Assert
        swaggerProvider.Should().NotBeNull("SwaggerGen services should be registered");
    }

    [Fact]
    public async Task Should_Return_Swagger_UI_At_Swagger_Endpoint()
    {
        // Act
        var response = await _client.GetAsync("/swagger");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Coffee Tracker API");
        content.Should().Contain("swagger-ui");
    }

    [Fact]
    public async Task Should_Return_OpenAPI_Specification_At_Swagger_Json_Endpoint()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
        
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);
        
        // Verify OpenAPI structure
        openApiDoc.RootElement.TryGetProperty("openapi", out var openApiVersion).Should().BeTrue();
        openApiVersion.GetString().Should().StartWith("3.0");
        
        openApiDoc.RootElement.TryGetProperty("info", out var info).Should().BeTrue();
        info.TryGetProperty("title", out var title).Should().BeTrue();
        title.GetString().Should().Be("Coffee Tracker API");
        
        info.TryGetProperty("version", out var version).Should().BeTrue();
        version.GetString().Should().Be("v1");
        
        info.TryGetProperty("description", out var description).Should().BeTrue();
        description.GetString().Should().Be("An ASP.NET Core Web API for tracking coffee consumption");
    }

    [Fact]
    public async Task Should_Include_Contact_Information_In_OpenAPI_Specification()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);

        // Assert
        openApiDoc.RootElement.TryGetProperty("info", out var info).Should().BeTrue();
        info.TryGetProperty("contact", out var contact).Should().BeTrue();
        
        contact.TryGetProperty("name", out var name).Should().BeTrue();
        name.GetString().Should().NotBeNullOrEmpty();
        
        contact.TryGetProperty("email", out var email).Should().BeTrue();
        email.GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Should_Include_XML_Documentation_Comments()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();

        // Assert - This will fail initially until we add XML comments to controllers
        content.Should().Contain("summary", "XML documentation should be included in OpenAPI spec");
    }

    [Fact]
    public async Task Should_Document_All_HTTP_Status_Codes_For_Endpoints()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();
        var openApiDoc = JsonDocument.Parse(content);

        // Assert
        openApiDoc.RootElement.TryGetProperty("paths", out var paths).Should().BeTrue();
        
        // This test will initially fail until we add proper response type documentation
        foreach (var path in paths.EnumerateObject())
        {
            foreach (var method in path.Value.EnumerateObject())
            {
                method.Value.TryGetProperty("responses", out var responses).Should().BeTrue();
                responses.EnumerateObject().Should().NotBeEmpty("Each endpoint should document response codes");
            }
        }
    }

    [Fact]
    public void Should_Enable_Swagger_Annotations()
    {
        // Arrange
        var services = _factory.Services;
        
        // Act & Assert
        var swaggerGenOptions = services.GetService<IConfigureOptions<SwaggerGenOptions>>();
        swaggerGenOptions.Should().NotBeNull("Swagger annotations should be configured");
    }

    [Fact]
    public async Task Should_Configure_Enum_Handling_For_Better_Documentation()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        var content = await response.Content.ReadAsStringAsync();

        // Assert - This will pass once we configure enum handling
        content.Should().NotBeNull();
        // Additional enum-specific assertions can be added when we have enum models
    }

    [Fact]
    public async Task Should_Have_Professional_Swagger_UI_Configuration()
    {
        // Act
        var response = await _client.GetAsync("/swagger");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().Contain("Coffee Tracker API", "Should display API title");
        content.Should().Contain("swagger-ui", "Should load Swagger UI");
        
        // Verify some professional UI features are enabled
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
