using CoffeeTracker.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
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
    /// Custom WebApplicationFactory that uses the real application logic with a test database
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        private static readonly object _lock = new object();
        private static string? _testDatabasePath;
        private static bool _databaseInitialized = false;
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Set testing environment
            builder.UseEnvironment("Testing");
            
            // Only override the database path to use a test database
            // Let all other services use the real production configuration
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Ensure we have a test database path
                lock (_lock)
                {
                    if (_testDatabasePath == null)
                    {
                        // Use the same logic as Program.cs to find the solution root and data directory
                        var currentDirectory = Directory.GetCurrentDirectory();
                        var solutionRoot = Directory.GetParent(currentDirectory)?.Parent?.FullName 
                            ?? currentDirectory;
                        var dataDirectory = Path.Combine(solutionRoot, "data");
                        Directory.CreateDirectory(dataDirectory);
                        _testDatabasePath = Path.Combine(dataDirectory, "coffee-tracker-test.db");
                        
                        // Clean up any existing test database to start fresh
                        if (File.Exists(_testDatabasePath))
                        {
                            File.Delete(_testDatabasePath);
                        }
                    }
                }
                
                // Override only the database path in configuration
                // This way the real Program.cs logic handles the database setup
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["TestDatabasePath"] = _testDatabasePath
                });
            });
            
            // Minimal service override - only replace database configuration and background services
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration and replace with test database
                var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<CoffeeTrackerDbContext>) ||
                                                     d.ServiceType == typeof(CoffeeTrackerDbContext))
                                         .ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }
                
                // Remove SessionCleanupService to prevent interference with tests
                var cleanupServiceDescriptor = services.FirstOrDefault(d => 
                    d.ImplementationType == typeof(CoffeeTracker.Api.Services.Background.SessionCleanupService));
                if (cleanupServiceDescriptor != null)
                {
                    services.Remove(cleanupServiceDescriptor);
                }

                // Re-add DbContext with test database path - keeping all other logic the same
                services.AddDbContext<CoffeeTrackerDbContext>(options =>
                {
                    options.UseSqlite($"Data Source={_testDatabasePath}");
                });

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
                            
                            // Verify database is working
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
                if (_testDatabasePath != null && _databaseInitialized && File.Exists(_testDatabasePath))
                {
                    using var connection = new SqliteConnection($"Data Source={_testDatabasePath}");
                    connection.Open();
                    using var command = connection.CreateCommand();
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
                // Individual test cleanup - do not delete the database file here
                // as it's shared across all tests in the collection
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
                if (_testDatabasePath != null && File.Exists(_testDatabasePath))
                {
                    try
                    {
                        File.Delete(_testDatabasePath);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
                _testDatabasePath = null;
                _databaseInitialized = false;
            }
        }
    }
}
