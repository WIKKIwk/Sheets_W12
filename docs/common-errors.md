# Common Errors & Solutions

## Database Connection Errors

### Error: "connection refused"

**Cause:** Database not running or wrong host/port

**Solution:**
```bash
# Check database status
docker compose ps converter_db

# Restart database
docker compose restart converter_db

# Verify connection
docker exec converter_db pg_isready
```

### Error: "password authentication failed"

**Cause:** Wrong database credentials

**Solution:**
```bash
# Check .env file
cat .env | grep DB_PASSWORD

# Update password
# Edit .env and restart services
docker compose restart
```

## API Errors

### Error: 401 Unauthorized

**Cause:** Invalid or expired JWT token

**Solution:**
- Login again to get new token
- Check token expiration time
- Verify JWT_SECRET in .env

### Error: 429 Too Many Requests

**Cause:** Rate limit exceeded

**Solution:**
- Wait for rate limit window to reset
- Check X-RateLimit-Reset header
- Reduce request frequency

## Docker Errors

### Error: "port already allocated"

**Cause:** Port in use by another process

**Solution:**
```bash
# Find process using port
lsof -i :8080

# Kill process or change port in docker-compose.yml
```

### Error: "no space left on device"

**Cause:** Disk full

**Solution:**
```bash
# Clean Docker resources
docker system prune -a

# Remove old images
docker image prune -a
```

## Build Errors

### Error: "go: module not found"

**Solution:**
```bash
cd backend-go
go mod download
go mod tidy
```

### Error: "npm ERR! code ELIFECYCLE"

**Solution:**
```bash
cd shlyux
rm -rf node_modules package-lock.json
npm install
```

## WebSocket Errors

### Error: "WebSocket connection failed"

**Cause:** CORS or network issue

**Solution:**
- Check ALLOWED_ORIGINS in .env
- Verify Elixir backend is running
- Check firewall rules
