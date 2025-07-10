# Epic 4: Blazor UI Components for Coffee Logging

**Epic Goal:** Create intuitive Blazor UI components for anonymous users to log coffee consumption and view their daily tracking.

**Estimated Time:** 4-5 days  
**Priority:** High (User Interface)  
**Dependencies:** Epic 2 (Coffee Logging API)  

---

## 📋 Tasks

### Task 4.1: Create Coffee Logging Form Component
**Estimated Time:** 4-5 hours  
**Priority:** High  

**Description:**
Create a responsive Blazor component for logging coffee consumption with intuitive form controls.

**Acceptance Criteria:**
- [ ] Form includes coffee type, size, and source selection
- [ ] Real-time caffeine calculation display
- [ ] Form validation with user-friendly error messages
- [ ] Mobile-friendly responsive design
- [ ] Submits data to API and handles responses
- [ ] Unit tests using bUnit

**Technical Details:**
- Use Blazor Server components with Bootstrap styling
- Implement client-side validation
- Show loading states during API calls
- Handle network errors gracefully

**Copilot Prompt:**
```
Create a Blazor component called `CoffeeEntryForm` for logging coffee consumption in a .NET 8 Blazor Web App.

Component requirements:
- File location: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Features/CoffeeEntryForm.razor`
- Use Bootstrap 5 for styling
- Mobile-first responsive design
- Support both Interactive Server and WebAssembly render modes

Form fields needed:
1. Coffee Type dropdown
   - Options: Espresso, Americano, Latte, Cappuccino, Mocha, Macchiato, Flat White, Black Coffee
   - Required field with validation
   - Bootstrap select styling

2. Coffee Size radio buttons or dropdown
   - Options: Small, Medium, Large, Extra Large
   - Required field with validation
   - Display caffeine multiplier info

3. Coffee Source text input
   - Optional field for coffee shop name or "Home"
   - Placeholder: "Coffee shop name or 'Home'"
   - Max length: 100 characters
   - Auto-suggest from recent entries (future enhancement placeholder)

4. Real-time caffeine display
   - Show calculated caffeine amount as user selects type/size
   - Update dynamically without form submission
   - Display in mg with friendly description

Features to implement:
- Form validation using DataAnnotations and Blazor validation
- Loading state during API submission
- Success/error message display
- Form reset after successful submission
- Responsive design for mobile and desktop
- Accessibility attributes (ARIA labels, etc.)

API integration:
- Inject HttpClient for API calls
- POST to /api/coffee-entries endpoint
- Handle HTTP errors and display user-friendly messages
- Show loading spinner during submission

State management:
- Use component state for form data
- Implement proper two-way binding
- Handle validation state changes
- Clear form after successful submission

User experience:
- Auto-focus on first field
- Tab order optimization
- Enter key submits form
- Visual feedback for all interactions
- Toast notifications for success/error

Create comprehensive unit tests using bUnit:
- Test form rendering with all fields
- Test validation behavior
- Test API integration (mocked)
- Test responsive behavior
- Test error handling scenarios

Create supporting CSS file: `CoffeeEntryForm.razor.css` with component-scoped styles.

Example usage:
```razor
<CoffeeEntryForm OnEntryAdded="HandleEntryAdded" />
```

Include XML documentation for all public properties and methods.
```

---

### Task 4.2: Create Daily Coffee Summary Component
**Estimated Time:** 3-4 hours  
**Priority:** High  

**Description:**
Create a component to display daily coffee consumption summary with visual indicators and statistics.

**Acceptance Criteria:**
- [ ] Shows total coffees consumed today
- [ ] Displays total caffeine intake with health indicators
- [ ] Lists individual coffee entries with timestamps
- [ ] Visual progress indicators for daily limits
- [ ] Responsive design for mobile and desktop
- [ ] Unit tests using bUnit

**Technical Details:**
- Use charts or progress bars for visual representation
- Color-coded caffeine levels (green, yellow, red)
- Real-time updates when new entries are added
- Handle empty state gracefully

**Copilot Prompt:**
```
Create a Blazor component called `DailyCoffeeSummary` that displays coffee consumption analytics for anonymous users in a .NET 8 Blazor Web App.

