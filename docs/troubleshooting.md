# Troubleshooting Guide

## Common Issues

### Docker Container Won't Start

**Issue:** Container exits immediately

**Solution:**
```bash
# Check logs
docker compose logs backend-go

# Check for port conflicts
netstat -tulpn | grep 8080

# Restart services
docker compose down
docker compose up -d
```

### Database Connection Failed

**Issue:** "could not connect to database"

**Solution:**
```bash
# Verify database is running
docker compose ps converter_db

# Check database logs
docker compose logs converter_db

# Test connection
docker exec converter_db pg_isready
```

### Frontend Not Loading

**Issue:** Blank page or 404 errors

**Solution:**
```bash
# Rebuild frontend
docker compose build --no-cache frontend
docker compose up -d frontend

# Check frontend logs
docker compose logs -f frontend
```

### High CPU Usage

**Solution:**
```bash
# Check resource usage
docker stats

# Limit resources in docker-compose.yml
# Add under service:
#   deploy:
#     resources:
#       limits:
#         cpus: '0.5'
#         memory: 512M
```

### WebSocket Connection Failed

**Issue:** Real-time features not working

**Solution:**
- Check CORS settings in .env
- Verify Elixir backend is running
- Check firewall rules for port 4000
