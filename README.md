# Coffee Tracker TDD - AI-Powered Development

A .NET Aspire application with Blazor frontend and Web API backend, demonstrating **Test-Driven Development (TDD) with AI collaboration**. This project showcases how GitHub Copilot and AI assistants can accelerate TDD practices while maintaining code quality and comprehensive testing.

## 🤖 AI + TDD Approach

This project is built using a **human-AI collaborative development workflow**:

- **🔴 Red Phase**: AI helps write comprehensive failing tests based on requirements
- **🟢 Green Phase**: AI assists in implementing minimal code to pass tests  
- **🔄 Refactor Phase**: AI suggests improvements while maintaining test coverage
- **📋 Issue-Driven**: Each feature has detailed GitHub issues with AI-ready prompts
- **🎯 Prompt Engineering**: Optimized prompts for consistent, high-quality AI assistance

### Why AI + TDD?

- **Faster Test Creation**: AI generates comprehensive test suites quickly
- **Better Coverage**: AI suggests edge cases and test scenarios humans might miss
- **Consistent Quality**: AI follows established patterns and coding standards
- **Rapid Iteration**: Quick feedback loops between tests and implementation
- **Knowledge Transfer**: AI prompts serve as documentation and learning tools

## 🏗️ Architecture

```
CoffeeTrackerTDD/
├── infrastructure/                     # Aspire orchestration
│   ├── CoffeeTracker.AppHost/          # Aspire application host
│   └── CoffeeTracker.ServiceDefaults/  # Shared service configurations
├── src/                                # Production code
│   ├── CoffeeTracker.Api/              # Web API backend
│   └── CoffeeTracker.Web/              # Blazor web application
│       ├── CoffeeTracker.Web/          # Server-side Blazor components
│       └── CoffeeTracker.Web.Client/   # Client-side Blazor components
├── test/                               # Test projects
│   ├── CoffeeTracker.Api.Tests/        # API unit tests
│   └── CoffeeTracker.Web.Tests/        # Web/Blazor component tests
├── data/                               # SQLite database location
└── README.md
```

## 🚀 Features

- **Blazor Interactive Auto** render mode for optimal user experience
- **Entity Framework Core** with SQLite database
- **Auth0 authentication** for secure user management
- **JWT Bearer authentication** for API security
- **CORS configuration** for cross-origin requests
- **Comprehensive testing** with xUnit, bUnit, and ASP.NET Core TestHost
- **.NET Aspire orchestration** for local development

## 🛠️ Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [.NET Aspire workload](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling)
- [Auth0 account](https://auth0.com/) (for authentication setup)

Install .NET Aspire workload:
```bash
dotnet workload update
dotnet workload install aspire
```

## ⚙️ Configuration

### Auth0 Setup

1. Create an Auth0 application at [auth0.com](https://auth0.com/)
2. Configure the following settings:

#### For the Web Application (src/CoffeeTracker.Web/appsettings.json):
```json
{
  "Auth0": {
    "Domain": "your-auth0-domain.auth0.com",
    "ClientId": "your-auth0-client-id",
    "ClientSecret": "your-auth0-client-secret"
  }
}
```

#### For the API Application (src/CoffeeTracker.Api/appsettings.json):
```json
{
  "Auth0": {
    "Domain": "https://your-auth0-domain.auth0.com/",
    "Audience": "your-api-identifier"
  }
}
```

3. In Auth0 Dashboard:
   - Set **Allowed Callback URLs**: `https://localhost:7001/callback`
   - Set **Allowed Logout URLs**: `https://localhost:7001/`
   - Set **Allowed Web Origins**: `https://localhost:7001`

### Database Configuration

The SQLite database will be automatically created in the `data/` directory when the application starts.

## 🏃‍♂️ Getting Started

### 1. Clone and Build
```bash
git clone <repository-url>
cd CoffeeTrackerTDD
dotnet restore
dotnet build
```

### 2. Run Tests
```bash
dotnet test
```

### 3. Run the Application

#### Using .NET Aspire (Recommended)
```bash
dotnet run --project infrastructure/CoffeeTracker.AppHost
```

This will:
- Start the Aspire dashboard at `https://localhost:15888`
- Launch the Web application at `https://localhost:7001`
- Launch the API at `https://localhost:7002`

#### Running Individual Projects
```bash
# Terminal 1 - API
dotnet run --project src/CoffeeTracker.Api

# Terminal 2 - Web App
dotnet run --project src/CoffeeTracker.Web/CoffeeTracker.Web
```

## 🧪 Testing

### Running All Tests
```bash
dotnet test
```

### Running Specific Test Projects
```bash
# API Tests
dotnet test test/CoffeeTracker.Api.Tests

# Web Tests  
dotnet test test/CoffeeTracker.Web.Tests
```

### Test Structure
- **API Tests**: Use `Microsoft.AspNetCore.Mvc.Testing` for integration testing
- **Web Tests**: Use `bUnit` for Blazor component testing
- **In-Memory Database**: Entity Framework Core InMemory provider for testing

## 📋 TDD Guidelines

This project follows Test-Driven Development principles:

1. **Red**: Write a failing test first
2. **Green**: Write minimal code to make the test pass
3. **Refactor**: Improve code while keeping tests green

### Key Testing Tools
- **xUnit**: Unit testing framework
- **bUnit**: Blazor component testing
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing for APIs
- **Microsoft.EntityFrameworkCore.InMemory**: In-memory database for testing
- **Moq**: Mocking framework (add as needed)

## 🔒 Security Features

- **Auth0 Integration**: Complete authentication flow
- **JWT Bearer Tokens**: API authentication
- **CORS Policy**: Configured for cross-origin requests
- **Authorization Attributes**: Ready for role-based access control

## 📁 Development Workflow

1. **Write failing tests** for new features
2. **Implement minimal code** to pass tests
3. **Refactor** while maintaining green tests
4. **Follow SOLID principles** and clean code practices
5. **Use Aspire** for local development orchestration

## 🎯 Next Steps

With the basic structure in place, you can now:

1. **Add domain models** and corresponding tests
2. **Implement business logic** following TDD practices
3. **Create API endpoints** with proper authentication
4. **Build Blazor components** with comprehensive testing
5. **Extend the database schema** as needed
6. **Add more sophisticated Auth0 features** (roles, permissions)

## 📚 Useful Commands

```bash
# Build solution
dotnet build

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Add new packages
dotnet add package <PackageName>

# Entity Framework migrations (when ready)
dotnet ef migrations add InitialCreate --project src/CoffeeTracker.Api
dotnet ef database update --project src/CoffeeTracker.Api

# Clean and rebuild
dotnet clean && dotnet build
```

## 🤝 Contributing

When contributing to this project:

1. Follow TDD practices
2. Ensure all tests pass
3. Maintain code coverage
4. Follow the established project structure
5. Update documentation as needed

---

Happy coding with TDD! 🚀
