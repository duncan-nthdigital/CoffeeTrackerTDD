using System.Net;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.IntegrationTests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace CoffeeTracker.Api.IntegrationTests;

/// <summary>
/// Tests for database connection issues and resilience
/// </summary>
public class DatabaseErrorTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public DatabaseErrorTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DatabaseError_Returns500WithCorrelationId()
    {
        // Arrange
        // Create a connection that will fail when used
        var connection = new SqliteConnection("DataSource=:memory:");
        // Open it, but then close it to simulate a connection failure during request
        connection.Open();
        connection.Close();

        // Create a factory with the failing connection
        var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CoffeeTrackerDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add DbContext with connection that will fail
                services.AddDbContext<CoffeeTrackerDbContext>(options =>
                {
                    options.UseSqlite(connection);
                });
            });
        });

        var client = factory.CreateClient();
        var request = TestDataBuilder.CreateCoffeeEntryRequest();

        // Act
        var response = await client.PostAsync("/api/coffee-entries", 
            new StringContent(
                System.Text.Json.JsonSerializer.Serialize(request),
                System.Text.Encoding.UTF8,
                "application/json"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        
        // Problem details should include a correlation ID
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(500);
        problemDetails.Title.Should().ContainEquivalentOf("error");
        
        // Should have a correlation ID to track the error
        problemDetails.Extensions.Should().NotBeEmpty();
        problemDetails.Extensions.Should().ContainKey("traceId");
        problemDetails.Extensions["traceId"].Should().NotBeNull();
    }
}
