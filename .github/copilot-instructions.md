# Copilot Instructions for Test-Driven Development (TDD) in C#

## 🧭 Guiding Principles

Follow the **Three Laws of TDD** as defined by Robert C. Martin (Uncle Bob):

1. **You may not write any production code unless it is to make a failing unit test pass.**
2. **You may not write more of a unit test than is sufficient to fail (compilation failures count).**
3. **You may not write more production code than is sufficient to pass the currently failing test.**

## 🛠️ Development Workflow

- Use the **Red-Green-Refactor** cycle:
  - **Red**: Write a failing single unit test first. Ensure it fails for the right reason. when running an expected red test, give yourself a timeout time, because its expected to fail, so don't wait too long for it to fail.
  - **Green**: Write the minimal code to make the test pass. Don't worry about perfect code yet.
  - **Refactor**: Clean up the code while keeping tests green. Improve design and readability.

- Prioritize **small, incremental steps**. Avoid writing large chunks of code without tests.
- **Run tests frequently** - after every small change.
- Use **xUnit** for unit testing. Place tests in a separate `Tests` project.

## 🧪 Test Style and Structure
- Only create one failing test at a time, during the red,green, refactor cycle.
- Use **descriptive test method names** that express intent (e.g., `Should_Return_Zero_When_Given_Empty_String`).
- Prefer **Arrange-Act-Assert** structure in tests, but simplify when appropriate.
- Use **Theory and InlineData** to reduce test duplication for similar scenarios.
- Extract shared test setup to fields or helper methods.
- Mock dependencies using **Moq** or similar libraries when needed.
- Avoid testing implementation details—focus on **observable behavior**.
- Test one concept per test method.

## 🧼 Clean Code Expectations

- Apply **SOLID principles** in production code.
- Keep methods short and focused (Single Responsibility Principle).
- Avoid duplication—refactor mercilessly after tests pass.
- Use meaningful names for classes, methods, variables, and constants.
- Extract magic numbers and strings into named constants.
- Add XML documentation for public APIs.
- Handle errors gracefully with meaningful error messages.

## 🔄 Refactoring Guidelines

- **Always follow clean code principles** during refactoring.
- **Ensure all tests pass** after each refactoring step.
- After tests are green, suggest refactorings to improve code quality.
- Ensure refactorings **do not change the behavior** of the code.
- Ensure that the production code **remains covered by tests** after refactoring.
- **Method organization**: Order class members logically - fields, properties (public then private), constructors, public methods, then private methods.
- Extract methods when logic becomes complex or to improve readability.
- Replace conditionals with polymorphism when appropriate.
- Use dependency injection for better testability.
- **Only Refactor Tests Or Production Code** - do not mix refactor both tests and production code without running and having green tests between the changes, for example refactor tests, run tests and get green so nothing is broken, refactor production code run tests and get green so nothing is broken.
- Avoid large refactorings that change multiple aspects of the code at once.

## 🤖 Copilot Behavior

- When writing new features, **always begin by generating a failing test**.
- Suggest only the **minimal code needed** to pass the current test.
- **Propose refactorings** after tests are green, not during the Green phase.
- Avoid suggesting code that bypasses test coverage.
- Run tests after every change to ensure nothing breaks.
- Focus on one requirement at a time.

## 🎯 Testing Best Practices

- **Test behavior, not implementation** - tests should be resilient to refactoring.
- Use **Given-When-Then** thinking even if not using BDD frameworks.
- **Isolate tests** - each test should be independent and able to run in any order.
- Use **meaningful assertions** - prefer specific assertions over generic ones.
- **Test edge cases** - empty inputs, null values, boundary conditions.
- Keep tests **simple and readable** - they serve as living documentation.

## 🔥 Blazor Development Best Practices

### 📱 Component Design

- **Single Responsibility**: Each component should have one clear purpose and responsibility.
- **Small, focused components**: Break large components into smaller, reusable pieces.
- **Use parameter validation**: Validate `[Parameter]` inputs using `[Required]`, `[EditorRequired]`, or custom validation.
- **Prefer composition over inheritance**: Build complex UI by composing smaller components.
- **Follow naming conventions**: Use PascalCase for component names (e.g., `ProductList.razor`).

### 🔧 Parameters and Data Binding

