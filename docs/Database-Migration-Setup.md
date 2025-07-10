# Database Migration Setup Instructions

This document provides step-by-step instructions for setting up and running Entity Framework Core migrations for the Coffee Tracker application.

## Prerequisites

- .NET 9.0 SDK installed
- Entity Framework Core tools installed globally (optional but recommended)

## Project Structure

- **API Project**: `src/CoffeeTracker.Api/`
- **DbContext**: `src/CoffeeTracker.Api/Data/CoffeeTrackerDbContext.cs`
- **Models**: `src/CoffeeTracker.Api/Models/`
- **Database**: SQLite database located at `data/coffee-tracker.db`
- **Migrations**: `src/CoffeeTracker.Api/Migrations/`

## Initial Setup

### 1. Install Entity Framework Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef
```

### 2. Verify Tools Installation

```bash
dotnet ef --version
```

## Migration Commands

All Entity Framework commands should be run from the API project directory:

```bash
cd src/CoffeeTracker.Api
```

### 3. Create a New Migration

```bash
dotnet ef migrations add [MigrationName]
```

Example:
```bash
dotnet ef migrations add AddCoffeeTrackingModels
```

### 4. Apply Migrations to Database

```bash
dotnet ef database update
```

### 5. Remove Last Migration (if needed)

```bash
dotnet ef migrations remove
```

### 6. View Migration History

```bash
dotnet ef migrations list
```

### 7. Generate SQL Script

```bash
dotnet ef migrations script
```

## Current Migrations

The project currently has the following migrations:

1. **20250710134316_AddCoffeeTrackingModels**
   - Creates `CoffeeEntries` table
   - Creates `CoffeeShops` table
   - Adds indexes for performance
   - Seeds sample coffee shop data

2. **20250710134420_FixSeedDataStaticValues**
   - Updates seed data to use static dates instead of dynamic values

## Database Schema

### CoffeeEntries Table

| Column    | Type     | Constraints | Description |
|-----------|----------|-------------|-------------|
| Id        | INTEGER  | PRIMARY KEY, AUTOINCREMENT | Unique identifier |
| CoffeeType| TEXT     | NOT NULL, MAX 50 chars | Type of coffee (Espresso, Latte, etc.) |
| Size      | TEXT     | NOT NULL | Size of coffee (Small, Medium, Large) |
| Source    | TEXT     | NULL, MAX 100 chars | Coffee shop name or source |
| Timestamp | TEXT     | NOT NULL, DEFAULT datetime('now') | When coffee was consumed (UTC) |

**Indexes:**
- `IX_CoffeeEntries_Timestamp` on `Timestamp` column

### CoffeeShops Table

| Column    | Type     | Constraints | Description |
|-----------|----------|-------------|-------------|
| Id        | INTEGER  | PRIMARY KEY, AUTOINCREMENT | Unique identifier |
| Name      | TEXT     | NOT NULL, MAX 100 chars | Coffee shop name |
| Address   | TEXT     | NULL, MAX 200 chars | Coffee shop address |
| IsActive  | INTEGER  | NOT NULL, DEFAULT 1 | Whether shop is active |
| CreatedAt | TEXT     | NOT NULL, DEFAULT datetime('now') | When record was created (UTC) |

**Indexes:**
- `IX_CoffeeShops_Name` on `Name` column
- `IX_CoffeeShops_IsActive` on `IsActive` column

## Seed Data

The database is automatically seeded with the following coffee shops:

1. Home
2. Starbucks
3. Dunkin' Donuts
4. Local Coffee House
5. Peet's Coffee
6. The Coffee Bean & Tea Leaf
7. Blue Bottle Coffee
8. Tim Hortons
9. Costa Coffee

## Development Workflow

### For New Features

1. **Create models** in `src/CoffeeTracker.Api/Models/`
2. **Update DbContext** in `src/CoffeeTracker.Api/Data/CoffeeTrackerDbContext.cs`
3. **Create migration**: `dotnet ef migrations add [FeatureName]`
4. **Review migration** files to ensure they're correct
5. **Apply migration**: `dotnet ef database update`
6. **Test** the changes

### Best Practices

- **Always review migration files** before applying them
- **Use descriptive migration names** that describe what they do
- **Test migrations** on a copy of production data before applying to production
- **Keep migrations small** and focused on a single change
- **Never modify existing migration files** that have been applied to production

## Troubleshooting

### "Table already exists" Error

If you encounter a "table already exists" error:

1. **Delete the database** file: `data/coffee-tracker.db`
2. **Reapply migrations**: `dotnet ef database update`

### "No migrations found" Error

Ensure you're running commands from the correct directory:
```bash
cd src/CoffeeTracker.Api
```

### Connection String Issues

The connection string is configured in `Program.cs` and points to:
```
[SolutionRoot]/data/coffee-tracker.db
```

## Testing

### Running Database Tests

```bash
cd test/CoffeeTracker.Api.Tests
dotnet test --filter "DatabaseMigrationTests"
```

### Verification Tests

The project includes comprehensive tests that verify:

- ✅ Database can be created
- ✅ CoffeeEntries table exists and works
- ✅ CoffeeShops table exists with seed data
- ✅ Data can be inserted and retrieved
- ✅ Model validation works correctly
- ✅ Indexes are created properly

## Production Deployment

For production deployment:

1. **Generate deployment script**: 
   ```bash
   dotnet ef migrations script --output migration.sql
   ```

2. **Review the script** before applying to production

3. **Apply to production database** using your preferred deployment method

4. **Verify** the migration completed successfully

## Additional Resources

- [Entity Framework Core Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [SQLite with Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/providers/sqlite/)

---

**Last Updated**: July 10, 2025  
**EF Core Version**: 9.0.7  
**Target Framework**: .NET 9.0
