# GitHub Issue Creation Scripts

This directory contains scripts to automatically create GitHub issues for the Coffee Tracker TDD project using the GitHub CLI.

## Prerequisites

1. **GitHub CLI**: Install the GitHub CLI tool
   - Windows: `winget install GitHub.cli` or download from https://cli.github.com/
   - macOS: `brew install gh`
   - Linux: Follow instructions at https://github.com/cli/cli/blob/trunk/docs/install_linux.md

2. **Authentication**: Authenticate with your GitHub account
   ```bash
   gh auth login
   ```
   Follow the prompts to authenticate with your GitHub account.

## Available Scripts

### Individual Epic Scripts

#### Epic 1 - Domain Models & Database
- **PowerShell**: `create-github-issues.ps1`
- **Bash**: `create-github-issues.sh`
- **Issues Created**: 5 issues (#001-#005)
- **Focus**: Core domain models and database setup

#### Epic 2 - Coffee Logging API
- **PowerShell**: `create-epic-2-github-issues.ps1`
- **Bash**: `create-epic-2-github-issues.sh`
- **Issues Created**: 7 issues (#006-#012)
- **Focus**: REST API endpoints for coffee logging with Swagger documentation

#### Epic 3 - Coffee Shop Data & Locator API
- **PowerShell**: `create-epic-3-github-issues.ps1`
- **Bash**: `create-epic-3-github-issues.sh`
- **Issues Created**: 5 issues (#013-#017)
- **Focus**: Coffee shop data management and locator functionality with Swagger documentation

### Combined Script

#### All Epics
- **PowerShell**: `create-all-github-issues.ps1`
- **Supports**: Selective epic creation or all at once

### Single Issue Script

#### Individual Issues
- **PowerShell**: `create-single-issue.ps1`
- **Bash**: `create-single-issue.sh`
- **Supports**: Creating a single specific issue by number

## Usage Examples

### PowerShell (Windows)

```powershell
# Navigate to the project root
cd CoffeeTrackerTDD

# Create Epic 1 issues only
.\scripts\create-github-issues.ps1

# Create Epic 2 issues only
.\scripts\create-epic-2-github-issues.ps1

# Create Epic 3 issues only
.\scripts\create-epic-3-github-issues.ps1

# Create all issues at once
.\scripts\create-all-github-issues.ps1

# Create specific epic with the combined script
.\scripts\create-all-github-issues.ps1 -Epic epic-1
.\scripts\create-all-github-issues.ps1 -Epic epic-2
.\scripts\create-all-github-issues.ps1 -Epic epic-3
.\scripts\create-all-github-issues.ps1 -Epic all

# Create a single specific issue
.\scripts\create-single-issue.ps1 -IssueNumber 12  # Create issue #012 (Swagger/OpenAPI Documentation)
```

### Bash (Linux/macOS)

```bash
# Navigate to the project root
cd CoffeeTrackerTDD

# Make scripts executable (one time only)
chmod +x scripts/*.sh

# Create Epic 1 issues only
./scripts/create-github-issues.sh

# Create Epic 2 issues only
./scripts/create-epic-2-github-issues.sh

# Create Epic 3 issues only
./scripts/create-epic-3-github-issues.sh

# Create a single specific issue
./scripts/create-single-issue.sh 12  # Create issue #012 (Swagger/OpenAPI Documentation)
```

## Issue Structure

Each script creates issues with the following structure:

### Epic 1 Issues
1. **Issue #001**: Create Coffee Entry Domain Model
2. **Issue #002**: Create Coffee Type and Size Enumerations
3. **Issue #003**: Update DbContext with Coffee Models
4. **Issue #004**: Create Coffee Shop Basic Model
5. **Issue #005**: Create Database Migration and Schema Update

### Epic 2 Issues
6. **Issue #006**: Create Coffee Logging Controller
7. **Issue #007**: Create Coffee Service Layer
8. **Issue #008**: Create Request/Response DTOs
9. **Issue #009**: Add Anonymous Session Management
10. **Issue #010**: Add Data Validation and Error Handling
11. **Issue #011**: Integration Tests for API Endpoints
12. **Issue #012**: Configure Swagger/OpenAPI Documentation

### Epic 3 Issues
13. **Issue #013**: Create Coffee Shop API Controller
14. **Issue #014**: Create Coffee Shop Service Layer
15. **Issue #015**: Create Coffee Shop Seed Data
16. **Issue #016**: Add Geographic Location Support
17. **Issue #017**: Create Coffee Shop Integration Tests

## Labels and Organization

Issues are automatically created with:
- **Epic labels**: `epic-1`, `epic-2`, `epic-3`
- **Priority labels**: `high-priority`, `medium-priority`, `low-priority`
- **Component labels**: `domain-model`, `api-controller`, `testing`, `swagger`, `documentation`, etc.
- **Milestone**: "Phase 1 - Anonymous User MVP" or "Phase 2 - Enhanced Features"

## Troubleshooting

### Common Issues

1. **GitHub CLI not authenticated**
   ```
   Error: authentication required
   ```
   **Solution**: Run `gh auth login` and follow the prompts.

2. **Permission denied (Bash scripts)**
   ```
   Permission denied: ./scripts/create-github-issues.sh
   ```
   **Solution**: Make the script executable: `chmod +x scripts/*.sh`
   
3. **Invalid issue number**
   ```
   Error: Issue number must be between 1 and 17
   ```
   **Solution**: Specify a valid issue number between 1 and 17.

3. **Repository not found**
   ```
   Error: repository not found
   ```
   **Solution**: Ensure you're in the correct repository directory and have push access.

4. **Issue already exists**
   ```
   Error: issue with this title already exists
   ```
   **Solution**: The issues have already been created. Check your GitHub repository.

### Verification

After running the scripts, verify the issues were created:

1. **Via GitHub CLI**:
   ```bash
   gh issue list --label epic-1
   gh issue list --label epic-2
   gh issue list --label epic-3
   ```

2. **Via GitHub Web Interface**:
   - Navigate to your repository on GitHub.com
   - Click on the "Issues" tab
   - Filter by labels: `epic-1`, `epic-2`, `epic-3`

## File Locations

All issue markdown files are located in:
```
architecture/github-issues/
├── Epic-1-Issues-Index.md          # Epic 1 overview
├── Epic-2-Issues-Index.md          # Epic 2 overview
├── Epic-3-Issues-Index.md          # Epic 3 overview
├── Issue-001-Coffee-Entry-Domain-Model.md
├── Issue-002-Coffee-Type-Size-Enums.md
├── Issue-003-Update-DbContext-Coffee-Models.md
├── Issue-004-Coffee-Shop-Basic-Model.md
├── Issue-005-Database-Migration.md
├── Issue-006-Coffee-Logging-Controller.md
├── Issue-007-Coffee-Service-Layer.md
├── Issue-008-Request-Response-DTOs.md
├── Issue-009-Anonymous-Session-Management.md
├── Issue-010-Data-Validation-Error-Handling.md
├── Issue-011-Integration-Tests-API-Endpoints.md
├── Issue-012-Configure-Swagger-OpenAPI.md
├── Issue-013-Coffee-Shop-API-Controller.md
├── Issue-014-Coffee-Shop-Service-Layer.md
├── Issue-015-Coffee-Shop-Seed-Data.md
├── Issue-016-Geographic-Location-Support.md
└── Issue-017-Coffee-Shop-Integration-Tests.md
```

## Customization

To customize the issues:

1. **Edit the markdown files** in `architecture/github-issues/`
2. **Modify the scripts** to change labels, milestones, or titles
3. **Add new issues** by creating new markdown files and updating the scripts

## Next Steps

After creating the issues:

1. **Review and organize** the issues in your GitHub repository
2. **Assign issues** to team members
3. **Start development** following the TDD approach outlined in each issue
4. **Track progress** using GitHub's project boards or milestones

---

**Note**: These scripts are designed for the Coffee Tracker TDD project structure. Adapt as needed for other projects.
