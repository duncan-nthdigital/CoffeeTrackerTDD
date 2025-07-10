# Epic 2: Coffee Logging API Endpoints

**Epic Goal:** Create REST API endpoints for anonymous users to log and retrieve their coffee consumption data.

**Estimated Time:** 3-4 days  
**Priority:** High (Core Feature)  
**Dependencies:** Epic 1 (Domain Models)  

---

## 📋 Tasks

### Task 2.1: Create Coffee Logging Controller
**Estimated Time:** 3-4 hours  
**Priority:** High  

**Description:**
Create a REST API controller for coffee logging operations with anonymous user support.

**Acceptance Criteria:**
- [ ] Controller handles POST /api/coffee-entries (create entry)
- [ ] Controller handles GET /api/coffee-entries (get today's entries)
- [ ] Proper HTTP status codes and error handling
- [ ] Input validation and model binding
- [ ] Unit tests for all endpoints

**Technical Details:**
- Use ASP.NET Core Web API controller
- Support anonymous access (no authentication required)
- Return appropriate DTOs, not domain models directly
- Use dependency injection for services

**Copilot Prompt:**
```
Create a `CoffeeEntriesController` for a .NET 8 Web API project that supports anonymous coffee logging.

Controller requirements:
- Base route: `[Route("api/coffee-entries")]`
- Support anonymous access (no [Authorize] attributes)
- Use dependency injection for services
- Follow REST conventions

Endpoints needed:
1. POST /api/coffee-entries
   - Accept CoffeeEntry creation request
   - Validate input data
   - Return 201 Created with created entry
   - Return 400 Bad Request for validation errors

2. GET /api/coffee-entries
   - Return today's entries for anonymous session
   - Support query parameter: date (optional, defaults to today)
   - Return 200 OK with list of entries
   - Return empty list if no entries found

DTOs to create:
- CreateCoffeeEntryRequest (input DTO)
- CoffeeEntryResponse (output DTO)

Error handling:
- Use ProblemDetails for error responses
- Handle validation errors gracefully
- Log errors appropriately

Create comprehensive unit tests using xUnit and Moq that test:
- Happy path scenarios
- Validation error cases
- Edge cases and error conditions
- HTTP status codes and response formats

Place controller in `Controllers/CoffeeEntriesController.cs` and tests in the test project.
```

---

### Task 2.2: Create Coffee Service Layer
**Estimated Time:** 3-4 hours  
**Priority:** High  

**Description:**
Create a service layer to handle coffee logging business logic and data access.

**Acceptance Criteria:**
- [ ] Service interface and implementation created
- [ ] Business logic for anonymous session handling
- [ ] Data validation and business rules
- [ ] Proper error handling and logging
- [ ] Unit tests with mocked dependencies

**Technical Details:**
- Create ICoffeeService interface
- Implement service with repository pattern
- Handle anonymous user session identification
- Include business rule validation

**Copilot Prompt:**
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

---

### Task 2.3: Create Request/Response DTOs
**Estimated Time:** 2 hours  
**Priority:** High  

**Description:**
Create Data Transfer Objects for API requests and responses.

**Acceptance Criteria:**
- [ ] DTOs separate from domain models
- [ ] Proper validation attributes on request DTOs
- [ ] Response DTOs include computed properties
- [ ] AutoMapper configuration for mapping
- [ ] Unit tests verify mapping works correctly

**Technical Details:**
- Use separate DTOs for API contract
- Include FluentValidation for complex validation
- Add AutoMapper profiles for object mapping

**Copilot Prompt:**
```
Create DTOs (Data Transfer Objects) for the coffee logging API in a .NET 8 Web API project.

Request DTOs needed:
1. CreateCoffeeEntryRequest
   - CoffeeType (string, required, must be valid enum value)
   - Size (string, required, must be valid enum value)  
   - Source (string, optional, max 100 chars)
   - Timestamp (DateTime, optional, defaults to now, cannot be future)

2. GetCoffeeEntriesRequest  
   - Date (DateTime?, optional, defaults to today)

Response DTOs needed:
1. CoffeeEntryResponse
   - Id (int)
   - CoffeeType (string)
   - Size (string)
   - Source (string)
   - Timestamp (DateTime)
   - CaffeineAmount (int)
   - FormattedTimestamp (string, e.g., "2:30 PM")

2. DailySummaryResponse
   - Date (DateTime)
   - TotalEntries (int)
   - TotalCaffeine (int)
   - Entries (List<CoffeeEntryResponse>)
   - AverageCaffeinePerEntry (decimal)

Requirements:
- Use System.ComponentModel.DataAnnotations for validation
- Add custom validation attributes where needed
- Include XML documentation
- Create AutoMapper profile to map between domain models and DTOs
- Add FluentValidation rules for complex business logic validation

Create unit tests that verify:
- DTO validation works correctly
- AutoMapper mappings are correct
- All edge cases are handled
- Custom validation rules work

Place DTOs in `DTOs/` directory and AutoMapper profile in `Mapping/`.
```

---

### Task 2.4: Add Anonymous Session Management
**Estimated Time:** 2-3 hours  
**Priority:** High  

**Description:**
Implement session management for anonymous users using browser-based identification.

**Acceptance Criteria:**
- [ ] Middleware or service to handle anonymous sessions
- [ ] Session ID generation and validation
- [ ] 24-hour data retention policy
- [ ] Background cleanup of expired data
- [ ] Unit tests verify session isolation

**Technical Details:**
- Use browser fingerprinting or generated session IDs
- Store session info in headers or cookies
- Implement cleanup job for old data

**Copilot Prompt:**
```
Create anonymous session management for a .NET 8 Web API project supporting coffee tracking without user authentication.

Components needed:

1. Anonymous Session Middleware
   - Extract or generate session ID from request headers/cookies
   - Add session ID to HttpContext for use in controllers/services
   - Generate new session ID if none exists
   - Set session cookie with 24-hour expiration

2. Session Service
   - Interface: ISessionService
   - Methods:
     - GetOrCreateSessionId(HttpContext context)
     - IsSessionValid(string sessionId)
     - CleanupExpiredSessions()

3. Background Service for Cleanup
   - Run every hour to clean up expired anonymous data
   - Remove coffee entries older than 24 hours for anonymous sessions
   - Log cleanup operations

Session ID requirements:
- Use cryptographically secure random strings
- 32-character alphanumeric identifiers
- Store in HTTP-only cookie named "coffee-session"
- Include SameSite and Secure attributes for security

Data retention policy:
- Anonymous coffee entries expire after 24 hours
- Cleanup runs automatically via background service
- Manual cleanup method for testing

Create unit tests that verify:
- Session IDs are generated correctly
- Session isolation works between different session IDs
- Cleanup removes only expired data
- Middleware sets session context properly

Integration with existing CoffeeService:
- Update service to use session IDs for data filtering
- Ensure anonymous users only see their own data

Place middleware in `Middleware/`, service in `Services/`, and background service in `Services/Background/`.
```

---

### Task 2.5: Add Data Validation and Error Handling
**Estimated Time:** 2-3 hours  
**Priority:** Medium  

**Description:**
Implement comprehensive data validation and error handling for the API.

**Acceptance Criteria:**
- [ ] Custom validation attributes for business rules
- [ ] Global exception handling middleware
- [ ] Proper HTTP status codes for all scenarios
- [ ] Structured error responses using ProblemDetails
- [ ] Unit tests verify all error scenarios

**Technical Details:**
- Use ASP.NET Core model validation
- Add custom validators for coffee-specific rules
- Implement global exception handling
- Return consistent error format

**Copilot Prompt:**
```
Create comprehensive validation and error handling for the coffee logging API in a .NET 8 Web API project.

Components needed:

1. Custom Validation Attributes
   - ValidCoffeeTypeAttribute: validates against CoffeeType enum
   - ValidCoffeeSizeAttribute: validates against CoffeeSize enum
   - NotFutureDateAttribute: ensures timestamp is not in future
   - MaxDailyCaffeineAttribute: validates daily caffeine limit (configurable)

2. Global Exception Handler Middleware
   - Handle all unhandled exceptions
   - Convert to appropriate HTTP status codes
   - Return structured ProblemDetails responses
   - Log errors with correlation IDs
   - Handle specific exception types:
     - ValidationException → 400 Bad Request
     - NotFoundException → 404 Not Found
     - BusinessRuleException → 422 Unprocessable Entity
     - General exceptions → 500 Internal Server Error

3. Custom Exceptions
   - CoffeeTrackingException (base)
   - ValidationException
   - BusinessRuleException  
   - SessionNotFoundException
   - DailyCaffeineLimitExceededException

4. Validation Service
   - ICaffeeValidationService interface
   - Business rule validation:
     - Max 10 entries per day per session
     - Max 1000mg caffeine per day per session
     - Valid coffee type/size combinations
     - Reasonable timestamp values

Error Response Format:
- Use ProblemDetails standard (RFC 7807)
- Include correlation ID for tracking
- Provide user-friendly messages
- Include validation errors details

Create comprehensive unit tests that verify:
- All validation attributes work correctly
- Exception handling returns proper status codes
- Error messages are user-friendly
- Correlation IDs are included
- Business rules are enforced

Place validation in `Validation/`, exceptions in `Exceptions/`, and middleware in `Middleware/`.
```

---

### Task 2.6: Integration Tests for API Endpoints
**Estimated Time:** 3-4 hours  
**Priority:** Medium  

**Description:**
Create integration tests that test the complete API flow from HTTP request to database.

**Acceptance Criteria:**
- [ ] Tests use TestServer and in-memory database
- [ ] Tests cover happy path and error scenarios
- [ ] Tests verify database state changes
- [ ] Tests check HTTP status codes and response format
- [ ] Tests run independently and can be parallelized

**Technical Details:**
- Use ASP.NET Core Test Host
- SQLite in-memory database for testing
- Test actual HTTP requests and responses
- Verify end-to-end functionality

**Copilot Prompt:**
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

---

## 🎯 Epic Definition of Done

- [ ] All API endpoints created and working
- [ ] Full CRUD operations for coffee entries (Create, Read)
- [ ] Anonymous session management implemented
- [ ] Data validation and error handling complete
- [ ] Unit tests achieve >90% coverage
- [ ] Integration tests cover all major scenarios
- [ ] API follows REST conventions
- [ ] Proper HTTP status codes and error responses
- [ ] Performance requirements met (<500ms response time)
- [ ] API documentation ready (via Swagger/OpenAPI)

## 📋 Notes

**Technical Decisions:**
- Anonymous sessions use secure random IDs in HTTP-only cookies
- 24-hour data retention for anonymous users
- Maximum 10 coffee entries per day per session
- Maximum 1000mg caffeine per day per session
- Business rule validation in service layer

**API Design:**
- RESTful endpoints following HTTP conventions
- Consistent error response format using ProblemDetails
- Input validation at multiple layers (DTO, service, business rules)
- Session-based data isolation for anonymous users

**Security Considerations:**
- No authentication required for anonymous access
- Session isolation prevents data leakage
- Input validation prevents injection attacks
- Rate limiting may be added in future versions

---

**Next Epic:** [Epic 4: Blazor UI Components](./Epic-4-Blazor-UI.md)  
**Previous Epic:** [Epic 1: Domain Models](./Epic-1-Domain-Models.md)
