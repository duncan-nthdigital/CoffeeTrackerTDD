# Issue #023: Create Blazor Component Integration Tests

**Labels:** `epic-4`, `testing`, `medium-priority`, `bunit-integration`, `component-testing`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 4 - Blazor UI Components for Coffee Logging  
**Estimated Time:** 3-4 hours  

## 📋 Description

Create comprehensive integration tests for Blazor components using bUnit framework to ensure components work together correctly, handle user interactions properly, and maintain accessibility standards.

## 🎯 Acceptance Criteria

- [ ] Tests cover all major UI components
- [ ] Tests verify component interactions
- [ ] Tests check responsive behavior
- [ ] Tests validate accessibility features
- [ ] Tests ensure API integration works
- [ ] All tests pass and run efficiently
- [ ] Code coverage >90% for UI components
- [ ] Performance tests for component rendering
- [ ] Cross-browser compatibility tests
- [ ] Integration flow tests (end-to-end scenarios)

## 🔧 Technical Requirements

- Use bUnit testing framework with Moq for mocking
- Test component rendering and behavior
- Verify event handling and state changes
- Mock HTTP client for API integration
- Test accessibility compliance
- Performance benchmarking for components

## 📝 Implementation Details

### File Locations
- **Base Test Class**: `test/CoffeeTracker.Web.Tests/TestUtilities/ComponentTestBase.cs`
- **Form Tests**: `test/CoffeeTracker.Web.Tests/Components/Features/CoffeeEntryFormTests.cs`
- **Summary Tests**: `test/CoffeeTracker.Web.Tests/Components/Features/DailyCoffeeSummaryTests.cs`
- **Selector Tests**: `test/CoffeeTracker.Web.Tests/Components/Shared/CoffeeTypeSelectorTests.cs`
- **Page Tests**: `test/CoffeeTracker.Web.Tests/Components/Pages/CoffeeTrackerPageTests.cs`
- **Integration Tests**: `test/CoffeeTracker.Web.Tests/Integration/CoffeeTrackingFlowTests.cs`

### Test Structure
```
Tests/
├── Components/
│   ├── Features/
│   │   ├── CoffeeEntryFormTests.cs
│   │   ├── DailyCoffeeSummaryTests.cs
│   │   └── QuickEntryButtonsTests.cs
│   ├── Shared/
│   │   ├── CoffeeTypeSelectorTests.cs
│   │   ├── ToastContainerTests.cs
│   │   └── LoadingSpinnerTests.cs
│   └── Pages/
│       └── CoffeeTrackerPageTests.cs
├── TestUtilities/
│   ├── ComponentTestBase.cs
│   ├── MockHttpClientFactory.cs
│   └── TestDataBuilder.cs
└── Integration/
    └── CoffeeTrackingFlowTests.cs
```

### Test Categories
1. **Component Rendering Tests**
   - Components render with correct structure
   - CSS classes applied correctly
   - Responsive behavior on different screen sizes
   - Accessibility attributes present

2. **User Interaction Tests**
   - Form submission with valid data
   - Button clicks and events
   - Keyboard navigation
   - Touch interactions on mobile

3. **API Integration Tests**
   - Successful API calls update UI
   - Error states display properly
   - Loading states show during requests
   - Network failures handled gracefully

4. **Component Communication Tests**
   - Parent-child component communication
   - EventCallback triggers work correctly
   - State management between components
   - Real-time updates propagate properly

## 🤖 Copilot Prompt

```
Create comprehensive integration tests for Blazor components using bUnit in a .NET 8 Blazor Web App.

Test project setup:
- Use bUnit, Moq, and FluentAssertions
- Test both component rendering and behavior
- Mock HTTP client for API integration
- Include accessibility testing

Base test class for common setup:
```csharp
public abstract class ComponentTestBase : TestContext
{
    protected Mock<HttpClient> MockHttpClient { get; private set; }
    protected Mock<IToastService> MockToastService { get; private set; }
    
