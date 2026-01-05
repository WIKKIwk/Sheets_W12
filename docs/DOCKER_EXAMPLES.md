# Docker Configuration Examples

This directory contains example Docker configurations for different deployment scenarios.

## Development Configuration

The default `docker-compose.yml` is optimized for development:

```yaml
version: '3.8'

services:
  frontend:
    build:
      context: ./shlyux
      dockerfile: Dockerfile
    ports:
      - "8001:80"
    environment:
      - VITE_API_URL=http://localhost:8080
      - VITE_WS_URL=ws://localhost:4000
    volumes:
      - ./shlyux:/app  # Hot reload
      - /app/node_modules
    depends_on:
      - backend-go

  backend-go:
    build:
      context: ./backend-go
      dockerfile: Dockerfile
    ports:
      - "8080:8080"
    env_file:
      - .env
    volumes:
      - ./backend-go:/app  # Hot reload
    depends_on:
      -converter_db
      - redis

  backend-elixir:
    build:
      context: ./backend-elixir
      dockerfile: Dockerfile
    ports:
      - "4000:4000"
    env_file:
      - .env
    depends_on:
      - converter_db
      - redis

  converter_db:
    image: postgres:15-alpine
    environment:
      POSTGRES_USER: user
      POSTGRES_PASSWORD: password
      POSTGRES_DB: converter_db
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data

volumes:
  postgres_data:
  redis_data:
```

**Features:**

- Hot reload enabled
- Ports exposed for debugging
- Simple configuration
- No SSL required

## Production Configuration

Use `docker-compose.prod.yml` for production:

```yaml
version: '3.8'

services:
  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx/nginx.prod.conf:/etc/nginx/nginx.conf
      - ./nginx/ssl:/etc/nginx/ssl
    depends_on:
      - frontend
      - backend-go
      - backend-elixir

  frontend:
    build:
      context: ./shlyux
      dockerfile: Dockerfile.prod
    expose:
      - "80"
    environment:
      - NODE_ENV=production

  backend-go:
    build:
      context: ./backend-go
      dockerfile: Dockerfile.prod
    expose:
      - "8080"
    env_file:
      - .env.production
    restart: always

  backend-elixir:
    build:
      context: ./backend-elixir
      dockerfile: Dockerfile.prod
    expose:
      - "4000"
    env_file:
      - .env.production
    restart: always

  converter_db:
    image: postgres:15-alpine
    environment:
      POSTGRES_USER: ${DB_USER}
      POSTGRES_PASSWORD: ${DB_PASSWORD}
      POSTGRES_DB: ${DB_NAME}
    volumes:
      - postgres_data:/var/lib/postgresql/data
    restart: always
    shm_size: 256mb

  redis:
    image: redis:7-alpine
    command: redis-server --requirepass ${REDIS_PASSWORD}
    volumes:
      - redis_data:/data
    restart: always

volumes:
  postgres_data:
  redis_data:
```

**Features:**

- Nginx reverse proxy with SSL
- No ports exposed (except 80/443)
- Production builds
- Auto-restart enabled
- Environment-based config

## Custom Scenarios

### Scenario 1: Separate Database Server

If using external database:

```yaml
services:
  backend-go:
    environment:
      - DB_HOST=external-db.example.com
      - DB_PORT=5432
    # Remove depends_on: converter_db
  
  # Remove converter_db service
```

### Scenario 2: Redis Cluster

For high availability:

```yaml
services:
  redis-master:
    image: redis:7-alpine
    command: redis-server --requirepass ${REDIS_PASSWORD}
    volumes:
      - redis_master:/data

  redis-replica:
    image: redis:7-alpine
    command: redis-server --slaveof redis-master 6379 --requirepass ${REDIS_PASSWORD}
    volumes:
      - redis_replica:/data
    depends_on:
      - redis-master

volumes:
  redis_master:
  redis_replica:
```

### Scenario 3: Multiple Backend Instances

For load balancing:

```yaml
services:
  backend-go-1:
    build: ./backend-go
    expose:
      - "8080"

  backend-go-2:
    build: ./backend-go
    expose:
      - "8080"

  nginx:
    # Configure upstream load balancing in nginx.conf
```

### Scenario 4: Development with External Services

Use some services locally, others external:

```yaml
services:
  frontend:
    build: ./shlyux
    environment:
      - VITE_API_URL=https://staging-api.example.com
      - VITE_WS_URL=wss://staging-ws.example.com

  # Only database and Redis locally
  converter_db:
    image: postgres:15-alpine
  
  redis:
    image: redis:7-alpine
```

## Health Checks

Add health checks for reliability:

```yaml
services:
  backend-go:
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s

  converter_db:
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U user"]
      interval: 10s
      timeout: 5s
      retries: 5

  redis:
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 10s
      timeout: 3s
      retries: 3
```

## Resource Limits

Control resource usage:

```yaml
services:
  backend-go:
    deploy:
      resources:
        limits:
          cpus: '1.0'
          memory: 512M
        reservations:
          cpus: '0.5'
          memory: 256M

  converter_db:
    deploy:
      resources:
        limits:
          memory: 2G
```

## Logging Configuration

Centralized logging:

```yaml
services:
  backend-go:
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"

  # Or use external logging
  backend-go:
    logging:
      driver: "syslog"
      options:
        syslog-address: "tcp://logs.example.com:514"
```

## Network Configuration

Custom networks:

```yaml
networks:
  frontend:
    driver: bridge
  backend:
    driver: bridge
  database:
    driver: bridge
    internal: true  # No external access

services:
  frontend:
    networks:
      - frontend

  backend-go:
    networks:
      - frontend
      - backend
      - database

  converter_db:
    networks:
      - database
```

## Using Docker Secrets

For sensitive data:

```yaml
secrets:
  db_password:
    file: ./secrets/db_password.txt
  jwt_secret:
    file: ./secrets/jwt_secret.txt

services:
  backend-go:
    secrets:
      - db_password
      - jwt_secret
    environment:
      - DB_PASSWORD_FILE=/run/secrets/db_password
      - JWT_SECRET_FILE=/run/secrets/jwt_secret
```

## Commands

**Development:**

```bash
docker compose up -d
docker compose logs -f
docker compose down
```

**Production:**

```bash
docker compose -f docker-compose.prod.yml up -d
docker compose -f docker-compose.prod.yml logs -f
docker compose -f docker-compose.prod.yml down
```

**Rebuild:**

```bash
docker compose up -d --build
docker compose -f docker-compose.prod.yml build --no-cache
```

## Best Practices

1. **Use .env files** for configuration
2. **Don't expose ports** in production (except nginx)
3. **Enable auto-restart** for production services
4. **Add health checks** for all services
5. **Set resource limits** to prevent abuse
6. **Use volumes** for persistent data
7. **Implement logging** for debugging
8. **Use networks** to isolate services
9. **Keep images small** (use Alpine Linux)
10. **Regular updates** of base images

## Troubleshooting

**Services won't start:**

```bash
docker compose ps
docker compose logs service-name
```

**Port conflicts:**

```bash
sudo lsof -i :PORT
```

**Rebuild specific service:**

```bash
docker compose up -d --build service-name
```

**Clean everything:**

```bash
docker compose down -v
docker system prune -a
```
