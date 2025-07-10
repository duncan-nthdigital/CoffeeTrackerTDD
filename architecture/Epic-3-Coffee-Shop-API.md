# Epic 3: Coffee Shop Data & Locator API

**Epic Goal:** Create coffee shop data management and basic locator functionality for anonymous users.

**Estimated Time:** 2-3 days  
**Priority:** Medium (Supporting Feature)  
**Dependencies:** Epic 1 (Domain Models)  

---

## 📋 Tasks

### Task 3.1: Create Coffee Shop API Controller
**Estimated Time:** 2-3 hours  
**Priority:** High  

**Description:**
Create REST API endpoints for coffee shop data retrieval and basic search functionality.

**Acceptance Criteria:**
- [ ] Controller handles GET /api/coffee-shops (list all active shops)
- [ ] Controller handles GET /api/coffee-shops/{id} (get shop details)
- [ ] Controller handles GET /api/coffee-shops/search (search by name/location)
- [ ] Proper error handling and validation
- [ ] Unit tests for all endpoints

**Technical Details:**
- Read-only API for Phase 1 (no creation/updates)
- Support basic search and filtering
- Return paginated results for large datasets

**Copilot Prompt:**
```
Create a `CoffeeShopsController` for a .NET 8 Web API project that provides coffee shop data for anonymous users.

Controller requirements:
- Base route: `[Route("api/coffee-shops")]`
- Read-only operations (GET only for Phase 1)
- Support anonymous access
- Use dependency injection for services

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

Create comprehensive unit tests using xUnit and Moq that test:
- All endpoints with valid parameters
- Pagination scenarios
- Search functionality
- Error conditions (not found, invalid parameters)
- HTTP status codes and response formats

Place controller in `Controllers/CoffeeShopsController.cs` and tests in the test project.
```

---

### Task 3.2: Create Coffee Shop Service Layer
**Estimated Time:** 2-3 hours  
**Priority:** High  

**Description:**
Create service layer for coffee shop data operations including search and filtering.

**Acceptance Criteria:**
- [ ] Service interface and implementation created
- [ ] Search functionality with fuzzy matching
- [ ] Pagination support for large datasets
- [ ] Caching for frequently accessed data
- [ ] Unit tests with mocked dependencies

**Technical Details:**
- Implement repository pattern for data access
- Add basic search algorithms
- Consider performance for large datasets
- Include caching strategy

**Copilot Prompt:**
```
Create a service layer for coffee shop data operations in a .NET 8 Web API project.

Create `ICoffeeShopService` interface with methods:
- Task<PagedResponse<CoffeeShopResponse>> GetCoffeeShopsAsync(int page, int pageSize, string? search = null)
- Task<CoffeeShopResponse?> GetCoffeeShopByIdAsync(int id)
- Task<PagedResponse<CoffeeShopResponse>> SearchCoffeeShopsAsync(string query, int page, int pageSize)
- Task<IEnumerable<CoffeeShopResponse>> GetNearestCoffeeShopsAsync(double latitude, double longitude, int maxResults = 10)

Create `CoffeeShopService` implementation with:
- Constructor injection of DbContext, ILogger, and IMemoryCache
- Efficient database queries with proper indexing
- Search functionality:
  - Case-insensitive search by name and address
  - Fuzzy matching using Levenshtein distance or similar
  - Support partial matches
- Caching strategy:
  - Cache frequently accessed shops for 1 hour
  - Cache search results for 15 minutes
  - Implement cache invalidation patterns
- Performance optimizations:
  - Use projection to DTOs in queries
  - Implement pagination at database level
  - Add query optimization for search

Key features:
- Only return active coffee shops (IsActive = true)
- Support ordering by name (alphabetical)
- Include total count for pagination
- Handle empty search results gracefully
- Log performance metrics for slow queries

Create unit tests that verify:
- Coffee shops can be retrieved and paginated
- Search functionality works with various inputs
- Caching improves performance
- Only active shops are returned
- Error conditions are handled properly

Use Entity Framework Core with optimized queries.
Place service in `Services/CoffeeShopService.cs` with interface in `Services/ICoffeeShopService.cs`.
```

