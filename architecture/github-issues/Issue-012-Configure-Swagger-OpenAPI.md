# Issue #012: Configure Swagger/OpenAPI Documentation

**Labels:** `epic-2`, `swagger`, `documentation`, `high-priority`, `infrastructure`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 2 - Coffee Logging API Endpoints  
**Estimated Time:** 1-2 hours  

## 📋 Description

Configure comprehensive Swagger/OpenAPI documentation for the Coffee Tracker API to provide interactive API documentation, testing capabilities, and clear API specifications for developers.

## 🎯 Acceptance Criteria

- [ ] Swagger/OpenAPI configured in Program.cs
- [ ] Swagger UI accessible at /swagger endpoint
- [ ] XML documentation generation enabled in project file
- [ ] API metadata properly configured (title, version, description)
- [ ] All endpoints documented with proper response types
- [ ] Request/response examples included in documentation
- [ ] Error response schemas documented
- [ ] OpenAPI specification file generated

## 🔧 Technical Requirements

- Use Swashbuckle.AspNetCore for Swagger generation
- Enable XML documentation file generation
- Configure proper API metadata and versioning
- Include comprehensive endpoint documentation
- Add example values for requests and responses
- Document all HTTP status codes and error responses

## 📝 Implementation Details

### File Locations
- **Configuration**: `src/CoffeeTracker.Api/Program.cs`
- **Project File**: `src/CoffeeTracker.Api/CoffeeTracker.Api.csproj`
- **Documentation**: `src/CoffeeTracker.Api/wwwroot/api-docs/`
- **Tests**: `test/CoffeeTracker.Api.Tests/Documentation/SwaggerTests.cs`

### Swagger Configuration Requirements
```csharp
// Program.cs additions
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Coffee Tracker API",
        Description = "An ASP.NET Core Web API for tracking coffee consumption",
        Contact = new OpenApiContact
        {
            Name = "Coffee Tracker Team",
            Email = "support@coffeetracker.com"
        }
    });
    
    // Include XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    // Configure examples and schemas
    options.EnableAnnotations();
    options.UseInlineDefinitionsForEnums();
});

// Configure Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Coffee Tracker API v1");
    options.RoutePrefix = "swagger";
});
```

### Project File Configuration
```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(AssemblyName).xml</DocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn> <!-- Suppress XML comment warnings -->
</PropertyGroup>
```

## 🤖 Copilot Prompt

```
Configure Swagger/OpenAPI documentation for a .NET 8 Web API project for coffee tracking.

Requirements:

1. Program.cs Configuration:
   - Add Swashbuckle.AspNetCore package
   - Configure SwaggerGen with proper API metadata:
     - Title: "Coffee Tracker API"
     - Version: "v1"
     - Description: "An ASP.NET Core Web API for tracking coffee consumption"
     - Contact information
   - Enable XML documentation inclusion
   - Configure Swagger UI at /swagger endpoint
   - Add annotation support for enhanced documentation

2. Project File Updates:
   - Enable XML documentation file generation
   - Suppress XML documentation warnings (1591)
   - Configure documentation file path

3. Enhanced Documentation Features:
   - Include XML comments from code
   - Enable Swagger annotations
   - Configure enum handling for better documentation
   - Add examples for request/response models
   - Document all HTTP status codes

4. Documentation Standards:
   - All controller actions must have XML documentation
   - Use [ProducesResponseType] attributes for response documentation
   - Include [SwaggerOperation] attributes for detailed descriptions
   - Add [SwaggerExample] for request/response examples
   - Document error responses with proper status codes

5. Testing:
   - Unit tests to verify Swagger configuration
   - Integration tests to ensure /swagger endpoint works
   - Validate generated OpenAPI specification
   - Test that all endpoints are documented

Create comprehensive setup that provides:
- Interactive API documentation
- Request/response examples
- Proper error documentation
- Clean, professional API presentation

Place configuration in Program.cs and update project file accordingly.
```

## ✅ Definition of Done

- [ ] Swashbuckle.AspNetCore package added to project
- [ ] Swagger configuration added to Program.cs
- [ ] XML documentation generation enabled in project file
- [ ] Swagger UI accessible at /swagger endpoint
- [ ] API metadata properly configured (title, version, description)
- [ ] OpenAPI specification file generated correctly
- [ ] All existing endpoints appear in Swagger documentation
- [ ] Request/response schemas properly documented
- [ ] HTTP status codes documented for all endpoints
- [ ] Error response formats documented
- [ ] Unit tests verify Swagger configuration
- [ ] Integration tests confirm /swagger endpoint functionality
- [ ] Documentation follows OpenAPI 3.0 standards
- [ ] Professional appearance and usability

## 🔗 Related Issues

- Depends on: Epic 1 completion (API project setup)
- Blocks: #006 (Controller), #008 (DTOs)
- Epic: #Epic-2 (Coffee Logging API Endpoints)
- Supports: All API development issues

## 📌 Notes

- **Foundation Issue**: This should be completed early to support other API development
- **Documentation First**: Enables API-first development approach
- **Developer Experience**: Essential for API usability and testing
- **Integration**: Works with controller actions and DTOs
- **Standards**: Follow OpenAPI 3.0 specification

## 🧪 Test Scenarios

### Configuration Tests
- Swagger services registered correctly
- SwaggerGen configuration applied
- XML documentation file generated
- API metadata accessible

### UI Tests
- /swagger endpoint returns Swagger UI
- All API endpoints visible in documentation
- Request/response schemas displayed correctly
- Examples and descriptions appear properly

### Integration Tests
- OpenAPI specification file accessible at /swagger/v1/swagger.json
- Generated specification validates against OpenAPI schema
- All documented endpoints are functional
- Error responses match documentation

### Documentation Quality Tests
- All endpoints have descriptions
- Request/response examples are helpful
- Error codes are properly documented
- API appears professional and complete

## 🔍 Swagger Configuration Checklist

- [ ] Swashbuckle.AspNetCore package installed
- [ ] SwaggerGen services configured
- [ ] Swagger UI middleware configured
- [ ] XML documentation enabled
- [ ] API metadata complete
- [ ] Annotation support enabled
- [ ] Custom styling (optional)
- [ ] Security definitions (future)

## 🏗️ Documentation Strategy

**Documentation Layers:**
1. **XML Comments**: In-code documentation
2. **Data Annotations**: Model validation and description
3. **Swagger Attributes**: Enhanced API documentation
4. **OpenAPI Specification**: Machine-readable API definition

**Quality Standards:**
- Clear, concise descriptions
- Helpful examples for all models
- Complete HTTP status code documentation
- Professional presentation
- Consistent terminology and formatting

---

**Assigned to:** Development Team  
**Created:** July 10, 2025  
**Last Updated:** July 10, 2025
