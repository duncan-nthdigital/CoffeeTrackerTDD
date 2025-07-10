# Issue #014: Create Coffee Shop Service Layer

**Epic:** Epic 3 - Coffee Shop Data & Locator API  
**Task:** 3.2 - Create Coffee Shop Service Layer  
**Estimated Time:** 2-3 hours  
**Priority:** High  
**Dependencies:** Issue #013 (Coffee Shop API Controller), Issue #004 (Coffee Shop Basic Model)

---

## 📝 Description

Create service layer for coffee shop data operations including search and filtering functionality. This service will provide the business logic layer between the API controller and data access layer, supporting efficient search, pagination, and caching.

## 🎯 Acceptance Criteria

### Core Functionality
- [ ] `ICoffeeShopService` interface created with all required methods
- [ ] `CoffeeShopService` implementation with dependency injection
- [ ] Search functionality with fuzzy matching implemented
- [ ] Pagination support for large datasets
- [ ] Caching strategy for frequently accessed data
- [ ] Unit tests with mocked dependencies achieving >90% coverage

### Service Interface Requirements
- [ ] `GetCoffeeShopsAsync(int page, int pageSize, string? search = null)` method
- [ ] `GetCoffeeShopByIdAsync(int id)` method  
- [ ] `SearchCoffeeShopsAsync(string query, int page, int pageSize)` method
- [ ] `GetNearestCoffeeShopsAsync(double latitude, double longitude, int maxResults = 10)` method

### Performance & Caching
- [ ] Memory cache implementation for frequently accessed shops (1 hour TTL)
- [ ] Search results cached for 15 minutes
- [ ] Database queries optimized with proper projections
- [ ] Only active coffee shops returned (IsActive = true)
- [ ] Logging for performance monitoring

### Testing Requirements
- [ ] Unit tests verify coffee shop retrieval and pagination
- [ ] Search functionality tested with various inputs
- [ ] Caching performance improvements verified
- [ ] Error conditions handled and tested
- [ ] Mock dependencies properly isolated

## 🔧 Technical Details

### Implementation Requirements
- Use repository pattern for data access
- Implement efficient database queries with Entity Framework Core
- Case-insensitive search by name and address
- Fuzzy matching using string similarity algorithms
- Support for partial matches and special characters
- Cache invalidation patterns for data consistency

### Performance Specifications
- Database queries should complete in <200ms
- Search operations should complete in <500ms
- Cache hit ratio should exceed 70% for frequently accessed data
- Support pagination at database level to minimize memory usage

### Search Algorithm
- Exact name matches get highest priority
- Partial name and address matching
- Case-insensitive search functionality
- Support for international names and special characters
- Results ordered by relevance (exact matches first, then partial)

## 🤖 Copilot Prompt

```csharp
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

## 📁 File Locations

```
src/CoffeeTracker.Api/
├── Services/
│   ├── ICoffeeShopService.cs           # Service interface
│   └── CoffeeShopService.cs            # Service implementation
test/CoffeeTracker.Api.Tests/
├── Services/
│   └── CoffeeShopServiceTests.cs       # Unit tests
```

## ✅ Definition of Done

- [ ] Service interface and implementation created with all required methods
- [ ] Dependency injection configured in Program.cs
- [ ] Search functionality working with fuzzy matching
- [ ] Pagination implemented efficiently at database level
- [ ] Caching strategy implemented and tested
- [ ] Unit tests written with >90% coverage
- [ ] All tests passing
- [ ] Code follows SOLID principles and clean code practices
- [ ] Performance requirements met (<500ms for search operations)
- [ ] Error handling implemented with proper logging
- [ ] Service integrates successfully with API controller
- [ ] No breaking changes to existing API contracts

## 🔗 Related Issues

- **Depends on:** Issue #004 (Coffee Shop Basic Model)
- **Depends on:** Issue #013 (Coffee Shop API Controller)
- **Blocks:** Issue #015 (Coffee Shop Seed Data)
- **Blocks:** Issue #017 (Coffee Shop Integration Tests)

## 📋 Notes

### Caching Strategy
- Use IMemoryCache for application-level caching
- Implement cache-aside pattern for data access
- Consider cache warming strategies for popular shops
- Monitor cache hit ratios and adjust TTL as needed

### Search Implementation
- Start with simple string Contains matching
- Consider implementing Levenshtein distance for fuzzy matching
- Prepare for future integration with full-text search solutions
- Ensure search results are consistently ordered

### Performance Monitoring
- Log slow queries (>200ms) for optimization
- Track cache hit/miss ratios
- Monitor memory usage for large datasets
- Implement performance counters for key operations
