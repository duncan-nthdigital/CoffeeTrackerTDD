using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Extensions;
using CoffeeTracker.Api.Middleware;
using CoffeeTracker.Api.Services;
using CoffeeTracker.Api.Services.Background;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.AddServiceDefaults();
}

// Add services to the container.
// Ensure data directory exists and create absolute path for SQLite database
var solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName 
    ?? Directory.GetCurrentDirectory();
var dataDirectory = Path.Combine(solutionRoot, "data");
Directory.CreateDirectory(dataDirectory);
var dbPath = Path.Combine(dataDirectory, "coffee-tracker.db");

builder.Services.AddDbContext<CoffeeTrackerDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add Authentication (only if Auth0 is properly configured)
var auth0Domain = builder.Configuration["Auth0:Domain"];
var auth0Audience = builder.Configuration["Auth0:Audience"];

if (!string.IsNullOrEmpty(auth0Domain) && 
    !string.IsNullOrEmpty(auth0Audience) && 
    !auth0Domain.Contains("your-auth0-domain") &&
    !auth0Audience.Contains("your-api-identifier"))
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = auth0Domain;
            options.Audience = auth0Audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true
            };
        });
    
    builder.Services.AddAuthorization();
}
else
{
    // For development without Auth0 configured, skip authentication
    builder.Services.AddAuthentication("Test")
        .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });
    
    builder.Services.AddAuthorization();
}

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Coffee Tracker API",
            Version = "v1",
            Description = "Anonymous coffee consumption tracking API that allows users to log and retrieve their coffee entries without authentication.",
            Contact = new()
            {
                Name = "Coffee Tracker Support",
                Email = "support@coffeetracker.app"
            }
        };
        return Task.CompletedTask;
    });
});

// Add controllers
builder.Services.AddControllers();

// TODO: Add FluentValidation
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// TODO: Add AutoMapper
// builder.Services.AddAutoMapper(typeof(Program));

// Configure session management
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.SameAsRequest;
});

// Register application services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICoffeeEntryService, CoffeeEntryService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ICoffeeEntryRepository, CoffeeEntryRepository>();
builder.Services.AddScoped<ICoffeeValidationService, CoffeeValidationService>();
builder.Services.AddHostedService<SessionCleanupService>();

var app = builder.Build();

// Map default endpoints that are added by AddServiceDefaults().
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Coffee Tracker API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Coffee Tracker API Documentation";
        c.DefaultModelsExpandDepth(2);
        c.DefaultModelExpandDepth(2);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.EnableFilter();
        c.EnableDeepLinking();
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowWebApp");

app.UseCookiePolicy();
app.UseGlobalExceptionHandler();
app.UseAnonymousSession();

app.UseAuthentication();
app.UseAuthorization();

// Ensure database is created
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
    
    // Log the database path for debugging
    var connectionString = context.Database.GetConnectionString();
    app.Logger.LogInformation("Database connection string: {ConnectionString}", connectionString);
    
    context.Database.EnsureCreated();
    app.Logger.LogInformation("Database created successfully");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Error creating database");
    throw;
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Map controllers
app.MapControllers();

app.Run();

// Simple test authentication handler for development
public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Always succeed for development
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Make Program class accessible for testing
public partial class Program { }
