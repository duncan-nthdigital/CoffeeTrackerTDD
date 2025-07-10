# GitHub Issues Creation Script - All Epics
# This script creates GitHub issues from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("epic-1", "epic-2", "epic-3", "all")]
    [string]$Epic = "all"
)

function New-Epic1Issues {
    Write-Host "Creating Epic 1 GitHub Issues..." -ForegroundColor Green

    # Issue #001: Coffee Entry Domain Model
    Write-Host "Creating Issue #001: Coffee Entry Domain Model" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Entry Domain Model" `
      --body-file "architecture/github-issues/Issue-001-Coffee-Entry-Domain-Model.md" `
      --label "epic-1,domain-model,high-priority,foundation" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #002: Coffee Type and Size Enumerations
    Write-Host "Creating Issue #002: Coffee Type and Size Enumerations" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Type and Size Enumerations" `
      --body-file "architecture/github-issues/Issue-002-Coffee-Type-Size-Enums.md" `
      --label "epic-1,enumeration,high-priority,foundation" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #003: Update DbContext with Coffee Models
    Write-Host "Creating Issue #003: Update DbContext with Coffee Models" -ForegroundColor Yellow
    gh issue create `
      --title "Update DbContext with Coffee Models" `
      --body-file "architecture/github-issues/Issue-003-Update-DbContext-Coffee-Models.md" `
      --label "epic-1,database,ef-core,high-priority" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #004: Create Coffee Shop Basic Model
    Write-Host "Creating Issue #004: Create Coffee Shop Basic Model" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Shop Basic Model" `
      --body-file "architecture/github-issues/Issue-004-Coffee-Shop-Basic-Model.md" `
      --label "epic-1,domain-model,medium-priority,phase-1" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #005: Create Database Migration and Schema Update
    Write-Host "Creating Issue #005: Create Database Migration and Schema Update" -ForegroundColor Yellow
    gh issue create `
      --title "Create Database Migration and Schema Update" `
      --body-file "architecture/github-issues/Issue-005-Database-Migration.md" `
      --label "epic-1,database,ef-migration,high-priority" `
      --milestone "Phase 1 - Anonymous User MVP"

    Write-Host "Epic 1 issues created successfully!" -ForegroundColor Green
}

function New-Epic2Issues {
    Write-Host "Creating Epic 2 GitHub Issues - Coffee Logging API Endpoints..." -ForegroundColor Green

    # Issue #006: Create Coffee Logging Controller
    Write-Host "Creating Issue #006: Create Coffee Logging Controller" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Logging Controller" `
      --body-file "architecture/github-issues/Issue-006-Coffee-Logging-Controller.md" `
      --label "epic-2,api-controller,high-priority,rest-api" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #007: Create Coffee Service Layer
    Write-Host "Creating Issue #007: Create Coffee Service Layer" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Service Layer" `
      --body-file "architecture/github-issues/Issue-007-Coffee-Service-Layer.md" `
      --label "epic-2,service-layer,high-priority,business-logic" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #008: Create Request/Response DTOs
    Write-Host "Creating Issue #008: Create Request/Response DTOs" -ForegroundColor Yellow
    gh issue create `
      --title "Create Request/Response DTOs" `
      --body-file "architecture/github-issues/Issue-008-Request-Response-DTOs.md" `
      --label "epic-2,dto,high-priority,api-contract" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #009: Add Anonymous Session Management
    Write-Host "Creating Issue #009: Add Anonymous Session Management" -ForegroundColor Yellow
    gh issue create `
      --title "Add Anonymous Session Management" `
      --body-file "architecture/github-issues/Issue-009-Anonymous-Session-Management.md" `
      --label "epic-2,session-management,high-priority,security" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #010: Add Data Validation and Error Handling
    Write-Host "Creating Issue #010: Add Data Validation and Error Handling" -ForegroundColor Yellow
    gh issue create `
      --title "Add Data Validation and Error Handling" `
      --body-file "architecture/github-issues/Issue-010-Data-Validation-Error-Handling.md" `
      --label "epic-2,validation,medium-priority,error-handling" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #011: Integration Tests for API Endpoints
    Write-Host "Creating Issue #011: Integration Tests for API Endpoints" -ForegroundColor Yellow
    gh issue create `
      --title "Integration Tests for API Endpoints" `
      --body-file "architecture/github-issues/Issue-011-Integration-Tests-API-Endpoints.md" `
      --label "epic-2,integration-tests,medium-priority,testing" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #012: Configure Swagger/OpenAPI Documentation
    Write-Host "Creating Issue #012: Configure Swagger/OpenAPI Documentation" -ForegroundColor Yellow
    gh issue create `
      --title "Configure Swagger/OpenAPI Documentation" `
      --body-file "architecture/github-issues/Issue-012-Configure-Swagger-OpenAPI.md" `
      --label "epic-2,swagger,documentation,high-priority,infrastructure" `
      --milestone "Phase 1 - Anonymous User MVP"

    Write-Host "Epic 2 issues created successfully!" -ForegroundColor Green
}

function New-Epic3Issues {
    Write-Host "Creating Epic 3 GitHub Issues - Coffee Shop Data & Locator API..." -ForegroundColor Green

    # Issue #013: Create Coffee Shop API Controller
    Write-Host "Creating Issue #013: Create Coffee Shop API Controller" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Shop API Controller" `
      --body-file "architecture/github-issues/Issue-013-Coffee-Shop-API-Controller.md" `
      --label "epic-3,api-controller,high-priority,swagger,documentation" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #014: Create Coffee Shop Service Layer
    Write-Host "Creating Issue #014: Create Coffee Shop Service Layer" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Shop Service Layer" `
      --body-file "architecture/github-issues/Issue-014-Coffee-Shop-Service-Layer.md" `
      --label "epic-3,service-layer,high-priority,caching" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #015: Create Coffee Shop Seed Data
    Write-Host "Creating Issue #015: Create Coffee Shop Seed Data" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Shop Seed Data" `
      --body-file "architecture/github-issues/Issue-015-Coffee-Shop-Seed-Data.md" `
      --label "epic-3,data,medium-priority,testing" `
      --milestone "Phase 1 - Anonymous User MVP"

    # Issue #016: Add Geographic Location Support
    Write-Host "Creating Issue #016: Add Geographic Location Support" -ForegroundColor Yellow
    gh issue create `
      --title "Add Geographic Location Support" `
      --body-file "architecture/github-issues/Issue-016-Geographic-Location-Support.md" `
      --label "epic-3,enhancement,low-priority,location,swagger" `
      --milestone "Phase 2 - Enhanced Features"

    # Issue #017: Create Coffee Shop Integration Tests
    Write-Host "Creating Issue #017: Create Coffee Shop Integration Tests" -ForegroundColor Yellow
    gh issue create `
      --title "Create Coffee Shop Integration Tests" `
      --body-file "architecture/github-issues/Issue-017-Coffee-Shop-Integration-Tests.md" `
      --label "epic-3,integration-tests,medium-priority,swagger" `
      --milestone "Phase 1 - Anonymous User MVP"

    Write-Host "Epic 3 issues created successfully!" -ForegroundColor Green
}

