# Issue #013: Create Coffee Shop API Controller

**Labels:** `epic-3`, `api-controller`, `high-priority`, `coffee-shop-api`, `swagger`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 3 - Coffee Shop Data & Locator API  
**Estimated Time:** 2-3 hours  

## 📋 Description

Create REST API endpoints for coffee shop data retrieval and basic search functionality with comprehensive Swagger/OpenAPI documentation. This controller provides read-only access to coffee shop data for anonymous users.

## 🎯 Acceptance Criteria

- [ ] Controller handles GET /api/coffee-shops (list all active shops)
- [ ] Controller handles GET /api/coffee-shops/{id} (get shop details)
- [ ] Controller handles GET /api/coffee-shops/search (search by name/location)
- [ ] Proper error handling and validation
- [ ] Unit tests for all endpoints with >90% coverage
- [ ] Swagger/OpenAPI documentation configured for all endpoints
- [ ] XML documentation comments added to all controller actions
- [ ] Response types documented with [ProducesResponseType] attributes
- [ ] Integration with existing Swagger UI from Epic 2

## 🔧 Technical Requirements

- Read-only API for Phase 1 (no creation/updates)
- Support basic search and filtering
- Return paginated results for large datasets
- Include comprehensive Swagger/OpenAPI documentation
- Follow same documentation standards as Epic 2 APIs
- Use dependency injection for services
- Support anonymous access (no authentication required)

## 📝 Implementation Details

### File Locations
- **Controller**: `src/CoffeeTracker.Api/Controllers/CoffeeShopsController.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Controllers/CoffeeShopsControllerTests.cs`

### API Endpoints
```http
GET /api/coffee-shops
- Return list of active coffee shops
- Support query parameters: page, pageSize, search
- Return 200 OK with paginated results

GET /api/coffee-shops/{id}
- Return specific coffee shop by ID
- Return 200 OK with shop details
- Return 404 Not Found if shop doesn't exist or inactive

GET /api/coffee-shops/search
- Search coffee shops by name or address
- Support query parameters: q (required), page, pageSize
- Return 200 OK with search results
- Return 400 Bad Request if query is empty
```

### DTOs Required
```csharp
// CoffeeShopResponse (output DTO)
public class CoffeeShopResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// CoffeeShopSearchResponse (search results with metadata)
public class CoffeeShopSearchResponse : PagedResponse<CoffeeShopResponse>
{
    public string SearchQuery { get; set; }
    public int SearchResultCount { get; set; }
}

// PagedResponse<T> (generic pagination wrapper)
public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
```

## 🤖 Copilot Prompt

