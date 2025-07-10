# Issue #003: Update DbContext with Coffee Models

**Labels:** `epic-1`, `database`, `ef-core`, `high-priority`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 1 - Core Domain Models & Database Setup  
**Estimated Time:** 1-2 hours  

## 📋 Description

Update the existing `CoffeeTrackerDbContext` to include coffee tracking tables with proper entity configuration, indexes, and constraints for optimal performance and data integrity.

## 🎯 Acceptance Criteria

- [ ] DbContext includes CoffeeEntry DbSet
- [ ] Proper entity configuration using Fluent API
- [ ] Appropriate indexes added for query performance
- [ ] Database constraints configured correctly
- [ ] Unit tests verify DbContext functionality

## 🔧 Technical Requirements

- Update existing `CoffeeTrackerDbContext` class
- Use Fluent API for entity configuration
- Configure relationships and constraints
- Add performance indexes
- Maintain SQLite compatibility

## 📝 Implementation Details

### File Locations
- **DbContext**: `src/CoffeeTracker.Api/Data/CoffeeTrackerDbContext.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Data/CoffeeTrackerDbContextTests.cs`

### Entity Configuration Requirements
```csharp
CoffeeEntry:
- Primary key on Id with auto-increment
- Required field validation (CoffeeType, Size, Timestamp)
- String length constraints (CoffeeType: 50, Source: 100)
- Index on Timestamp for time-based queries
- Default value for Timestamp (UTC now)
- Calculated caffeine amount storage
```

### Performance Considerations
- Index on Timestamp for daily/weekly queries
- Proper column types for SQLite
- Efficient query patterns for anonymous users

## 🤖 Copilot Prompt

```
Update the existing `CoffeeTrackerDbContext` in a .NET 8 Web API project to include coffee tracking for anonymous users.

Current context is in `src/CoffeeTracker.Api/Data/CoffeeTrackerDbContext.cs`

Add:
- DbSet<CoffeeEntry> CoffeeEntries property
- Entity configuration using Fluent API in OnModelCreating:
  - Primary key on Id with auto-increment
  - Required fields validation for CoffeeType, Size, Timestamp
  - String length constraints (CoffeeType: 50 chars, Source: 100 chars)
  - Index on Timestamp for performance
  - Default value for Timestamp to UTC now
  - Proper column types for SQLite compatibility

Requirements:
- Use Entity Framework Core 8
- Follow existing patterns in the DbContext
- Add XML documentation for new DbSet
- Ensure proper column types for SQLite compatibility
- Configure for optimal query performance

Also update any database initialization code to handle the new entities.

Create unit tests that verify:
- DbContext can be created successfully
- CoffeeEntry entities can be added and retrieved
- Indexes are created correctly
- Constraints work as expected
- Connection and disposal work properly

Follow TDD principles and existing test patterns.
```

## ✅ Definition of Done

- [ ] CoffeeTrackerDbContext updated with CoffeeEntry DbSet
- [ ] Fluent API configuration implemented in OnModelCreating
- [ ] Required field validation configured
- [ ] String length constraints applied
- [ ] Performance indexes created
- [ ] Default value for Timestamp configured
- [ ] SQLite compatibility verified
- [ ] Unit tests written with >90% coverage
- [ ] All tests pass
- [ ] XML documentation updated

## 🔗 Related Issues

- Depends on: #001 (Coffee Entry Domain Model), #002 (Coffee Type Enums)
- Blocks: #005 (Create Migration)
- Related to: #004 (Coffee Shop Model)
- Epic: #Epic-1 (Core Domain Models & Database Setup)

## 📌 Notes

- Maintain compatibility with existing database structure
- Focus on query performance for time-based operations
- Ensure proper disposal patterns for DbContext
- Consider future scalability needs
- SQLite compatibility is essential for local development

## 🧪 Test Scenarios

- Create and dispose DbContext successfully
- Add new CoffeeEntry and verify persistence
- Query CoffeeEntry by date range (index usage)
- Validate required field constraints
- Test string length validation
- Verify default timestamp assignment

## 🔍 Review Checklist

- [ ] Entity configuration follows EF Core best practices
- [ ] Indexes improve query performance without overhead
- [ ] Constraints match domain model validation
- [ ] Code follows existing DbContext patterns
- [ ] Tests cover all configuration scenarios

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
