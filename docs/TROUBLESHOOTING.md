# Troubleshooting Guide

## Common Issues and Solutions

### Installation & Setup Issues

#### Issue: Docker services fail to start

**Symptoms:**

- `docker compose up` fails
- Services show "Exited" status
- Containers restart repeatedly

**Solutions:**

1. **Check Docker daemon:**

   ```bash
   sudo systemctl status docker
   sudo systemctl start docker
   ```

2. **Check port conflicts:**

   ```bash
   # Check if ports are already in use
   sudo lsof -i :8080  # Go backend
   sudo lsof -i :4000  # Elixir backend
   sudo lsof -i :5432  # PostgreSQL
   sudo lsof -i :6379  # Redis
   sudo lsof -i :8001  # Frontend
   ```

3. **View service logs:**

   ```bash
   docker compose logs backend-go
   docker compose logs converter_db
   docker compose logs redis
   ```

4. **Clean and rebuild:**

   ```bash
   docker compose down -v
   docker compose up -d --build
   ```

#### Issue: Database connection failed

**Symptoms:**

- Backend logs show "connection refused"
- API endpoints return 500 errors
- "dial tcp: no such host" errors

**Solutions:**

1. **Verify database is running:**

   ```bash
   docker compose ps converter_db
   ```

2. **Check database logs:**

   ```bash
   docker compose logs converter_db
   ```

3. **Test database connection:**

   ```bash
   docker exec converter_db psql -U user -d converter_db -c "SELECT 1;"
   ```

4. **Verify environment variables:**

   ```bash
   # Check .env file
   cat .env | grep DB_
   ```

5. **Recreate database:**

   ```bash
   docker compose down -v
   docker volume rm database_v_6.3_postgres_data
   docker compose up -d
   ```

#### Issue: Redis connection errors

**Symptoms:**

- Real-time updates not working
- Backend logs show Redis errors
- "ECONNREFUSED" errors

**Solutions:**

1. **Check Redis status:**

   ```bash
   docker compose ps redis
   docker exec redis redis-cli ping  # Should return "PONG"
   ```

2. **View Redis logs:**

   ```bash
   docker compose logs redis
   ```

3. **Restart Redis:**

   ```bash
   docker compose restart redis
   ```

### Frontend Issues

#### Issue: Blank white page

**Symptoms:**

- Frontend loads but shows nothing
- Browser console shows errors
- React fails to initialize

**Solutions:**

1. **Check browser console:**
   - Open DevTools (F12)
   - Look for JavaScript errors
   - Check Network tab for failed requests

2. **Verify environment variables:**

   ```bash
   # Check if VITE_ variables are set
   cat shlyux/.env
   ```

3. **Rebuild frontend:**

   ```bash
   docker compose up -d --build frontend
   ```

4. **Check API connectivity:**

   ```bash
   curl http://localhost:8080/health
   ```

#### Issue: Real-time updates not working

**Symptoms:**

- Multiple users don't see each other's changes
- WebSocket connection fails
- Presence indicators don't show

**Solutions:**

1. **Check WebSocket connection:**
   - Open browser DevTools → Network → WS tab
   - Look for WebSocket connection to `ws://localhost:4000`

2. **Verify Elixir backend:**

   ```bash
   docker compose logs backend-elixir
   ```

3. **Check firewall:**

   ```bash
   sudo ufw status
   sudo ufw allow 4000/tcp
   ```

4. **Test WebSocket manually:**

   ```javascript
   // Browser console
   const ws = new WebSocket('ws://localhost:4000/socket');
   ws.onopen = () => console.log('Connected');
   ws.onerror = (e) => console.error('Error:', e);
   ```

### API Issues

#### Issue: 401 Unauthorized errors

**Symptoms:**

- API requests return 401
- "Invalid token" errors
- Cannot access protected endpoints

**Solutions:**

1. **Check token expiration:**
   - Tokens expire after 24 hours by default
   - Log in again to get new token

2. **Verify Authorization header:**

   ```bash
   curl -H "Authorization: Bearer YOUR_TOKEN" \
     http://localhost:8080/api/v1/me
   ```

