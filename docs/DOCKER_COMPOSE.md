# Docker Compose Reference

## Common Commands

### Start
```bash
docker compose up -d
```

### Stop
```bash
docker compose down
```

### Logs
```bash
docker compose logs -f
docker compose logs backend-go
```

### Rebuild
```bash
docker compose build --no-cache
docker compose up -d --force-recreate
```

### Exec
```bash
docker compose exec backend-go sh
docker compose exec converter_db psql -U user
```