---

### Task 3.3: Create Coffee Shop Seed Data
**Estimated Time:** 2-3 hours  
**Priority:** Medium  

**Description:**
Create comprehensive seed data for coffee shops to support development and testing.

**Acceptance Criteria:**
- [ ] At least 50 diverse coffee shop entries
- [ ] Realistic names, addresses, and locations
- [ ] Mix of chain and independent shops
- [ ] Data supports search and filtering scenarios
- [ ] Seed data runs during database initialization

**Technical Details:**
- Create realistic test data
- Include variety of shop types and locations
- Consider international/diverse names
- Ensure data quality for testing

**Copilot Prompt:**
```
Create comprehensive seed data for coffee shops in a .NET 8 Web API project.

Requirements:
- Create at least 50 diverse coffee shop entries
- Include mix of:
  - Chain coffee shops (Starbucks, Costa, etc.)
  - Independent local coffee shops
  - Specialty/artisan coffee roasters
  - Café/bistro combinations
- Realistic data including:
  - Creative but believable shop names
  - Diverse address formats and locations
  - Mix of urban, suburban, and small town locations
  - Variety in naming styles (modern, traditional, quirky)

Seed data should support testing:
- Search functionality (partial name matches)
- Alphabetical sorting
- Pagination scenarios
- Geographic diversity

Create `CoffeeShopSeeder` class with:
- Static method: `SeedCoffeeShops(CoffeeTrackerDbContext context)`
- Check if data already exists before seeding
- Use realistic but fictional addresses
- Set all shops as IsActive = true
- Set CreatedAt to various dates in the past month

Example shop types to include:
- "The Daily Grind", "Bean There Coffee Co.", "Roasted & Ready"
- "Café Mocha", "Espresso Express", "The Coffee Corner"
- "Java Junction", "Brew & Bite", "Steam & Bean"
- Chain references: "Metro Coffee", "City Roast", "Quick Cup"

Geographic diversity:
- Urban: downtown addresses, numbered streets
- Suburban: residential area addresses
- Small town: main street addresses
- Various formats: "123 Main St", "45 Oak Avenue", "Unit 5, Shopping Plaza"

Create unit tests that verify:
- Seed data loads correctly
- All shops have valid required fields
- No duplicate names in seed data
- Search functionality works with seeded data

Integrate with database initialization:
- Call seeder in DbContext initialization
- Ensure seeding only happens once
- Handle errors gracefully

Place seeder in `Data/Seeders/CoffeeShopSeeder.cs` and update DbContext initialization.
```

---

### Task 3.4: Add Geographic Location Support
**Estimated Time:** 3-4 hours  
**Priority:** Low (Future Enhancement)  

**Description:**
Add basic geographic location support for coffee shops with distance calculations.

**Acceptance Criteria:**
- [ ] Coffee shop model includes latitude/longitude
- [ ] API supports location-based queries
- [ ] Distance calculation utilities
- [ ] "Nearest shops" functionality
- [ ] Unit tests verify location calculations

**Technical Details:**
- Add coordinates to existing model
- Implement Haversine formula for distance
- Consider performance implications
- Prepare for future GPS integration

