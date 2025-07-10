# Issue #001: Create Coffee Entry Domain Model

**Labels:** `epic-1`, `domain-model`, `high-priority`, `foundation`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 1 - Core Domain Models & Database Setup  
**Estimated Time:** 2-3 hours  

## 📋 Description

Create the core `CoffeeEntry` model for anonymous users with basic tracking properties. This is the foundational model for all coffee consumption tracking in the application.

## 🎯 Acceptance Criteria

- [ ] Model includes: Id, CoffeeType, Size, Source, Timestamp, CaffeineAmount
- [ ] Model has proper validation attributes
- [ ] Model follows domain-driven design principles
- [ ] Unit tests verify model behavior
- [ ] XML documentation for all public properties
- [ ] Caffeine calculation method implemented

## 🔧 Technical Requirements

- Create in `src/CoffeeTracker.Api/Models/` directory
- Use System.ComponentModel.DataAnnotations for validation
- Include XML documentation for all public properties
- Follow C# naming conventions
- Use .NET 8 features where appropriate

## 📝 Implementation Details

### File Location
- **Model**: `src/CoffeeTracker.Api/Models/CoffeeEntry.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Models/CoffeeEntryTests.cs`

### Properties Required
```csharp
- Id (int, primary key)
- CoffeeType (string, required, max 50 chars) - e.g., "Espresso", "Latte", "Cappuccino"
- Size (string, required) - e.g., "Small", "Medium", "Large" 
- Source (string, optional, max 100 chars) - coffee shop name or "Home"
- Timestamp (DateTime, required, defaults to UTC now)
- CaffeineAmount (int, calculated based on type and size)
```

## 🤖 Copilot Prompt

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

## ✅ Definition of Done

- [ ] CoffeeEntry model created with all required properties
- [ ] Data validation attributes applied correctly
- [ ] XML documentation added to all public members
- [ ] Constructor sets Timestamp to UTC automatically
- [ ] Caffeine calculation method implemented and tested
- [ ] Unit tests written with >90% coverage
- [ ] All tests pass
- [ ] Code follows clean code principles
- [ ] Model follows domain-driven design patterns

## 🔗 Related Issues

- Depends on: None (this is the foundation)
- Blocks: #002 (Coffee Type Enumeration)
- Epic: #Epic-1 (Core Domain Models & Database Setup)

## 📌 Notes

- This model is designed for Phase 1 anonymous users
- Keep it simple but extensible for future authenticated user features
- Caffeine calculations should be based on realistic values
- Consider future internationalization needs

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