Component requirements:
- File location: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Features/DailyCoffeeSummary.razor`
- Use Bootstrap 5 for styling with custom CSS
- Responsive design for mobile and desktop
- Auto-refresh when new entries are added

Summary display sections:
1. Daily Overview Cards
   - Total coffees today (large number with icon)
   - Total caffeine (mg) with health indicator
   - Average per coffee entry
   - Time of last coffee

2. Caffeine Health Indicator
   - Progress bar showing daily caffeine intake
   - Color coding:
     - Green: 0-200mg (Safe)
     - Yellow: 200-400mg (Moderate)
     - Orange: 400-600mg (High)
     - Red: 600mg+ (Very High)
   - Recommended daily limit indicator (400mg)

3. Coffee Entries List
   - Chronological list of today's entries
   - Each entry shows: time, type, size, source, caffeine amount
   - Mobile-friendly card layout
   - Empty state message when no entries

4. Quick Stats
   - Most consumed coffee type today
   - Favorite coffee source
   - Peak consumption time
   - Comparison to yesterday (if available in localStorage)

Visual features:
- Icons for different coffee types
- Color-coded caffeine amounts
- Smooth animations for data updates
- Loading skeletons while data loads
- Responsive grid layout

API integration:
- Inject HttpClient for API calls
- GET from /api/coffee-entries endpoint
- GET daily summary from API
- Handle loading states and errors
- Refresh data on component initialization

Real-time updates:
- Subscribe to coffee entry events (via EventCallback)
- Refresh data when OnEntryAdded is triggered
- Smooth animations for value changes

User experience:
- Friendly empty state with call-to-action
- Tooltips explaining caffeine levels
- Accessible color coding (not just color-dependent)
- Print-friendly layout

Create comprehensive unit tests using bUnit:
- Test rendering with sample data
- Test empty state display
- Test caffeine level calculations and color coding
- Test responsive behavior
- Test data refresh scenarios

Create supporting CSS file: `DailyCoffeeSummary.razor.css` with:
- Custom progress bar styles
- Coffee type icons (or use Font Awesome)
- Mobile-responsive grid
- Color scheme for caffeine levels

Data models for component:
```csharp
public class DailySummaryViewModel
{
    public int TotalEntries { get; set; }
    public int TotalCaffeine { get; set; }
    public decimal AverageCaffeine { get; set; }
    public DateTime? LastCoffeeTime { get; set; }
    public string CaffeineLevel { get; set; } // "Safe", "Moderate", "High", "VeryHigh"
    public List<CoffeeEntryViewModel> Entries { get; set; }
}
```

Example usage:
```razor
<DailyCoffeeSummary OnRefreshRequested="RefreshData" />
```
```

---

### Task 4.3: Create Coffee Type Selection Component
**Estimated Time:** 2-3 hours  
**Priority:** Medium  

**Description:**
Create a reusable component for selecting coffee types with visual cards and caffeine information.

**Acceptance Criteria:**
- [ ] Visual card-based selection interface
- [ ] Shows coffee type images or icons
- [ ] Displays caffeine content for each type
- [ ] Supports single and multi-selection modes
- [ ] Accessible keyboard navigation
- [ ] Unit tests using bUnit

**Technical Details:**
- Reusable component for different contexts
- Support for different selection modes
- Include caffeine information display
- Mobile-friendly touch interactions

**Copilot Prompt:**
```
Create a reusable Blazor component called `CoffeeTypeSelector` for selecting coffee types with visual cards in a .NET 8 Blazor Web App.

Component requirements:
- File location: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Shared/CoffeeTypeSelector.razor`
- Reusable across different contexts (forms, filters, etc.)
- Support both single and multi-selection modes
- Visual card-based interface with coffee type information

Card design for each coffee type:
1. Coffee type icon or image placeholder
2. Coffee type name (large, bold text)
3. Base caffeine content (e.g., "90mg")
4. Brief description (e.g., "Strong, concentrated shot")
5. Selection indicator (border, checkmark, etc.)

Coffee types to support:
- Espresso: 90mg, "Strong, concentrated shot"
- Americano: 120mg, "Espresso with hot water"
- Latte: 80mg, "Espresso with steamed milk"
- Cappuccino: 80mg, "Equal parts espresso, milk, foam"
- Mocha: 90mg, "Espresso with chocolate"
- Macchiato: 120mg, "Espresso 'marked' with milk"
- Flat White: 130mg, "Double shot with microfoam"
- Black Coffee: 95mg, "Traditional brewed coffee"

