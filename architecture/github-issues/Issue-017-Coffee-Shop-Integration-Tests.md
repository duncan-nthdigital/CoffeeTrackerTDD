# Issue #017: Create Coffee Shop Integration Tests

**Epic:** Epic 3 - Coffee Shop Data & Locator API  
**Task:** 3.5 - Create Coffee Shop Integration Tests  
**Estimated Time:** 2-3 hours  
**Priority:** Medium  
**Dependencies:** Issue #013 (Coffee Shop API Controller), Issue #014 (Coffee Shop Service Layer), Issue #015 (Coffee Shop Seed Data)

---

## 📝 Description

Create comprehensive integration tests for coffee shop API endpoints that test the complete HTTP request/response flow. These tests will validate the entire coffee shop API functionality including search, pagination, error handling, and performance requirements, ensuring the system works correctly end-to-end.

## 🎯 Acceptance Criteria

### Test Infrastructure
- [ ] Tests use `TestServer` and in-memory SQLite database
- [ ] Tests cover all coffee shop API endpoints comprehensively
- [ ] Tests verify search and pagination functionality works correctly
- [ ] Tests check performance requirements (<500ms response time)
- [ ] Tests run independently without shared state
- [ ] Fresh database state created for each test class

### API Endpoint Coverage
- [ ] GET /api/coffee-shops (list with pagination)
- [ ] GET /api/coffee-shops/{id} (individual shop details)
- [ ] GET /api/coffee-shops/search (search functionality)
- [ ] GET /api/coffee-shops/nearest (location-based queries, if implemented)

### Search & Pagination Testing
- [ ] Search algorithms work with partial matches
- [ ] Case-insensitive search functionality verified
- [ ] Pagination edge cases tested (first, middle, last pages)
- [ ] Page size validation enforced correctly
- [ ] Sort order consistency verified across pages

### Error Handling & Validation
- [ ] Invalid shop IDs return 404 Not Found
- [ ] Malformed requests return 400 Bad Request
- [ ] Empty search queries handled appropriately
- [ ] Invalid pagination parameters rejected

### Swagger/OpenAPI Testing
- [ ] Swagger endpoint accessible and includes coffee shop APIs
- [ ] OpenAPI specification validates correctly for all endpoints
- [ ] All endpoints documented with proper schemas and examples
- [ ] Response documentation matches actual API responses
- [ ] Parameter validation rules documented and enforced

## 🔧 Technical Details

### Test Setup Requirements
- Use Microsoft.AspNetCore.Mvc.Testing for TestServer
- Configure SQLite in-memory database with realistic seed data
- Implement test utilities for common scenarios
- Create helper methods for asserting API responses
- Set up performance measurement utilities

### Performance Testing
- Response time measurements for all endpoints
- Concurrent request handling verification
- Large dataset performance testing (1000+ shops)
- Cache effectiveness validation
- Memory usage monitoring during tests

### Search Algorithm Validation
- Test exact name matches (highest priority)
- Verify partial name matching functionality
- Test address search capabilities
- Validate fuzzy matching tolerance
- Ensure search result ranking/relevance

## 🤖 Copilot Prompt

```csharp
Create comprehensive integration tests for the coffee shop API in a .NET 8 Web API project.

Test setup requirements:
- Use Microsoft.AspNetCore.Mvc.Testing.TestServer
- SQLite in-memory database with seed data
- Fresh database state for each test class
- Load realistic seed data for testing
- Test complete HTTP request/response cycle

Test scenarios to cover:

1. GET /api/coffee-shops
   - Happy path: retrieve paginated coffee shops
   - Pagination: first page, middle page, last page
   - Page size validation: default, minimum, maximum
   - Empty results: when no shops exist
   - Performance: response time under load

2. GET /api/coffee-shops/{id}
   - Happy path: valid shop ID returns shop details
   - Not found: invalid shop ID returns 404
   - Inactive shops: inactive shops return 404
   - Edge cases: negative IDs, zero, very large IDs

3. GET /api/coffee-shops/search
   - Happy path: search finds matching shops
   - Partial matches: search works with partial names
   - Case insensitive: uppercase/lowercase/mixed case
   - No results: search with no matches returns empty
   - Invalid input: empty search query returns 400
   - Special characters: search handles special characters

4. Pagination testing
   - Correct total count calculation
   - Correct page boundaries
   - Sort order consistency
   - Page size limits enforced

5. Performance tests
   - Search response time < 500ms with 1000 shops
   - Pagination performance with large datasets
   - Concurrent request handling
   - Cache effectiveness

Search algorithm tests:
- Exact name matches (highest priority)
- Partial name matches
- Address search functionality
- Fuzzy matching tolerance
- Search ranking/relevance

Error handling tests:
- Malformed requests
- Invalid query parameters
- Database connection issues (simulate)
- Large result sets

Swagger/OpenAPI tests:
- Verify /swagger endpoint includes coffee shop APIs
- Validate OpenAPI specification for coffee shop endpoints
- Test that all endpoints are documented with proper schemas
- Verify response examples are present and accurate
- Test API documentation accessibility and completeness

Create test utilities:
- Helper methods for creating test data
- Extensions for asserting API responses
- Performance measurement utilities
- Search relevance assertion helpers
- Swagger documentation validation utilities

Test data requirements:
- Use diverse coffee shop names for search testing
- Include edge cases (short names, long names, special characters)
- Test with both chain and independent shop names
- Include various address formats

Place integration tests in `CoffeeTracker.Api.IntegrationTests/CoffeeShops/`

Test naming convention: `{Method}_{Endpoint}_{Scenario}_ReturnsExpectedResult`
Example: `Get_CoffeeShops_WithValidPagination_ReturnsPagedResults`
```

