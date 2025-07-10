#!/bin/bash

# Epic 3: Coffee Shop Data & Locator API - GitHub Issues Creator
# This script creates 5 GitHub issues for Epic 3 with Swagger/OpenAPI documentation requirements

set -euo pipefail

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Global variables
DRY_RUN=false
REPO=""
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Function to print colored output
print_color() {
    local color=$1
    local message=$2
    echo -e "${color}${message}${NC}"
}

# Function to show usage
show_usage() {
    cat << EOF
Usage: $0 [OPTIONS]

Creates GitHub issues for Epic 3: Coffee Shop Data & Locator API

OPTIONS:
    -d, --dry-run           Show what issues would be created without creating them
    -r, --repo REPO         GitHub repository in format "owner/repo"
    -h, --help              Show this help message

EXAMPLES:
    $0                      Create all Epic 3 issues in current repository
    $0 --dry-run            Show what issues would be created
    $0 --repo user/repo     Create issues in specified repository

EOF
}

# Function to check if GitHub CLI is installed
check_github_cli() {
    if ! command -v gh &> /dev/null; then
        print_color "$RED" "❌ GitHub CLI (gh) is not installed or not in PATH"
        print_color "$YELLOW" "Please install GitHub CLI: https://cli.github.com/"
        exit 1
    fi
}

# Function to get current repository
get_current_repo() {
    if git rev-parse --git-dir > /dev/null 2>&1; then
        local remote_url
        remote_url=$(git config --get remote.origin.url 2>/dev/null || echo "")
        if [[ $remote_url =~ github\.com[:/]([^/]+/[^/]+?)(\.git)?$ ]]; then
            echo "${BASH_REMATCH[1]}"
            return 0
        fi
    fi
    return 1
}

# Function to test repository access
test_repo_access() {
    local repo=$1
    if gh api "repos/$repo" &> /dev/null; then
        return 0
    else
        return 1
    fi
}

# Function to get issue file path
get_issue_file_path() {
    local issue_number=$1
    local issue_file="$SCRIPT_DIR/../architecture/github-issues/Issue-$issue_number.md"
    
    if [[ ! -f "$issue_file" ]]; then
        print_color "$RED" "❌ Issue file not found: $issue_file"
        exit 1
    fi
    
    echo "$issue_file"
}

# Function to extract issue content
get_issue_content() {
    local issue_file=$1
    local content
    content=$(cat "$issue_file")
    
    # Extract title (first # heading)
    local title
    title=$(echo "$content" | grep -m1 "^# " | sed 's/^# //')
    
    if [[ -z "$title" ]]; then
        print_color "$RED" "❌ Could not extract title from $issue_file"
        exit 1
    fi
    
    # Remove the title from content to avoid duplication
    local body
    body=$(echo "$content" | sed '1{/^# /d;}')
    
    echo "$title|$body"
}

# Function to create GitHub issue
create_github_issue() {
    local repo=$1
    local title=$2
    local body=$3
    shift 3
    local labels=("$@")
    
    local label_args=()
    if [[ ${#labels[@]} -gt 0 ]]; then
        label_args+=("--label")
        IFS=','
        label_args+=("${labels[*]}")
        IFS=' '
    fi
    
    gh issue create --repo "$repo" --title "$title" --body "$body" "${label_args[@]}"
}

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        -d|--dry-run)
            DRY_RUN=true
            shift
            ;;
        -r|--repo)
            REPO="$2"
            shift 2
            ;;
        -h|--help)
            show_usage
            exit 0
            ;;
        *)
            print_color "$RED" "❌ Unknown option: $1"
            show_usage
            exit 1
            ;;
    esac
done

