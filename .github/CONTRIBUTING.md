# Contributing to W12C Sheets

Thank you for your interest! 🎉

## How to Contribute

### Reporting Bugs

1. Search existing issues
2. Use bug report template
3. Include:
   - Steps to reproduce
   - Expected behavior
   - Actual behavior
   - Environment details

### Suggesting Features

1. Search existing feature requests
2. Use feature request template
3. Explain use case and benefits

### Code Contributions

#### Setup

```bash
git clone https://github.com/WIKKIwk/Sheets_W12.git
cd Sheets_W12
cp .env.example .env
docker compose up -d
```

#### Development Workflow

1. Fork repository
2. Create feature branch: `git checkout -b feature/amazing-feature`
3. Make changes
4. Write tests
5. Ensure tests pass: `./scripts/run-tests.sh`
6. Commit: `git commit -m "feat: add amazing feature"`
7. Push: `git push origin feature/amazing-feature`
8. Create Pull Request

#### Code Style

- **Go**: Run `gofmt`
- **TypeScript**: Use Prettier
- **Commits**: Follow Conventional Commits

#### Pull Request Guidelines

- Reference related issues
- Update documentation
- Add tests for new features
- Ensure CI passes
- Request review from maintainers

## Development

### Running Tests

```bash
# All tests
make test

# Specific tests
cd backend-go && go test ./handlers -v
```

### Code Review

- Be respectful and constructive
- Explain reasoning for changes
- Test suggestions before requesting changes
- Approve when ready

## Questions?

- GitHub Discussions
- Open an issue
- Email: dev@w12c.dev

## License

By contributing, you agree that your contributions will be licensed under Apache License 2.0.