Component parameters:
```csharp
[Parameter] public string? SelectedValue { get; set; }
[Parameter] public EventCallback<string> SelectedValueChanged { get; set; }
[Parameter] public List<string>? SelectedValues { get; set; } // For multi-select
[Parameter] public EventCallback<List<string>> SelectedValuesChanged { get; set; }
[Parameter] public bool MultiSelect { get; set; } = false;
[Parameter] public bool ShowCaffeineInfo { get; set; } = true;
[Parameter] public string CssClass { get; set; } = "";
[Parameter] public RenderFragment? EmptyTemplate { get; set; }
```

Visual features:
- Responsive grid layout (1-2-3-4 columns based on screen size)
- Hover effects and smooth transitions
- Selected state visual feedback
- Touch-friendly sizing for mobile
- Loading state for dynamic data

Accessibility features:
- ARIA labels and descriptions
- Keyboard navigation (arrow keys, space/enter)
- Focus indicators
- Screen reader support
- Semantic HTML structure

User experience:
- Visual feedback on selection
- Smooth animations
- Intuitive selection behavior
- Clear selection indicators
- Optional search/filter capability

Styling:
- Bootstrap card components as base
- Custom CSS for coffee type theming
- Consistent spacing and typography
- Mobile-first responsive design
- Dark/light mode considerations

Create comprehensive unit tests using bUnit:
- Test single selection mode
- Test multi-selection mode
- Test keyboard navigation
- Test parameter binding
- Test event callbacks
- Test accessibility features

Create supporting CSS file: `CoffeeTypeSelector.razor.css` with:
- Card hover effects
- Selection state styles
- Responsive grid layout
- Coffee type theming
- Animation transitions

Data service integration:
- Support for getting coffee types from API
- Local coffee type data as fallback
- Caffeine calculation utilities

Example usage:
```razor
<!-- Single selection -->
<CoffeeTypeSelector @bind-SelectedValue="selectedType" 
                   ShowCaffeineInfo="true" />

<!-- Multi-selection -->
<CoffeeTypeSelector @bind-SelectedValues="selectedTypes" 
                   MultiSelect="true" />
```

Include coffee type icons or use emoji as fallback:
☕ Espresso, 🥤 Americano, 🥛 Latte, ☕ Cappuccino, 🍫 Mocha, ☕ Macchiato, 🥛 Flat White, ☕ Black Coffee
```

---

### Task 4.4: Create Main Coffee Tracking Page
**Estimated Time:** 3-4 hours  
**Priority:** High  

**Description:**
Create the main page that combines all coffee tracking components into a cohesive user experience.

**Acceptance Criteria:**
- [ ] Integrates coffee entry form and daily summary
- [ ] Handles state management between components
- [ ] Responsive layout for all screen sizes
- [ ] Loading states and error handling
- [ ] Navigation and page structure
- [ ] Unit tests using bUnit

**Technical Details:**
- Use Blazor page routing
- Implement proper state management
- Handle component communication
- Include page metadata and SEO

**Copilot Prompt:**
```
Create the main coffee tracking page component called `CoffeeTracker` in a .NET 8 Blazor Web App that combines all coffee logging functionality.

Page requirements:
- File location: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Pages/CoffeeTracker.razor`
- Route: `/coffee-tracker` and `/track` (with redirect from `/`)
- Page title: "Coffee Tracker - Log Your Daily Coffee"
- Responsive design for mobile, tablet, and desktop

Page layout structure:
1. Header section
   - Page title and subtitle
   - Current date display
   - Quick stats (optional)

2. Main content area (two-column on desktop, stacked on mobile)
   - Left column: Coffee entry form
   - Right column: Daily summary

3. Footer section (optional)
   - Tips about caffeine consumption
   - Links to help or about pages

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
- OnAfterRender: Set up any JavaScript interop
- Handle component refresh scenarios

API integration:
- Load initial data on page load
- Refresh data after new entries added
- Handle API errors gracefully
- Show loading states appropriately

User experience features:
- Smooth page transitions
- Loading skeletons while data loads
- Success notifications after logging coffee
- Error handling with retry options
- Offline detection (future enhancement)

Responsive design:
- Mobile: Stacked layout, touch-friendly
- Tablet: Responsive columns, optimized spacing
- Desktop: Two-column layout, full features

Page-level features:
- Browser title updates with coffee count
- Meta tags for SEO
- Open Graph tags for social sharing
- Favicon updates (future enhancement)

Real-time updates:
- Handle form submission success
- Refresh summary data automatically
- Show immediate feedback
- Update browser title with new count

Error handling:
- Network connection issues
- API service unavailable
- Invalid session states
- Graceful degradation

Create comprehensive unit tests using bUnit:
- Test page rendering and layout
- Test component integration
- Test state management
- Test API error scenarios
- Test responsive behavior

Page parameters and query strings:
- Support for deep linking
- Handle date parameter for historical viewing
- Preserve state during navigation

SEO and metadata:
```razor
<PageTitle>Coffee Tracker - @DateTime.Today.ToString("MMMM dd, yyyy")</PageTitle>
<HeadContent>
    <meta name="description" content="Track your daily coffee consumption and caffeine intake with our easy-to-use coffee tracker." />
    <meta property="og:title" content="Coffee Tracker" />
    <meta property="og:description" content="Log and analyze your coffee consumption patterns" />