# Main script execution
main() {
    print_color "$CYAN" "🚀 Epic 3: Coffee Shop Data & Locator API - GitHub Issues Creator"
    print_color "$BLUE" "$(printf '=%.0s' {1..80})"
    
    # Check if GitHub CLI is installed
    check_github_cli
    
    # Determine repository
    if [[ -z "$REPO" ]]; then
        if ! REPO=$(get_current_repo); then
            print_color "$RED" "❌ Could not determine repository. Please specify with --repo parameter"
            exit 1
        fi
    fi
    
    print_color "$GREEN" "📁 Target Repository: $REPO"
    
    # Test repository access
    if ! test_repo_access "$REPO"; then
        print_color "$RED" "❌ Cannot access repository '$REPO'. Check permissions and repository name."
        exit 1
    fi
    
    # Define Epic 3 issues
    declare -A issues=(
        ["013-Coffee-Shop-API-Controller"]="epic-3,api,high-priority,swagger,documentation"
        ["014-Coffee-Shop-Service-Layer"]="epic-3,service,high-priority,caching"
        ["015-Coffee-Shop-Seed-Data"]="epic-3,data,medium-priority,testing"
        ["016-Geographic-Location-Support"]="epic-3,enhancement,low-priority,location,swagger"
        ["017-Coffee-Shop-Integration-Tests"]="epic-3,testing,medium-priority,integration,swagger"
    )
    
    print_color "$YELLOW" ""
    print_color "$YELLOW" "📋 Epic 3 Issues to Create:"
    print_color "$BLUE" "  • Total Issues: ${#issues[@]}"
    print_color "$BLUE" "  • High Priority: 2 issues"
    print_color "$BLUE" "  • Medium Priority: 2 issues"
    print_color "$BLUE" "  • Low Priority: 1 issue"
    print_color "$BLUE" "  • All issues include Swagger/OpenAPI documentation requirements"
    
    if [[ "$DRY_RUN" == "true" ]]; then
        print_color "$YELLOW" ""
        print_color "$YELLOW" "🔍 DRY RUN MODE - No issues will be created"
    fi
    
    echo ""
    
    # Create issues
    local created_count=0
    local failed_count=0
    declare -a failed_issues=()
    
    for issue_number in "${!issues[@]}"; do
        local labels_string="${issues[$issue_number]}"
        IFS=',' read -ra labels <<< "$labels_string"
        
        print_color "$CYAN" "📝 Processing Issue $issue_number..."
        
        # Get issue file path
        local issue_file
        issue_file=$(get_issue_file_path "$issue_number")
        print_color "$BLUE" "   📁 File: $(basename "$issue_file")"
        
        # Extract issue content
        local issue_content
        issue_content=$(get_issue_content "$issue_file")
        IFS='|' read -r title body <<< "$issue_content"
        
        print_color "$BLUE" "   📰 Title: $title"
        print_color "$BLUE" "   🏷️  Labels: ${labels[*]}"
        
        if [[ "$DRY_RUN" == "true" ]]; then
            print_color "$GREEN" "   ✅ Would create issue (DRY RUN)"
            ((created_count++))
        else
            # Create the GitHub issue
            if result=$(create_github_issue "$REPO" "$title" "$body" "${labels[@]}" 2>&1); then
                print_color "$GREEN" "   ✅ Created: $result"
                ((created_count++))
            else
                print_color "$RED" "   ❌ Failed: $result"
                failed_issues+=("$issue_number: $result")
                ((failed_count++))
            fi
        fi
        
        echo "" # Empty line for spacing
    done
    
    # Summary
    print_color "$CYAN" "📊 SUMMARY"
    print_color "$BLUE" "$(printf '=%.0s' {1..40})"
    print_color "$GREEN" "✅ Successfully processed: $created_count/${#issues[@]} issues"
    
    if [[ $failed_count -gt 0 ]]; then
        print_color "$RED" "❌ Failed issues: $failed_count"
        for failed in "${failed_issues[@]}"; do
            print_color "$RED" "   • $failed"
        done
    fi
    
    if [[ "$DRY_RUN" == "true" ]]; then
        print_color "$YELLOW" ""
        print_color "$YELLOW" "💡 This was a dry run. To create the issues, run the script without --dry-run"
    else
        print_color "$GREEN" ""
        print_color "$GREEN" "🎉 Epic 3 GitHub issues creation completed!"
        print_color "$BLUE" "🔗 View issues: https://github.com/$REPO/issues"
    fi
}

# Run main function
main "$@"
