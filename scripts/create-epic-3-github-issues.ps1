# GitHub Issues Creation Script for Epic 3
# This script creates GitHub issues from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

Write-Host "Creating Epic 3 GitHub Issues - Coffee Shop Data & Locator API..." -ForegroundColor Green

# Ensure required labels exist
Write-Host "Checking and creating required labels..." -ForegroundColor Cyan
$requiredLabels = @(
    @{Name="epic-3"; Description="Epic 3: Coffee Shop Data & Locator API"; Color="0052cc"},
    @{Name="caching"; Description="Caching and performance optimization"; Color="fbca04"},
    @{Name="data"; Description="Data management and seed data"; Color="d4c5f9"},
    @{Name="low-priority"; Description="Low priority items"; Color="0e8a16"},
    @{Name="location"; Description="Geographic location and mapping features"; Color="c2e0c6"},
    @{Name="integration"; Description="Integration testing and cross-component tests"; Color="7f1b9a"}
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
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #017: Create Coffee Shop Integration Tests
Write-Host "Creating Issue #017: Create Coffee Shop Integration Tests" -ForegroundColor Yellow
gh issue create `
  --title "Create Coffee Shop Integration Tests" `
  --body-file "architecture/github-issues/Issue-017-Coffee-Shop-Integration-Tests.md" `
  --label "epic-3,testing,medium-priority,integration,swagger" `
  --milestone "Phase 1 - Anonymous User MVP"

Write-Host "All Epic 3 GitHub issues created successfully! 🎉" -ForegroundColor Green
Write-Host "View them at: https://github.com/duncan-nthdigital/CoffeeTrackerTDD/issues" -ForegroundColor Cyan
