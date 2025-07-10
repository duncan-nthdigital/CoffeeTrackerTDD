using Microsoft.EntityFrameworkCore;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Tests.Data;

/// <summary>
/// Unit tests for CoffeeTrackerDbContext
/// </summary>
public class CoffeeTrackerDbContextTests : IDisposable
{
    private readonly CoffeeTrackerDbContext _context;
    private readonly DbContextOptions<CoffeeTrackerDbContext> _options;

    public CoffeeTrackerDbContextTests()
    {
        _options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        _context = new CoffeeTrackerDbContext(_options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void DbContext_Should_Be_Created_Successfully()
    {
        // Arrange & Act
        using var context = new CoffeeTrackerDbContext(_options);

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public void DbContext_Should_Have_CoffeeEntries_DbSet()
    {
        // Arrange & Act & Assert
        Assert.NotNull(_context.CoffeeEntries);
    }

    [Fact]
    public void CoffeeEntry_Should_Be_Added_And_Retrieved_Successfully()
    {
        // Arrange
        var coffeeEntry = new CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = "Medium",
            Source = "Local Cafe",
            Timestamp = DateTime.UtcNow
        };

        // Act
        _context.CoffeeEntries.Add(coffeeEntry);
        _context.SaveChanges();

        // Assert
        var retrievedEntry = _context.CoffeeEntries.FirstOrDefault();
        Assert.NotNull(retrievedEntry);
        Assert.Equal("Espresso", retrievedEntry.CoffeeType);
        Assert.Equal("Medium", retrievedEntry.Size);
        Assert.Equal("Local Cafe", retrievedEntry.Source);
    }

    [Fact]
    public void CoffeeEntry_Should_Have_Auto_Increment_Primary_Key()
    {
        // Arrange
        var entry1 = new CoffeeEntry { CoffeeType = "Latte", Size = "Large" };
        var entry2 = new CoffeeEntry { CoffeeType = "Cappuccino", Size = "Small" };

        // Act
        _context.CoffeeEntries.AddRange(entry1, entry2);
        _context.SaveChanges();

        // Assert
        Assert.True(entry1.Id > 0);
        Assert.True(entry2.Id > 0);
        Assert.NotEqual(entry1.Id, entry2.Id);
    }

    [Fact]
    public void CoffeeEntry_Should_Require_CoffeeType()
    {
        // Arrange
        var coffeeEntry = new CoffeeEntry
        {
            CoffeeType = null!, // Null should fail validation
            Size = "Medium"
        };

        // Act & Assert
        _context.CoffeeEntries.Add(coffeeEntry);
        Assert.Throws<DbUpdateException>(() => _context.SaveChanges());
    }

    [Fact]
    public void CoffeeEntry_Should_Require_Size()
    {
        // Arrange
        var coffeeEntry = new CoffeeEntry
        {
            CoffeeType = "Espresso",
            Size = null! // Null should fail validation
        };

        // Act & Assert
        _context.CoffeeEntries.Add(coffeeEntry);
        Assert.Throws<DbUpdateException>(() => _context.SaveChanges());
    }

    [Fact]
    public void CoffeeEntry_Should_Enforce_CoffeeType_MaxLength()
    {
        // Arrange & Act
        var model = _context.Model;
        var coffeeEntryType = model.FindEntityType(typeof(CoffeeEntry));
        var coffeeTypeProperty = coffeeEntryType?.FindProperty(nameof(CoffeeEntry.CoffeeType));

        // Assert
        Assert.NotNull(coffeeEntryType);
        Assert.NotNull(coffeeTypeProperty);
        Assert.Equal(CoffeeEntry.CoffeeTypeMaxLength, coffeeTypeProperty.GetMaxLength());
    }

    [Fact]
    public void CoffeeEntry_Should_Enforce_Source_MaxLength()
    {
        // Arrange & Act
        var model = _context.Model;
        var coffeeEntryType = model.FindEntityType(typeof(CoffeeEntry));
        var sourceProperty = coffeeEntryType?.FindProperty(nameof(CoffeeEntry.Source));

        // Assert
        Assert.NotNull(coffeeEntryType);
        Assert.NotNull(sourceProperty);
        Assert.Equal(CoffeeEntry.SourceMaxLength, sourceProperty.GetMaxLength());
    }

    [Fact]
    public void CoffeeEntry_Should_Have_Default_Timestamp_Value()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow.AddSeconds(-1);
        
        var coffeeEntry = new CoffeeEntry
        {
            CoffeeType = "Latte",
            Size = "Medium"
            // Timestamp not explicitly set
        };

        // Act
        _context.CoffeeEntries.Add(coffeeEntry);
        _context.SaveChanges();

        var afterCreation = DateTime.UtcNow.AddSeconds(1);

        // Assert
        var savedEntry = _context.CoffeeEntries.First();
        Assert.True(savedEntry.Timestamp >= beforeCreation);
        Assert.True(savedEntry.Timestamp <= afterCreation);
    }

    [Fact]
    public void CoffeeEntry_Should_Have_Timestamp_Index_For_Performance()
    {
        // Arrange & Act
        var model = _context.Model;
        var coffeeEntryType = model.FindEntityType(typeof(CoffeeEntry));
        var timestampProperty = coffeeEntryType?.FindProperty(nameof(CoffeeEntry.Timestamp));

        // Assert
        Assert.NotNull(coffeeEntryType);
        Assert.NotNull(timestampProperty);
        
        var indexes = coffeeEntryType.GetIndexes();
        var timestampIndex = indexes.FirstOrDefault(i => 
            i.Properties.Any(p => p.Name == nameof(CoffeeEntry.Timestamp)));
        
        Assert.NotNull(timestampIndex);
    }

    [Fact]
    public void DbContext_Should_Dispose_Properly()
    {
        // Arrange
        CoffeeTrackerDbContext? disposableContext;

        // Act
        using (disposableContext = new CoffeeTrackerDbContext(_options))
        {
            Assert.NotNull(disposableContext);
        }

        // Assert - No exception should be thrown, context should be disposed
        Assert.NotNull(disposableContext); // Reference still exists but context is disposed
    }

    [Fact]
    public void Database_Should_Be_Created_And_Connected()
    {
        // Arrange & Act
        var canConnect = _context.Database.CanConnect();

        // Assert
        Assert.True(canConnect);
    }

    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}
