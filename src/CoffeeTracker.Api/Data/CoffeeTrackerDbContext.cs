using Microsoft.EntityFrameworkCore;

namespace CoffeeTracker.Api.Data;

/// <summary>
/// Entity Framework DbContext for the Coffee Tracker application
/// </summary>
public class CoffeeTrackerDbContext : DbContext
{
    public CoffeeTrackerDbContext(DbContextOptions<CoffeeTrackerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Add any entity configurations here
    }
}
