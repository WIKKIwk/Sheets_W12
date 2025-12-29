# Git Workflow

## Branch Strategy

### Main Branches
- `main` - Production-ready code
- `develop` - Integration branch

### Feature Branches
- `feature/user-auth` - New features
- `fix/login-bug` - Bug fixes
- `docs/api-guide` - Documentation
- `refactor/cleanup` - Code refactoring

## Workflow

### Starting New Feature

```bash
# Update develop
git checkout develop
git pull origin develop

# Create feature branch
git checkout -b feature/my-feature

# Work on feature
# Make commits
git add .
git commit -m "feat: add new feature"
```

### Committing Changes

Follow [Conventional Commits](https://www.conventionalcommits.org/):

```bash
# Features
git commit -m "feat: add user profile"

# Bug fixes
git commit -m "fix: resolve login issue"

# Documentation
git commit -m "docs: update API guide"

# Refactoring
git commit -m "refactor: simplify auth logic"

# Tests
git commit -m "test: add user tests"

# Chores
git commit -m "chore: update dependencies"
```

### Creating Pull Request

```bash
# Push branch
git push origin feature/my-feature

# Create PR on GitHub
# Request reviews
# Address feedback
```

### Merging

```bash
# After approval, merge via GitHub UI
# Or locally:
git checkout develop
git merge feature/my-feature
git push origin develop

# Delete feature branch
git branch -d feature/my-feature
git push origin --delete feature/my-feature
```

## Best Practices

1. **Small commits** - Focused, atomic changes
2. **Clear messages** - Descriptive commit messages
3. **Regular pushes** - Don't hoard commits locally
4. **Review code** - Before requesting review
5. **Update often** - Rebase on develop regularly
6. **Test first** - Ensure tests pass

## Common Commands

```bash
# Check status
git status

# View changes
git diff

# Amend last commit
git commit --amend

# Undo last commit (keep changes)
git reset --soft HEAD~1

# Discard changes
git checkout -- file.txt

# Stash changes
git stash
git stash pop
```