</HeadContent>
```

Accessibility:
- Proper heading hierarchy
- Skip links for keyboard navigation
- ARIA landmarks and labels
- High contrast support
- Screen reader compatibility

Performance considerations:
- Lazy loading of non-critical components
- Efficient data fetching
- Minimize re-renders
- Optimize for mobile performance

Create supporting CSS file: `CoffeeTracker.razor.css` with:
- Page-specific layout styles
- Responsive grid system
- Animation and transition effects
- Mobile-optimized spacing
```

---

### Task 4.5: Add Client-Side Validation and UX Polish
**Estimated Time:** 3-4 hours  
**Priority:** Medium  

**Description:**
Enhance the UI with advanced client-side validation, animations, and user experience improvements.

**Acceptance Criteria:**
- [ ] Real-time validation feedback
- [ ] Smooth animations and transitions
- [ ] Loading states and progress indicators
- [ ] Toast notifications for user feedback
- [ ] Keyboard shortcuts and accessibility
- [ ] Unit tests for validation scenarios

**Technical Details:**
- Use Blazor validation components
- Add CSS animations and transitions
- Implement toast notification system
- Enhance keyboard navigation

**Copilot Prompt:**
```
Enhance the coffee tracking UI with advanced client-side validation and UX polish in a .NET 8 Blazor Web App.

Validation enhancements needed:
1. Real-time validation feedback
   - Show validation messages as user types
   - Visual indicators for valid/invalid fields
   - Prevent form submission with invalid data
   - Custom validation attributes for business rules

2. Advanced form validation
   - Cross-field validation (e.g., reasonable caffeine limits)
   - Custom validators for coffee-specific rules
   - Async validation for duplicate detection
   - Validation summary component

Validation components to create:
```csharp
// Custom validation attributes
public class MaxDailyCaffeineAttribute : ValidationAttribute
public class ValidCoffeeTypeAttribute : ValidationAttribute
public class ReasonableTimestampAttribute : ValidationAttribute

// Validation helper service
public interface IClientValidationService
{
    Task<bool> ValidateDailyCaffeineLimit(int currentCaffeine, int newCaffeine);
    bool ValidateCoffeeTypeSize(string coffeeType, string size);
    string GetValidationMessage(string propertyName, object value);
}
```

UX polish features:
1. Loading states and spinners
   - Form submission loading
   - Data refresh indicators
   - Skeleton loading for content
   - Progress bars for operations

2. Toast notification system
   - Success notifications for coffee logged
   - Error notifications for failures
   - Warning notifications for high caffeine
   - Auto-dismiss with manual dismiss option

3. Smooth animations
   - Form field focus animations
   - Card hover effects
   - Data update transitions
   - Page transition effects

4. Enhanced interactions
   - Auto-save draft entries to localStorage
   - Keyboard shortcuts (Ctrl+Enter to submit)
   - Swipe gestures for mobile
   - Drag-and-drop for reordering (future)

Toast notification component:
```razor
<ToastContainer Position="ToastPosition.TopRight" />

