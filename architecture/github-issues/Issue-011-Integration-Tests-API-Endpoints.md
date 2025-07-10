# Issue #011: Integration Tests for API Endpoints

**Labels:** `epic-2`, `integration-tests`, `medium-priority`, `testing`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 2 - Coffee Logging API Endpoints  
**Estimated Time:** 3-4 hours  

## 📋 Description

Create comprehensive integration tests that test the complete API flow from HTTP request to database, ensuring all components work together correctly in realistic scenarios.

## 🎯 Acceptance Criteria

- [ ] Tests use TestServer and in-memory database
- [ ] Tests cover happy path and error scenarios
- [ ] Tests verify database state changes
- [ ] Tests check HTTP status codes and response format
- [ ] Tests run independently and can be parallelized
- [ ] Performance tests validate response time requirements
- [ ] Tests verify session management integration

## 🔧 Technical Requirements

- Use ASP.NET Core Test Host
- SQLite in-memory database for testing
- Test actual HTTP requests and responses
- Verify end-to-end functionality
- Ensure test isolation and cleanup
- Mock external dependencies appropriately

## 📝 Implementation Details

### File Locations
- **Test Project**: `test/CoffeeTracker.Api.IntegrationTests/`
- **Base Classes**: `test/CoffeeTracker.Api.IntegrationTests/TestBase.cs`
- **Test Utilities**: `test/CoffeeTracker.Api.IntegrationTests/Utilities/`
- **Test Data**: `test/CoffeeTracker.Api.IntegrationTests/TestData/`

### Test Structure
```csharp
// Base test class with common setup
public class ApiIntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    // Common setup for all integration tests
    // In-memory database configuration
    // Test server setup
    // Helper methods for HTTP requests
}

// Coffee Entries API Integration Tests
public class CoffeeEntriesApiTests : ApiIntegrationTestBase
{
    // POST /api/coffee-entries tests
    // GET /api/coffee-entries tests
    // Error scenario tests
    // Session management tests
}
```

### Test Categories
1. **Happy Path Tests**: Successful operations
2. **Validation Tests**: Input validation scenarios
3. **Business Rule Tests**: Business logic enforcement
4. **Session Tests**: Anonymous session management
5. **Error Handling Tests**: Exception scenarios
6. **Performance Tests**: Response time validation

## 🤖 Copilot Prompt

```
Create comprehensive integration tests for the coffee logging API in a .NET 8 Web API project.

Test setup requirements:
- Use Microsoft.AspNetCore.Mvc.Testing.TestServer
- SQLite in-memory database for each test
- Fresh database state for each test method
- Mock external dependencies if any
- Test the complete request/response cycle

Test scenarios to cover:

1. POST /api/coffee-entries
   - Happy path: valid coffee entry creation
   - Validation errors: invalid coffee type, size, future date
   - Business rule violations: too many entries per day, caffeine limit
   - Session isolation: entries from different sessions don't interfere

2. GET /api/coffee-entries  
   - Happy path: retrieve today's entries
   - Empty result: no entries for session
   - Date filtering: entries for specific date
   - Session isolation: only get entries for current session

3. Error handling
   - Invalid JSON in request body
   - Missing required fields
   - Database connection issues (simulate)
   - Large request payloads

4. Session management
   - New session ID generation
   - Session ID persistence across requests
   - Session expiration and cleanup

Test structure:
- Use xUnit with IClassFixture<WebApplicationFactory>
- Create base test class with common setup
- Use meaningful test names following AAA pattern
- Assert HTTP status codes, response headers, and body content
- Verify database state after operations

Performance tests:
- Test response times under normal load
- Verify API can handle concurrent requests
- Test with large datasets (cleanup scenarios)

Create test utilities:
- Helper methods for creating test coffee entries
- Extensions for asserting API responses
- Data builders for test data creation

Place integration tests in separate test project: `CoffeeTracker.Api.IntegrationTests`
```

## ✅ Definition of Done

