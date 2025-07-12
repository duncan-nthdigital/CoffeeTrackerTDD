using System.Net;
using CoffeeTracker.Api.DTOs;
using FluentAssertions;

namespace CoffeeTracker.Api.IntegrationTests.Utilities;

/// <summary>
/// Extension methods for asserting API responses in tests
/// </summary>
public static class ApiTestExtensions
{
    /// <summary>
    /// Asserts that an HTTP response message is successful with specified status code
    /// </summary>
    public static void ShouldBeSuccessStatusCode(this HttpResponseMessage response, HttpStatusCode expectedStatusCode)
    {
        response.StatusCode.Should().Be(expectedStatusCode);
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    /// <summary>
    /// Asserts that a coffee entry response is valid
    /// </summary>
    public static void ShouldBeValidCoffeeEntry(this CoffeeEntryResponse response, string coffeeType, string size, string? source = null)
    {
        response.Should().NotBeNull();
        response.Id.Should().BeGreaterThan(0);
        response.CoffeeType.Should().Be(coffeeType);
        response.Size.Should().Be(size);
        
        if (source != null)
        {
            response.Source.Should().Be(source);
        }
        
        response.Timestamp.Should().NotBe(default);
        response.CaffeineAmount.Should().BeGreaterThan(0);
    }
}
