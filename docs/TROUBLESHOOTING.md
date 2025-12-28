# Troubleshooting Guide

## Common Issues

### Issue: Database Connection Failed

**Symptoms:**

- Backend logs show "connection refused"
- Error: "could not connect to database"

**Solutions:**

```bash
# Check database status
docker compose logs converter_db

# Restart database
docker compose restart converter_db

# Verify credentials in .env
cat .env | grep DB_
```

### Issue: Frontend Not Loading

**Symptoms:**

- Blank white screen
- Console errors

**Solutions:**

```bash
# Check frontend logs
docker compose logs frontend

# Rebuild frontend
docker compose down
docker compose build --no-cache frontend
docker compose up -d
```

### Issue: WebSocket Connection Failed

**Symptoms:**

- Real-time features not working
- "WebSocket failed to connect"

**Solutions:**

```bash
# Check Elixir backend
docker compose logs backend-elixir

# Verify CORS settings
# Ensure ALLOWED_ORIGINS includes frontend URL
```

### Issue: High Memory Usage

**Solutions:**

```bash
# Check container stats
docker stats

# Increase PostgreSQL shared_buffers
# Add to postgresql.conf:
shared_buffers = 256MB

# Restart
docker compose restart converter_db
```

## Getting Help

- GitHub Issues: <https://github.com/WIKKIwk/Sheets_W12/issues>
- Documentation: Check README.md
- Logs: `docker compose logs -f`
