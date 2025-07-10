#!/bin/bash

# GitHub Issues Creation Script for Epic 2
# This script creates GitHub issues from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

echo "Creating Epic 2 GitHub Issues - Coffee Logging API Endpoints..."

# Issue #006: Create Coffee Logging Controller
echo "Creating Issue #006: Create Coffee Logging Controller"
gh issue create \
  --title "Create Coffee Logging Controller" \
  --body-file "architecture/github-issues/Issue-006-Coffee-Logging-Controller.md" \
  --label "epic-2,api-controller,high-priority,rest-api" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #007: Create Coffee Service Layer
echo "Creating Issue #007: Create Coffee Service Layer"
gh issue create \
  --title "Create Coffee Service Layer" \
  --body-file "architecture/github-issues/Issue-007-Coffee-Service-Layer.md" \
  --label "epic-2,service-layer,high-priority,business-logic" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #008: Create Request/Response DTOs
echo "Creating Issue #008: Create Request/Response DTOs"
gh issue create \
  --title "Create Request/Response DTOs" \
  --body-file "architecture/github-issues/Issue-008-Request-Response-DTOs.md" \
  --label "epic-2,dto,high-priority,api-contract" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #009: Add Anonymous Session Management
echo "Creating Issue #009: Add Anonymous Session Management"
gh issue create \
  --title "Add Anonymous Session Management" \
  --body-file "architecture/github-issues/Issue-009-Anonymous-Session-Management.md" \
  --label "epic-2,session-management,high-priority,security" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #010: Add Data Validation and Error Handling
echo "Creating Issue #010: Add Data Validation and Error Handling"
gh issue create \
  --title "Add Data Validation and Error Handling" \
  --body-file "architecture/github-issues/Issue-010-Data-Validation-Error-Handling.md" \
  --label "epic-2,validation,medium-priority,error-handling" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #011: Integration Tests for API Endpoints
echo "Creating Issue #011: Integration Tests for API Endpoints"
gh issue create \
  --title "Integration Tests for API Endpoints" \
  --body-file "architecture/github-issues/Issue-011-Integration-Tests-API-Endpoints.md" \
  --label "epic-2,integration-tests,medium-priority,testing" \
  --milestone "Phase 1 - Anonymous User MVP"

# Issue #012: Configure Swagger/OpenAPI Documentation
echo "Creating Issue #012: Configure Swagger/OpenAPI Documentation"
gh issue create \
  --title "Configure Swagger/OpenAPI Documentation" \
  --body-file "architecture/github-issues/Issue-012-Configure-Swagger-OpenAPI.md" \
  --label "epic-2,swagger,documentation,high-priority,infrastructure" \
  --milestone "Phase 1 - Anonymous User MVP"

echo "All Epic 2 issues created successfully!"
echo "Epic 2 includes 7 issues focused on coffee logging API endpoints with comprehensive Swagger documentation."
echo "Visit your GitHub repository to view and organize the issues."
