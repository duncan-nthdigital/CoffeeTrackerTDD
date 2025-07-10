#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Creates GitHub issues for Epic 3: Coffee Shop Data & Locator API

.DESCRIPTION
    This script creates 5 GitHub issues for Epic 3 of the CoffeeTrackerTDD project.
    Each issue represents a specific task within the Coffee Shop API epic with comprehensive
    Swagger/OpenAPI documentation requirements.

.PARAMETER DryRun
    When specified, shows what issues would be created without actually creating them

.PARAMETER Repo
    GitHub repository in format "owner/repo" (defaults to current repo if run from git directory)

.EXAMPLE
    .\create-epic-3-github-issues.ps1
    Creates all Epic 3 issues in the current repository

.EXAMPLE
    .\create-epic-3-github-issues.ps1 -DryRun
    Shows what issues would be created without creating them

.EXAMPLE
    .\create-epic-3-github-issues.ps1 -Repo "username/CoffeeTrackerTDD"
    Creates issues in the specified repository
#>

param(
    [switch]$DryRun,
    [string]$Repo = ""
)

# Set error action preference
$ErrorActionPreference = "Stop"

# Colors for output
$Colors = @{
    Green = "Green"
    Yellow = "Yellow"
    Red = "Red"
    Cyan = "Cyan"
    Blue = "Blue"
}

function Write-ColorOutput {
    param(
        [string]$Message,
        [string]$Color = "White"
    )
    Write-Host $Message -ForegroundColor $Color
}

function Test-GitHubCli {
    try {
        $null = gh --version
        return $true
    }
    catch {
        return $false
    }
}

function Get-CurrentRepo {
    try {
        $remoteUrl = git config --get remote.origin.url
        if ($remoteUrl -match "github\.com[:/]([^/]+/[^/]+?)(?:\.git)?$") {
            return $matches[1]
        }
    }
    catch {
        # Ignore git errors
    }
    return $null
}

function Test-RepoAccess {
    param([string]$Repository)
    
    try {
        $null = gh api "repos/$Repository" 2>$null
        return $true
    }
    catch {
        return $false
    }
}

function Get-IssueFilePath {
    param([string]$IssueNumber)
    
    $baseDir = $PSScriptRoot
    $issueFile = Join-Path $baseDir ".." "architecture" "github-issues" "Issue-$IssueNumber.md"
    
    if (-not (Test-Path $issueFile)) {
        throw "Issue file not found: $issueFile"
    }
    
    return $issueFile
}

function Get-IssueContent {
    param([string]$IssueFile)
    
    $content = Get-Content $IssueFile -Raw
    
    # Extract title (first # heading)
    if ($content -match "^#\s+(.+)$") {
        $title = $matches[1].Trim()
    } else {
        throw "Could not extract title from $IssueFile"
    }
    
    # Remove the title from content to avoid duplication
    $body = $content -replace "^#\s+.+\r?\n", ""
    
    return @{
        Title = $title
        Body = $body.Trim()
    }
}

function New-GitHubIssue {
    param(
        [string]$Repository,
        [string]$Title,
        [string]$Body,
        [string[]]$Labels = @()
    )
    
    $labelArgs = @()
    if ($Labels.Count -gt 0) {
        $labelArgs += "--label"
        $labelArgs += ($Labels -join ",")
    }
    
    $arguments = @("issue", "create", "--repo", $Repository, "--title", $Title, "--body", $Body) + $labelArgs
    
    try {
        $result = & gh @arguments
        return $result
    }
    catch {
        throw "Failed to create issue: $($_.Exception.Message)"
    }
}

