using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Services;
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
        private static readonly object _lock = new object();
        private static SqliteConnection? _staticConnection;
        private static bool _databaseInitialized = false;
        
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

                // Replace the session service with test version for consistent session handling
                var sessionServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ISessionService));
                if (sessionServiceDescriptor != null)
                {
                    services.Remove(sessionServiceDescriptor);
                }
                services.AddScoped<ISessionService, TestSessionService>();

                // Ensure we have a persistent connection for all tests
                lock (_lock)
                {
                    if (_staticConnection == null)
                    {
                        _staticConnection = new SqliteConnection("DataSource=:memory:");
                        _staticConnection.Open(); // Keep the connection open for the in-memory database
                    }
                }

                // Add the new DbContext with the shared in-memory connection
                services.AddSingleton<DbContextOptions<CoffeeTrackerDbContext>>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<CoffeeTrackerDbContext>();
                    optionsBuilder.UseSqlite(_staticConnection);
                    return optionsBuilder.Options;
                });

                services.AddScoped<CoffeeTrackerDbContext>();

                // Ensure database is created only once
                lock (_lock)
                {
                    if (!_databaseInitialized)
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        using (var scope = serviceProvider.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<CoffeeTrackerDbContext>();
                            
                            // Ensure database and tables are created
                            db.Database.EnsureCreated();
                            
                            // Force initialization to verify tables exist
                            var tableExists = db.Database.CanConnect();
                            if (!tableExists)
                            {
                                throw new InvalidOperationException("Failed to create database tables");
                            }
                            
                            _databaseInitialized = true;
                        }
                    }
                }
            });
        }
        
        /// <summary>
        /// Clears all data from the database while keeping the schema
        /// </summary>
        public void ClearDatabase()
        {
            lock (_lock)
            {
                if (_staticConnection != null && _databaseInitialized)
                {
                    using var command = _staticConnection.CreateCommand();
                    command.CommandText = @"
                        DELETE FROM CoffeeEntries;
                        DELETE FROM CoffeeShops;
                        INSERT INTO ""CoffeeShops"" (""Id"", ""Address"", ""CreatedAt"", ""IsActive"", ""Name"")
                        VALUES 
                        (1, NULL, '2025-01-01 00:00:00', 1, 'Home'),
                        (2, 'Multiple Locations', '2025-01-01 00:00:00', 1, 'Starbucks'),
                        (3, 'Multiple Locations', '2025-01-01 00:00:00', 1, 'Dunkin'' Donuts'),
                        (4, '123 Main Street', '2025-01-01 00:00:00', 1, 'Local Coffee House'),
                        (5, '456 Oak Avenue', '2025-01-01 00:00:00', 1, 'Peet''s Coffee'),
                        (6, '789 Elm Street', '2025-01-01 00:00:00', 1, 'The Coffee Bean & Tea Leaf'),
                        (7, '321 Pine Road', '2025-01-01 00:00:00', 1, 'Blue Bottle Coffee'),
                        (8, '654 Maple Drive', '2025-01-01 00:00:00', 1, 'Tim Hortons'),
                        (9, '987 Cedar Lane', '2025-01-01 00:00:00', 1, 'Costa Coffee');";
                    command.ExecuteNonQuery();
                }
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Don't dispose the static connection here as it's shared across all tests
                // It will be disposed when the application domain unloads
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Static cleanup method to be called when all tests are done
        /// </summary>
        public static void DisposeStaticResources()
        {
            lock (_lock)
            {
                _staticConnection?.Close();
                _staticConnection?.Dispose();
                _staticConnection = null;
                _databaseInitialized = false;
            }
        }
    }
}
