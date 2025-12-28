# Development Setup

## Prerequisites

- Docker 20.10+
- Docker Compose v2+
- Git
- (Optional) Node.js 20+ for local frontend
- (Optional) Go 1.21+ for local backend

## Quick Start

```bash
# Clone repository
git clone https://github.com/WIKKIwk/Sheets_W12.git
cd Sheets_W12

# Copy environment file
cp .env.example .env

# Start all services
docker compose up -d

# View logs
docker compose logs -f

# Access application
open http://localhost:8001
```

## Development Workflow

### Backend Development (Go)

```bash
# Enter backend container
docker compose exec backend-go sh

# Run tests
go test ./...

# Format code
gofmt -w .

# Build
go build -o server cmd/server/main.go
```

### Frontend Development

```bash
# Local development with hot reload
cd shlyux
npm install
npm run dev

# Open http://localhost:5173
```

### Database Migrations

```bash
# Create migration
docker compose exec backend-go migrate create -ext sql -dir migrations add_users

# Run migrations
docker compose exec backend-go migrate -path migrations -database "$DB_DSN" up
```

## Debugging

### Enable Debug Logging

```bash
# In .env
LOG_LEVEL=debug
GIN_MODE=debug
```

### Attach to Container

```bash
docker compose exec backend-go sh
docker compose exec frontend sh
```

### Database Access

```bash
docker exec -it converter_db psql -U user -d converter_db
```