3. **Check JWT_SECRET:**

   ```bash
   # Ensure JWT_SECRET is set and matches
   cat .env | grep JWT_SECRET
   ```

#### Issue: 500 Internal Server Error

**Symptoms:**

- API returns 500 errors
- Unexpected server crashes
- Database query failures

**Solutions:**

1. **Check backend logs:**

   ```bash
   docker compose logs -f backend-go
   ```

2. **Verify database connectivity:**

   ```bash
   docker exec backend-go ping converter_db
   ```

3. **Check database constraints:**
   - Look for foreign key violations
   - Check for duplicate entries
   - Verify data types

#### Issue: CORS errors

**Symptoms:**

- Browser shows CORS policy errors
- "Access-Control-Allow-Origin" missing
- Cross-origin requests blocked

**Solutions:**

1. **Verify ALLOWED_ORIGINS:**

   ```bash
   cat .env | grep ALLOWED_ORIGINS
   ```

2. **Update .env file:**

   ```bash
   ALLOWED_ORIGINS=http://localhost:8001,http://localhost:5173
   ```

3. **Restart backend:**

   ```bash
   docker compose restart backend-go
   ```

### Formula & Calculation Issues

#### Issue: Formulas not calculating

**Symptoms:**

- Formula shows as text
- No calculated value
- #ERROR displayed

**Solutions:**

1. **Check formula syntax:**
   - Must start with `=`
   - Valid function names
   - Proper cell references

2. **Common formula errors:**

   ```javascript
   // Wrong
   =SUM(A1-A10)  // Use : not -
   =AVERAGE A1:A10  // Missing parentheses
   
   // Correct
   =SUM(A1:A10)
   =AVERAGE(A1:A10)
   ```

3. **Check circular references:**
   - Formula cannot reference itself
   - No circular dependency chains

#### Issue: Slow formula calculation

**Symptoms:**

- Spreadsheet freezes
- Long calculation times
- Browser becomes unresponsive

**Solutions:**

1. **Reduce formula complexity:**

   ```javascript
   // Slow: Volatile functions
   =NOW()
   =RAND()
   =INDIRECT()
   
   // Fast: Static functions
   =SUM(A1:A10)
   =AVERAGE(B1:B10)
   ```

2. **Limit range sizes:**

   ```javascript
   // Slow
   =SUM(A:A)  // Entire column
   
   // Fast
   =SUM(A1:A100)  // Specific range
   ```

3. **Use manual calculation mode:**
   - Settings → Calculation → Manual
   - Press F9 to recalculate

### AI Integration Issues

#### Issue: AI not responding

**Symptoms:**

- AI panel doesn't respond
- "API key not set" error
- Requests timeout

**Solutions:**

1. **Verify API key:**
   - Settings → AI Configuration
   - Enter valid Gemini API key
   - Check key has quota remaining

2. **Check API connectivity:**

   ```bash
   curl https://generativelanguage.googleapis.com
   ```

3. **View browser console:**
   - Look for API errors
   - Check network requests
   - Verify error messages

4. **Test with simple prompt:**
   - Try: "What is 2+2?"
   - Verify basic functionality

#### Issue: AI actions fail to apply

**Symptoms:**

- AI generates response but doesn't apply changes
- "Failed to parse AI response" error
- No sheet modifications

**Solutions:**

1. **Select target range:**
   - Select cells before AI command
   - Provides context for AI

2. **Use specific commands:**

   ```javascript
   // Vague
   "Do something with the data"
   
   // Specific
   "Sort A1:C10 by column B in descending order"
   ```

3. **Check sheet permissions:**
   - Must be owner or editor
   - Viewers cannot modify

### Performance Issues

#### Issue: Slow spreadsheet loading

**Symptoms:**

- Takes >5 seconds to load
- Browser freezes during load
- High memory usage

**Solutions:**

1. **Reduce file size:**
   - Delete unused rows/columns
   - Remove unnecessary formulas
   - Clear formatted empty cells

