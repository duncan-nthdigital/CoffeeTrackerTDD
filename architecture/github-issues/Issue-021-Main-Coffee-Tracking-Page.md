# Issue #021: Create Main Coffee Tracking Page

**Labels:** `epic-4`, `blazor-page`, `high-priority`, `ui-integration`, `routing`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 4 - Blazor UI Components for Coffee Logging  
**Estimated Time:** 3-4 hours  

## 📋 Description

Create the main page that combines all coffee tracking components into a cohesive user experience with proper routing, state management, and responsive layout.

## 🎯 Acceptance Criteria

- [ ] Integrates coffee entry form and daily summary
- [ ] Handles state management between components
- [ ] Responsive layout for all screen sizes
- [ ] Loading states and error handling
- [ ] Navigation and page structure
- [ ] Unit tests using bUnit with >90% coverage
- [ ] Page routing and deep linking
- [ ] SEO metadata and page titles
- [ ] Component communication handling
- [ ] Performance optimization

## 🔧 Technical Requirements

- Use Blazor page routing with multiple route patterns
- Implement proper state management between components
- Handle component communication via EventCallbacks
- Include page metadata and SEO optimization
- Responsive two-column layout (desktop) / stacked (mobile)
- Handle loading states and error scenarios

## 📝 Implementation Details

### File Locations
- **Page**: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Pages/CoffeeTracker.razor`
- **CSS**: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Pages/CoffeeTracker.razor.css`
- **Tests**: `test/CoffeeTracker.Web.Tests/Components/Pages/CoffeeTrackerPageTests.cs`

### Page Layout Structure
1. **Header Section**
   - Page title and subtitle
   - Current date display
   - Quick stats (optional)

2. **Main Content Area**
   - Left column: Coffee entry form (desktop)
   - Right column: Daily summary (desktop)
   - Stacked layout on mobile

3. **Footer Section**
   - Tips about caffeine consumption
   - Links to help or about pages

### Routing Configuration
- Primary route: `/coffee-tracker`
- Alternative route: `/track`
- Home redirect: `/` → `/coffee-tracker`

## 🤖 Copilot Prompt

```
Create the main coffee tracking page component called `CoffeeTracker` in a .NET 8 Blazor Web App that combines all coffee logging functionality.

Page requirements:
- File location: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Pages/CoffeeTracker.razor`
- Route: `/coffee-tracker` and `/track` (with redirect from `/`)
- Page title: "Coffee Tracker - Log Your Daily Coffee"
- Responsive design for mobile, tablet, and desktop

Page layout structure:
1. Header section - Page title, subtitle, current date, quick stats
2. Main content area - Two-column on desktop, stacked on mobile
   - Left column: Coffee entry form
   - Right column: Daily summary
3. Footer section - Tips and help links

Component integration:
- Use CoffeeEntryForm component for logging
- Use DailyCoffeeSummary component for analytics
- Handle communication between components
- Manage shared state (current entries list)

State management:
```csharp
private List<CoffeeEntryResponse> _todayEntries = new();
private DailySummaryViewModel _dailySummary = new();
private bool _isLoading = true;
private string? _errorMessage;
```

Page lifecycle:
- OnInitializedAsync: Load today's entries
- Handle component refresh scenarios
- Manage loading and error states

API integration:
- Load initial data on page load
- Refresh data after new entries added
- Handle API errors gracefully
- Show loading states appropriately

SEO and metadata:
- Dynamic page titles with coffee count
- Meta descriptions for search engines
- Open Graph tags for social sharing
- Responsive viewport meta tags

Create comprehensive unit tests using bUnit covering page rendering, component integration, state management, and responsive behavior.

Example component integration:
```razor
<CoffeeEntryForm OnEntryAdded="HandleEntryAdded" />
<DailyCoffeeSummary Entries="_todayEntries" OnRefreshRequested="RefreshData" />
```
```

## ✅ Definition of Done

- [ ] CoffeeTracker page component created
- [ ] Multiple route patterns configured
- [ ] Component integration working properly
- [ ] State management between components
- [ ] Responsive layout (desktop/mobile)
- [ ] Loading states and error handling
- [ ] Page metadata and SEO optimization
- [ ] Browser title updates dynamically
- [ ] Real-time component communication
- [ ] Unit tests with >90% coverage
- [ ] All tests passing
- [ ] Performance optimized
- [ ] Accessibility compliance
- [ ] Cross-browser compatibility

## 🔗 Related Issues

- Depends on: #018 (Coffee Entry Form), #019 (Daily Summary), #020 (Coffee Type Selector)
- Blocks: #022 (Client-Side Validation), #023 (Integration Tests)
- Epic: Epic 4 (Blazor UI Components)
- Integrates: All Epic 4 UI components

## 📌 Notes

- **State Management**: Central page manages state for child components
- **Component Communication**: Use EventCallbacks for parent-child communication
- **Performance**: Efficient re-rendering with proper state management
- **SEO**: Dynamic page titles and meta descriptions
- **Accessibility**: Proper heading hierarchy and navigation structure

## 🧪 Test Scenarios

### Page Rendering Tests
- Page loads with correct layout
- Header, main content, and footer display
- Responsive layout adaptation
- Component integration verification

### State Management Tests
- Initial data loading
- Component communication
- State updates after form submission
- Error state handling

### Routing Tests
- Multiple route patterns work
- Deep linking functionality
- Navigation between routes
- Home page redirect

### Integration Tests
- Form submission updates summary
- Real-time data synchronization
- Component event handling
- Error recovery scenarios

---

**Assigned to:** Development Team  
**Created:** July 22, 2025  
**Last Updated:** July 22, 2025