public enum ToastType { Success, Error, Warning, Info }
public enum ToastPosition { TopRight, TopLeft, BottomRight, BottomLeft }
```

Animation and transition CSS:
- Fade in/out effects
- Slide animations for mobile
- Micro-interactions for buttons
- Loading shimmer effects
- Smooth color transitions

Accessibility enhancements:
- Enhanced focus management
- ARIA live regions for dynamic content
- High contrast mode support
- Reduced motion preferences
- Screen reader announcements

Performance optimizations:
- Debounced validation
- Efficient re-rendering
- Lazy loading of components
- CSS-in-JS for dynamic styles
- Image optimization

Advanced features:
1. Auto-complete for coffee sources
   - Remember previous sources
   - Suggest popular coffee shops
   - Filter suggestions as user types

2. Quick entry modes
   - "Same as last time" button
   - Favorite coffee combinations
   - One-tap logging for common drinks

3. Offline support preparation
   - Store entries in localStorage
   - Sync when connection restored
   - Offline indicator

Error boundary and recovery:
- Global error boundary component
- Automatic retry mechanisms
- Graceful degradation
- User-friendly error messages

Create comprehensive unit tests using bUnit:
- Test validation behavior
- Test animation triggers
- Test toast notifications
- Test keyboard shortcuts
- Test accessibility features
- Test error recovery

CSS framework integration:
- Custom CSS animations
- Bootstrap component enhancements
- Responsive utility classes
- CSS custom properties for theming
- Dark mode support preparation

Component structure:
```
Components/
├── Shared/
│   ├── ToastContainer.razor
│   ├── LoadingSpinner.razor
│   ├── ValidationSummary.razor
│   └── ErrorBoundary.razor
├── Features/
│   ├── CoffeeEntryForm.razor (enhanced)
│   ├── DailyCoffeeSummary.razor (enhanced)
│   └── QuickEntryButtons.razor
└── Validators/
    ├── CoffeeValidationService.cs
    └── ValidationAttributes.cs
```

JavaScript interop for advanced features:
- Focus management
- Clipboard API for sharing
- Vibration API for mobile feedback
- Local storage management

Example enhancements:
```razor
<EditForm Model="coffeeEntry" OnValidSubmit="HandleSubmit">
    <FluentValidationValidator />
    <ValidationSummary />
    
    <div class="form-group fade-in">
        <InputSelect @bind-Value="coffeeEntry.CoffeeType" 
                    class="form-control @(IsValid("CoffeeType") ? "is-valid" : "is-invalid")">
            <!-- options -->
        </InputSelect>
        <ValidationMessage For="() => coffeeEntry.CoffeeType" />
    </div>
    
    <LoadingButton IsLoading="isSubmitting" 
                  LoadingText="Logging coffee..." 
                  Type="submit">
        Log Coffee
    </LoadingButton>
</EditForm>
```
```

---

### Task 4.6: Create Blazor Component Integration Tests
**Estimated Time:** 3-4 hours  
**Priority:** Medium  

**Description:**
Create comprehensive integration tests for Blazor components using bUnit framework.

**Acceptance Criteria:**
- [ ] Tests cover all major UI components
- [ ] Tests verify component interactions
- [ ] Tests check responsive behavior
- [ ] Tests validate accessibility features
- [ ] Tests ensure API integration works
- [ ] All tests pass and run efficiently

**Technical Details:**
- Use bUnit testing framework
- Mock HTTP client for API calls
- Test component rendering and behavior
- Verify event handling and state changes

**Copilot Prompt:**
```
Create comprehensive integration tests for Blazor components using bUnit in a .NET 8 Blazor Web App.

Test project setup:
- Project: `test/CoffeeTracker.Web.Tests/`
- Use bUnit, Moq, and FluentAssertions
- Test both component rendering and behavior
- Mock HTTP client for API integration

Test structure and organization:
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

Base test class for common setup:
```csharp
public abstract class ComponentTestBase : TestContext
{
    protected Mock<HttpClient> MockHttpClient { get; private set; }
    protected Mock<IToastService> MockToastService { get; private set; }
    
    protected ComponentTestBase()
    {
        // Common setup for all component tests
        MockHttpClient = new Mock<HttpClient>();
        MockToastService = new Mock<IToastService>();
        
        Services.AddSingleton(MockHttpClient.Object);
        Services.AddSingleton(MockToastService.Object);
        
        // Add other common services
        JSInterop.SetupVoid("blazorBootstrap.toast.show");
        JSInterop.SetupVoid("blazorBootstrap.toast.hide");
    }
}
```

Test scenarios for CoffeeEntryForm:
1. Rendering tests
   - Form renders with all required fields
   - Validation messages appear correctly
   - Loading state displays appropriately
   - Success state shows confirmation

2. User interaction tests
   - Form submission with valid data
   - Validation errors prevent submission
   - Real-time caffeine calculation
   - Form reset after successful submission

3. API integration tests
   - Successful API call creates entry
   - API errors display appropriate messages
   - Loading states during API calls
   - Network timeout handling

Test scenarios for DailyCoffeeSummary:
1. Data display tests
   - Empty state when no entries
   - Summary statistics calculation
   - Entry list rendering
   - Caffeine level indicators

2. Responsive behavior tests
   - Mobile layout adaptation
   - Desktop layout features
   - Component resizing behavior

3. Real-time updates tests
   - Component refreshes when new entry added
   - Statistics update correctly
   - Visual indicators change appropriately

Test scenarios for CoffeeTypeSelector:
1. Selection behavior tests
   - Single selection mode
   - Multi-selection mode
   - Keyboard navigation
   - Touch interactions

2. Accessibility tests
   - ARIA attributes present
   - Focus management
   - Screen reader compatibility
   - Keyboard shortcuts

End-to-end flow tests:
1. Complete coffee logging flow
   - User selects coffee type and size
   - Form validates and submits
   - Summary updates immediately
   - Success notification appears

2. Error handling flows
   - API failures show error messages
   - Network errors trigger retry options
   - Validation errors prevent progression

Mock data and utilities:
```csharp
public static class TestDataBuilder
{
    public static CreateCoffeeEntryRequest ValidCoffeeEntry() =>
        new()
        {
            CoffeeType = "Latte",
            Size = "Medium",
            Source = "Test Coffee Shop"
        };
        
