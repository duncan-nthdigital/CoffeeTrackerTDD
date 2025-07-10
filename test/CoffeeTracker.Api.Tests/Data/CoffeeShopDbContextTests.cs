using Microsoft.EntityFrameworkCore;
using CoffeeTracker.Api.Data;
using CoffeeTracker.Api.Models;
using FluentAssertions;
using Xunit;

namespace CoffeeTracker.Api.Tests.Data;

/// <summary>
/// Integration tests for CoffeeShop model with CoffeeTrackerDbContext.
/// Verifies database configuration, querying, and seed data functionality.
/// </summary>
public class CoffeeShopDbContextTests : IDisposable
{
    private readonly CoffeeTrackerDbContext _context;

    public CoffeeShopDbContextTests()
    {
        var options = new DbContextOptionsBuilder<CoffeeTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CoffeeTrackerDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void CoffeeShops_DbSet_Should_Be_Configured()
    {
        // Act & Assert
        _context.CoffeeShops.Should().NotBeNull();
        _context.CoffeeShops.Should().BeAssignableTo<DbSet<CoffeeShop>>();
    }

    [Fact]
    public void CoffeeShop_Should_Be_Saved_To_Database()
    {
        // Arrange
        var coffeeShop = new CoffeeShop
        {
            Name = "Test Coffee Shop",
            Address = "123 Test Street",
            IsActive = true
        };

        // Act
        _context.CoffeeShops.Add(coffeeShop);
        _context.SaveChanges();

        // Assert
        var savedShop = _context.CoffeeShops.Where(c => c.Name == "Test Coffee Shop").First();
        savedShop.Id.Should().BeGreaterThan(0);
        savedShop.Name.Should().Be("Test Coffee Shop");
        savedShop.Address.Should().Be("123 Test Street");
        savedShop.IsActive.Should().BeTrue();
        savedShop.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CoffeeShop_Should_Generate_Id_Automatically()
    {
        // Arrange
        var shop1 = new CoffeeShop { Name = "Shop 1" };
        var shop2 = new CoffeeShop { Name = "Shop 2" };

        // Act
        _context.CoffeeShops.AddRange(shop1, shop2);
        _context.SaveChanges();

        // Assert
        shop1.Id.Should().BeGreaterThan(0);
        shop2.Id.Should().BeGreaterThan(0);
        shop1.Id.Should().NotBe(shop2.Id);
    }

    [Fact]
    public void CoffeeShop_Name_Index_Should_Allow_Efficient_Queries()
    {
        // Arrange
        var shops = new[]
        {
            new CoffeeShop { Name = "Starbucks" },
            new CoffeeShop { Name = "Dunkin' Donuts" },
            new CoffeeShop { Name = "Local Coffee House" }
        };

        _context.CoffeeShops.AddRange(shops);
        _context.SaveChanges();

        // Act
        var foundShop = _context.CoffeeShops
            .Where(s => s.Name == "Starbucks")
            .FirstOrDefault();

        // Assert
        foundShop.Should().NotBeNull();
        foundShop!.Name.Should().Be("Starbucks");
    }

    [Fact]
    public void CoffeeShop_Should_Filter_By_IsActive()
    {
        // Arrange
        var testShops = new[]
        {
            new CoffeeShop { Name = "Active Test Shop", IsActive = true },
            new CoffeeShop { Name = "Inactive Test Shop", IsActive = false }
        };

        _context.CoffeeShops.AddRange(testShops);
        _context.SaveChanges();

        // Act
        var activeShops = _context.CoffeeShops
            .Where(s => s.IsActive && s.Name.Contains("Test"))
            .ToList();

        // Assert
        activeShops.Should().HaveCount(1);
        activeShops.First().Name.Should().Be("Active Test Shop");
    }

    [Fact]
    public void Seed_Data_Should_Be_Present()
    {
        // Act
        var seedShops = _context.CoffeeShops.ToList();

        // Assert
        seedShops.Should().HaveCount(c => c >= 8 && c <= 10);
        
        // Should contain common coffee shops
        seedShops.Should().Contain(s => s.Name.Contains("Home"));
        seedShops.Should().Contain(s => s.Name.Contains("Starbucks"));
        seedShops.Should().Contain(s => s.Name.Contains("Dunkin"));
        
        // All should be active by default
        seedShops.Should().OnlyContain(s => s.IsActive);
        
        // All should have proper CreatedAt timestamps
        seedShops.Should().OnlyContain(s => s.CreatedAt.Kind == DateTimeKind.Utc);
    }

    [Fact]
    public void CoffeeShop_Should_Have_Database_Configuration()
    {
        // Arrange & Act
        var entityType = _context.Model.FindEntityType(typeof(CoffeeShop));

        // Assert
        entityType.Should().NotBeNull();
        
        // Check primary key
        var primaryKey = entityType!.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey!.Properties.Should().HaveCount(1);
        primaryKey.Properties.First().Name.Should().Be("Id");
        
        // Check Name property configuration
        var nameProperty = entityType.FindProperty("Name");
        nameProperty.Should().NotBeNull();
        nameProperty!.IsNullable.Should().BeFalse();
        nameProperty.GetMaxLength().Should().Be(CoffeeShop.NameMaxLength);
        
        // Check Address property configuration  
        var addressProperty = entityType.FindProperty("Address");
        addressProperty.Should().NotBeNull();
        addressProperty!.IsNullable.Should().BeTrue();
        addressProperty.GetMaxLength().Should().Be(CoffeeShop.AddressMaxLength);
        
        // Check IsActive property configuration
        var isActiveProperty = entityType.FindProperty("IsActive");
        isActiveProperty.Should().NotBeNull();
        isActiveProperty!.IsNullable.Should().BeFalse();
        
        // Check CreatedAt property configuration
        var createdAtProperty = entityType.FindProperty("CreatedAt");
        createdAtProperty.Should().NotBeNull();
        createdAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void CoffeeShop_Should_Have_Indexes_Configured()
    {
        // Arrange & Act
        var entityType = _context.Model.FindEntityType(typeof(CoffeeShop));

        // Assert
        entityType.Should().NotBeNull();
        
        var indexes = entityType!.GetIndexes();
        indexes.Should().HaveCount(2); // Name and IsActive indexes
        
        // Check Name index
        var nameIndex = indexes.FirstOrDefault(i => i.Properties.Any(p => p.Name == "Name"));
        nameIndex.Should().NotBeNull();
        
        // Check IsActive index
        var isActiveIndex = indexes.FirstOrDefault(i => i.Properties.Any(p => p.Name == "IsActive"));
        isActiveIndex.Should().NotBeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
