#!/bin/bash
# GitHub Issues Creation Script - Single Issue
# This script creates a single GitHub issue from the markdown files using GitHub CLI
# Prerequisites: GitHub CLI must be installed and authenticated
# Install: https://cli.github.com/
# Auth: gh auth login

# Check if issue number is provided
if [ $# -ne 1 ]; then
    echo "Usage: $0 <issue-number>"
    echo "Example: $0 12"
    exit 1
fi

ISSUE_NUMBER=$1

# Validate issue number
if ! [[ "$ISSUE_NUMBER" =~ ^[0-9]+$ ]] || [ "$ISSUE_NUMBER" -lt 1 ] || [ "$ISSUE_NUMBER" -gt 17 ]; then
    echo "Error: Issue number must be between 1 and 17"
    exit 1
fi

# Function to get epic number from issue number
get_epic_number() {
    local issue=$1
    if [ $issue -ge 1 ] && [ $issue -le 5 ]; then
        echo 1
    elif [ $issue -ge 6 ] && [ $issue -le 12 ]; then
        echo 2
    elif [ $issue -ge 13 ] && [ $issue -le 17 ]; then
        echo 3
    else
        echo "Invalid issue number: $issue"
        exit 1
    fi
}

# Pad issue number with zeros
get_padded_issue() {
    printf "%03d" $1
}

# Get issue details based on issue number
get_issue_details() {
    local issue=$1
    local title=""
    local body_file=""
    local labels=""
    local milestone=""

    case $issue in
        # Epic 1 Issues
        1)
            title="Create Coffee Entry Domain Model"
            body_file="architecture/github-issues/Issue-001-Coffee-Entry-Domain-Model.md"
            labels="epic-1,domain-model,high-priority,foundation"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        2)
            title="Create Coffee Type and Size Enumerations"
            body_file="architecture/github-issues/Issue-002-Coffee-Type-Size-Enums.md"
            labels="epic-1,enumeration,high-priority,foundation"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        3)
            title="Update DbContext with Coffee Models"
            body_file="architecture/github-issues/Issue-003-Update-DbContext-Coffee-Models.md"
            labels="epic-1,database,ef-core,high-priority"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        4)
            title="Create Coffee Shop Basic Model"
            body_file="architecture/github-issues/Issue-004-Coffee-Shop-Basic-Model.md"
            labels="epic-1,domain-model,medium-priority,phase-1"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        5)
            title="Create Database Migration and Schema Update"
            body_file="architecture/github-issues/Issue-005-Database-Migration.md"
            labels="epic-1,database,ef-migration,high-priority"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
            
        # Epic 2 Issues
        6)
            title="Create Coffee Logging Controller"
            body_file="architecture/github-issues/Issue-006-Coffee-Logging-Controller.md"
            labels="epic-2,api-controller,high-priority,rest-api"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        7)
            title="Create Coffee Service Layer"
            body_file="architecture/github-issues/Issue-007-Coffee-Service-Layer.md"
            labels="epic-2,service-layer,high-priority,business-logic"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        8)
            title="Create Request/Response DTOs"
            body_file="architecture/github-issues/Issue-008-Request-Response-DTOs.md"
            labels="epic-2,dto,high-priority,api-contract"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        9)
            title="Add Anonymous Session Management"
            body_file="architecture/github-issues/Issue-009-Anonymous-Session-Management.md"
            labels="epic-2,session-management,high-priority,security"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        10)
            title="Add Data Validation and Error Handling"
            body_file="architecture/github-issues/Issue-010-Data-Validation-Error-Handling.md"
            labels="epic-2,validation,medium-priority,error-handling"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        11)
            title="Integration Tests for API Endpoints"
            body_file="architecture/github-issues/Issue-011-Integration-Tests-API-Endpoints.md"
            labels="epic-2,integration-tests,medium-priority,testing"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        12)
            title="Configure Swagger/OpenAPI Documentation"
            body_file="architecture/github-issues/Issue-012-Configure-Swagger-OpenAPI.md"
            labels="epic-2,swagger,documentation,high-priority,infrastructure"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
            
        # Epic 3 Issues
        13)
            title="Create Coffee Shop API Controller"
            body_file="architecture/github-issues/Issue-013-Coffee-Shop-API-Controller.md"
            labels="epic-3,api-controller,high-priority,swagger,documentation"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        14)
            title="Create Coffee Shop Service Layer"
            body_file="architecture/github-issues/Issue-014-Coffee-Shop-Service-Layer.md"
            labels="epic-3,service-layer,high-priority,caching"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        15)
            title="Create Coffee Shop Seed Data"
            body_file="architecture/github-issues/Issue-015-Coffee-Shop-Seed-Data.md"
            labels="epic-3,data,medium-priority,testing"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        16)
            title="Add Geographic Location Support"
            body_file="architecture/github-issues/Issue-016-Geographic-Location-Support.md"
            labels="epic-3,enhancement,low-priority,location,swagger"
            milestone="Phase 2 - Enhanced Features"
            ;;
        17)
            title="Create Coffee Shop Integration Tests"
            body_file="architecture/github-issues/Issue-017-Coffee-Shop-Integration-Tests.md"
            labels="epic-3,integration-tests,medium-priority,swagger"
            milestone="Phase 1 - Anonymous User MVP"
            ;;
        *)
            echo "Issue #$issue not found"
            exit 1
            ;;
    esac
    
    echo "$title|$body_file|$labels|$milestone"
}

# Create single issue
create_single_issue() {
    local issue_number=$1
    local epic_number=$(get_epic_number $issue_number)
    local padded_issue=$(get_padded_issue $issue_number)
    
    echo -e "\e[33mCreating Issue #$padded_issue (Epic $epic_number)...\e[0m"
    
    local issue_details=$(get_issue_details $issue_number)
    IFS='|' read -r title body_file labels milestone <<< "$issue_details"
    
    if [ -n "$title" ] && [ -n "$body_file" ]; then
        gh issue create \
          --title "$title" \
          --body-file "$body_file" \
          --label "$labels" \
          --milestone "$milestone"
        
        echo -e "\e[32mIssue #$padded_issue created successfully!\e[0m"
    else
        echo -e "\e[31mFailed to create Issue #$padded_issue: Details not found\e[0m"
    fi
}

# Main execution
create_single_issue $ISSUE_NUMBER

# Display usage examples
echo ""
echo "Usage examples:"
echo "  ./create-single-issue.sh 12  # Create issue #012 (Swagger/OpenAPI Documentation)"
echo "  ./create-single-issue.sh 6   # Create issue #006 (Coffee Logging Controller)"