2. **Enable virtual scrolling:**
   - Already enabled by default
   - Verifies only visible cells render

3. **Check browser resources:**

   ```javascript
   // Browser console
   console.log(performance.memory);
   ```

4. **Use lighter formulas:**
   - Avoid array formula
   - Minimize volatile functions

#### Issue: High memory usage

**Symptoms:**

- Browser uses >2GB RAM
- Tab crashes
- System slowdown

**Solutions:**

1. **Close unused tabs:**
   - Each spreadsheet uses memory
   - Close when not in use

2. **Reduce rendered cells:**
   - Scroll less often
   - Use Ctrl+G to jump to cells

3. **Clear browser cache:**

   ```bash
   # Chrome
   Ctrl+Shift+Delete → Clear browsing data
   ```

### Sharing & Collaboration Issues

#### Issue: Cannot share with user

**Symptoms:**

- Share dialog doesn't work
- User not receiving invitation
- "User not found" error

**Solutions:**

1. **Verify email address:**
   - Check for typos
   - Ensure user has account

2. **Check user registration:**

   ```bash
   # User must be registered
   curl http://localhost:8080/api/v1/register \
     -d '{"email":"user@example.com","password":"pass"}'
   ```

3. **Check share permissions:**
   - Only owners can share
   - Verify your role

#### Issue: Real-time sync not working

**Symptoms:**

- Changes not appearing for other users
- Delayed updates
- Conflicts

**Solutions:**

1. **Check WebSocket connection:**
   - All users must have active WS connection
   - Check browser DevTools → Network → WS

2. **Verify same file:**
   - All users on same file ID
   - Check URL

3. **Restart WebSocket:**
   - Reload page
   - Reconnects automatically

### Deployment Issues

#### Issue: SSL certificate errors

**Symptoms:**

- HTTPS not working
- Certificate expired
- Browser security warning

**Solutions:**

1. **Renew certificate:**

   ```bash
   sudo certbot renew
   docker compose restart nginx
   ```

2. **Check certificate validity:**

   ```bash
   openssl x509 -in /etc/letsencrypt/live/domain/cert.pem -text -noout
   ```

3. **Test renewal:**

   ```bash
   sudo certbot renew --dry-run
   ```

#### Issue: Production build fails

**Symptoms:**

- `docker compose build` fails
- TypeScript errors
- Go compilation errors

**Solutions:**

1. **Check build logs:**

   ```bash
   docker compose -f docker-compose.prod.yml build --no-cache 2>&1 | tee build.log
   ```

2. **Verify environment variables:**

   ```bash
   cat .env.production
   ```

3. **Clear build cache:**

   ```bash
   docker builder prune -a
   docker compose build --no-cache
   ```

## Diagnostic Commands

### Health Checks

```bash
# Check all services
docker compose ps

# Check API health
curl http://localhost:8080/health

# Check database
docker exec converter_db psql -U user -c "SELECT version();"

# Check Redis
docker exec redis redis-cli ping

# Check disk space
df -h

# Check memory
free -h

# Check logs
docker compose logs --tail=100
```

### Performance Debugging

```bash
# Monitor resource usage
docker stats

# Check database queries
docker exec converter_db psql -U user -d converter_db \
  -c "SELECT * FROM pg_stat_activity;"

# Check slow queries (PostgreSQL)
docker exec converter_db psql -U user -d converter_db \
  -c "SELECT query, calls, total_time FROM pg_stat_statements ORDER BY total_time DESC LIMIT 10;"
```

## Getting Help

If you can't resolve the issue:

1. **Check documentation:**
   - README.md
   - docs/ folder
   - API reference

2. **Search issues:**
   - GitHub Issues
   - Look for similar problems

3. **Provide details:**
   - Error messages
   - Log output
   - Steps to reproduce
   - Environment (OS, Docker version)

4. **Create issue:**
   - Include all diagnostics
   - Screenshots if relevant
   - Minimal reproduction case

## Prevention Tips

- Regular backups
- Monitor logs
- Update dependencies
- Run health checks
- Test before production deploy
- Keep documentation updated
