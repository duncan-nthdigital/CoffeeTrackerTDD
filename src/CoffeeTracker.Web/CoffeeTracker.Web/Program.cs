using CoffeeTracker.Web.Client.Pages;
using CoffeeTracker.Web.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add HTTP client for API
builder.Services.AddHttpClient("CoffeeTrackerApi", client =>
{
    // This will be configured to use the API endpoint from Aspire
    client.BaseAddress = new Uri("https://localhost:7001/"); // Default fallback
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add Authentication (conditional based on Auth0 configuration)
var auth0Domain = builder.Configuration["Auth0:Domain"];
var auth0ClientId = builder.Configuration["Auth0:ClientId"];
var auth0ClientSecret = builder.Configuration["Auth0:ClientSecret"];

if (!string.IsNullOrEmpty(auth0Domain) && 
    !string.IsNullOrEmpty(auth0ClientId) && 
    !string.IsNullOrEmpty(auth0ClientSecret))
{
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = $"https://{auth0Domain}/";
            options.ClientId = auth0ClientId;
            options.ClientSecret = auth0ClientSecret;
            options.ResponseType = "code";
            options.CallbackPath = "/signin-oidc";
            options.SignedOutCallbackPath = "/signout-callback-oidc";
            options.ClaimsIssuer = "Auth0";
            options.SaveTokens = true;
            
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            
            options.Events = new OpenIdConnectEvents
            {
                OnRedirectToIdentityProviderForSignOut = context =>
                {
                    var logoutUri = $"https://{auth0Domain}/v2/logout?client_id={auth0ClientId}";
                    var postLogoutUri = context.Properties.RedirectUri;
                    if (!string.IsNullOrEmpty(postLogoutUri))
                    {
                        if (postLogoutUri.StartsWith("/"))
                        {
                            var request = context.Request;
                            postLogoutUri = $"{request.Scheme}://{request.Host}{postLogoutUri}";
                        }
                        logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                    }
                    context.Response.Redirect(logoutUri);
                    context.HandleResponse();
                    return Task.CompletedTask;
                }
            };
        });
}
else
{
    // For development without Auth0 configured, use simple cookie authentication
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
        });
}

builder.Services.AddAuthorization();

var app = builder.Build();

// Map default endpoints that are added by AddServiceDefaults().
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

// Add authentication endpoints
app.MapGet("/login", async (HttpContext context) =>
{
    var auth0Domain = app.Configuration["Auth0:Domain"];
    if (!string.IsNullOrEmpty(auth0Domain))
    {
        // Use Auth0 OpenID Connect - let it handle the redirect automatically
        await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
    else
    {
        // Simple development authentication
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();
        
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), authProperties);
        
        context.Response.Redirect("/");
    }
});

app.MapPost("/logout", async (HttpContext context) =>
{
    var auth0Domain = app.Configuration["Auth0:Domain"];
    if (!string.IsNullOrEmpty(auth0Domain))
    {
        // Auth0 logout - let OIDC middleware handle the logout flow
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
    else
    {
        // Simple logout
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        context.Response.Redirect("/");
    }
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CoffeeTracker.Web.Client._Imports).Assembly);

app.Run();
