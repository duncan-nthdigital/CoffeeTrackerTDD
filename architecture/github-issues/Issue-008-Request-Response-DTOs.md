# Issue #008: Create Request/Response DTOs

**Labels:** `epic-2`, `dto`, `high-priority`, `api-contract`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 2 - Coffee Logging API Endpoints  
**Estimated Time:** 2 hours  

## 📋 Description

Create Data Transfer Objects (DTOs) for API requests and responses to establish a clean contract between the API and its consumers, separate from domain models.

## 🎯 Acceptance Criteria

- [ ] DTOs separate from domain models
- [ ] Proper validation attributes on request DTOs
- [ ] Response DTOs include computed properties
- [ ] AutoMapper configuration for mapping between DTOs and domain models
- [ ] Unit tests verify mapping works correctly
- [ ] FluentValidation rules for complex business logic validation
- [ ] XML documentation for all DTOs
- [ ] Swagger/OpenAPI schema annotations for enhanced documentation
- [ ] Example values configured for Swagger documentation

## 🔧 Technical Requirements

- Use separate DTOs for API contract
- Include FluentValidation for complex validation
- Add AutoMapper profiles for object mapping
- Use System.ComponentModel.DataAnnotations for basic validation
- Include computed properties in response DTOs
- Maintain API versioning compatibility
- Add Swagger/OpenAPI schema annotations for better documentation
- Include example values for Swagger UI demonstration

## 📝 Implementation Details

### File Locations
- **DTOs**: `src/CoffeeTracker.Api/DTOs/`
- **Mapping**: `src/CoffeeTracker.Api/Mapping/CoffeeTrackerProfile.cs`
- **Validation**: `src/CoffeeTracker.Api/Validation/`
- **Tests**: `test/CoffeeTracker.Api.Tests/DTOs/` and `test/CoffeeTracker.Api.Tests/Mapping/`

### Request DTOs
```csharp
// CreateCoffeeEntryRequest
public class CreateCoffeeEntryRequest
{
    [Required]
    public string CoffeeType { get; set; }
    
    [Required]
    public string Size { get; set; }
    
    [MaxLength(100)]
    public string? Source { get; set; }
    
    public DateTime? Timestamp { get; set; }
}

// GetCoffeeEntriesRequest
public class GetCoffeeEntriesRequest
{
    public DateTime? Date { get; set; }
}
```

### Response DTOs
```csharp
// CoffeeEntryResponse
public class CoffeeEntryResponse
{
    public int Id { get; set; }
    public string CoffeeType { get; set; }
    public string Size { get; set; }
    public string? Source { get; set; }
    public DateTime Timestamp { get; set; }
    public int CaffeineAmount { get; set; }
    public string FormattedTimestamp { get; set; }
}

// DailySummaryResponse
public class DailySummaryResponse
{
    public DateTime Date { get; set; }
    public int TotalEntries { get; set; }
    public int TotalCaffeine { get; set; }
    public List<CoffeeEntryResponse> Entries { get; set; }
    public decimal AverageCaffeinePerEntry { get; set; }
}
```

## 🤖 Copilot Prompt

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
- Add Swagger/OpenAPI schema annotations for enhanced documentation:
  - Use [JsonPropertyName] for consistent JSON serialization
  - Add [Example] attributes for Swagger UI examples
  - Use [Description] attributes for property documentation
  - Configure [JsonIgnore] for properties that shouldn't appear in API

Swagger Documentation Requirements:
- Add XML documentation comments (/// <summary>) to all DTOs
- Include example values using [Example] attributes
- Add property descriptions using [Description] attributes
- Configure schema examples for Swagger UI
- Ensure proper JSON serialization naming conventions

Create unit tests that verify:
- DTO validation works correctly
- AutoMapper mappings are correct
- All edge cases are handled
- Custom validation rules work

Place DTOs in `DTOs/` directory and AutoMapper profile in `Mapping/`.
```

## ✅ Definition of Done

- [ ] All request DTOs created with proper validation
- [ ] All response DTOs created with computed properties
- [ ] AutoMapper profile configured for all mappings
- [ ] FluentValidation rules implemented for complex validation
- [ ] Custom validation attributes created where needed
- [ ] XML documentation added to all DTOs
- [ ] Swagger/OpenAPI schema annotations configured
- [ ] Example values added for Swagger documentation
- [ ] Property descriptions added using [Description] attributes
- [ ] JSON serialization configured properly for API
- [ ] Unit tests written for DTO validation
- [ ] Unit tests written for AutoMapper mappings
- [ ] All tests passing with >90% coverage
- [ ] DTOs follow naming conventions
- [ ] API contract is clean and intuitive

## 🔗 Related Issues

- Depends on: Epic 1 completion (Domain Models)
- Blocks: #006 (Controller), #007 (Service Layer)
- Epic: #Epic-2 (Coffee Logging API Endpoints)
- Works with: #009 (Session Management), #010 (Validation)

## 📌 Notes

- **Separation of Concerns**: DTOs separate API contract from domain models
- **Validation Strategy**: Multiple layers of validation (attributes, FluentValidation)
- **Mapping**: AutoMapper for clean object transformation
- **API Contract**: DTOs define the public API interface
- **Future-Proofing**: Design allows for API versioning

## 🧪 Test Scenarios

### DTO Validation Tests
- Required field validation
- String length validation
- Date range validation
- Enum value validation
- Custom business rule validation

### AutoMapper Tests
- Domain model to response DTO mapping
- Request DTO to domain model mapping
- Computed property calculation
- Null value handling
- Collection mapping

### FluentValidation Tests
- Complex business rule validation
- Cross-field validation
- Custom validation messages
- Conditional validation rules

### Integration Tests
- DTOs work correctly with model binding
- Validation messages are user-friendly
- Response format is consistent
- Error handling works with DTOs

## 🔍 DTO Design Checklist

- [ ] Clear naming conventions
- [ ] Appropriate validation attributes
- [ ] XML documentation complete
- [ ] AutoMapper configuration tested
- [ ] FluentValidation rules comprehensive
- [ ] Response DTOs include all needed data
- [ ] Request DTOs are minimal and focused
- [ ] API contract is intuitive

## 🏗️ Architecture Notes

**DTO Responsibilities:**
- Define API contract
- Provide input validation
- Shape response data
- Enable API versioning
- Separate concerns from domain models

**Validation Strategy:**
1. **Data Annotations**: Basic validation (required, length, format)
2. **FluentValidation**: Complex business rules
3. **Custom Attributes**: Coffee-specific validation
4. **Service Layer**: Final business rule enforcement

**Mapping Strategy:**
- AutoMapper for object transformation
- Computed properties in response DTOs
- Clean separation from domain models
- Performance-optimized mappings

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
