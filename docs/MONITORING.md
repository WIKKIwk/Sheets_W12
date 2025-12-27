# Monitoring and Logging

## Application Logs

### Go Backend

```bash
# View logs
docker compose logs -f backend-go

# Filter errors only
docker compose logs backend-go | grep ERROR
```

### Frontend

```bash
docker compose logs -f frontend
```

## Database Monitoring

### PostgreSQL Queries

```sql
-- Slow queries
SELECT query, calls, total_time, mean_time
FROM pg_stat_statements
ORDER BY total_time DESC LIMIT 10;

-- Active connections
SELECT count(*) FROM pg_stat_activity;
```

## Redis Monitoring

```bash
# Connect to Redis
docker exec -it redis redis-cli

# Monitor commands
MONITOR

# Get stats
INFO stats
```

## Health Checks

```bash
# Backend health
curl http://localhost:8080/health

# Database health
docker exec converter_db pg_isready
```
