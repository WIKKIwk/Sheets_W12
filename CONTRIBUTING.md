# Contributing to W12C Sheets

Thank you for your interest in contributing to W12C Sheets! 🎉

## How to Contribute

### Reporting Bugs

- Use GitHub Issues
- Include clear description
- Provide steps to reproduce
- Include system information

### Suggesting Features

- Open a GitHub Issue with "Feature Request" label
- Describe the feature and use case
- Explain why it would be valuable

### Code Contributions

#### Setup Development Environment

```bash
git clone https://github.com/WIKKIwk/Sheets_W12.git
cd Sheets_W12
docker compose up -d
```

#### Making Changes

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Make your changes
4. Test thoroughly
5. Commit with descriptive messages
6. Push to your fork
7. Open a Pull Request

### Code Style

- **Go**: Follow `gofmt` standards
- **TypeScript**: Use Prettier
- **Elixir**: Use `mix format`

### Testing

```bash
# Backend (Go)
cd backend-go && go test ./...

# Frontend
cd shlyux && npm test

# Elixir
cd backend-elixir && mix test
```

## License

By contributing, you agree that your contributions will be licensed under Apache License 2.0.
