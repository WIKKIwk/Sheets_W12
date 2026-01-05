# Contributing to W12C Sheets

Thank you for your interest in contributing to W12C Sheets! This document provides guidelines and instructions for contributing.

## Code of Conduct

- Be respectful and inclusive
- Provide constructive feedback
- Focus on what is best for the project
- Show empathy towards other contributors

## How to Contribute

### Reporting Bugs

**Before submitting a bug report:**

- Check existing issues to avoid duplicates
- Test with the latest version
- Gather relevant information (OS, browser, versions)

**Creating a bug report:**

1. Use a clear, descriptive title
2. Describe the exact steps to reproduce
3. Include expected vs actual behavior
4. Add screenshots if helpful
5. List your environment details

**Example:**

```markdown
**Title:** Formula calculation error for VLOOKUP with ranges

**Description:**
VLOOKUP returns #ERROR when using cell ranges

**Steps to reproduce:**
1. Enter data in A1:B10
2. Enter formula =VLOOKUP("value", A1:B10, 2, FALSE) in C1
3. Formula shows #ERROR

**Expected:** Should return matching value
**Actual:** Shows #ERROR

**Environment:**
- Browser: Chrome 120
- OS: Ubuntu 22.04
- Version: commit abc123
```

### Suggesting Features

**Before suggesting:**

- Check if feature already exists
- Search existing feature requests
- Consider if it fits project scope

**Creating a feature request:**

1. Clear, descriptive title
2. Detailed description
3. Use cases and benefits
4. Possible implementation approach
5. Alternatives considered

### Contributing Code

#### Setup Development Environment

1. **Fork the repository**

   ```bash
   # Navigate to https://github.com/WIKKIwk/Sheets_W12
   # Click "Fork" button
   ```

2. **Clone your fork**

   ```bash
   git clone https://github.com/YOUR_USERNAME/Sheets_W12.git
   cd Sheets_W12
   ```

3. **Add upstream remote**

   ```bash
   git remote add upstream https://github.com/WIKKIwk/Sheets_W12.git
   ```

4. **Install dependencies**

   ```bash
   docker compose up -d
   ```

#### Development Workflow

1. **Create a branch**

   ```bash
   git checkout -b feature/my-new-feature
   # or
   git checkout -b fix/bug-description
   ```

2. **Make your changes**
   - Write clean, readable code
   - Follow project conventions
   - Add tests for new features
   - Update documentation

3. **Test your changes**

   ```bash
   # Run tests
   npm test  # Frontend
   go test ./...  # Backend Go
   mix test  # Backend Elixir
   
   # Build and verify
   docker compose up -d --build
   ```

4. **Commit your changes**

   ```bash
   git add .
   git commit -m "feat: add new feature description"
   ```

5. **Push to your fork**

   ```bash
   git push origin feature/my-new-feature
   ```

6. **Create Pull Request**
   - Go to your fork on GitHub
   - Click "New Pull Request"
   - Fill in the PR template
   - Request review

#### Commit Message Guidelines

Use conventional commits format:

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**

- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

**Examples:**

```bash
feat(grid): add virtual scrolling for large sheets
fix(formulas): correct VLOOKUP range parsing
docs(api): update authentication endpoint docs
refactor(backend): simplify file handler logic
test(frontend): add tests for formula engine
```

#### Code Style

**TypeScript/JavaScript:**

- Use TypeScript for type safety
- 2 spaces for indentation
- Single quotes for strings
- Semicolons required
- Use async/await over promises

**Go:**

- Follow Go idioms
- Use `gofmt` for formatting
- Write clear error messages
- Add comments for exported functions

**Elixir:**

- Follow Elixir style guide
- Use `mix format`
- Pattern matching over conditionals
- Use pipelines for transformations

#### Testing

**Required:**

- Add tests for new features
- Update tests for bug fixes
- Ensure all tests pass
- Maintain or improve coverage

**Test Types:**

- Unit tests for business logic
- Integration tests for API endpoints
- E2E tests for critical user flows

#### Documentation

Update documentation when:

- Adding new features
- Changing API endpoints
- Modifying configuration
- Updating dependencies

Files to update:

- README.md
- docs/ directory
- Code comments (for complex logic)
- CHANGELOG.md

### Pull Request Process

1. **Before submitting:**
   - [ ] All tests pass
   - [ ] Code follows style guidelines
   - [ ] Documentation updated
   - [ ] Commits are clean and descriptive
   - [ ] Branch is up to date with main

2. **PR checklist:**
   - [ ] Clear title and description
   - [ ] Reference related issues
   - [ ] Screenshots for UI changes
   - [ ] Breaking changes noted
   - [ ] Reviewer assigned (if known)

3. **After submission:**
   - Respond to review comments
   - Make requested changes
   - Keep PR updated with main branch
   - Be patient and respectful

4. **Merging:**
   - Maintainers will merge approved PRs
   - Squash and merge for clean history
   - PR author should delete branch after merge

## Project Structure

Important directories:

```
├── backend-go/         # Go REST API
├── backend-elixir/     # Elixir real-time backend
├── shlyux/             # React frontend
├── docs/               # Documentation
├── examples/           # Code examples
└── tests/              # E2E tests
```

## Development Tips

### Running Specific Services

```bash
# Frontend only
docker compose up frontend

# Backend only
docker compose up backend-go backend-elixir

# Database only
docker compose up converter_db redis
```

### Debugging

**Frontend:**

- Use React DevTools
- Check browser console
- Use debugger statements

**Go Backend:**

- Use `fmt.Println` for quick debugging
- Use Delve debugger for complex issues
- Check logs: `docker compose logs backend-go`

**Elixir Backend:**

- Use `IO.inspect` with labels
- Check IEx REPL
- View logs: `docker compose logs backend-elixir`

### Common Tasks

**Rebuild after changes:**

```bash
docker compose up -d --build service-name
```

**View logs:**

```bash
docker compose logs -f service-name
```

**Access database:**

```bash
docker exec -it converter_db psql -U user -d converter_db
```

**Clear and restart:**

```bash
docker compose down -v
docker compose up -d
```

## Getting Help

- **Questions:** Open a GitHub Discussion
- **Bugs:** Create an Issue
- **Chat:** Join Discord (if available)
- **Email:** Contact maintainers

## Recognition

Contributors will be:

- Listed in README.md
- Mentioned in release notes
- Credited in CHANGELOG.md

## License

By contributing, you agree that your contributions will be licensed under the same license as the project (Apache 2.0).

---

Thank you for contributing to W12C Sheets! 🎉
