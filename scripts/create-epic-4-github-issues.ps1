# GitHub Issues Creation Script for Epic 4
# This script creates GitHub issues from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

Write-Host "Creating Epic 4 GitHub Issues - Blazor UI Components for Coffee Logging..." -ForegroundColor Green

# Ensure required labels exist
Write-Host "Checking and creating required labels..." -ForegroundColor Cyan
$requiredLabels = @(
    @{Name="epic-4"; Description="Epic 4: Blazor UI Components for Coffee Logging"; Color="6f42c1"},
    @{Name="blazor-component"; Description="Blazor component development"; Color="20232a"},
    @{Name="ui-form"; Description="User interface forms and input components"; Color="28a745"},
    @{Name="ui-dashboard"; Description="Dashboard and summary UI components"; Color="17a2b8"},
    @{Name="ui-selector"; Description="Selection and picker UI components"; Color="fd7e14"},
    @{Name="ui-integration"; Description="UI component integration and page composition"; Color="6610f2"},
    @{Name="blazor-page"; Description="Blazor page components and routing"; Color="e83e8c"},
    @{Name="reusable"; Description="Reusable components and utilities"; Color="20c997"},
    @{Name="ux-enhancement"; Description="User experience improvements and polish"; Color="ffc107"},
    @{Name="animations"; Description="CSS animations and transitions"; Color="6c757d"},
    @{Name="bunit-integration"; Description="bUnit integration testing for Blazor"; Color="dc3545"},
    @{Name="component-testing"; Description="Component-level testing"; Color="fd7e14"},
    @{Name="analytics"; Description="Analytics and reporting features"; Color="28a745"},
    @{Name="routing"; Description="Page routing and navigation"; Color="6610f2"}
)

foreach ($label in $requiredLabels) {
    try {
        gh label create $label.Name --description $label.Description --color $label.Color 2>$null
        Write-Host "  ✓ Created label: $($label.Name)" -ForegroundColor Green
    } catch {
        # Label might already exist, which is fine
        Write-Host "  - Label already exists: $($label.Name)" -ForegroundColor Yellow
    }
}

# Ensure required milestone exists
Write-Host "Checking milestone..." -ForegroundColor Cyan
try {
    $repoInfo = gh repo view --json owner,name | ConvertFrom-Json
    $repoFullName = "$($repoInfo.owner.login)/$($repoInfo.name)"
    $milestones = gh api "repos/$repoFullName/milestones" | ConvertFrom-Json
    $phaseOneMilestone = $milestones | Where-Object { $_.title -eq "Phase 1 - Anonymous User MVP" }
    if (-not $phaseOneMilestone) {
        Write-Host "  ✓ Creating milestone: Phase 1 - Anonymous User MVP" -ForegroundColor Green
        gh api "repos/$repoFullName/milestones" -f title="Phase 1 - Anonymous User MVP" -f description="Phase 1 MVP for anonymous coffee tracking" | Out-Null
    } else {
        Write-Host "  - Milestone already exists: Phase 1 - Anonymous User MVP" -ForegroundColor Yellow
    }
} catch {
    Write-Host "  - Milestone check skipped (likely already exists)" -ForegroundColor Yellow
}

Write-Host "`nCreating issues..." -ForegroundColor Cyan

# Issue #018: Create Coffee Logging Form Component
Write-Host "Creating Issue #018: Create Coffee Logging Form Component" -ForegroundColor Yellow
gh issue create `
  --title "Create Coffee Logging Form Component" `
  --body-file "architecture/github-issues/Issue-018-Coffee-Logging-Form-Component.md" `
  --label "epic-4,blazor-component,high-priority,ui-form,validation" `
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #019: Create Daily Coffee Summary Component
Write-Host "Creating Issue #019: Create Daily Coffee Summary Component" -ForegroundColor Yellow
gh issue create `
  --title "Create Daily Coffee Summary Component" `
  --body-file "architecture/github-issues/Issue-019-Daily-Coffee-Summary-Component.md" `
  --label "epic-4,blazor-component,high-priority,ui-dashboard,analytics" `
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #020: Create Coffee Type Selection Component
Write-Host "Creating Issue #020: Create Coffee Type Selection Component" -ForegroundColor Yellow
gh issue create `
  --title "Create Coffee Type Selection Component" `
  --body-file "architecture/github-issues/Issue-020-Coffee-Type-Selection-Component.md" `
  --label "epic-4,blazor-component,medium-priority,ui-selector,reusable" `
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #021: Create Main Coffee Tracking Page
Write-Host "Creating Issue #021: Create Main Coffee Tracking Page" -ForegroundColor Yellow
gh issue create `
  --title "Create Main Coffee Tracking Page" `
  --body-file "architecture/github-issues/Issue-021-Main-Coffee-Tracking-Page.md" `
  --label "epic-4,blazor-page,high-priority,ui-integration,routing" `
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #022: Add Client-Side Validation and UX Polish
Write-Host "Creating Issue #022: Add Client-Side Validation and UX Polish" -ForegroundColor Yellow
gh issue create `
  --title "Add Client-Side Validation and UX Polish" `
  --body-file "architecture/github-issues/Issue-022-Client-Side-Validation-UX-Polish.md" `
  --label "epic-4,validation,medium-priority,ux-enhancement,animations" `
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #023: Create Blazor Component Integration Tests
Write-Host "Creating Issue #023: Create Blazor Component Integration Tests" -ForegroundColor Yellow
gh issue create `
  --title "Create Blazor Component Integration Tests" `
  --body-file "architecture/github-issues/Issue-023-Blazor-Component-Integration-Tests.md" `
  --label "epic-4,testing,medium-priority,bunit-integration,component-testing" `
  --milestone "Phase 1 - Anonymous User MVP"

Write-Host "All Epic 4 GitHub issues created successfully! 🎉" -ForegroundColor Green
Write-Host "View them at: https://github.com/duncan-nthdigital/CoffeeTrackerTDD/issues" -ForegroundColor Cyan
