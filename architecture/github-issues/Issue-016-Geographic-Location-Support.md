# Issue #016: Add Geographic Location Support

**Epic:** Epic 3 - Coffee Shop Data & Locator API  
**Task:** 3.4 - Add Geographic Location Support  
**Estimated Time:** 3-4 hours  
**Priority:** Low (Future Enhancement)  
**Dependencies:** Issue #004 (Coffee Shop Basic Model), Issue #005 (Database Migration), Issue #013 (Coffee Shop API Controller)

---

## 📝 Description

Add basic geographic location support for coffee shops with distance calculations and location-based queries. This enhancement prepares the system for future GPS integration and provides "nearest shops" functionality for mobile applications.

## 🎯 Acceptance Criteria

### Model Updates
- [ ] Coffee shop model includes latitude/longitude fields
- [ ] Database migration created for new geographic fields
- [ ] Existing seed data updated with realistic coordinates
- [ ] Database indexes added for geographic queries

### Location Services
- [ ] `ILocationService` interface created with distance calculation methods
- [ ] `LocationService` implementation with Haversine formula
- [ ] Coordinate validation for latitude (-90 to 90) and longitude (-180 to 180)
- [ ] Support for both kilometers and miles distance units

### API Enhancements
- [ ] New endpoint: GET /api/coffee-shops/nearest for location-based queries
- [ ] Updated `CoffeeShopResponse` DTO with distance and location properties
- [ ] Comprehensive Swagger/OpenAPI documentation for location endpoints
- [ ] XML documentation comments for all location-related methods

### Testing Requirements
- [ ] Unit tests verify distance calculation accuracy
- [ ] Coordinate validation tests for edge cases
- [ ] "Nearest shops" functionality tested
- [ ] Performance tests with large datasets
- [ ] Integration tests for location-based API endpoints

## 🔧 Technical Details

### Database Schema Changes
- Add `Latitude` (decimal, nullable, precision 10,8) to CoffeeShop model
- Add `Longitude` (decimal, nullable, precision 11,8) to CoffeeShop model
- Add computed property `HasCoordinates` for easy checking
- Create database indexes for efficient geographic queries
- Update seed data with realistic coordinates for major cities

### Distance Calculation
- Implement Haversine formula for accurate distance calculation
- Support distance results in both kilometers and miles
- Handle edge cases (North/South poles, International Date Line)
- Optimize for performance with large datasets

### API Specifications
- Location endpoint returns shops sorted by distance (nearest first)
- Include distance property in response (decimal, 2 decimal places)
- Limit search radius to reasonable maximum (e.g., 50km)
- Support query parameters: lat, lng, maxResults, maxDistance

## 🤖 Copilot Prompt

```csharp
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
   - Add comprehensive Swagger documentation for location parameters
   - Document coordinate validation rules and error responses

2. Update CoffeeShopResponse DTO:
   - Add optional Distance property (decimal, in km)
   - Add HasLocation property (bool)
   - Include Swagger schema annotations for new properties

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

Swagger/OpenAPI Documentation:
- Document all location-related endpoints with comprehensive parameter descriptions
- Add example coordinates and responses
- Document validation rules and error scenarios
- Include geographic coordinate format specifications
- Add proper response schemas for location-based queries

XML Documentation Requirements:
- Document all location service methods with clear summaries
- Include parameter descriptions for latitude, longitude, and distance values
- Document return values and error conditions
- Provide usage examples for location calculations

Note: This is a foundation for future GPS integration in mobile apps.

Place location service in `Services/LocationService.cs` and update relevant DTOs and controllers.
```

## 📁 File Locations

```
src/CoffeeTracker.Api/
├── Models/
│   └── CoffeeShop.cs                   # Updated with lat/lng fields
├── Services/
│   ├── ILocationService.cs             # Location service interface
│   └── LocationService.cs              # Location service implementation
├── Controllers/
│   └── CoffeeShopsController.cs        # Updated with nearest endpoint
├── DTOs/
│   └── CoffeeShopResponse.cs           # Updated with distance/location
├── Data/
│   ├── Migrations/                     # New migration for coordinates
│   └── Seeders/
│       └── CoffeeShopSeeder.cs         # Updated with coordinates
test/CoffeeTracker.Api.Tests/
├── Services/
│   └── LocationServiceTests.cs         # Location service tests
├── Controllers/
│   └── CoffeeShopsControllerTests.cs   # Updated with location tests
```

## ✅ Definition of Done

- [ ] Coffee shop model updated with latitude/longitude fields
- [ ] Database migration created and applied successfully
- [ ] `ILocationService` and `LocationService` implemented with Haversine formula
- [ ] New "nearest shops" API endpoint created and documented
- [ ] `CoffeeShopResponse` DTO updated with distance and location properties
- [ ] Comprehensive Swagger/OpenAPI documentation for all location features
- [ ] XML documentation comments added to all location-related methods
- [ ] Unit tests achieve >90% coverage for location functionality
- [ ] Integration tests verify location-based API endpoints
- [ ] Performance tests confirm acceptable response times
- [ ] Seed data updated with realistic coordinates
- [ ] Coordinate validation working for edge cases
- [ ] Distance calculations accurate within acceptable tolerance
- [ ] All tests passing
- [ ] No breaking changes to existing API contracts

## 🔗 Related Issues

- **Depends on:** Issue #004 (Coffee Shop Basic Model)
- **Depends on:** Issue #005 (Database Migration)
- **Depends on:** Issue #013 (Coffee Shop API Controller)
- **Enhances:** Issue #015 (Coffee Shop Seed Data)
- **Tested by:** Issue #017 (Coffee Shop Integration Tests)

## 📋 Notes

### Haversine Formula Implementation
The Haversine formula calculates great-circle distances between two points on Earth:
```
a = sin²(Δφ/2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ/2)
c = 2 ⋅ atan2( √a, √(1−a) )
d = R ⋅ c
```
Where φ is latitude, λ is longitude, R is earth's radius

### Sample Coordinates for Testing
- New York City: 40.7128, -74.0060
- London: 51.5074, -0.1278
- Sydney: -33.8688, 151.2093
- Tokyo: 35.6762, 139.6503
- San Francisco: 37.7749, -122.4194

### Performance Considerations
- Database indexes on latitude and longitude columns
- Efficient bounding box calculations before distance computation
- Limit maximum search radius to prevent expensive queries
- Consider spatial database extensions for production use

### Future Enhancements
- Integration with mapping services for address geocoding
- Advanced spatial queries using PostGIS or SQL Server spatial types
- Caching of distance calculations for frequently requested locations
- Support for polygonal search areas instead of circular radius
