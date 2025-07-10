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

### Core Application Features
- **Anonymous Coffee Tracking** - MVP approach for rapid user feedback
- **Blazor Interactive Auto** render mode for optimal user experience
- **Entity Framework Core** with SQLite database
- **Auth0 authentication** for secure user management (Phase 2)
- **JWT Bearer authentication** for API security
- **CORS configuration** for cross-origin requests
- **.NET Aspire orchestration** for local development

### AI-Enhanced Development Features
- **📝 GitHub Issues with AI Prompts** - Ready-to-use prompts for each development task
- **🤖 Copilot-Optimized Code Structure** - Organized for maximum AI assistance
- **📊 Epic-Based Development** - Clear progression from MVP to full features
- **🧪 TDD-First Approach** - Tests written before implementation using AI
- **📋 Acceptance Criteria** - Clear, AI-understandable requirements for each task

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

## 📋 AI-Powered TDD Workflow

This project demonstrates a comprehensive **AI-assisted Test-Driven Development** approach:

### 🎯 Development Process

1. **📖 Epic Planning**: Features broken into AI-friendly GitHub issues
2. **🤖 AI Prompt Execution**: Copy-paste prompts for consistent implementation  
3. **🔴 Red Phase**: AI writes failing tests based on acceptance criteria
4. **🟢 Green Phase**: AI implements minimal code to pass tests
5. **🔄 Refactor Phase**: AI suggests improvements while keeping tests green
6. **✅ Validation**: Ensure all acceptance criteria are met

### 🛠️ AI Development Tools

- **GitHub Copilot**: Primary AI pair programming assistant
- **AI-Optimized Prompts**: Detailed prompts in each GitHub issue
- **Context-Rich Issues**: Complete information for AI understanding
- **Test-First Mentality**: All features start with comprehensive tests
- **Progressive Enhancement**: Build from simple to complex features

### 📊 Project Structure for AI

```
📁 architecture/
├── 📄 Initial-PRD.md                    # Product requirements
├── 📄 Development-Plan.md               # Epic overview and progress
├── 📄 Epic-1-Domain-Models.md          # Detailed epic with AI prompts
└── 📁 github-issues/                   # Individual GitHub issues
    ├── 📄 Issue-001-Coffee-Entry-Domain-Model.md
    ├── 📄 Issue-002-Coffee-Type-Size-Enums.md
    └── 📄 [5 issues total with AI prompts]
```

### TDD Guidelines

This project follows Test-Driven Development principles with AI enhancement:

1. **Red**: Write a failing test first (AI assists with test generation)
2. **Green**: Write minimal code to make the test pass (AI suggests implementation)
3. **Refactor**: Improve code while keeping tests green (AI suggests optimizations)

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

## 📁 AI-Enhanced Development Workflow

1. **📋 Epic-Driven Development**: Features organized as GitHub issues with AI prompts
2. **🤖 AI-First Implementation**: Write failing tests and implementation using AI
3. **🔄 Iterative Refinement**: Refactor with AI suggestions while maintaining tests
4. **✅ Acceptance-Driven**: Clear criteria for AI understanding and validation
5. **📊 Progress Tracking**: Visual progress through epic completion
6. **🚀 Rapid Deployment**: Anonymous MVP approach for fast user feedback

## 🤝 Contributing with AI

When contributing to this project:

1. **🧪 Follow TDD practices** with AI assistance
2. **🤖 Use provided AI prompts** in GitHub issues
3. **✅ Ensure all tests pass** before submission
4. **📊 Maintain code coverage** (90%+ target)
5. **📋 Follow established patterns** for AI consistency
6. **📝 Update documentation** and acceptance criteria

## 🔗 AI Development Resources

- **📋 Development Plan**: `architecture/Development-Plan.md`
- **🎯 Epic Files**: `architecture/Epic-*.md` (detailed AI prompts)
- **🎫 GitHub Issues**: `architecture/github-issues/` (copy-paste ready)
- **🤖 AI Prompts**: Embedded in each issue for immediate use
- **📊 Progress Tracking**: Visual checkboxes in development plan

---

**🚀 Ready to build with AI + TDD!** Start with Epic 1, Issue #001 and let AI accelerate your development! 🤖☕

## 🎯 Getting Started with AI + TDD

### Quick Start (AI-Assisted Development)

1. **📋 Review the Development Plan**: Check `architecture/Development-Plan.md`
2. **🎫 Start with Issue #001**: Open `architecture/github-issues/Issue-001-Coffee-Entry-Domain-Model.md`
3. **🤖 Copy the AI Prompt**: Use the detailed prompt with GitHub Copilot
4. **🔴 Write Failing Tests**: Let AI help generate comprehensive tests
5. **🟢 Implement Code**: AI assists with minimal implementation
6. **✅ Validate**: Ensure all acceptance criteria are met

### Next Steps for AI Development

With the AI-ready structure in place, you can:

1. **🎯 Follow Epic 1**: Start with domain models using AI prompts
2. **📝 Use GitHub Issues**: Each issue has complete context for AI
3. **🤖 Leverage AI Assistance**: Prompts optimized for Copilot
4. **🧪 Maintain TDD**: AI helps write tests first, then implementation
5. **📊 Track Progress**: Check off tasks as you complete them
6. **🚀 Deploy MVP**: Anonymous user features ready for production in 2-3 weeks

### 🤖 AI Development Workflow

```
📖 Read Issue → 🤖 Copy AI Prompt → 🔴 Write Tests → 🟢 Implement → ✅ Validate
     ↓              ↓                ↓              ↓             ↓
Epic File → GitHub Copilot → TDD Red Phase → TDD Green → Acceptance Criteria
```

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

## 🤝 Contributing with AI

When contributing to this project:

1. **🧪 Follow TDD practices** with AI assistance
2. **🤖 Use provided AI prompts** in GitHub issues
3. **✅ Ensure all tests pass** before submission
4. **📊 Maintain code coverage** (90%+ target)
5. **📋 Follow established patterns** for AI consistency
6. **📝 Update documentation** and acceptance criteria

## 🔗 AI Development Resources

- **📋 Development Plan**: `architecture/Development-Plan.md`
- **🎯 Epic Files**: `architecture/Epic-*.md` (detailed AI prompts)
- **🎫 GitHub Issues**: `architecture/github-issues/` (copy-paste ready)
- **🤖 AI Prompts**: Embedded in each issue for immediate use
- **📊 Progress Tracking**: Visual checkboxes in development plan

---

**🚀 Ready to build with AI + TDD!** Start with Epic 1, Issue #001 and let AI accelerate your development! 🤖☕
