# Creating GitHub Issues from Epic Files

This guide explains how to create GitHub issues from the Epic 1 markdown files.

## 📋 Available Options

### Option 1: Manual Creation (Recommended for First Time)
1. Go to your GitHub repository
2. Click **Issues** → **New Issue**
3. For each issue file:
   - Copy the title (e.g., "Create Coffee Entry Domain Model")
   - Copy the entire markdown content as the issue description
   - Add labels: `epic-1`, `high-priority`, etc.
   - Set milestone: "Phase 1 - Anonymous User MVP"

### Option 2: GitHub CLI Automation (Fastest)

#### Prerequisites
1. **Install GitHub CLI**: https://cli.github.com/
2. **Authenticate**: Run `gh auth login`
3. **Verify access**: Run `gh repo view` to confirm you can access the repo

#### Using PowerShell (Windows)
```powershell
# Navigate to project root
cd "c:\Users\dunca\source\public-repos\Lession-7-AI-TDD\CoffeeTrackerTDD"

# Run the PowerShell script
.\scripts\create-github-issues.ps1
```

#### Using Bash (Linux/Mac/WSL)
```bash
# Navigate to project root
cd "/c/Users/dunca/source/public-repos/Lession-7-AI-TDD/CoffeeTrackerTDD"

# Make script executable
chmod +x scripts/create-github-issues.sh

# Run the bash script
./scripts/create-github-issues.sh
```

## 📝 What Gets Created

Each script will create 5 GitHub issues:

1. **Issue #001**: Create Coffee Entry Domain Model
   - Labels: `epic-1`, `domain-model`, `high-priority`, `foundation`
   - Complete copilot prompt included

2. **Issue #002**: Create Coffee Type and Size Enumerations
   - Labels: `epic-1`, `enumeration`, `high-priority`, `foundation`
   - Caffeine calculation requirements

3. **Issue #003**: Update DbContext with Coffee Models
   - Labels: `epic-1`, `database`, `ef-core`, `high-priority`
   - Entity Framework configuration

4. **Issue #004**: Create Coffee Shop Basic Model
   - Labels: `epic-1`, `domain-model`, `medium-priority`, `phase-1`
   - Simple model for Phase 1

5. **Issue #005**: Create Database Migration and Schema Update
   - Labels: `epic-1`, `database`, `ef-migration`, `high-priority`
   - EF Core migration requirements

## 🎯 After Creation

Once issues are created in GitHub:

1. **Assign team members** to specific issues
2. **Create project board** to track progress
3. **Link issues** to pull requests when implementing
4. **Check off acceptance criteria** as work is completed
5. **Close issues** when definition of done is met

## 🔧 Troubleshooting

### GitHub CLI Issues
- **Not authenticated**: Run `gh auth login`
- **No repo access**: Ensure you have write access to the repository
- **Milestone doesn't exist**: Create "Phase 1 - Anonymous User MVP" milestone first

### Script Issues
- **Permission denied (bash)**: Run `chmod +x scripts/create-github-issues.sh`
- **PowerShell execution policy**: Run `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`

### Manual Creation Issues
- **Formatting lost**: Copy as plain text, GitHub will render the markdown
- **Large content**: GitHub issues support large markdown content
- **Labels don't exist**: Create the labels first or add them after issue creation

## 📚 Next Steps

After creating the issues:

1. **Start with Issue #001** - Foundation model
2. **Follow the copilot prompts** in each issue
3. **Use TDD approach** as outlined in the coding instructions
4. **Track progress** in GitHub project boards
5. **Link pull requests** to issues for traceability

---

**Note**: The issue files contain complete context and ready-to-use copilot prompts, so they work perfectly for GitHub issue creation.
