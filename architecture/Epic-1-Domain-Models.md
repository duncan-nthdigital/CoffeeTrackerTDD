# Epic 1: Core Domain Models & Database Setup

**Epic Goal:** Create the foundational data models and database structure for anonymous coffee tracking.

**Estimated Time:** 2-3 days  
**Priority:** High (Foundation)  
**Dependencies:** None  

---

## 📋 Tasks

### Task 1.1: Create Coffee Entry Domain Model
**Estimated Time:** 2-3 hours  
**Priority:** High  

**Description:**
Create the core `CoffeeEntry` model for anonymous users with basic tracking properties.

**Acceptance Criteria:**
- [ ] Model includes: Id, CoffeeType, Size, Source, Timestamp, CaffeineAmount
- [ ] Model has proper validation attributes
- [ ] Model follows domain-driven design principles
- [ ] Unit tests verify model behavior

**Technical Details:**
- Create in `src/CoffeeTracker.Api/Models/` directory
- Use System.ComponentModel.DataAnnotations for validation
- Include XML documentation for all public properties
- Follow C# naming conventions

**Copilot Prompt:**
```
Create a C# domain model class called `CoffeeEntry` for anonymous coffee tracking in a .NET 8 Web API project. The model should include:

Properties:
- Id (int, primary key)
- CoffeeType (string, required, max 50 chars) - e.g., "Espresso", "Latte", "Cappuccino"
- Size (string, required) - e.g., "Small", "Medium", "Large" 
- Source (string, optional, max 100 chars) - coffee shop name or "Home"
- Timestamp (DateTime, required, defaults to UTC now)
- CaffeineAmount (int, calculated based on type and size)

Requirements:
- Use data annotations for validation
- Add XML documentation for all properties
- Include a constructor that sets Timestamp to UTC now
- Add a method to calculate caffeine amount based on type and size
- Follow clean code principles and domain-driven design

Create accompanying unit tests using xUnit that verify:
- Model validation works correctly
- Caffeine calculation is accurate
- Constructor sets timestamp properly
- Invalid data throws appropriate exceptions

Place the model in `Models/CoffeeEntry.cs` and tests in the test project.
```

---

### Task 1.2: Create Coffee Type Enumeration
**Estimated Time:** 1-2 hours  
**Priority:** High  

**Description:**
Create an enumeration for coffee types with associated caffeine content.

**Acceptance Criteria:**
- [ ] Enum includes common coffee types (Espresso, Latte, Cappuccino, etc.)
- [ ] Extension methods provide caffeine content per serving
- [ ] Size variations affect caffeine calculation
- [ ] Unit tests verify calculations

**Technical Details:**
- Create enum with descriptive values
- Add extension methods for caffeine calculations
- Consider internationalization for display names

**Copilot Prompt:**
```
Create a C# enumeration called `CoffeeType` for a .NET 8 Web API project with these coffee types:
- Espresso (90mg caffeine base)
- Americano (120mg caffeine base)
- Latte (80mg caffeine base) 
- Cappuccino (80mg caffeine base)
- Mocha (90mg caffeine base)
- MacchiatoCoffee (120mg caffeine base)
- FlatWhite (130mg caffeine base)
- BlackCoffee (95mg caffeine base)

Also create a `CoffeeSize` enum:
- Small (0.8x multiplier)
- Medium (1.0x multiplier)  
- Large (1.3x multiplier)
- ExtraLarge (1.6x multiplier)

Create extension methods:
- `GetBaseCaffeineContent(this CoffeeType type)` - returns base mg
- `GetCaffeineContent(this CoffeeType type, CoffeeSize size)` - returns calculated mg
- `GetDisplayName(this CoffeeType type)` - returns user-friendly name

Requirements:
- Use System.ComponentModel.DataAnnotations.Display for display names
- Add XML documentation
- Create unit tests that verify all caffeine calculations
- Follow clean code principles

Place in `Models/CoffeeType.cs` and `Models/CoffeeSize.cs` with tests in the test project.
```

---

### Task 1.3: Update DbContext with Coffee Models
**Estimated Time:** 1-2 hours  
**Priority:** High  

**Description:**
Update the existing DbContext to include coffee tracking tables.

**Acceptance Criteria:**
- [ ] DbContext includes CoffeeEntry DbSet
- [ ] Proper entity configuration with fluent API
- [ ] Database migration created and tested
- [ ] Indexes added for performance

**Technical Details:**
- Update existing `CoffeeTrackerDbContext`
- Configure relationships and constraints
- Add appropriate indexes for queries

