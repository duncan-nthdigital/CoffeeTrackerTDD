using CoffeeTracker.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace CoffeeTracker.Api.IntegrationTests
{
    /// <summary>
    /// Collection definition for integration tests to share the WebApplicationFactory
    /// </summary>
    [CollectionDefinition("Integration Tests")]
    public class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactory> { }

    /// <summary>
    /// Custom WebApplicationFactory that overrides configuration for testing
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        private SqliteConnection? _connection;
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Configure the web host to use testing environment
            builder.UseEnvironment("Testing");
            
            // Replace services with test versions
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration completely
                var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<CoffeeTrackerDbContext>) ||
                                                     d.ServiceType == typeof(CoffeeTrackerDbContext) ||
                                                     d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))
                                         .ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                // Create and keep a persistent connection for the in-memory database
                if (_connection == null)
                {
                    _connection = new SqliteConnection("DataSource=:memory:");
                    _connection.Open(); // Keep the connection open for the in-memory database
                }

                // Add the new DbContext with the in-memory connection as a singleton
                services.AddSingleton<DbContextOptions<CoffeeTrackerDbContext>>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<CoffeeTrackerDbContext>();
                    optionsBuilder.UseSqlite(_connection);
                    return optionsBuilder.Options;
                });

                services.AddScoped<CoffeeTrackerDbContext>();

                // Ensure database is created when the factory is first used
                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
                    db.Database.EnsureCreated();
                }
            });
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
