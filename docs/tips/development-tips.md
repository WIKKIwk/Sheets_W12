# Development Tips

## Quick Commands

### Database Access
```bash
./scripts/db-console.sh
```

### View Logs
```bash
docker compose logs -f backend-go
docker compose logs -f frontend
```

### Rebuild After Changes
```bash
docker compose down
docker compose up -d --build
```

## Debugging

### Check Service Health
```bash
curl http://localhost:8080/health
```

### Inspect Database
```bash
docker exec converter_db psql -U user -d converter_db -c '\dt'
```

### Redis CLI
```bash
docker exec -it redis redis-cli
```

## Performance

### Monitor Resources
```bash
docker stats
```

### Check Database Connections
```sql
SELECT count(*) FROM pg_stat_activity;
```
