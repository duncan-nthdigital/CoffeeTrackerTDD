# Issue #002: Create Coffee Type and Size Enumerations

**Labels:** `epic-1`, `enumeration`, `high-priority`, `foundation`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 1 - Core Domain Models & Database Setup  
**Estimated Time:** 1-2 hours  

## 📋 Description

Create enumerations for coffee types and sizes with associated caffeine content calculations. This provides standardized coffee types and enables accurate caffeine tracking across the application.

## 🎯 Acceptance Criteria

- [ ] CoffeeType enum includes common coffee types with base caffeine values
- [ ] CoffeeSize enum includes size variations with multipliers
- [ ] Extension methods provide caffeine content calculations
- [ ] Display names configured for user-friendly presentation
- [ ] Unit tests verify all calculations are accurate

## 🔧 Technical Requirements

- Create separate enum files for organization
- Use System.ComponentModel.DataAnnotations.Display for display names
- Add extension methods for calculations
- Include XML documentation
- Follow C# naming conventions

## 📝 Implementation Details

### File Locations
- **Coffee Types**: `src/CoffeeTracker.Api/Models/CoffeeType.cs`
- **Coffee Sizes**: `src/CoffeeTracker.Api/Models/CoffeeSize.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Models/CoffeeTypeTests.cs`

### Coffee Types Required
```csharp
- Espresso (90mg caffeine base)
- Americano (120mg caffeine base)
- Latte (80mg caffeine base) 
- Cappuccino (80mg caffeine base)
- Mocha (90mg caffeine base)
- Macchiato (120mg caffeine base)
- FlatWhite (130mg caffeine base)
- BlackCoffee (95mg caffeine base)
```

### Coffee Sizes Required
```csharp
- Small (0.8x multiplier)
- Medium (1.0x multiplier)  
- Large (1.3x multiplier)
- ExtraLarge (1.6x multiplier)
```

## 🤖 Copilot Prompt

```
Create C# enumerations for coffee types and sizes in a .NET 8 Web API project with these specifications:

Create a `CoffeeType` enum with these coffee types:
- Espresso (90mg caffeine base)
- Americano (120mg caffeine base)
- Latte (80mg caffeine base) 
- Cappuccino (80mg caffeine base)
- Mocha (90mg caffeine base)
- Macchiato (120mg caffeine base)
- FlatWhite (130mg caffeine base)
- BlackCoffee (95mg caffeine base)

Create a `CoffeeSize` enum:
- Small (0.8x multiplier)
- Medium (1.0x multiplier)  
- Large (1.3x multiplier)
- ExtraLarge (1.6x multiplier)

Create extension methods:
- `GetBaseCaffeineContent(this CoffeeType type)` - returns base mg
- `GetCaffeineContent(this CoffeeType type, CoffeeSize size)` - returns calculated mg
- `GetDisplayName(this CoffeeType type)` - returns user-friendly name
- `GetDisplayName(this CoffeeSize size)` - returns user-friendly name

Requirements:
- Use System.ComponentModel.DataAnnotations.Display for display names
- Add XML documentation for all members
- Create comprehensive unit tests that verify all caffeine calculations
- Follow clean code principles
- Make calculations accurate and realistic

Place in `Models/CoffeeType.cs` and `Models/CoffeeSize.cs` with tests in the test project.
```

## ✅ Definition of Done

- [ ] CoffeeType enum created with all required coffee types
- [ ] CoffeeSize enum created with size multipliers
- [ ] Extension methods implemented for caffeine calculations
- [ ] Display attributes added for user-friendly names
- [ ] XML documentation added to all public members
- [ ] Unit tests written covering all calculations
- [ ] All caffeine calculations verified for accuracy
- [ ] Tests achieve >90% coverage
- [ ] All tests pass

## 🔗 Related Issues

- Depends on: None
- Related to: #001 (Coffee Entry Domain Model)
- Blocks: #003 (Update DbContext)
- Epic: #Epic-1 (Core Domain Models & Database Setup)

## 📌 Notes

- Caffeine values are based on industry standards and research
- Extension methods enable easy calculation throughout the app
- Enums provide type safety and consistency
- Display names support future internationalization
- Keep calculations realistic for health tracking

## 🧪 Test Scenarios

- Verify each coffee type returns correct base caffeine
- Test size multipliers calculate correctly
- Validate all combinations of type + size
- Ensure display names are user-friendly
- Test edge cases and boundary conditions

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