# Main execution
switch ($Epic) {
    "epic-1" {
        New-Epic1Issues
        Write-Host "Epic 1 (5 issues) created successfully!" -ForegroundColor Cyan
    }
    "epic-2" {
        New-Epic2Issues
        Write-Host "Epic 2 (7 issues) created successfully!" -ForegroundColor Cyan
    }
    "epic-3" {
        New-Epic3Issues
        Write-Host "Epic 3 (5 issues) created successfully!" -ForegroundColor Cyan
    }
    "all" {
        New-Epic1Issues
        Write-Host "" # Empty line for separation
        New-Epic2Issues
        Write-Host "" # Empty line for separation
        New-Epic3Issues
        Write-Host "All GitHub issues created successfully!" -ForegroundColor Cyan
        Write-Host "Epic 1: 5 issues (Domain Models & Database)" -ForegroundColor Cyan
        Write-Host "Epic 2: 7 issues (Coffee Logging API)" -ForegroundColor Cyan
        Write-Host "Epic 3: 5 issues (Coffee Shop Data & Locator API)" -ForegroundColor Cyan
        Write-Host "Total: 17 issues created" -ForegroundColor Cyan
    }
}

Write-Host "Visit your GitHub repository to view and organize the issues." -ForegroundColor Cyan

# Usage examples:
Write-Host ""
Write-Host "Usage examples:" -ForegroundColor Gray
Write-Host "  .\create-all-github-issues.ps1              # Create all issues" -ForegroundColor Gray
Write-Host "  .\create-all-github-issues.ps1 -Epic epic-1 # Create only Epic 1 issues" -ForegroundColor Gray
Write-Host "  .\create-all-github-issues.ps1 -Epic epic-2 # Create only Epic 2 issues" -ForegroundColor Gray
Write-Host "  .\create-all-github-issues.ps1 -Epic epic-3 # Create only Epic 3 issues" -ForegroundColor Gray
