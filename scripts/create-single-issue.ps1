# GitHub Issues Creation Script - Single Issue
# This script creates a single GitHub issue from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

param(
    [Parameter(Mandatory=$true)]
    [ValidateRange(1, 17)]
    [int]$IssueNumber
)

function Get-EpicNumber {
    param (
        [int]$IssueNumber
    )
    
    if ($IssueNumber -ge 1 -and $IssueNumber -le 5) {
        return 1
    } elseif ($IssueNumber -ge 6 -and $IssueNumber -le 12) {
        return 2
    } elseif ($IssueNumber -ge 13 -and $IssueNumber -le 17) {
        return 3
    } else {
        throw "Invalid issue number: $IssueNumber"
    }
}

function Get-IssuePadded {
    param (
        [int]$IssueNumber
    )
    
    return $IssueNumber.ToString("000")
}

function New-SingleIssue {
    param (
        [int]$IssueNumber
    )
    
    $epicNumber = Get-EpicNumber -IssueNumber $IssueNumber
    $issuePadded = Get-IssuePadded -IssueNumber $IssueNumber
    
    Write-Host "Creating Issue #$issuePadded (Epic $epicNumber)..." -ForegroundColor Yellow
    
    $issueDetails = Get-IssueDetails -IssueNumber $IssueNumber
    
    if ($issueDetails) {
        gh issue create `
          --title $issueDetails.Title `
          --body-file $issueDetails.BodyFile `
          --label $issueDetails.Labels `
          --milestone $issueDetails.Milestone
        
        Write-Host "Issue #$issuePadded created successfully!" -ForegroundColor Green
    } else {
        Write-Host "Failed to create Issue #$($issuePadded): Details not found" -ForegroundColor Red
    }
}

function Get-IssueDetails {
    param (
        [int]$IssueNumber
    )
    
    # Define all issue details
    $issueDetailsMap = @{
        # Epic 1 Issues
        1 = @{
            Title = "Create Coffee Entry Domain Model"
            BodyFile = "architecture/github-issues/Issue-001-Coffee-Entry-Domain-Model.md"
            Labels = "epic-1,domain-model,high-priority,foundation"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        2 = @{
            Title = "Create Coffee Type and Size Enumerations"
            BodyFile = "architecture/github-issues/Issue-002-Coffee-Type-Size-Enums.md"
            Labels = "epic-1,enumeration,high-priority,foundation"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        3 = @{
            Title = "Update DbContext with Coffee Models"
            BodyFile = "architecture/github-issues/Issue-003-Update-DbContext-Coffee-Models.md"
            Labels = "epic-1,database,ef-core,high-priority"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        4 = @{
            Title = "Create Coffee Shop Basic Model"
            BodyFile = "architecture/github-issues/Issue-004-Coffee-Shop-Basic-Model.md"
            Labels = "epic-1,domain-model,medium-priority,phase-1"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        5 = @{
            Title = "Create Database Migration and Schema Update"
            BodyFile = "architecture/github-issues/Issue-005-Database-Migration.md"
            Labels = "epic-1,database,ef-migration,high-priority"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        
        # Epic 2 Issues
        6 = @{
            Title = "Create Coffee Logging Controller"
            BodyFile = "architecture/github-issues/Issue-006-Coffee-Logging-Controller.md"
            Labels = "epic-2,api-controller,high-priority,rest-api"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        7 = @{
            Title = "Create Coffee Service Layer"
            BodyFile = "architecture/github-issues/Issue-007-Coffee-Service-Layer.md"
            Labels = "epic-2,service-layer,high-priority,business-logic"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        8 = @{
            Title = "Create Request/Response DTOs"
            BodyFile = "architecture/github-issues/Issue-008-Request-Response-DTOs.md"
            Labels = "epic-2,dto,high-priority,api-contract"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        9 = @{
            Title = "Add Anonymous Session Management"
            BodyFile = "architecture/github-issues/Issue-009-Anonymous-Session-Management.md"
            Labels = "epic-2,session-management,high-priority,security"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        10 = @{
            Title = "Add Data Validation and Error Handling"
            BodyFile = "architecture/github-issues/Issue-010-Data-Validation-Error-Handling.md"
            Labels = "epic-2,validation,medium-priority,error-handling"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        11 = @{
            Title = "Integration Tests for API Endpoints"
            BodyFile = "architecture/github-issues/Issue-011-Integration-Tests-API-Endpoints.md"
            Labels = "epic-2,integration-tests,medium-priority,testing"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        12 = @{
            Title = "Configure Swagger/OpenAPI Documentation"
            BodyFile = "architecture/github-issues/Issue-012-Configure-Swagger-OpenAPI.md"
            Labels = "epic-2,swagger,documentation,high-priority,infrastructure"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        
        # Epic 3 Issues
        13 = @{
            Title = "Create Coffee Shop API Controller"
            BodyFile = "architecture/github-issues/Issue-013-Coffee-Shop-API-Controller.md"
            Labels = "epic-3,api-controller,high-priority,swagger,documentation"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        14 = @{
            Title = "Create Coffee Shop Service Layer"
            BodyFile = "architecture/github-issues/Issue-014-Coffee-Shop-Service-Layer.md"
            Labels = "epic-3,service-layer,high-priority,caching"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        15 = @{
            Title = "Create Coffee Shop Seed Data"
            BodyFile = "architecture/github-issues/Issue-015-Coffee-Shop-Seed-Data.md"
            Labels = "epic-3,data,medium-priority,testing"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
        16 = @{
            Title = "Add Geographic Location Support"
            BodyFile = "architecture/github-issues/Issue-016-Geographic-Location-Support.md"
            Labels = "epic-3,enhancement,low-priority,location,swagger"
            Milestone = "Phase 2 - Enhanced Features"
        }
        17 = @{
            Title = "Create Coffee Shop Integration Tests"
            BodyFile = "architecture/github-issues/Issue-017-Coffee-Shop-Integration-Tests.md"
            Labels = "epic-3,integration-tests,medium-priority,swagger"
            Milestone = "Phase 1 - Anonymous User MVP"
        }
    }
    
    return $issueDetailsMap[$IssueNumber]
}

# Main execution
New-SingleIssue -IssueNumber $IssueNumber

# Usage examples
Write-Host ""
Write-Host "Usage examples:" -ForegroundColor Gray
Write-Host "  .\create-single-issue.ps1 -IssueNumber 12  # Create issue #012 (Swagger/OpenAPI Documentation)" -ForegroundColor Gray
Write-Host "  .\create-single-issue.ps1 -IssueNumber 6   # Create issue #006 (Coffee Logging Controller)" -ForegroundColor Gray
