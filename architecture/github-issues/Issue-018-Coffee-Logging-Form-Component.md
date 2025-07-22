# Issue #018: Create Coffee Logging Form Component

**Labels:** `epic-4`, `blazor-component`, `high-priority`, `ui-form`, `validation`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 4 - Blazor UI Components for Coffee Logging  
**Estimated Time:** 4-5 hours  

## 📋 Description

Create a responsive Blazor component for logging coffee consumption with intuitive form controls, real-time caffeine calculation, and comprehensive validation.

## 🎯 Acceptance Criteria

- [ ] Form includes coffee type, size, and source selection
- [ ] Real-time caffeine calculation display
- [ ] Form validation with user-friendly error messages
- [ ] Mobile-friendly responsive design
- [ ] Submits data to API and handles responses
- [ ] Unit tests using bUnit with >90% coverage
- [ ] Loading states during API calls
- [ ] Success/error message display
- [ ] Form reset after successful submission
- [ ] Accessibility features (ARIA labels, keyboard navigation)

## 🔧 Technical Requirements

- Use Blazor Server components with Bootstrap 5 styling
- Implement client-side validation using DataAnnotations
- Show loading states during API calls
- Handle network errors gracefully
- Mobile-first responsive design
- Auto-focus and tab order optimization

## 📝 Implementation Details

### File Locations
- **Component**: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Features/CoffeeEntryForm.razor`
- **CSS**: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Features/CoffeeEntryForm.razor.css`
- **Tests**: `test/CoffeeTracker.Web.Tests/Components/Features/CoffeeEntryFormTests.cs`

### Form Fields Required
1. **Coffee Type Dropdown**
   - Options: Espresso, Americano, Latte, Cappuccino, Mocha, Macchiato, Flat White, Black Coffee
   - Required field with validation
   - Bootstrap select styling

2. **Coffee Size Selection**
   - Options: Small, Medium, Large, Extra Large
   - Required field with validation
   - Display caffeine multiplier info

3. **Coffee Source Input**
   - Optional field for coffee shop name or "Home"
   - Placeholder: "Coffee shop name or 'Home'"
   - Max length: 100 characters

4. **Real-time Caffeine Display**
   - Show calculated caffeine amount as user selects type/size
   - Update dynamically without form submission
   - Display in mg with friendly description

### API Integration
- POST to `/api/coffee-entries` endpoint
- Handle HTTP errors with user-friendly messages
- Show loading spinner during submission
- Success/error toast notifications

## 🤖 Copilot Prompt

```
Create a Blazor component called `CoffeeEntryForm` for logging coffee consumption in a .NET 8 Blazor Web App.

Component requirements:
- File location: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Features/CoffeeEntryForm.razor`
- Use Bootstrap 5 for styling
- Mobile-first responsive design
- Support both Interactive Server and WebAssembly render modes

Form fields needed:
1. Coffee Type dropdown - Required with validation, Bootstrap select styling
2. Coffee Size radio buttons - Required with validation, display caffeine multiplier
3. Coffee Source text input - Optional, max 100 chars, auto-suggest placeholder
4. Real-time caffeine display - Dynamic calculation, friendly descriptions

Features to implement:
- Form validation using DataAnnotations and Blazor validation
- Loading state during API submission with spinner
- Success/error message display with toast notifications
- Form reset after successful submission
- Responsive design for mobile and desktop
- Accessibility attributes (ARIA labels, keyboard navigation)
- Auto-focus on first field, optimized tab order
- Enter key submits form, visual feedback for all interactions

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

Create comprehensive unit tests using bUnit:
- Test form rendering with all fields
- Test validation behavior
- Test API integration (mocked)
- Test responsive behavior
- Test error handling scenarios

Create supporting CSS file with component-scoped styles.

Example usage: `<CoffeeEntryForm OnEntryAdded="HandleEntryAdded" />`
```

## ✅ Definition of Done

- [ ] CoffeeEntryForm component created with all required fields
- [ ] Coffee type dropdown with all supported types
- [ ] Coffee size selection with validation
- [ ] Coffee source optional input field
- [ ] Real-time caffeine calculation and display
- [ ] Form validation with DataAnnotations
- [ ] API integration with error handling
- [ ] Loading states and success/error feedback
- [ ] Responsive design for all screen sizes
- [ ] Accessibility features implemented
- [ ] Unit tests written with >90% coverage
- [ ] All tests passing
- [ ] Form reset functionality working
- [ ] Toast notifications for user feedback
- [ ] Code follows project coding standards

## 🔗 Related Issues

- Depends on: Epic 2 completion (Coffee Logging API), Epic 3 completion (Coffee Shop API)
- Blocks: #019 (Daily Summary Component), #021 (Main Coffee Tracking Page)
- Epic: Epic 4 (Blazor UI Components)
- Works with: All other Epic 4 UI components

## 📌 Notes

- **Mobile-First**: Design should work perfectly on mobile devices
- **Real-Time Feedback**: Caffeine calculation should update immediately
- **User Experience**: Focus on intuitive, fast coffee logging
- **Accessibility**: Ensure screen reader compatibility and keyboard navigation
- **Performance**: Efficient re-rendering and API calls

## 🧪 Test Scenarios

### Happy Path Tests
- Submit valid coffee entry form
- Real-time caffeine calculation updates
- Form resets after successful submission
- Success notification displays

### Validation Tests
- Required field validation (coffee type, size)
- Maximum length validation (coffee source)
- Invalid data handling
- Form submission prevention with invalid data

### Error Handling Tests
- API service unavailable
- Network connection issues
- Invalid API responses
- Timeout scenarios

### Accessibility Tests
- Keyboard navigation works correctly
- Screen reader compatibility
- Focus management
- ARIA attributes present

### Responsive Design Tests
- Mobile layout optimization
- Desktop layout features
- Touch vs mouse interactions
- Form usability on different screen sizes

---

**Assigned to:** Development Team  
**Created:** July 22, 2025  
**Last Updated:** July 22, 2025
