# Issue #009: Add Anonymous Session Management

**Labels:** `epic-2`, `session-management`, `high-priority`, `security`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 2 - Coffee Logging API Endpoints  
**Estimated Time:** 2-3 hours  

## 📋 Description

Implement session management for anonymous users using browser-based identification. This enables data isolation and session continuity without requiring user authentication.

## 🎯 Acceptance Criteria

- [ ] Middleware or service to handle anonymous sessions
- [ ] Session ID generation and validation
- [ ] 24-hour data retention policy implementation
- [ ] Background cleanup of expired data
- [ ] Unit tests verify session isolation
- [ ] Session cookies are secure and HTTP-only
- [ ] Session management integrates with existing services

## 🔧 Technical Requirements

- Use browser fingerprinting or generated session IDs
- Store session info in HTTP-only cookies
- Implement cleanup job for old data
- Ensure session isolation between users
- Use cryptographically secure session IDs
- Implement background service for data cleanup

## 📝 Implementation Details

### File Locations
- **Middleware**: `src/CoffeeTracker.Api/Middleware/AnonymousSessionMiddleware.cs`
- **Service**: `src/CoffeeTracker.Api/Services/ISessionService.cs`
- **Implementation**: `src/CoffeeTracker.Api/Services/SessionService.cs`
- **Background Service**: `src/CoffeeTracker.Api/Services/Background/DataCleanupService.cs`
- **Tests**: `test/CoffeeTracker.Api.Tests/Services/SessionServiceTests.cs`

### Session Management Components
```csharp
// ISessionService Interface
public interface ISessionService
{
    string GetOrCreateSessionId(HttpContext context);
    bool IsSessionValid(string sessionId);
    Task CleanupExpiredSessionsAsync();
}

// Anonymous Session Middleware
public class AnonymousSessionMiddleware
{
    // Extract or generate session ID from request headers/cookies
    // Add session ID to HttpContext for use in controllers/services
    // Generate new session ID if none exists
    // Set session cookie with 24-hour expiration
}

// Background Service for Cleanup
public class DataCleanupService : BackgroundService
{
    // Run every hour to clean up expired anonymous data
    // Remove coffee entries older than 24 hours for anonymous sessions
    // Log cleanup operations
}
```

### Session Requirements
- Use cryptographically secure random strings
- 32-character alphanumeric identifiers
- Store in HTTP-only cookie named "coffee-session"
- Include SameSite and Secure attributes for security
- 24-hour expiration time

## 🤖 Copilot Prompt

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

## ✅ Definition of Done

- [ ] AnonymousSessionMiddleware created and registered
- [ ] ISessionService interface and implementation created
- [ ] Session ID generation is cryptographically secure
- [ ] Session cookies are properly configured (HTTP-only, Secure, SameSite)
- [ ] Background cleanup service implemented and running
- [ ] 24-hour data retention policy enforced
- [ ] Session isolation verified between different users
- [ ] Integration with CoffeeService completed
- [ ] Unit tests written with >90% coverage
- [ ] Integration tests verify end-to-end session flow
- [ ] All tests passing
- [ ] Performance tested under expected load

## 🔗 Related Issues

- Depends on: #007 (Service Layer), #008 (DTOs)
- Blocks: #010 (Validation), #011 (Integration Tests)
- Epic: #Epic-2 (Coffee Logging API Endpoints)
- Works with: #006 (Controller)

## 📌 Notes

- **Security Focus**: Session IDs must be cryptographically secure
- **Data Privacy**: Ensure complete isolation between sessions
- **Performance**: Optimize session lookup and cleanup operations
- **Compliance**: Consider GDPR implications for anonymous data
- **Monitoring**: Log session creation and cleanup for analytics

## 🧪 Test Scenarios

### Session Management Tests
- Generate unique session IDs
- Validate session ID format and security
- Handle missing session cookies
- Create new sessions for new users
- Maintain session continuity across requests

### Session Isolation Tests
- Users with different session IDs see different data
- Session ID tampering doesn't allow data access
- Expired sessions are properly cleaned up
- New sessions start with empty data

### Background Cleanup Tests
- Cleanup runs on schedule
- Only expired data is removed
- Active sessions are preserved
- Cleanup logging works correctly

### Integration Tests
- Middleware integrates with controllers
- Service layer respects session boundaries
- End-to-end session flow works
- Performance under concurrent load

## 🔍 Security Checklist

- [ ] Session IDs are cryptographically secure
- [ ] Session cookies are HTTP-only
- [ ] Session cookies include Secure attribute
- [ ] Session cookies use SameSite policy
- [ ] Session validation prevents tampering
- [ ] Data isolation is complete
- [ ] No session data leakage between users
- [ ] Cleanup removes all expired data

## 🏗️ Architecture Notes

**Session Management Flow:**
1. Request arrives → Middleware extracts/generates session ID
2. Session ID added to HttpContext
3. Controllers/Services use session ID for data filtering
4. Response includes session cookie if new session
5. Background service cleans up expired data

**Security Considerations:**
- Use `System.Security.Cryptography.RandomNumberGenerator`
- Implement proper cookie security attributes
- Validate session IDs to prevent injection
- Log security-relevant events

**Performance Considerations:**
- Cache active session information
- Optimize database queries for session filtering
- Batch cleanup operations
- Monitor background service performance

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