**Copilot Prompt:**
```
Add geographic location support to the coffee shop system in a .NET 8 Web API project.

Model updates needed:
1. Update CoffeeShop model to include:
   - Latitude (decimal, nullable, precision 10,8)
   - Longitude (decimal, nullable, precision 11,8)
   - HasCoordinates (computed property)

2. Create migration for new fields
   - Update existing seed data with realistic coordinates
   - Add indexes for geographic queries

Location services needed:
1. Create `ILocationService` interface with methods:
   - double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
   - Task<IEnumerable<CoffeeShopResponse>> GetNearestShopsAsync(decimal latitude, decimal longitude, int maxResults, double maxDistanceKm)
   - bool IsValidCoordinate(decimal latitude, decimal longitude)

2. Implement `LocationService` with:
   - Haversine formula for distance calculation
   - Input validation for coordinates
   - Performance-optimized database queries
   - Support for both miles and kilometers

API updates:
1. Add to CoffeeShopsController:
   - GET /api/coffee-shops/nearest?lat={lat}&lng={lng}&maxResults={n}&maxDistance={km}
   - Return shops sorted by distance with distance included

2. Update CoffeeShopResponse DTO:
   - Add optional Distance property (decimal, in km)
   - Add HasLocation property (bool)

Geographic features:
- Validate latitude (-90 to 90) and longitude (-180 to 180)
- Handle shops without coordinates gracefully
- Return distance in kilometers with 2 decimal precision
- Sort results by distance (nearest first)
- Limit results to reasonable radius (e.g., 50km max)

Seed data updates:
- Add realistic coordinates for major cities
- Use coordinates for diverse locations (US, Europe, Australia, etc.)
- Ensure coordinates match the addresses provided

Create comprehensive unit tests:
- Distance calculation accuracy (test known distances)
- Coordinate validation
- Nearest shops functionality
- Edge cases (North/South poles, International Date Line)
- Performance with large datasets

Note: This is a foundation for future GPS integration in mobile apps.

Place location service in `Services/LocationService.cs` and update relevant DTOs and controllers.
```

---

### Task 3.5: Create Coffee Shop Integration Tests
**Estimated Time:** 2-3 hours  
**Priority:** Medium  

**Description:**
Create integration tests for coffee shop API endpoints testing the complete flow.

**Acceptance Criteria:**
- [ ] Tests use TestServer and in-memory database
- [ ] Tests cover all API endpoints
- [ ] Tests verify search and pagination functionality
- [ ] Tests check performance requirements
- [ ] Tests run independently

**Technical Details:**
- Test with realistic seed data
- Verify search algorithms work correctly
- Test pagination edge cases
- Ensure performance benchmarks

**Copilot Prompt:**
```
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

Create test utilities:
- Helper methods for creating test data
- Extensions for asserting API responses
- Performance measurement utilities
- Search relevance assertion helpers

Test data requirements:
- Use diverse coffee shop names for search testing
- Include edge cases (short names, long names, special characters)
- Test with both chain and independent shop names
- Include various address formats

Place integration tests in `CoffeeTracker.Api.IntegrationTests/CoffeeShops/`

Test naming convention: `{Method}_{Endpoint}_{Scenario}_ReturnsExpectedResult`
Example: `Get_CoffeeShops_WithValidPagination_ReturnsPagedResults`
```

---

## 🎯 Epic Definition of Done

- [ ] Coffee shop API endpoints created and working
- [ ] Search functionality with fuzzy matching implemented
- [ ] Pagination support for large datasets
- [ ] Comprehensive seed data with 50+ realistic entries
- [ ] Basic geographic location support (optional)
- [ ] Unit tests achieve >90% coverage
- [ ] Integration tests cover all scenarios
- [ ] Performance requirements met (<500ms response time)
- [ ] API follows REST conventions
- [ ] Caching implemented for better performance

## 📋 Notes

**Technical Decisions:**
- Read-only API for Phase 1 (no shop creation/editing)
- Fuzzy search using simple string matching (can be enhanced later)
- In-memory caching for frequently accessed data
- Geographic coordinates optional but prepared for future GPS features

**Search Algorithm:**
- Case-insensitive partial matching on name and address
- Results ordered by relevance (exact matches first)
- Support for special characters and international names
- Pagination to handle large result sets

**Performance Considerations:**
- Database queries optimized with proper indexing
- Caching strategy for frequently accessed shops
- Pagination implemented at database level
- Search results cached for 15 minutes

**Future Enhancements:**
- Advanced search with filters (cuisine type, ratings, hours)
- Geographic search with GPS integration
- Shop details with photos and reviews (Phase 4)
- Real-time shop data integration

---

**Next Epic:** [Epic 5: Daily Summary & Analytics](./Epic-5-Analytics.md)  
**Previous Epic:** [Epic 2: Coffee Logging API](./Epic-2-Coffee-Logging-API.md)
