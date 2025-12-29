# Docker Compose Tips & Tricks

## Quick Commands

### Start services
```bash
docker compose up -d
```

### Stop services
```bash
docker compose down
```

### View logs
```bash
docker compose logs -f
docker compose logs -f backend-go
docker compose logs --tail=100 frontend
```

### Rebuild specific service
```bash
docker compose up -d --build backend-go
```

### Execute commands in container
```bash
docker compose exec backend-go sh
docker compose exec converter_db psql -U user
```

## Troubleshooting

### Service won't start
```bash
# Check logs
docker compose logs backend-go

# Restart service
docker compose restart backend-go

# Force recreate
docker compose up -d --force-recreate backend-go
```

### Port already in use
```bash
# Find process using port
lsof -i :8080

# Kill process or change port in docker-compose.yml
```

### Database not accessible
```bash
# Check if database is running
docker compose ps converter_db

# Restart database
docker compose restart converter_db

# Access database console
docker compose exec converter_db psql -U user -d converter_db
```

## Performance

### Check resource usage
```bash
docker stats
```

### Limit resources
Add to service in docker-compose.yml:
```yaml
deploy:
  resources:
    limits:
      cpus: '1.0'
      memory: 1G
```

### Clean up
```bash
# Remove stopped containers
docker compose down

# Remove volumes
docker compose down -v

# Prune system
docker system prune -a
```