    public static List<CoffeeEntryResponse> SampleDailyEntries() =>
        new()
        {
            new() { Id = 1, CoffeeType = "Espresso", Size = "Small", CaffeineAmount = 90 },
            new() { Id = 2, CoffeeType = "Latte", Size = "Large", CaffeineAmount = 120 }
        };
}
```

Performance and load tests:
- Component rendering performance
- Large dataset handling
- Memory usage during long sessions
- Event handler efficiency

Accessibility testing:
- Screen reader compatibility
- Keyboard navigation
- Color contrast validation
- Focus management
- ARIA compliance

Cross-browser compatibility tests:
- Different viewport sizes
- Touch vs mouse interactions
- CSS feature support
- JavaScript compatibility

Test naming conventions:
```csharp
[Fact]
public void CoffeeEntryForm_WhenSubmittedWithValidData_ShouldCallApiAndShowSuccess()

[Fact]
public void DailyCoffeeSummary_WhenNoEntries_ShouldDisplayEmptyState()

[Theory]
[InlineData("Espresso", "Small", 72)]
[InlineData("Latte", "Large", 104)]
public void CoffeeTypeSelector_WhenTypeAndSizeSelected_ShouldCalculateCorrectCaffeine(
    string type, string size, int expectedCaffeine)
```

Test utilities for assertions:
```csharp
public static class ComponentAssertions
{
    public static void ShouldHaveClass(this IElement element, string className)
    public static void ShouldBeVisible(this IElement element)
    public static void ShouldContainText(this IRenderedComponent<T> component, string text)
    public static void ShouldHaveValidationError(this IRenderedComponent<T> component, string fieldName)
}
```

Continuous integration considerations:
- Tests run in headless browser environment
- Parallel test execution
- Test result reporting
- Code coverage measurement
- Performance benchmarking
```

---

## 🎯 Epic Definition of Done

- [ ] All Blazor UI components created and functional
- [ ] Responsive design works on mobile, tablet, and desktop
- [ ] Form validation provides clear user feedback
- [ ] Components integrate properly with API endpoints
- [ ] Real-time caffeine calculations display correctly
- [ ] Loading states and error handling implemented
- [ ] Toast notifications provide user feedback
- [ ] Unit tests achieve >90% coverage
- [ ] Integration tests cover component interactions
- [ ] Accessibility features implemented (ARIA, keyboard navigation)
- [ ] Performance requirements met (<2s initial load)
- [ ] Cross-browser compatibility verified

## 📋 Notes

**Technical Decisions:**
- Using Blazor Server for real-time updates and reduced bandwidth
- Bootstrap 5 for consistent styling and responsive design
- Component-based architecture for reusability
- Client-side state management for better user experience

**User Experience Focus:**
- Mobile-first responsive design
- One-tap coffee logging for common drinks
- Real-time feedback for all user actions
- Graceful error handling with recovery options
- Accessibility compliance for inclusive design

**Performance Considerations:**
- Efficient re-rendering with proper state management
- Lazy loading of non-critical components
- Optimized API calls with caching
- Smooth animations without blocking UI

**Future Enhancements:**
- Progressive Web App (PWA) capabilities
- Offline support with sync
- Push notifications for reminders
- Advanced analytics visualizations

---

**Next Epic:** [Epic 5: Daily Summary & Analytics](./Epic-5-Analytics.md)  
**Previous Epic:** [Epic 3: Coffee Shop API](./Epic-3-Coffee-Shop-API.md)