- [ ] Integration test project created and configured
- [ ] Base test class with common setup implemented
- [ ] Tests for POST /api/coffee-entries endpoint (happy path and errors)
- [ ] Tests for GET /api/coffee-entries endpoint (happy path and filtering)
- [ ] Session management integration tests
- [ ] Error handling integration tests
- [ ] Performance tests for response time requirements
- [ ] Database state verification tests
- [ ] Test utilities and helpers created
- [ ] All tests passing and can run in parallel
- [ ] Test coverage reports generated
- [ ] CI/CD integration ready

## 🔗 Related Issues

- Depends on: All previous Epic 2 issues (#006-#010)
- Epic: #Epic-2 (Coffee Logging API Endpoints)
- Validates: Complete Epic 2 implementation

## 📌 Notes

- **End-to-End Testing**: Tests the complete request/response cycle
- **Realistic Scenarios**: Use real HTTP requests and database operations
- **Test Isolation**: Each test runs with fresh state
- **Performance Validation**: Ensure <500ms response time requirement
- **Session Testing**: Verify anonymous session management works correctly

## 🧪 Test Scenarios

### POST /api/coffee-entries Tests
```csharp
[Fact]
public async Task CreateCoffeeEntry_WithValidData_Returns201Created()

[Fact] 
public async Task CreateCoffeeEntry_WithInvalidCoffeeType_Returns400BadRequest()

[Fact]
public async Task CreateCoffeeEntry_ExceedingDailyLimit_Returns422UnprocessableEntity()

[Fact]
public async Task CreateCoffeeEntry_WithDifferentSessions_IsolatesData()
```

### GET /api/coffee-entries Tests
```csharp
[Fact]
public async Task GetCoffeeEntries_ForToday_ReturnsCurrentDayEntries()

[Fact]
public async Task GetCoffeeEntries_WithDateFilter_ReturnsFilteredEntries()

[Fact]
public async Task GetCoffeeEntries_WithEmptyResult_ReturnsEmptyList()

[Fact]
public async Task GetCoffeeEntries_WithDifferentSessions_ReturnsOnlySessionData()
```

### Session Management Tests
```csharp
[Fact]
public async Task NewRequest_WithoutSessionCookie_GeneratesNewSession()

[Fact]
public async Task SubsequentRequests_WithSessionCookie_MaintainsSession()

[Fact]
public async Task ExpiredSession_CleansUpData_AfterRetentionPeriod()
```

### Error Handling Tests
```csharp
[Fact]
public async Task InvalidJson_Returns400WithProblemDetails()

[Fact]
public async Task DatabaseError_Returns500WithCorrelationId()

[Fact]
public async Task ValidationError_Returns400WithValidationDetails()
```

### Performance Tests
```csharp
[Fact]
public async Task CreateCoffeeEntry_CompletesWithin500Milliseconds()

[Fact]
public async Task GetCoffeeEntries_WithLargeDataset_CompletesWithin500Milliseconds()

[Fact]
public async Task ConcurrentRequests_HandleMultipleSessionsCorrectly()
```

## 🔍 Integration Test Checklist

- [ ] TestServer configured correctly
- [ ] In-memory database isolated per test
- [ ] HTTP requests use realistic data
- [ ] Response format validation
- [ ] Status code verification
- [ ] Database state assertions
- [ ] Session cookie handling
- [ ] Error response format validation
- [ ] Performance benchmarks met
- [ ] Concurrent request handling

## 🏗️ Test Architecture

**Test Organization:**
```
CoffeeTracker.Api.IntegrationTests/
├── TestBase.cs (Common setup)
├── CoffeeEntriesApiTests.cs (Endpoint tests)
├── SessionManagementTests.cs (Session tests)
├── ErrorHandlingTests.cs (Error scenarios)
├── PerformanceTests.cs (Performance validation)
├── Utilities/
│   ├── TestDataBuilder.cs
│   ├── ApiTestExtensions.cs
│   └── DatabaseTestHelpers.cs
└── TestData/
    ├── ValidCoffeeEntries.json
    └── InvalidRequestExamples.json
```

**Test Data Management:**
- Use builder pattern for test data creation
- Include realistic test scenarios
- Cover edge cases and boundary conditions
- Maintain test data separate from production data

**Assertion Strategy:**
- Verify HTTP status codes
- Validate response content and format
- Check database state changes
- Assert session behavior
- Validate error response structure

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
