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
using CoffeeTracker.Api.Filters;

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Coffee Tracker API",
        Description = "An ASP.NET Core Web API for tracking coffee consumption",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Coffee Tracker Team",
            Email = "support@coffeetracker.com"
        }
    });
    
    // Include XML comments
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    
    // Configure examples and schemas
    options.EnableAnnotations();
    options.UseInlineDefinitionsForEnums();
});

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

// Add controllers with Problem Details support
builder.Services.AddProblemDetails(options =>
{
    // Configure problem details to handle various exceptions
    options.CustomizeProblemDetails = context =>
    {
        // Ensure the content type is always set correctly
        context.HttpContext.Response.ContentType = "application/problem+json";
        
        // Add correlation ID
        if (context.ProblemDetails.Extensions == null)
        {
            context.ProblemDetails.Extensions = new Dictionary<string, object?>();
        }
        context.ProblemDetails.Extensions["correlationId"] = context.HttpContext.TraceIdentifier;
    };
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
})
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Invalid input data provided.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "See the errors property for details.",
                Instance = context.HttpContext.Request.Path
            };

            var result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(problemDetails);
            result.ContentTypes.Add("application/problem+json");
            result.ContentTypes.Add("application/problem+json");
            return result;
        };
    });

// TODO: Add FluentValidation
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// TODO: Add AutoMapper
// builder.Services.AddAutoMapper(typeof(Program));

// Configure session management
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // Allow essential cookies (like session cookies) without consent
    // For anonymous tracking, we consider session cookies essential for app functionality
    options.CheckConsentNeeded = context => false; // Allow essential cookies
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.SameAsRequest;
});

// Register application services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICoffeeEntryService, CoffeeEntryService>();
builder.Services.AddScoped<ICoffeeService, CoffeeService>();
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
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Coffee Tracker API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Coffee Tracker API v1 Documentation";
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelExpandDepth(2);
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        options.EnableFilter();
        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.HeadContent = @"
            <style>
                .topbar-wrapper img[alt='Swagger UI'], .topbar-wrapper span { 
                    visibility: visible; 
                }
                .topbar-wrapper .link:after { 
                    content: ' v1'; 
                    font-weight: bold; 
                }
            </style>";
    });
}

app.UseHttpsRedirection();

// Configure problem details for built-in error handling (404, JSON parsing, etc.)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

// Configure status code pages to return problem details for 404, etc.
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    var statusCode = response.StatusCode;
    
    if (statusCode == 404)
    {
        response.ContentType = "application/problem+json";
        
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = statusCode,
            Title = "Not Found",
            Detail = $"The requested resource was not found.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Instance = context.HttpContext.Request.Path
        };
        
        await response.WriteAsJsonAsync(problemDetails);
    }
});

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
