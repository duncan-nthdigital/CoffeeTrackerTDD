# Issue #007: Create Coffee Service Layer

**Labels:** `epic-2`, `service-layer`, `high-priority`, `business-logic`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 2 - Coffee Logging API Endpoints  
**Estimated Time:** 3-4 hours  

## 📋 Description

Create a service layer to handle coffee logging business logic and data access. This service will encapsulate all business rules and provide a clean interface between controllers and data access.

## 🎯 Acceptance Criteria

- [ ] Service interface and implementation created
- [ ] Business logic for anonymous session handling
- [ ] Data validation and business rules implementation
- [ ] Proper error handling and logging
- [ ] Unit tests with mocked dependencies achieving >90% coverage
- [ ] Repository pattern implementation for data access
- [ ] Asynchronous operation support

## 🔧 Technical Requirements

- Create ICoffeeService interface
- Implement service with repository pattern
- Handle anonymous user session identification
- Include business rule validation
- Use Entity Framework Core for data access
- Implement proper logging and error handling
- Support dependency injection

## 📝 Implementation Details

### File Locations
- **Interface**: `src/CoffeeTracker.Api/Services/ICoffeeService.cs`
- **Implementation**: `src/CoffeeTracker.Api/Services/CoffeeService.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Services/CoffeeServiceTests.cs`

### Service Interface
```csharp
public interface ICoffeeService
{
    Task<CoffeeEntryResponse> CreateCoffeeEntryAsync(CreateCoffeeEntryRequest request, string sessionId);
    Task<IEnumerable<CoffeeEntryResponse>> GetCoffeeEntriesAsync(string sessionId, DateTime? date = null);
    Task<DailySummaryResponse> GetDailySummaryAsync(string sessionId, DateTime? date = null);
}
```

### Business Rules
- Maximum 10 coffee entries per day per session
- Maximum 1000mg caffeine per day per session
- Caffeine calculation based on type and size
- Timestamp validation (not future dates)
- Data retention: 24 hours for anonymous users
- Session isolation for data access

## 🤖 Copilot Prompt

```
Create a service layer for coffee logging in a .NET 8 Web API project.

Create `ICoffeeService` interface with methods:
- Task<CoffeeEntryResponse> CreateCoffeeEntryAsync(CreateCoffeeEntryRequest request, string sessionId)
- Task<IEnumerable<CoffeeEntryResponse>> GetCoffeeEntriesAsync(string sessionId, DateTime? date = null)
- Task<DailySummaryResponse> GetDailySummaryAsync(string sessionId, DateTime? date = null)

Create `CoffeeService` implementation with:
- Constructor injection of DbContext and ILogger
- Anonymous session handling using sessionId (could be browser fingerprint/cookie)
- Business rules validation:
  - Maximum 10 coffee entries per day
  - Caffeine calculation based on type and size
  - Timestamp validation (not future dates)
- Proper error handling with custom exceptions
- Logging for important operations

Key business logic:
- Anonymous users identified by sessionId (passed from frontend)
- Data retention: only keep entries for 24 hours for anonymous users
- Calculate total daily caffeine intake
- Validate coffee type and size combinations

Create unit tests that verify:
- Coffee entries can be created and retrieved
- Business rules are enforced
- Error conditions are handled properly
- Session isolation works correctly
- Data cleanup for old anonymous entries

Use Entity Framework Core for data access.
Place service in `Services/CoffeeService.cs` with interface in `Services/ICoffeeService.cs`.
```

## ✅ Definition of Done

- [ ] ICoffeeService interface defined with all required methods
- [ ] CoffeeService implementation completed
- [ ] Business rules validation implemented and tested
- [ ] Anonymous session handling working correctly
- [ ] Caffeine calculation logic implemented
- [ ] Data retention policy enforced
- [ ] Error handling with custom exceptions
- [ ] Comprehensive logging implemented
- [ ] Unit tests written with >90% coverage
- [ ] All tests passing
- [ ] Dependency injection configured
- [ ] Performance optimized for expected load

## 🔗 Related Issues

- Depends on: Epic 1 completion (Domain Models & Database)
- Blocks: #006 (Controller), #009 (Session Management)
- Epic: #Epic-2 (Coffee Logging API Endpoints)
- Works with: #008 (DTOs), #010 (Validation)

## 📌 Notes

- **Business Logic Focus**: Encapsulate all coffee tracking rules
- **Session Management**: Handle anonymous user sessions securely
- **Data Integrity**: Enforce business rules at service layer
- **Performance**: Optimize for concurrent anonymous users
- **Testing**: Mock all external dependencies for unit tests

## 🧪 Test Scenarios

### Business Rule Tests
- Enforce daily entry limit (10 per session)
- Enforce daily caffeine limit (1000mg per session)
- Validate coffee type and size combinations
- Prevent future date entries
- Calculate caffeine amounts correctly

### Session Management Tests
- Isolate data between different sessions
- Handle invalid or missing session IDs
- Clean up expired anonymous data
- Maintain session state consistency

### Data Access Tests
- Create coffee entries successfully
- Retrieve entries by session and date
- Generate daily summaries correctly
- Handle database connection issues

### Error Handling Tests
- Invalid input data
- Business rule violations
- Database exceptions
- Missing required dependencies

## 🔍 Service Design Checklist

- [ ] Interface segregation principle followed
- [ ] Dependency injection ready
- [ ] Asynchronous operations implemented
- [ ] Business rules clearly defined
- [ ] Error handling comprehensive
- [ ] Logging strategically placed
- [ ] Performance considerations addressed
- [ ] Unit testing comprehensive

## 🏗️ Architecture Notes

**Service Layer Responsibilities:**
- Business logic implementation
- Data validation and transformation
- Session management integration
- Error handling and logging
- Performance optimization

**Dependencies:**
- DbContext for data access
- ILogger for logging
- AutoMapper for object mapping
- Custom exceptions for error handling

**Integration Points:**
- Called by Controllers
- Uses Domain Models
- Integrates with Session Management
- Connects to Database via EF Core

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
