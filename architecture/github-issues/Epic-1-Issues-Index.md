# GitHub Issues - Epic 1: Core Domain Models & Database Setup

**Epic Overview:** Create the foundational data models and database structure for anonymous coffee tracking.  
**Total Issues:** 5  
**Epic Priority:** High (Foundation)  
**Epic Status:** 🚧 Ready for Development  

---

## 📋 Issue List

### Issue #001: Create Coffee Entry Domain Model
- **File:** [`Issue-001-Coffee-Entry-Domain-Model.md`](./Issue-001-Coffee-Entry-Domain-Model.md)
- **Priority:** High
- **Estimated Time:** 2-3 hours
- **Status:** 🆕 Ready for Development
- **Dependencies:** None (foundation)
- **Description:** Create the core CoffeeEntry model for anonymous coffee tracking

### Issue #002: Create Coffee Type and Size Enumerations  
- **File:** [`Issue-002-Coffee-Type-Size-Enums.md`](./Issue-002-Coffee-Type-Size-Enums.md)
- **Priority:** High
- **Estimated Time:** 1-2 hours
- **Status:** 🆕 Ready for Development
- **Dependencies:** None
- **Description:** Create enumerations for coffee types and sizes with caffeine calculations

### Issue #003: Update DbContext with Coffee Models
- **File:** [`Issue-003-Update-DbContext-Coffee-Models.md`](./Issue-003-Update-DbContext-Coffee-Models.md)
- **Priority:** High
- **Estimated Time:** 1-2 hours
- **Status:** ⏳ Waiting (depends on #001, #002)
- **Dependencies:** Issues #001, #002
- **Description:** Update DbContext to include coffee tracking tables with proper configuration

### Issue #004: Create Coffee Shop Basic Model
- **File:** [`Issue-004-Coffee-Shop-Basic-Model.md`](./Issue-004-Coffee-Shop-Basic-Model.md)
- **Priority:** Medium
- **Estimated Time:** 1-2 hours
- **Status:** ⏳ Waiting (depends on #003)
- **Dependencies:** Issue #003
- **Description:** Create basic coffee shop model for coffee source selection

### Issue #005: Create Database Migration and Schema Update
- **File:** [`Issue-005-Database-Migration.md`](./Issue-005-Database-Migration.md)
- **Priority:** High
- **Estimated Time:** 1 hour
- **Status:** ⏳ Waiting (depends on all previous)
- **Dependencies:** Issues #001, #002, #003, #004
- **Description:** Create and apply EF Core migrations for the new models

---

## 🎯 Epic Progress

### Completion Status
- [ ] Issue #001: Create Coffee Entry Domain Model
- [ ] Issue #002: Create Coffee Type and Size Enumerations
- [ ] Issue #003: Update DbContext with Coffee Models
- [ ] Issue #004: Create Coffee Shop Basic Model
- [ ] Issue #005: Create Database Migration and Schema Update

### Epic Definition of Done
- [ ] All domain models created with proper validation
- [ ] Database schema updated with migrations
- [ ] Unit tests achieve >90% coverage
- [ ] All tests pass
- [ ] Code follows clean architecture principles
- [ ] XML documentation on all public members
- [ ] Database can be created from scratch using migrations
- [ ] Sample seed data loads correctly

---

## 🚀 Getting Started

### Recommended Development Order
1. **Start with Issue #001** - Foundation model that everything depends on
2. **Complete Issue #002** - Enumerations needed for model completeness
3. **Work on Issue #003** - Database context updates
4. **Add Issue #004** - Coffee shop model for complete data structure
5. **Finish with Issue #005** - Database migration to implement everything

### Prerequisites
- .NET 8 SDK installed
- Entity Framework tools: `dotnet tool install --global dotnet-ef`
- SQLite support configured
- xUnit testing framework ready

### Development Workflow
1. **Read Issue**: Review the full issue file for context
2. **Copy Prompt**: Use the "Copilot Prompt" section for implementation
3. **Follow TDD**: Write tests first, then implement
4. **Validate**: Ensure all acceptance criteria are met
5. **Update Status**: Mark issue as complete and move to next

---

## 📊 Epic Dependencies

```
Epic 1 (Foundation) → All Other Epics
     ↓
   Issue #001 → Issue #002 → Issue #003 → Issue #004 → Issue #005
                                              ↓
                                        Complete Epic 1
                                              ↓
                                          Epic 2 Ready
```

### Blocking Relationships
- **Epic 1** blocks all other epics (foundation dependency)
- **Issue #001** should be completed first (core model)
- **Issue #005** requires all previous issues (migration needs all models)

---

## 🔧 Technical Notes

### Key Technologies
- **.NET 8** - Latest framework version
- **Entity Framework Core 8** - ORM for database operations
- **SQLite** - Local development database
- **xUnit** - Unit testing framework
- **System.ComponentModel.DataAnnotations** - Model validation

### Code Quality Standards
- **TDD Approach** - Tests written before implementation
- **Clean Code** - Follow SOLID principles and clean architecture
- **XML Documentation** - All public members documented
- **90%+ Test Coverage** - Comprehensive testing required
- **Domain-Driven Design** - Models reflect business domain

### File Organization
```
src/CoffeeTracker.Api/
├── Models/
│   ├── CoffeeEntry.cs
│   ├── CoffeeType.cs
│   ├── CoffeeSize.cs
│   └── CoffeeShop.cs
├── Data/
│   └── CoffeeTrackerDbContext.cs
└── Migrations/
    └── [timestamp]_AddCoffeeTrackingModels.cs

test/CoffeeTracker.Api.Tests/
├── Models/
└── Data/
```

---

## 📝 Notes for Developers

### Using the Issues
1. Each issue is self-contained with complete context
2. Copilot prompts are ready to use for implementation  
3. Acceptance criteria define completion requirements
4. All dependencies are clearly documented

### TDD Workflow
1. **Red**: Write failing test based on acceptance criteria
2. **Green**: Write minimal code to make test pass
3. **Refactor**: Clean up code while keeping tests green
4. **Repeat**: Continue until all acceptance criteria met

### Getting Help
- Issues contain complete context for implementation
- Copilot prompts are crafted for optimal AI assistance
- Dependencies and relationships are clearly documented
- Epic file contains additional technical details

---

**Created:** July 10, 2025  
**Last Updated:** July 10, 2025  
**Epic Status:** Ready for Development  
**Next Step:** Begin with Issue #001