- **Use `[Parameter]` attributes** for component inputs and mark them as `public`.
- **Use `[EditorRequired]`** for mandatory parameters to get design-time warnings.
- **Implement `[Parameter] EventCallback<T>`** for child-to-parent communication.
- **Use two-way binding sparingly**: Prefer explicit `ValueChanged` callbacks for better control.
- **Validate parameter combinations** in `OnParametersSet()` or `SetParametersAsync()`.

```csharp
[Parameter, EditorRequired] public string Title { get; set; } = string.Empty;
[Parameter] public EventCallback<string> OnTitleChanged { get; set; }
```

### 🎣 Lifecycle Management

- **Use `OnInitializedAsync()`** for one-time initialization (API calls, subscriptions).
- **Use `OnParametersSetAsync()`** when you need to react to parameter changes.
- **Implement `IDisposable`** or `IAsyncDisposable` to clean up resources, event subscriptions, and timers.
- **Avoid heavy operations in `OnAfterRender()`** unless absolutely necessary.
- **Use `StateHasChanged()`** judiciously - only when the framework can't detect changes automatically.

### 💾 State Management

- **Use dependency injection** for shared services and state management.
- **Implement proper scoping**: Singleton for app-wide state, Scoped for user session state.
- **Use `INotifyPropertyChanged`** or reactive patterns for automatic UI updates.
- **Avoid storing state in static fields** - use DI container instead.
- **Use cascade parameters sparingly** - prefer explicit dependency injection.

```csharp
public class ProductService : INotifyPropertyChanged
{
    private List<Product> _products = new();
    public IReadOnlyList<Product> Products => _products.AsReadOnly();
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

### 🌐 JavaScript Interop

- **Minimize JS interop usage** - prefer C# solutions when possible.
- **Use `IJSRuntime` with try-catch blocks** for error handling.
- **Implement proper disposal** for JS object references using `IJSObjectReference`.
- **Use `[JSInvokable]`** methods for callbacks from JavaScript to C#.
- **Avoid frequent JS interop calls** - batch operations when possible.

### 🎨 CSS and Styling

- **Use CSS isolation** (`.razor.css` files) for component-specific styles.
- **Follow BEM methodology** or similar CSS naming conventions.
- **Use CSS custom properties (variables)** for theming and consistency.
- **Prefer CSS Grid and Flexbox** over absolute positioning.
- **Use semantic HTML elements** and proper ARIA attributes for accessibility.

### 🔒 Security Best Practices

- **Always validate user inputs** on both client and server side.
- **Use `[Authorize]` attributes** for route-level security.
- **Sanitize HTML content** when using `MarkupString` or `@((MarkupString))`.
- **Implement proper CSRF protection** for forms.
- **Use HTTPS** and secure headers in production.

### ⚡ Performance Optimization

- **Use `@key` directive** for dynamic lists to optimize rendering.
- **Implement `ShouldRender()`** to prevent unnecessary re-renders.
- **Use `@rendermode`** appropriately (Static, Interactive Server, Interactive WebAssembly, Interactive Auto).
- **Minimize component re-renders** by avoiding object creation in markup.
- **Use `Virtualization` component** for large data sets.
- **Lazy load components** using `@using` and dynamic imports.

```razor
@foreach (var item in Items)
{
    <ProductCard @key="item.Id" Product="item" />
}
```

### 🧪 Blazor Testing with bUnit

- **Use bUnit** for component integration testing.
- **Test component behavior**, not internal implementation details.
- **Mock dependencies** using services in the test context.
- **Test user interactions** using bUnit's event simulation.
- **Verify rendered output** using CSS selectors and assertions.

```csharp
[Fact]
public void ProductCard_Should_Display_Product_Name()
{
    // Arrange
    using var ctx = new TestContext();
    var product = new Product { Name = "Test Product" };
    
    // Act
    var component = ctx.RenderComponent<ProductCard>(parameters => parameters
        .Add(p => p.Product, product));
    
    // Assert
    component.Find("h3").TextContent.Should().Be("Test Product");
}
```

### 📁 Project Structure

- **Organize components** in logical folders (Components, Pages, Shared).
- **Use feature folders** for complex applications (Features/Products/, Features/Orders/).
- **Separate models and services** into appropriate projects/folders.
- **Keep razor files clean** - extract complex logic to code-behind files (.razor.cs).
- **Use shared projects** for common components across multiple Blazor apps.

---

These instructions ensure disciplined TDD practice with clean, maintainable code as the outcome.