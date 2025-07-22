# Issue #020: Create Coffee Type Selection Component

**Labels:** `epic-4`, `blazor-component`, `medium-priority`, `ui-selector`, `reusable`  
**Milestone:** Phase 1 - Anonymous User MVP  
**Epic:** Epic 4 - Blazor UI Components for Coffee Logging  
**Estimated Time:** 2-3 hours  

## 📋 Description

Create a reusable component for selecting coffee types with visual cards, caffeine information, and support for both single and multi-selection modes.

## 🎯 Acceptance Criteria

- [ ] Visual card-based selection interface
- [ ] Shows coffee type images or icons
- [ ] Displays caffeine content for each type
- [ ] Supports single and multi-selection modes
- [ ] Accessible keyboard navigation
- [ ] Unit tests using bUnit with >90% coverage
- [ ] Touch-friendly mobile interactions
- [ ] Reusable across different contexts
- [ ] Smooth hover and selection animations

## 🔧 Technical Requirements

- Reusable component for different contexts (forms, filters, etc.)
- Support for different selection modes
- Include caffeine information display
- Mobile-friendly touch interactions
- Accessibility compliance
- Bootstrap card components with custom styling

## 📝 Implementation Details

### File Locations
- **Component**: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Shared/CoffeeTypeSelector.razor`
- **CSS**: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Shared/CoffeeTypeSelector.razor.css`
- **Tests**: `test/CoffeeTracker.Web.Tests/Components/Shared/CoffeeTypeSelectorTests.cs`

### Coffee Types to Support
- Espresso: 90mg, "Strong, concentrated shot"
- Americano: 120mg, "Espresso with hot water"
- Latte: 80mg, "Espresso with steamed milk"
- Cappuccino: 80mg, "Equal parts espresso, milk, foam"
- Mocha: 90mg, "Espresso with chocolate"
- Macchiato: 120mg, "Espresso 'marked' with milk"
- Flat White: 130mg, "Double shot with microfoam"
- Black Coffee: 95mg, "Traditional brewed coffee"

### Component Parameters
- `SelectedValue` and `SelectedValueChanged` for single selection
- `SelectedValues` and `SelectedValuesChanged` for multi-selection
- `MultiSelect` boolean parameter
- `ShowCaffeineInfo` to display caffeine content
- `CssClass` for additional styling

## 🤖 Copilot Prompt

```
Create a reusable Blazor component called `CoffeeTypeSelector` for selecting coffee types with visual cards in a .NET 8 Blazor Web App.

Component requirements:
- File location: `src/CoffeeTracker.Web/CoffeeTracker.Web/Components/Shared/CoffeeTypeSelector.razor`
- Reusable across different contexts
- Support both single and multi-selection modes
- Visual card-based interface with coffee type information

Card design for each coffee type:
1. Coffee type icon or emoji
2. Coffee type name (large, bold text)
3. Base caffeine content (e.g., "90mg")
4. Brief description
5. Selection indicator (border, checkmark, etc.)

Component parameters:
- SelectedValue/SelectedValueChanged for single selection
- SelectedValues/SelectedValuesChanged for multi-selection
- MultiSelect boolean parameter
- ShowCaffeineInfo display toggle
- CssClass for additional styling

Visual features:
- Responsive grid layout (1-2-3-4 columns based on screen size)
- Hover effects and smooth transitions
- Selected state visual feedback
- Touch-friendly sizing for mobile

Accessibility features:
- ARIA labels and descriptions
- Keyboard navigation (arrow keys, space/enter)
- Focus indicators
- Screen reader support

Create comprehensive unit tests using bUnit covering single/multi selection, keyboard navigation, and accessibility.

Example usage:
```razor
<CoffeeTypeSelector @bind-SelectedValue="selectedType" ShowCaffeineInfo="true" />
<CoffeeTypeSelector @bind-SelectedValues="selectedTypes" MultiSelect="true" />
```
```

## ✅ Definition of Done

- [ ] CoffeeTypeSelector component created
- [ ] Visual card interface for all coffee types
- [ ] Single and multi-selection modes working
- [ ] Caffeine information display
- [ ] Keyboard navigation implemented
- [ ] Touch-friendly mobile interactions
- [ ] Accessibility features (ARIA, screen reader)
- [ ] Responsive grid layout
- [ ] Hover effects and animations
- [ ] Unit tests with >90% coverage
- [ ] All tests passing
- [ ] Reusable component architecture
- [ ] Component documentation

## 🔗 Related Issues

- Depends on: Basic Blazor project setup
- Used by: #018 (Coffee Entry Form), #021 (Main Page)
- Epic: Epic 4 (Blazor UI Components)
- Works with: All Epic 4 form components

## 🧪 Test Scenarios

- Single selection mode behavior
- Multi-selection mode behavior
- Keyboard navigation (arrow keys, space, enter)
- Touch interactions on mobile
- Accessibility compliance testing
- Parameter binding verification

---

**Assigned to:** Development Team  
**Created:** July 22, 2025  
**Last Updated:** July 22, 2025