```
Create a `CoffeeShopsController` for a .NET 8 Web API project that provides coffee shop data for anonymous users with comprehensive Swagger documentation.

Controller requirements:
- Base route: `[Route("api/coffee-shops")]`
- Read-only operations (GET only for Phase 1)
- Support anonymous access
- Use dependency injection for services
- Follow REST conventions

Endpoints needed:
1. GET /api/coffee-shops
   - Return list of active coffee shops
   - Support query parameters:
     - page (int, default 1)
     - pageSize (int, default 20, max 100)
     - search (string, optional) - search by name
   - Return 200 OK with paginated results

2. GET /api/coffee-shops/{id}
   - Return specific coffee shop by ID
   - Return 200 OK with shop details
   - Return 404 Not Found if shop doesn't exist or inactive

3. GET /api/coffee-shops/search
   - Search coffee shops by name or address
   - Support query parameters:
     - q (string, required) - search query
     - page, pageSize (same as above)
   - Return 200 OK with search results
   - Return 400 Bad Request if query is empty

DTOs to create:
- CoffeeShopResponse (output DTO)
- CoffeeShopSearchResponse (search results with metadata)
- PagedResponse<T> (generic pagination wrapper)

Error handling:
- Use ProblemDetails for error responses
- Validate query parameters
- Handle service layer exceptions

Swagger/OpenAPI Documentation:
- Add XML documentation comments (/// <summary>) to all controller actions
- Use [ProducesResponseType] attributes to document all possible responses
- Include [SwaggerOperation] attributes for detailed endpoint descriptions
- Add parameter descriptions using [SwaggerParameter] attributes
- Document all HTTP status codes (200, 400, 404, 500)
- Include example responses for all endpoints
- Add API tags to group coffee shop endpoints together: [Tags("Coffee Shops")]

XML Documentation Requirements:
- Document all controller actions with clear summaries
- Document all parameters with /// <param> tags
- Document return values with /// <returns> tags
- Include usage examples in documentation
- Document error scenarios and status codes

Example XML Documentation:
```csharp
/// <summary>
/// Retrieves a paginated list of active coffee shops
/// </summary>
/// <param name="page">Page number (default: 1)</param>
/// <param name="pageSize">Number of items per page (default: 20, max: 100)</param>
/// <param name="search">Optional search term to filter by shop name</param>
/// <returns>A paginated list of coffee shops</returns>
/// <response code="200">Returns the paginated list of coffee shops</response>
/// <response code="400">Invalid pagination parameters</response>
[HttpGet]
[Tags("Coffee Shops")]
[ProducesResponseType(typeof(PagedResponse<CoffeeShopResponse>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
public async Task<ActionResult<PagedResponse<CoffeeShopResponse>>> GetCoffeeShops(...)
```

Create comprehensive unit tests using xUnit and Moq that test:
- All endpoints with valid parameters
- Pagination scenarios
- Search functionality
- Error conditions (not found, invalid parameters)
- HTTP status codes and response formats

Place controller in `Controllers/CoffeeShopsController.cs` and tests in the test project.
```

## ✅ Definition of Done

- [ ] CoffeeShopsController created with all required endpoints
- [ ] GET /api/coffee-shops endpoint with pagination support
- [ ] GET /api/coffee-shops/{id} endpoint for individual shop details
- [ ] GET /api/coffee-shops/search endpoint with search functionality
- [ ] Proper HTTP status codes returned for all scenarios
- [ ] Input validation and error handling implemented
- [ ] ProblemDetails responses for error scenarios
- [ ] Unit tests written with >90% coverage
- [ ] All tests passing
- [ ] Swagger/OpenAPI documentation complete and integrated
- [ ] XML documentation comments added to all actions
- [ ] [ProducesResponseType] attributes configured
- [ ] API appears correctly in existing Swagger UI
- [ ] Coffee shop endpoints grouped under "Coffee Shops" tag
- [ ] Request/response examples visible in Swagger
- [ ] Code follows project coding standards
- [ ] Dependencies properly injected

## 🔗 Related Issues

- Depends on: Epic 1 completion (Domain Models), #012 (Swagger Configuration)
- Blocks: #014 (Service Layer), #017 (Integration Tests)
- Epic: #Epic-3 (Coffee Shop Data & Locator API)
- Works with: #015 (Seed Data), #016 (Geographic Features)

## 📌 Notes

- **Read-Only Focus**: Phase 1 only supports data retrieval, no creation/editing
- **Swagger Integration**: Must integrate seamlessly with Epic 2 Swagger setup
- **Performance**: Designed for pagination to handle large datasets
- **Search**: Basic search functionality with room for future enhancement
- **Documentation**: Follow same high standards established in Epic 2

## 🧪 Test Scenarios

### Happy Path Tests
- Retrieve paginated coffee shops with default parameters
- Retrieve coffee shop by valid ID
- Search coffee shops with valid query
- Handle pagination boundaries correctly

### Validation Tests
- Invalid page numbers (negative, zero)
- Invalid page sizes (too large, negative)
- Invalid shop IDs (negative, non-existent)
- Empty search queries
- Malformed request parameters

### Error Handling Tests
- Service layer exceptions
- Database connection issues
- Not found scenarios
- Invalid search parameters

### Swagger Documentation Tests
- All endpoints appear in Swagger UI
- Documentation includes proper descriptions
- Response examples are accurate
- Parameter validation documented

## 🔍 API Design Checklist

- [ ] RESTful endpoint design
- [ ] Consistent naming conventions with Epic 2
- [ ] Proper HTTP methods usage
- [ ] Appropriate status codes
- [ ] Error response format standardized
- [ ] Pagination implemented consistently
- [ ] Search functionality intuitive
- [ ] Swagger documentation comprehensive

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
