#!/bin/bash

# GitHub Issues Creation Script (Bash version)
# This script creates GitHub issues from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

echo "🚀 Creating Epic 1 GitHub Issues..."

# Issue #001: Coffee Entry Domain Model
echo "📝 Creating Issue #001: Coffee Entry Domain Model"
gh issue create \
  --title "Create Coffee Entry Domain Model" \
  --body-file "architecture/github-issues/Issue-001-Coffee-Entry-Domain-Model.md" \
  --label "epic-1,domain-model,high-priority,foundation" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #002: Coffee Type and Size Enumerations
echo "📝 Creating Issue #002: Coffee Type and Size Enumerations"
gh issue create \
  --title "Create Coffee Type and Size Enumerations" \
  --body-file "architecture/github-issues/Issue-002-Coffee-Type-Size-Enums.md" \
  --label "epic-1,enumeration,high-priority,foundation" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #003: Update DbContext with Coffee Models
echo "📝 Creating Issue #003: Update DbContext with Coffee Models"
gh issue create \
  --title "Update DbContext with Coffee Models" \
  --body-file "architecture/github-issues/Issue-003-Update-DbContext-Coffee-Models.md" \
  --label "epic-1,database,ef-core,high-priority" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #004: Create Coffee Shop Basic Model
echo "📝 Creating Issue #004: Create Coffee Shop Basic Model"
gh issue create \
  --title "Create Coffee Shop Basic Model" \
  --body-file "architecture/github-issues/Issue-004-Coffee-Shop-Basic-Model.md" \
  --label "epic-1,domain-model,medium-priority,phase-1" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #005: Create Database Migration and Schema Update
echo "📝 Creating Issue #005: Create Database Migration and Schema Update"
gh issue create \
  --title "Create Database Migration and Schema Update" \
  --body-file "architecture/github-issues/Issue-005-Database-Migration.md" \
  --label "epic-1,database,ef-migration,high-priority" \
  --milestone "Phase 1 - Anonymous User MVP"

echo "✅ All Epic 1 issues created successfully!"
echo "🌐 Visit your GitHub repository to view and organize the issues."
