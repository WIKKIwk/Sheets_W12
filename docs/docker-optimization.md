# Docker Optimization

## Image Size Reduction

### Multi-stage Builds

```dockerfile
# Build stage
FROM golang:1.21 AS builder
WORKDIR /app
COPY . .
RUN go build -o server

# Runtime stage
FROM alpine:latest
COPY --from=builder /app/server /server
CMD ["/server"]
```

### Use .dockerignore

```
node_modules
.git
*.log
.env
.DS_Store
coverage/
dist/
```

### Alpine Images

```dockerfile
FROM node:20-alpine  # Instead of node:20
FROM golang:1.21-alpine
```

## Build Performance

### Layer Caching

```dockerfile
# Copy dependencies first (cached if unchanged)
COPY package.json package-lock.json ./
RUN npm install

# Then copy source code
COPY . .
```

### BuildKit

```bash
# Enable BuildKit
export DOCKER_BUILDKIT=1

# Parallel builds
docker compose build --parallel
```

## Runtime Optimization

### Resource Limits

```yaml
services:
  backend:
    deploy:
      resources:
        limits:
          cpus: '1.0'
          memory: 512M
        reservations:
          cpus: '0.5'
          memory: 256M
```

### Health Checks

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost/health"]
  interval: 30s
  timeout: 10s
  retries: 3
  start_period: 40s
```

## Network Optimization

### Use Networks

```yaml
networks:
  frontend:
  backend:

services:
  web:
    networks:
      - frontend
  api:
    networks:
      - frontend
      - backend
```

## Security

### Non-root User

```dockerfile
RUN addgroup -g 1001 appuser \
    && adduser -D -u 1001 -G appuser appuser
USER appuser
```

### Read-only Filesystem

```yaml
services:
  backend:
    read_only: true
    tmpfs:
      - /tmp
```

## Monitoring

```bash
# Resource usage
docker stats

# Logs
docker compose logs -f

# Inspect
docker inspect container_name
```
