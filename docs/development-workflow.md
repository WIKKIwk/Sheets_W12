# Development Workflow

## Setting Up

1. Clone repository
```bash
git clone https://github.com/WIKKIwk/Sheets_W12.git
cd Sheets_W12
```

2. Configure environment
```bash
cp .env.example .env
# Edit .env with your settings
```

3. Start services
```bash
docker compose up -d
```

## Making Changes

### Backend (Go)

1. Make code changes in `backend-go/`
2. Rebuild and restart:
```bash
docker compose up -d --build backend-go
```

3. Run tests:
```bash
cd backend-go
go test ./...
```

### Frontend (React)

1. Make changes in `shlyux/`
2. Hot reload is enabled in development
3. Or rebuild:
```bash
docker compose up -d --build frontend
```

### Database Changes

1. Create migration in `backend-go/migrations/`
2. Apply migration:
```bash
docker compose exec backend-go migrate -path /migrations -database "$DB_DSN" up
```

## Testing

### Run all tests
```bash
./scripts/run-tests.sh
```

### Specific tests
```bash
# Go backend
cd backend-go && go test ./handlers -v

# Frontend
cd shlyux && npm test
```

## Debugging

### View logs
```bash
./scripts/monitor.sh
docker compose logs -f backend-go
```

### Database console
```bash
./scripts/db-console.sh
```

### Check health
```bash
curl http://localhost:8080/health
```

## Committing Changes

1. Follow conventional commits
```bash
git commit -m "feat: add user profile"
git commit -m "fix: resolve login issue"
git commit -m "docs: update API guide"
```

2. Test before committing
3. Push to your branch
4. Create pull request
