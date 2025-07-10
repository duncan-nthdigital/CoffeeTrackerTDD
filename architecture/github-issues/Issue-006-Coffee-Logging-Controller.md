# Issue #006: Create Coffee Logging Controller

**Labels:** `epic-2`, `api-controller`, `high-priority`, `rest-api`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 2 - Coffee Logging API Endpoints  
**Estimated Time:** 3-4 hours  

## 📋 Description

Create a REST API controller for coffee logging operations with anonymous user support. This controller will be the main entry point for coffee entry creation and retrieval.

## 🎯 Acceptance Criteria

- [ ] Controller handles POST /api/coffee-entries (create entry)
- [ ] Controller handles GET /api/coffee-entries (get today's entries)
- [ ] Proper HTTP status codes and error handling
- [ ] Input validation and model binding
- [ ] Unit tests for all endpoints with >90% coverage
- [ ] Swagger/OpenAPI documentation auto-generated and configured
- [ ] Swagger UI accessible at /swagger endpoint
- [ ] API documentation includes examples and descriptions
- [ ] Anonymous access support (no authentication required)

## 🔧 Technical Requirements

- Use ASP.NET Core Web API controller
- Support anonymous access (no authentication required)
- Return appropriate DTOs, not domain models directly
- Use dependency injection for services
- Follow REST conventions and HTTP standards
- Implement proper error handling with ProblemDetails
- Configure Swagger/OpenAPI documentation
- Add XML documentation comments for Swagger generation

## 📝 Implementation Details

### File Locations
- **Controller**: `src/CoffeeTracker.Api/Controllers/CoffeeEntriesController.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Controllers/CoffeeEntriesControllerTests.cs`

### API Endpoints
```http
POST /api/coffee-entries
- Accept CoffeeEntry creation request
- Validate input data
- Return 201 Created with created entry
- Return 400 Bad Request for validation errors

GET /api/coffee-entries
- Return today's entries for anonymous session
- Support query parameter: date (optional, defaults to today)
- Return 200 OK with list of entries
- Return empty list if no entries found
```

### Controller Structure
```csharp
[ApiController]
[Route("api/coffee-entries")]
public class CoffeeEntriesController : ControllerBase
{
    // Constructor injection for services
    // POST endpoint for creating entries
    // GET endpoint for retrieving entries
    // Proper error handling and validation
}
```

## 🤖 Copilot Prompt

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

Swagger/OpenAPI Configuration:
- Configure Swagger in Program.cs with proper API info (title, version, description)
- Add XML documentation generation to project file
- Include XML comments on all controller actions for Swagger documentation
- Use [ProducesResponseType] attributes to document response types
- Add examples for request/response DTOs using [Example] attributes
- Configure Swagger UI to be accessible at /swagger endpoint
- Include proper HTTP status code documentation

XML Documentation Requirements:
- Add /// <summary> comments to all controller actions
- Document parameters with /// <param> tags
- Document return values with /// <returns> tags
- Include example values in documentation

Create comprehensive unit tests using xUnit and Moq that test:
- Happy path scenarios
- Validation error cases
- Edge cases and error conditions
- HTTP status codes and response formats

Place controller in `Controllers/CoffeeEntriesController.cs` and tests in the test project.
```

## ✅ Definition of Done

- [ ] CoffeeEntriesController created with all required endpoints
- [ ] POST endpoint creates coffee entries successfully
- [ ] GET endpoint retrieves entries with optional date filtering
- [ ] Proper HTTP status codes returned for all scenarios
- [ ] Input validation works correctly
- [ ] Error handling returns appropriate ProblemDetails responses
- [ ] Unit tests written with >90% coverage
- [ ] All tests passing
- [ ] Swagger/OpenAPI configuration completed in Program.cs
- [ ] Swagger UI accessible at /swagger endpoint
- [ ] XML documentation generated and included in Swagger
- [ ] All controller actions documented with XML comments
- [ ] Response types documented with [ProducesResponseType] attributes
- [ ] Swagger documentation includes examples and proper descriptions
- [ ] Code follows project coding standards
- [ ] Dependencies properly injected
- [ ] Anonymous access works without authentication

## 🔗 Related Issues

- Depends on: Epic 1 completion (Domain Models & Database)
- Blocks: #007 (Coffee Service Layer), #010 (Data Validation)
- Epic: #Epic-2 (Coffee Logging API Endpoints)
- Works with: #008 (DTOs), #009 (Session Management)

## 📌 Notes

- **REST API Focus**: Follow HTTP conventions and REST best practices
- **Anonymous Support**: No authentication required for MVP
- **Error Handling**: Use ASP.NET Core ProblemDetails standard
- **Testing**: Comprehensive unit tests for all scenarios
- **Documentation**: Auto-generated Swagger/OpenAPI docs

## 🧪 Test Scenarios

### Happy Path Tests
- Create coffee entry with valid data
- Retrieve today's coffee entries
- Retrieve entries for specific date
- Handle empty results gracefully

### Validation Tests
- Invalid coffee type
- Invalid coffee size
- Missing required fields
- Future timestamp validation
- Malformed JSON requests

### Error Handling Tests
- Service layer exceptions
- Database connection issues
- Unexpected server errors
- Large request payloads

### HTTP Status Code Tests
- 200 OK for successful GET requests
- 201 Created for successful POST requests
- 400 Bad Request for validation errors
- 500 Internal Server Error for server issues

## 🔍 API Design Checklist

- [ ] RESTful endpoint design
- [ ] Consistent naming conventions
- [ ] Proper HTTP methods usage
- [ ] Appropriate status codes
- [ ] Error response format standardized
- [ ] Request/response DTOs defined
- [ ] API versioning considered
- [ ] Rate limiting prepared (future)

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
