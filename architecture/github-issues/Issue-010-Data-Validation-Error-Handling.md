# Issue #010: Add Data Validation and Error Handling

**Labels:** `epic-2`, `validation`, `medium-priority`, `error-handling`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 2 - Coffee Logging API Endpoints  
**Estimated Time:** 2-3 hours  

## 📋 Description

Implement comprehensive data validation and error handling for the API to ensure data integrity and provide excellent user experience with clear, actionable error messages.

## 🎯 Acceptance Criteria

- [ ] Custom validation attributes for business rules
- [ ] Global exception handling middleware
- [ ] Proper HTTP status codes for all scenarios
- [ ] Structured error responses using ProblemDetails (RFC 7807)
- [ ] Unit tests verify all error scenarios
- [ ] User-friendly error messages
- [ ] Correlation IDs for error tracking

## 🔧 Technical Requirements

- Use ASP.NET Core model validation
- Add custom validators for coffee-specific rules
- Implement global exception handling
- Return consistent error format using ProblemDetails
- Include correlation IDs for error tracking
- Implement business rule validation service

## 📝 Implementation Details

### File Locations
- **Validation**: `src/CoffeeTracker.Api/Validation/`
- **Exceptions**: `src/CoffeeTracker.Api/Exceptions/`
- **Middleware**: `src/CoffeeTracker.Api/Middleware/GlobalExceptionHandlerMiddleware.cs`
- **Service**: `src/CoffeeTracker.Api/Services/ICoffeeValidationService.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Validation/`

### Custom Validation Attributes
```csharp
// ValidCoffeeTypeAttribute: validates against CoffeeType enum
[AttributeUsage(AttributeTargets.Property)]
public class ValidCoffeeTypeAttribute : ValidationAttribute

// ValidCoffeeSizeAttribute: validates against CoffeeSize enum
[AttributeUsage(AttributeTargets.Property)]
public class ValidCoffeeSizeAttribute : ValidationAttribute

// NotFutureDateAttribute: ensures timestamp is not in future
[AttributeUsage(AttributeTargets.Property)]
public class NotFutureDateAttribute : ValidationAttribute

// MaxDailyCaffeineAttribute: validates daily caffeine limit
[AttributeUsage(AttributeTargets.Class)]
public class MaxDailyCaffeineAttribute : ValidationAttribute
```

### Custom Exceptions
```csharp
public class CoffeeTrackingException : Exception
public class ValidationException : CoffeeTrackingException
public class BusinessRuleException : CoffeeTrackingException
public class SessionNotFoundException : CoffeeTrackingException
public class DailyCaffeineLimitExceededException : BusinessRuleException
```

### Validation Service
```csharp
public interface ICoffeeValidationService
{
    Task ValidateCreateCoffeeEntryAsync(CreateCoffeeEntryRequest request, string sessionId);
    Task ValidateDailyLimitsAsync(string sessionId, DateTime date);
    Task ValidateSessionAsync(string sessionId);
}
```

## 🤖 Copilot Prompt

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
   - ICoffeeValidationService interface
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

## ✅ Definition of Done

- [ ] All custom validation attributes created and tested
- [ ] Global exception handling middleware implemented
- [ ] Custom exception hierarchy defined
- [ ] Validation service interface and implementation created
- [ ] ProblemDetails responses implemented for all error scenarios
- [ ] Correlation IDs included in all error responses
- [ ] User-friendly error messages for all validation scenarios
- [ ] Business rule validation integrated with service layer
- [ ] Unit tests written for all validation scenarios
- [ ] Integration tests verify error handling end-to-end
- [ ] All tests passing with >90% coverage
- [ ] Error logging implemented with appropriate levels

## 🔗 Related Issues

- Depends on: #006 (Controller), #007 (Service Layer), #008 (DTOs)
- Blocks: #011 (Integration Tests)
- Epic: #Epic-2 (Coffee Logging API Endpoints)
- Works with: #009 (Session Management)

## 📌 Notes

- **User Experience**: Error messages should be clear and actionable
- **Consistency**: All errors follow the same ProblemDetails format
- **Tracking**: Correlation IDs enable error investigation
- **Security**: Don't expose sensitive system information in errors
- **Performance**: Validation should be efficient and not impact performance

## 🧪 Test Scenarios

### Validation Attribute Tests
- ValidCoffeeType with valid/invalid enum values
- ValidCoffeeSize with valid/invalid enum values  
- NotFutureDate with past/present/future dates
- MaxDailyCaffeine with various caffeine amounts

### Business Rule Validation Tests
- Daily entry limit enforcement (10 per day)
- Daily caffeine limit enforcement (1000mg)
- Coffee type/size combination validation
- Session-based validation rules

### Exception Handling Tests
- ValidationException → 400 Bad Request
- BusinessRuleException → 422 Unprocessable Entity
- SessionNotFoundException → 404 Not Found
- General exceptions → 500 Internal Server Error

### Error Response Format Tests
- ProblemDetails format compliance
- Correlation ID presence
- User-friendly messages
- Validation error details inclusion

## 🔍 Validation Strategy

**Multi-Layer Validation:**
1. **DTO Level**: Data annotations for basic validation
2. **Custom Attributes**: Coffee-specific validation rules
3. **Service Level**: Business rule enforcement
4. **Database Level**: Constraint validation (final safety net)

**Error Handling Flow:**
1. Request validation fails → Return 400 with validation details
2. Business rule violation → Return 422 with business error
3. Resource not found → Return 404 with helpful message
4. System error → Return 500 with correlation ID (hide details)

**Error Response Structure (ProblemDetails):**
```json
{
  "type": "https://api.coffeetracker.com/problems/validation-error",
  "title": "Validation Error",
  "status": 400,
  "detail": "The coffee type 'InvalidType' is not supported",
  "instance": "/api/coffee-entries",
  "correlationId": "abc123-def456-ghi789",
  "errors": {
    "CoffeeType": ["Must be a valid coffee type"]
  }
}
```

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
