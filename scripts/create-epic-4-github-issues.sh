#!/bin/bash

# GitHub Issues Creation Script for Epic 4
# This script creates GitHub issues from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

echo -e "\033[0;32mCreating Epic 4 GitHub Issues - Blazor UI Components for Coffee Logging...\033[0m"

# Ensure required labels exist
echo -e "\033[0;36mChecking and creating required labels...\033[0m"
declare -A required_labels=(
    ["epic-4"]="Epic 4: Blazor UI Components for Coffee Logging,6f42c1"
    ["blazor-component"]="Blazor component development,20232a"
    ["ui-form"]="User interface forms and input components,28a745"
    ["ui-dashboard"]="Dashboard and summary UI components,17a2b8"
    ["ui-selector"]="Selection and picker UI components,fd7e14"
    ["ui-integration"]="UI component integration and page composition,6610f2"
    ["blazor-page"]="Blazor page components and routing,e83e8c"
    ["reusable"]="Reusable components and utilities,20c997"
    ["ux-enhancement"]="User experience improvements and polish,ffc107"
    ["animations"]="CSS animations and transitions,6c757d"
    ["bunit-integration"]="bUnit integration testing for Blazor,dc3545"
    ["component-testing"]="Component-level testing,fd7e14"
    ["analytics"]="Analytics and reporting features,28a745"
    ["routing"]="Page routing and navigation,6610f2"
)

for label_name in "${!required_labels[@]}"; do
    IFS=',' read -r description color <<< "${required_labels[$label_name]}"
    if gh label create "$label_name" --description "$description" --color "$color" 2>/dev/null; then
        echo -e "  \033[0;32m✓ Created label: $label_name\033[0m"
    else
        echo -e "  \033[1;33m- Label already exists: $label_name\033[0m"
    fi
done

# Ensure required milestone exists
echo -e "\033[0;36mChecking milestone...\033[0m"
repo_full_name=$(gh repo view --json owner,name -q '.owner.login + "/" + .name')
milestones=$(gh api "repos/$repo_full_name/milestones" 2>/dev/null || echo "[]")
if ! echo "$milestones" | grep -q '"title": "Phase 1 - Anonymous User MVP"'; then
    echo -e "  \033[0;32m✓ Creating milestone: Phase 1 - Anonymous User MVP\033[0m"
    gh api "repos/$repo_full_name/milestones" -f title="Phase 1 - Anonymous User MVP" -f description="Phase 1 MVP for anonymous coffee tracking" >/dev/null 2>&1 || true
else
    echo -e "  \033[1;33m- Milestone already exists: Phase 1 - Anonymous User MVP\033[0m"
fi

echo -e "\n\033[0;36mCreating issues...\033[0m"

# Issue #018: Create Coffee Logging Form Component
echo -e "\033[1;33mCreating Issue #018: Create Coffee Logging Form Component\033[0m"
gh issue create \
  --title "Create Coffee Logging Form Component" \
  --body-file "architecture/github-issues/Issue-018-Coffee-Logging-Form-Component.md" \
  --label "epic-4,blazor-component,high-priority,ui-form,validation" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #019: Create Daily Coffee Summary Component
echo -e "\033[1;33mCreating Issue #019: Create Daily Coffee Summary Component\033[0m"
gh issue create \
  --title "Create Daily Coffee Summary Component" \
  --body-file "architecture/github-issues/Issue-019-Daily-Coffee-Summary-Component.md" \
  --label "epic-4,blazor-component,high-priority,ui-dashboard,analytics" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #020: Create Coffee Type Selection Component
echo -e "\033[1;33mCreating Issue #020: Create Coffee Type Selection Component\033[0m"
gh issue create \
  --title "Create Coffee Type Selection Component" \
  --body-file "architecture/github-issues/Issue-020-Coffee-Type-Selection-Component.md" \
  --label "epic-4,blazor-component,medium-priority,ui-selector,reusable" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #021: Create Main Coffee Tracking Page
echo -e "\033[1;33mCreating Issue #021: Create Main Coffee Tracking Page\033[0m"
gh issue create \
  --title "Create Main Coffee Tracking Page" \
  --body-file "architecture/github-issues/Issue-021-Main-Coffee-Tracking-Page.md" \
  --label "epic-4,blazor-page,high-priority,ui-integration,routing" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #022: Add Client-Side Validation and UX Polish
echo -e "\033[1;33mCreating Issue #022: Add Client-Side Validation and UX Polish\033[0m"
gh issue create \
  --title "Add Client-Side Validation and UX Polish" \
  --body-file "architecture/github-issues/Issue-022-Client-Side-Validation-UX-Polish.md" \
  --label "epic-4,validation,medium-priority,ux-enhancement,animations" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #023: Create Blazor Component Integration Tests
echo -e "\033[1;33mCreating Issue #023: Create Blazor Component Integration Tests\033[0m"
gh issue create \
  --title "Create Blazor Component Integration Tests" \
  --body-file "architecture/github-issues/Issue-023-Blazor-Component-Integration-Tests.md" \
  --label "epic-4,testing,medium-priority,bunit-integration,component-testing" \
  --milestone "Phase 1 - Anonymous User MVP"

echo -e "\033[0;32mAll Epic 4 GitHub issues created successfully! 🎉\033[0m"
echo -e "\033[0;36mView them at: https://github.com/duncan-nthdigital/CoffeeTrackerTDD/issues\033[0m"
