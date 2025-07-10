# Issue #005: Create Database Migration and Schema Update

**Labels:** `epic-1`, `database`, `ef-migration`, `high-priority`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 1 - Core Domain Models & Database Setup  
**Estimated Time:** 1 hour  

## 📋 Description

Create and apply Entity Framework Core migrations for the new coffee tracking models (CoffeeEntry and CoffeeShop). This establishes the database schema foundation for the entire application.

## 🎯 Acceptance Criteria

- [ ] Migration created for CoffeeEntry and CoffeeShop tables
- [ ] Migration applies successfully to SQLite database
- [ ] Database schema matches model definitions exactly
- [ ] Migration includes all indexes and constraints
- [ ] Seed data applied through migration or separate mechanism
- [ ] Database verification tests pass

## 🔧 Technical Requirements

- Use EF Core migration tools (dotnet ef)
- Target SQLite database for development
- Include all constraints, indexes, and relationships
- Verify migration can be applied to clean database
- Document migration process for team

## 📝 Implementation Details

### File Locations
- **Migration**: `src/CoffeeTracker.Api/Migrations/[timestamp]_AddCoffeeTrackingModels.cs`
- **Database**: `data/coffee-tracker.db`
- **Tests**: `test/CoffeeTracker.Api.Tests/Migrations/MigrationTests.cs`

### Migration Requirements
```csharp
Tables to create:
- CoffeeEntries table with all fields and constraints
- CoffeeShops table with all fields and constraints
- Indexes on Timestamp (CoffeeEntries) and Name (CoffeeShops)
- Proper foreign key relationships if applicable
```

### Commands to Execute
```bash
# From src/CoffeeTracker.Api/ directory
dotnet ef migrations add AddCoffeeTrackingModels
dotnet ef database update
```

## 🤖 Copilot Prompt

```
Create Entity Framework Core migrations for the new coffee tracking models in a .NET 8 Web API project.

The project structure:
- API project: `src/CoffeeTracker.Api/`
- DbContext: `src/CoffeeTracker.Api/Data/CoffeeTrackerDbContext.cs`
- Models: `src/CoffeeTracker.Api/Models/`
- Database: SQLite in `data/coffee-tracker.db`

Tasks needed:
1. Create a new migration named "AddCoffeeTrackingModels" using dotnet ef
2. Verify the migration includes:
   - CoffeeEntries table with all properties and constraints
   - CoffeeShops table with all properties and constraints
   - Indexes on Timestamp (CoffeeEntries) and Name (CoffeeShops)
   - Proper column types for SQLite
   - Default values and constraints

3. Apply the migration to the development database
4. Verify the database schema is correct

Steps to follow:
- Navigate to src/CoffeeTracker.Api/ directory
- Run: dotnet ef migrations add AddCoffeeTrackingModels
- Review the generated migration file
- Run: dotnet ef database update
- Verify tables and indexes were created

Also create database verification tests that:
- Confirm both tables exist with correct structure
- Can insert and retrieve test data from both tables
- Verify all constraints work (required fields, string lengths)
- Test that indexes are created and functional

Include step-by-step instructions for other developers to:
- Install EF tools if needed: dotnet tool install --global dotnet-ef
- Run migrations on their local environment
- Verify database setup is correct

Create a simple smoke test that validates the entire database setup works end-to-end.
```

## ✅ Definition of Done

- [ ] Migration file created successfully
- [ ] Migration includes CoffeeEntries table with all constraints
- [ ] Migration includes CoffeeShops table with all constraints
- [ ] All indexes created correctly
- [ ] Migration applies to clean database without errors
- [ ] Database schema matches model definitions
- [ ] Seed data mechanism established
- [ ] Database verification tests written and passing
- [ ] Documentation for migration process created
- [ ] All team members can run migrations successfully

## 🔗 Related Issues

- Depends on: #001 (Coffee Entry Model), #002 (Enums), #003 (DbContext), #004 (Coffee Shop Model)
- Blocks: Epic 2 tasks (API development)
- Epic: #Epic-1 (Core Domain Models & Database Setup)

## 📌 Notes

- **Critical Foundation**: This migration establishes the database foundation
- **SQLite Focus**: Ensure compatibility with SQLite for local development
- **Team Coordination**: All developers need to run this migration
- **Clean Setup**: Migration should work on completely fresh database
- **Rollback Plan**: Document how to rollback if issues arise

## 🧪 Test Scenarios

- Apply migration to fresh database
- Verify all tables created with correct schema
- Insert test data into both tables
- Query data to verify indexes work
- Test all constraints (required fields, lengths)
- Verify foreign key relationships if any
- Test database connection and disposal

## 🔍 Migration Verification Checklist

- [ ] CoffeeEntries table created
- [ ] CoffeeShops table created  
- [ ] All columns have correct types
- [ ] Required field constraints applied
- [ ] String length constraints applied
- [ ] Indexes created on Timestamp and Name
- [ ] Default values work correctly
- [ ] Can insert and query data successfully

## 📋 Instructions for Team

1. **Prerequisites**: Ensure dotnet-ef tools installed
2. **Pull Latest**: Get latest code with new models
3. **Navigate**: `cd src/CoffeeTracker.Api/`
4. **Update**: `dotnet ef database update`
5. **Verify**: Run verification tests

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