**Copilot Prompt:**
```
Update the existing `CoffeeTrackerDbContext` in a .NET 8 Web API project to include coffee tracking for anonymous users.

Current context is in `src/CoffeeTracker.Api/Data/CoffeeTrackerDbContext.cs`

Add:
- DbSet<CoffeeEntry> CoffeeEntries property
- Entity configuration using Fluent API in OnModelCreating:
  - Primary key on Id with auto-increment
  - Required fields validation
  - String length constraints
  - Index on Timestamp for performance
  - Default value for Timestamp

Requirements:
- Use Entity Framework Core 8
- Follow existing patterns in the DbContext
- Add XML documentation
- Ensure proper column types for SQLite compatibility
- Create a new migration for these changes

Also update the database initialization code to handle any seeding if needed.

Create unit tests that verify:
- DbContext can be created
- Entities can be added and retrieved
- Migrations work correctly
```

---

### Task 1.4: Create Coffee Shop Basic Model
**Estimated Time:** 1-2 hours  
**Priority:** Medium  

**Description:**
Create a basic coffee shop model for anonymous users to select as coffee source.

**Acceptance Criteria:**
- [ ] Model includes: Id, Name, Address, IsActive
- [ ] Model has validation attributes
- [ ] DbContext updated with CoffeeShop table
- [ ] Unit tests verify model behavior

**Technical Details:**
- Simple model for Phase 1 (enhanced in later phases)
- Focus on essential properties only
- Prepare for future enhancement with reviews, ratings, etc.

**Copilot Prompt:**
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
- Add XML documentation
- Include constructor that sets CreatedAt
- Keep it simple for Phase 1 (no ratings, reviews, etc. yet)

Update the existing `CoffeeTrackerDbContext` to include:
- DbSet<CoffeeShop> CoffeeShops property
- Fluent API configuration
- Index on Name for search performance
- Seed data with 5-10 sample coffee shops

Create unit tests that verify:
- Model validation works
- DbContext integration works
- Can query coffee shops
- Seed data is loaded correctly

Place in `Models/CoffeeShop.cs` and update the DbContext accordingly.
```

---

### Task 1.5: Create Migration and Update Database
**Estimated Time:** 1 hour  
**Priority:** High  

**Description:**
Create and apply Entity Framework migrations for the new models.

**Acceptance Criteria:**
- [ ] Migration created for CoffeeEntry and CoffeeShop tables
- [ ] Migration applies successfully to SQLite database
- [ ] Database schema matches model definitions
- [ ] Migration includes seed data

**Technical Details:**
- Use EF Core migration tools
- Test migration on clean database
- Verify all constraints and indexes

**Copilot Prompt:**
```
Create Entity Framework Core migrations for the new coffee tracking models in a .NET 8 Web API project.

The project structure:
- API project: `src/CoffeeTracker.Api/`
- DbContext: `src/CoffeeTracker.Api/Data/CoffeeTrackerDbContext.cs`
- Models: `src/CoffeeTracker.Api/Models/`
- Database: SQLite in `data/coffee-tracker.db`

Tasks:
1. Create a new migration named "AddCoffeeTrackingModels"
2. Verify migration includes CoffeeEntry and CoffeeShop tables
3. Apply migration to development database
4. Verify database schema is correct

Commands to run:
- Use dotnet ef commands from the API project directory
- Ensure migration includes all indexes and constraints
- Add seed data for sample coffee shops

Also create a simple database verification test that:
- Confirms tables exist
- Can insert and retrieve test data
- Verifies all constraints work

Include step-by-step instructions for other developers to run migrations.
```

---

## 🎯 Epic Definition of Done

- [ ] All domain models created with proper validation
- [ ] Database schema updated with migrations
- [ ] Unit tests achieve >90% coverage
- [ ] All tests pass
- [ ] Code follows clean architecture principles
- [ ] XML documentation on all public members
- [ ] Database can be created from scratch using migrations
- [ ] Sample seed data loads correctly

## 📋 Notes

**Technical Decisions:**
- Using Entity Framework Core with SQLite for local development
- Simple models for Phase 1, extensible for future phases
- Following domain-driven design principles
- Caffeine calculations based on research data

**Future Considerations:**
- Models are designed to be extended for authenticated users
- Coffee shop model will be enhanced with ratings/reviews in Phase 4
- Consider adding nutrition information in future versions

---

**Next Epic:** [Epic 2: Coffee Logging API Endpoints](./Epic-2-Coffee-Logging-API.md)
