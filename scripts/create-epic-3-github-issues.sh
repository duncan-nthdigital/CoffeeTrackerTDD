#!/bin/bash

# GitHub Issues Creation Script for Epic 3
# This script creates GitHub issues from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

echo -e "\033[0;32mCreating Epic 3 GitHub Issues - Coffee Shop Data & Locator API...\033[0m"

# Ensure required labels exist
echo -e "\033[0;36mChecking and creating required labels...\033[0m"
declare -A required_labels=(
    ["epic-3"]="Epic 3: Coffee Shop Data & Locator API,0052cc"
    ["caching"]="Caching and performance optimization,fbca04"
    ["data"]="Data management and seed data,d4c5f9"
    ["low-priority"]="Low priority items,0e8a16"
    ["location"]="Geographic location and mapping features,c2e0c6"
    ["integration"]="Integration testing and cross-component tests,7f1b9a"
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

# Issue #013: Create Coffee Shop API Controller
echo -e "\033[1;33mCreating Issue #013: Create Coffee Shop API Controller\033[0m"
gh issue create \
  --title "Create Coffee Shop API Controller" \
  --body-file "architecture/github-issues/Issue-013-Coffee-Shop-API-Controller.md" \
  --label "epic-3,api-controller,high-priority,swagger,documentation" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #014: Create Coffee Shop Service Layer
echo -e "\033[1;33mCreating Issue #014: Create Coffee Shop Service Layer\033[0m"
gh issue create \
  --title "Create Coffee Shop Service Layer" \
  --body-file "architecture/github-issues/Issue-014-Coffee-Shop-Service-Layer.md" \
  --label "epic-3,service-layer,high-priority,caching" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #015: Create Coffee Shop Seed Data
echo -e "\033[1;33mCreating Issue #015: Create Coffee Shop Seed Data\033[0m"
gh issue create \
  --title "Create Coffee Shop Seed Data" \
  --body-file "architecture/github-issues/Issue-015-Coffee-Shop-Seed-Data.md" \
  --label "epic-3,data,medium-priority,testing" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #016: Add Geographic Location Support
echo -e "\033[1;33mCreating Issue #016: Add Geographic Location Support\033[0m"
gh issue create \
  --title "Add Geographic Location Support" \
  --body-file "architecture/github-issues/Issue-016-Geographic-Location-Support.md" \
  --label "epic-3,enhancement,low-priority,location,swagger" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #017: Create Coffee Shop Integration Tests
echo -e "\033[1;33mCreating Issue #017: Create Coffee Shop Integration Tests\033[0m"
gh issue create \
  --title "Create Coffee Shop Integration Tests" \
  --body-file "architecture/github-issues/Issue-017-Coffee-Shop-Integration-Tests.md" \
  --label "epic-3,testing,medium-priority,integration,swagger" \
  --milestone "Phase 1 - Anonymous User MVP"

echo -e "\033[0;32mAll Epic 3 GitHub issues created successfully! 🎉\033[0m"
echo -e "\033[0;36mView them at: https://github.com/duncan-nthdigital/CoffeeTrackerTDD/issues\033[0m"