## 📁 File Locations

```
test/CoffeeTracker.Api.IntegrationTests/
├── CoffeeShops/
│   ├── CoffeeShopsControllerTests.cs           # Main controller tests
│   ├── CoffeeShopsSearchTests.cs               # Search functionality tests
│   ├── CoffeeShopsPaginationTests.cs           # Pagination tests
│   ├── CoffeeShopsPerformanceTests.cs          # Performance tests
│   └── CoffeeShopsSwaggerTests.cs              # Swagger/OpenAPI tests
├── Infrastructure/
│   ├── TestWebApplicationFactory.cs            # Test server setup
│   ├── DatabaseFixture.cs                      # Database test fixture
│   └── TestDataHelpers.cs                      # Test data utilities
└── Extensions/
    ├── HttpResponseMessageExtensions.cs        # Response assertion helpers
    └── TestServiceCollectionExtensions.cs      # Test service setup
```

## ✅ Definition of Done

- [ ] Integration tests created for all coffee shop API endpoints
- [ ] Test infrastructure set up with TestServer and in-memory database
- [ ] Search functionality thoroughly tested with various scenarios
- [ ] Pagination testing covers all edge cases and validation
- [ ] Error handling tests verify proper HTTP status codes
- [ ] Performance tests confirm <500ms response time requirement
- [ ] Swagger/OpenAPI documentation tests validate completeness
- [ ] All tests run independently and pass consistently
- [ ] Test coverage includes happy path and error scenarios
- [ ] Test utilities created for maintainable test code
- [ ] Performance benchmarks established and monitored
- [ ] No flaky tests or race conditions
- [ ] Tests provide clear failure messages for debugging
- [ ] Documentation updated with testing approach and guidelines

## 🔗 Related Issues

- **Depends on:** Issue #013 (Coffee Shop API Controller)
- **Depends on:** Issue #014 (Coffee Shop Service Layer)
- **Depends on:** Issue #015 (Coffee Shop Seed Data)
- **May include:** Issue #016 (Geographic Location Support) if implemented
- **Validates:** All Epic 3 coffee shop API functionality

## 📋 Notes

### Test Categories

**Functional Tests:**
- API endpoint behavior verification
- Request/response validation
- Business logic correctness
- Data integrity checks

**Non-Functional Tests:**
- Performance and response time validation
- Concurrent request handling
- Memory usage and resource management
- Cache effectiveness verification

**Documentation Tests:**
- Swagger/OpenAPI specification validation
- Response schema accuracy
- Parameter documentation completeness
- Example response correctness

### Performance Benchmarks
- Individual endpoint response time: <200ms
- Search with 1000+ shops: <500ms
- Pagination performance: consistent across pages
- Concurrent requests: support 10+ simultaneous users
- Memory usage: stable under load

### Test Data Strategy
- Create diverse coffee shop names for comprehensive search testing
- Include shops with special characters, international names
- Mix of short and long shop names
- Various address formats and geographic locations
- Edge cases for boundary testing

### Mocking Strategy
- Use real database for integration tests (in-memory SQLite)
- Mock external dependencies if any are added later
- Avoid mocking core business logic components
- Focus on testing real integration scenarios
