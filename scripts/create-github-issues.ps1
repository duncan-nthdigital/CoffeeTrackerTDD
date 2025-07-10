# GitHub Issues Creation Script
# This script creates GitHub issues from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

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

Write-Host "All Epic 1 issues created successfully!" -ForegroundColor Green
Write-Host "Visit your GitHub repository to view and organize the issues." -ForegroundColor Cyan
