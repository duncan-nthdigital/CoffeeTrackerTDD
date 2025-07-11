using System.Text.Json.Serialization;

namespace CoffeeTracker.Api.IntegrationTests;

/// <summary>
/// Problem details returned by ASP.NET Core for validation errors
/// </summary>
public class ValidationProblemDetails : ProblemDetails
{
    /// <summary>
    /// Gets or sets the validation errors
    /// </summary>
    [JsonPropertyName("errors")]
    public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}

/// <summary>
/// Problem details returned by ASP.NET Core for errors
/// </summary>
public class ProblemDetails
{
    /// <summary>
    /// Gets or sets the type of the problem
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    /// <summary>
    /// Gets or sets the title of the problem
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    /// <summary>
    /// Gets or sets the status code of the problem
    /// </summary>
    [JsonPropertyName("status")]
    public int? Status { get; set; }
    
    /// <summary>
    /// Gets or sets the detail of the problem
    /// </summary>
    [JsonPropertyName("detail")]
    public string? Detail { get; set; }
    
    /// <summary>
    /// Gets or sets the instance of the problem
    /// </summary>
    [JsonPropertyName("instance")]
    public string? Instance { get; set; }
    
    /// <summary>
    /// Gets or sets the extensions of the problem
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
}