# Main script execution
try {
    Write-ColorOutput "🚀 Epic 3: Coffee Shop Data & Locator API - GitHub Issues Creator" $Colors.Cyan
    Write-ColorOutput "=" * 80 $Colors.Blue
    
    # Check if GitHub CLI is installed
    if (-not (Test-GitHubCli)) {
        Write-ColorOutput "❌ GitHub CLI (gh) is not installed or not in PATH" $Colors.Red
        Write-ColorOutput "Please install GitHub CLI: https://cli.github.com/" $Colors.Yellow
        exit 1
    }
    
    # Determine repository
    if (-not $Repo) {
        $Repo = Get-CurrentRepo
        if (-not $Repo) {
            Write-ColorOutput "❌ Could not determine repository. Please specify with -Repo parameter" $Colors.Red
            exit 1
        }
    }
    
    Write-ColorOutput "📁 Target Repository: $Repo" $Colors.Green
    
    # Test repository access
    if (-not (Test-RepoAccess -Repository $Repo)) {
        Write-ColorOutput "❌ Cannot access repository '$Repo'. Check permissions and repository name." $Colors.Red
        exit 1
    }
    
    # Define Epic 3 issues
    $issues = @(
        @{
            Number = "013-Coffee-Shop-API-Controller"
            Labels = @("epic-3", "api", "high-priority", "swagger", "documentation")
        },
        @{
            Number = "014-Coffee-Shop-Service-Layer"
            Labels = @("epic-3", "service", "high-priority", "caching")
        },
        @{
            Number = "015-Coffee-Shop-Seed-Data"
            Labels = @("epic-3", "data", "medium-priority", "testing")
        },
        @{
            Number = "016-Geographic-Location-Support"
            Labels = @("epic-3", "enhancement", "low-priority", "location", "swagger")
        },
        @{
            Number = "017-Coffee-Shop-Integration-Tests"
            Labels = @("epic-3", "testing", "medium-priority", "integration", "swagger")
        }
    )
    
    Write-ColorOutput "`n📋 Epic 3 Issues to Create:" $Colors.Yellow
    Write-ColorOutput "  • Total Issues: $($issues.Count)" $Colors.Blue
    Write-ColorOutput "  • High Priority: 2 issues" $Colors.Blue
    Write-ColorOutput "  • Medium Priority: 2 issues" $Colors.Blue
    Write-ColorOutput "  • Low Priority: 1 issue" $Colors.Blue
    Write-ColorOutput "  • All issues include Swagger/OpenAPI documentation requirements" $Colors.Blue
    
    if ($DryRun) {
        Write-ColorOutput "`n🔍 DRY RUN MODE - No issues will be created" $Colors.Yellow
    }
    
    Write-ColorOutput "`n" -NoNewline
    
    # Create issues
    $createdIssues = @()
    $failedIssues = @()
    
    foreach ($issueConfig in $issues) {
        $issueNumber = $issueConfig.Number
        $labels = $issueConfig.Labels
        
        try {
            Write-ColorOutput "📝 Processing Issue $issueNumber..." $Colors.Cyan
            
            # Get issue file path
            $issueFile = Get-IssueFilePath -IssueNumber $issueNumber
            Write-ColorOutput "   📁 File: $(Split-Path $issueFile -Leaf)" $Colors.Blue
            
            # Extract issue content
            $issueData = Get-IssueContent -IssueFile $issueFile
            Write-ColorOutput "   📰 Title: $($issueData.Title)" $Colors.Blue
            Write-ColorOutput "   🏷️  Labels: $($labels -join ', ')" $Colors.Blue
            
            if ($DryRun) {
                Write-ColorOutput "   ✅ Would create issue (DRY RUN)" $Colors.Green
                $createdIssues += $issueNumber
            } else {
                # Create the GitHub issue
                $result = New-GitHubIssue -Repository $Repo -Title $issueData.Title -Body $issueData.Body -Labels $labels
                Write-ColorOutput "   ✅ Created: $result" $Colors.Green
                $createdIssues += $issueNumber
            }
        }
        catch {
            Write-ColorOutput "   ❌ Failed: $($_.Exception.Message)" $Colors.Red
            $failedIssues += @{
                Number = $issueNumber
                Error = $_.Exception.Message
            }
        }
        
        Write-ColorOutput "" # Empty line for spacing
    }
    
    # Summary
    Write-ColorOutput "📊 SUMMARY" $Colors.Cyan
    Write-ColorOutput "=" * 40 $Colors.Blue
    Write-ColorOutput "✅ Successfully processed: $($createdIssues.Count)/$($issues.Count) issues" $Colors.Green
    
    if ($failedIssues.Count -gt 0) {
        Write-ColorOutput "❌ Failed issues: $($failedIssues.Count)" $Colors.Red
        foreach ($failed in $failedIssues) {
            Write-ColorOutput "   • $($failed.Number): $($failed.Error)" $Colors.Red
        }
    }
    
    if ($DryRun) {
        Write-ColorOutput "`n💡 This was a dry run. To create the issues, run the script without -DryRun" $Colors.Yellow
    } else {
        Write-ColorOutput "`n🎉 Epic 3 GitHub issues creation completed!" $Colors.Green
        Write-ColorOutput "🔗 View issues: https://github.com/$Repo/issues" $Colors.Blue
    }
    
} catch {
    Write-ColorOutput "💥 Script failed: $($_.Exception.Message)" $Colors.Red
    Write-ColorOutput $_.ScriptStackTrace $Colors.Red
    exit 1
}