    protected ComponentTestBase()
    {
        MockHttpClient = new Mock<HttpClient>();
        MockToastService = new Mock<IToastService>();
        
        Services.AddSingleton(MockHttpClient.Object);
        Services.AddSingleton(MockToastService.Object);
        
        JSInterop.SetupVoid("blazorBootstrap.toast.show");
        JSInterop.SetupVoid("blazorBootstrap.toast.hide");
    }
}
```

Test scenarios for each component:

CoffeeEntryForm tests:
- Form renders with all required fields
- Form submission with valid data
- Validation errors prevent submission  
- Real-time caffeine calculation
- API integration success/failure scenarios

DailyCoffeeSummary tests:
- Empty state when no entries
- Summary statistics calculation
- Entry list rendering
- Real-time updates when entries added

CoffeeTypeSelector tests:
- Single selection mode
- Multi-selection mode
- Keyboard navigation
- Accessibility compliance

CoffeeTrackerPage tests:
- Page layout and component integration
- State management between components
- Route handling and navigation
- Responsive behavior

End-to-end flow tests:
- Complete coffee logging workflow
- Form submission updates summary
- Error handling throughout flow
- Component communication chain

Mock data utilities:
```csharp
public static class TestDataBuilder
{
    public static CreateCoffeeEntryRequest ValidCoffeeEntry() => new()
    {
        CoffeeType = "Latte",
        Size = "Medium", 
        Source = "Test Coffee Shop"
    };
    
    public static List<CoffeeEntryResponse> SampleDailyEntries() => new()
    {
        new() { Id = 1, CoffeeType = "Espresso", Size = "Small", CaffeineAmount = 90 },
        new() { Id = 2, CoffeeType = "Latte", Size = "Large", CaffeineAmount = 120 }
    };
}
```

Accessibility testing:
- Screen reader compatibility
- Keyboard navigation
- Focus management
- ARIA compliance
- Color contrast validation

Performance testing:
- Component rendering benchmarks
- Memory usage during interactions
- Event handler efficiency
- Large dataset handling

Test naming conventions:
[Fact] ComponentName_WhenCondition_ShouldExpectedResult()
[Theory] ComponentName_WithDifferentInputs_ShouldProduceExpectedOutputs()

Create test utilities for common assertions and setup patterns.
```

## ✅ Definition of Done

- [ ] Comprehensive test suite created using bUnit
- [ ] All major UI components have test coverage >90%
- [ ] Component interaction tests verify proper communication
- [ ] API integration tests with mocked HTTP client
- [ ] Accessibility tests validate ARIA and keyboard navigation
- [ ] Responsive behavior tests for different screen sizes
- [ ] Performance tests identify rendering bottlenecks
- [ ] End-to-end integration flow tests
- [ ] All tests pass consistently
- [ ] Test utilities created for reusable patterns
- [ ] Mock data builders for consistent test data
- [ ] CI/CD pipeline integration ready

## 🔗 Related Issues

- Depends on: #018-#022 (All Epic 4 components)
- Tests: All Epic 4 Blazor components
- Epic: Epic 4 (Blazor UI Components)
- Validates: Component integration and user flows

## 📌 Notes

- **Test Organization**: Separate tests by component type and functionality
- **Performance**: Include performance benchmarks to catch regressions
- **Accessibility**: Ensure comprehensive accessibility testing coverage
- **Maintenance**: Create reusable test utilities to reduce duplication
- **CI Integration**: Tests should run efficiently in continuous integration

## 🧪 Test Categories

### Unit Tests
- Individual component rendering
- Component parameter binding
- Event callback verification
- State management within components

### Integration Tests  
- Component-to-component communication
- API integration with mocked services
- Form submission and data flow
- Error handling across components

### Accessibility Tests
- Screen reader compatibility
- Keyboard navigation paths
- Focus management
- ARIA attribute presence and accuracy

### Performance Tests
- Component rendering speed
- Memory usage patterns
- Event handler efficiency
- Large data set handling

### Responsive Tests
- Mobile layout adaptation
- Touch interaction handling
- Desktop-specific features
- Tablet optimization

---

**Assigned to:** Development Team  
**Created:** July 22, 2025  
**Last Updated:** July 22, 2025
