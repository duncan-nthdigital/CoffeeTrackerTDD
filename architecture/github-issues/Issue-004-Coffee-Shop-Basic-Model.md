# Issue #004: Create Coffee Shop Basic Model

**Labels:** `epic-1`, `domain-model`, `medium-priority`, `phase-1`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 1 - Core Domain Models & Database Setup  
**Estimated Time:** 1-2 hours  

## 📋 Description

Create a basic coffee shop model for anonymous users to select as their coffee source. This provides a foundation for coffee shop selection while keeping Phase 1 simple, with plans for enhancement in later phases.

## 🎯 Acceptance Criteria

- [ ] CoffeeShop model includes: Id, Name, Address, IsActive, CreatedAt
- [ ] Model has proper validation attributes
- [ ] DbContext updated with CoffeeShop table
- [ ] Fluent API configuration for optimal queries
- [ ] Seed data included for sample coffee shops
- [ ] Unit tests verify model behavior

## 🔧 Technical Requirements

- Simple model for Phase 1 (no ratings, reviews, etc.)
- Focus on essential properties only
- Prepare structure for future enhancement
- Include basic seed data for testing

## 📝 Implementation Details

### File Locations
- **Model**: `src/CoffeeTracker.Api/Models/CoffeeShop.cs`
- **DbContext Update**: `src/CoffeeTracker.Api/Data/CoffeeTrackerDbContext.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Models/CoffeeShopTests.cs`

### Properties Required
```csharp
- Id (int, primary key)
- Name (string, required, max 100 chars)
- Address (string, optional, max 200 chars) 
- IsActive (bool, default true)
- CreatedAt (DateTime, auto-set to UTC now)
```

### Seed Data
Include 5-10 sample coffee shops for development and testing:
- Starbucks, Dunkin', Local Coffee House, etc.
- Mix of chain and independent shops
- Various locations for testing

## 🤖 Copilot Prompt

```
Create a basic `CoffeeShop` domain model for Phase 1 anonymous coffee tracking in a .NET 8 Web API project.

Properties needed:
- Id (int, primary key)
- Name (string, required, max 100 chars)
- Address (string, optional, max 200 chars) 
- IsActive (bool, default true)
- CreatedAt (DateTime, auto-set to UTC now)

Requirements:
- Use data annotations for validation
- Add XML documentation for all properties
- Include constructor that sets CreatedAt to UTC now
- Keep it simple for Phase 1 (no ratings, reviews, etc. yet)
- Follow clean code and domain-driven design principles

Update the existing `CoffeeTrackerDbContext` to include:
- DbSet<CoffeeShop> CoffeeShops property
- Fluent API configuration in OnModelCreating:
  - Primary key configuration
  - Required field validation
  - String length constraints
  - Index on Name for search performance
  - Default values configuration

Create seed data with 8-10 sample coffee shops:
- Mix of popular chains (Starbucks, Dunkin', etc.)
- Local coffee shops
- "Home" option for home brewing
- Various addresses for testing

Create comprehensive unit tests that verify:
- Model validation works correctly
- DbContext integration functions properly
- Can query coffee shops efficiently
- Seed data loads correctly
- Constructor sets CreatedAt properly
- IsActive filtering works

Place in `Models/CoffeeShop.cs` and update the DbContext accordingly.
```

## ✅ Definition of Done

- [ ] CoffeeShop model created with all required properties
- [ ] Data validation attributes applied correctly
- [ ] XML documentation added to all public members
- [ ] Constructor sets CreatedAt automatically
- [ ] DbContext updated with CoffeeShops DbSet
- [ ] Fluent API configuration implemented
- [ ] Index on Name field for search performance
- [ ] Seed data created with sample coffee shops
- [ ] Unit tests written with >90% coverage
- [ ] All tests pass
- [ ] Model follows domain-driven design patterns

## 🔗 Related Issues

- Depends on: #003 (Update DbContext)
- Related to: #001 (Coffee Entry Domain Model)
- Blocks: #005 (Create Migration)
- Epic: #Epic-1 (Core Domain Models & Database Setup)

## 📌 Notes

- **Phase 1 Scope**: Keep model simple, extensible for future phases
- **Future Enhancement**: Model designed for later addition of:
  - Reviews and ratings (Phase 4)
  - Photos and detailed info (Phase 4)
  - Location coordinates (Phase 3)
  - Hours and contact info (Phase 3)
- **Seed Data**: Include realistic sample data for development
- **Performance**: Index on Name for search functionality

## 🧪 Test Scenarios

- Create CoffeeShop with valid data
- Validate required field constraints
- Test string length validation
- Verify IsActive filtering
- Query coffee shops by name (index usage)
- Load and verify seed data
- Test constructor sets CreatedAt properly

## 🔍 Review Checklist

- [ ] Model is simple but extensible
- [ ] Validation rules are appropriate
- [ ] Database configuration is optimal
- [ ] Seed data is realistic and useful
- [ ] Tests cover all scenarios
- [ ] Follows existing code patterns

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
