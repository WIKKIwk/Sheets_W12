# Best Practices for W12C Sheets

## Development Best Practices

### 1. Environment Configuration

**DO:**

- ✅ Use separate `.env` files for dev/prod
- ✅ Generate strong secrets (64+ characters)
- ✅ Never commit `.env` files
- ✅ Use `.env.example` as template

**DON'T:**

- ❌ Use weak passwords in production
- ❌ Share API keys in code
- ❌ Use same secrets for dev/prod

### 2. Authentication & Security

**API Key Management:**

```javascript
// Good: Store in environment
const apiKey = process.env.GEMINI_API_KEY;

// Bad: Hardcoded
const apiKey = "AIza..."; // Never do this!
```

**JWT Tokens:**

- Store tokens in httpOnly cookies or secure storage
- Implement token refresh mechanism
- Set reasonable expiration times (24 hours)
- Validate tokens on every protected route

### 3. Real-time Collaboration

**WebSocket Connection:**

```javascript
// Good: Handle reconnection
client.onclose = () => {
  setTimeout(() => reconnect(), 5000);
};

// Bad: No error handling
client.connect(); // What if it fails?
```

**CRDT Operations:**

- Send granular updates, not entire document
- Batch multiple changes when possible
- Handle network failures gracefully

### 4. Formula Usage

**Performance:**

```javascript
// Good: Efficient formula
=SUMIF(A:A, ">100", B:B)

// Bad: Slow volatile function
=SUM(IF(A:A>100, B:B))  // Array formula
```

**Best Practices:**

- Avoid circular references
- Minimize volatile functions (NOW, RAND)
- Use structured references
- Cache calculated values when possible

### 5. AI Integration

**Context Management:**

```javascript
// Good: Provide relevant context
const context = {
  selectedRange: 'A1:D10',
  headers: ['Name', 'Price', 'Quantity', 'Total'],
};

// Bad: Send entire sheet
const context = entireSpreadsheet; // Too much data!
```

**Prompt Engineering:**

- Be specific and clear
- Provide examples when needed
- Select relevant data range
- Use structured commands

## Architecture Best Practices

### 1. API Design

**RESTful Endpoints:**

```
Good:
POST   /api/v1/files
GET    /api/v1/files/:id
PATCH  /api/v1/files/:id
DELETE /api/v1/files/:id

Bad:
POST   /api/createFile
GET    /api/getFileById?id=123
POST   /api/updateFile
```

**Error Handling:**

```javascript
// Good: Descriptive errors
{
  "error": "Invalid cell range",
  "code": "INVALID_RANGE",
  "details": "Range A1:ZZZ999999 exceeds limits"
}

// Bad: Generic errors
{
  "error": "Bad request"
}
```

### 2. Database Optimization

**Indexing:**

```sql
-- Good: Add indexes for frequent queries
CREATE INDEX idx_files_user_id ON files(user_id);
CREATE INDEX idx_cells_file_id ON cells(file_id);

-- Monitor query performance
EXPLAIN ANALYZE SELECT * FROM files WHERE user_id = 123;
```

**Connection Pooling:**

```go
// Good: Use connection pool
db.SetMaxOpenConns(25)
db.SetMaxIdleConns(5)
db.SetConnMaxLifetime(5 * time.Minute)
```

### 3. Frontend Performance

**Component Optimization:**

```tsx
// Good: Memoize expensive calculations
const gridData = useMemo(() => 
  processGridData(cells), 
  [cells]
);

// Bad: Recalculate on every render
const gridData = processGridData(cells);
```

**Virtualization:**

- Use virtual scrolling for large datasets
- Render only visible cells
- Lazy load off-screen data

### 4. Caching Strategy

**Redis Usage:**

```javascript
// Cache frequently accessed data
const cacheKey = `file:${fileId}:cells`;
const ttl = 300; // 5 minutes

// Check cache first
let data = await redis.get(cacheKey);
if (!data) {
  data = await db.getCells(fileId);
  await redis.setex(cacheKey, ttl, JSON.stringify(data));
}
```

## Deployment Best Practices

### 1. Docker Configuration

**Multi-stage Builds:**

```dockerfile
# Good: Separate build and runtime
FROM golang:1.21 AS builder
WORKDIR /build
COPY . .
RUN go build -o server

FROM alpine:latest
COPY --from=builder /build/server /app/
CMD ["/app/server"]

# Bad: Include build tools in production
FROM golang:1.21
COPY . .
RUN go build -o server
CMD ["./server"]
```

### 2. Environment Variables

**Production Secrets:**

```bash
# Generate strong secrets
openssl rand -base64 64 > jwt_secret.txt
openssl rand -base64 48 > db_password.txt

# Use in docker-compose
secrets:
  jwt_secret:
    file: ./jwt_secret.txt
```

### 3. Monitoring & Logging

**Structured Logging:**

```go
// Good: Structured logs
logger.Info("User login",
  "user_id", userId,
  "ip", clientIP,
  "timestamp", time.Now(),
)

// Bad: String concatenation
fmt.Printf("User %d logged in from %s\n", userId, clientIP)
```

**Health Checks:**

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
  interval: 30s
  timeout: 10s
  retries: 3
```

### 4. Backup Strategy

**Automated Backups:**

```bash
# Daily backups at 2 AM
0 2 * * * /app/scripts/backup.sh

# Retain last 30 days
find /backups -mtime +30 -delete
```

## Testing Best Practices

### 1. Unit Tests

**Test Coverage:**

```go
// Good: Test edge cases
func TestCellValidation(t *testing.T) {
  tests := []struct{
    cell string
    valid bool
  }{
    {"A1", true},
    {"Z999", true},
    {"AA1", true},
    {"", false},
    {"1A", false},
  }
  
  for _, tt := range tests {
    result := ValidateCell(tt.cell)
    assert.Equal(t, tt.valid, result)
  }
}
```

### 2. Integration Tests

**API Testing:**

```javascript
describe('File API', () => {
  it('should create and retrieve file', async () => {
    const file = await createFile(token, 'Test');
    expect(file.id).toBeDefined();
    
    const retrieved = await getFile(token, file.id);
    expect(retrieved.name).toBe('Test');
  });
});
```

### 3. E2E Tests

**User Workflows:**

```javascript
test('User can create spreadsheet and add formulas', async () => {
  await page.goto('http://localhost:8001');
  await page.click('[data-testid="new-file"]');
  await page.fill('[data-testid="cell-A1"]', '=SUM(B1:B10)');
  await page.keyboard.press('Enter');
  
  const value = await page.textContent('[data-testid="cell-A1"]');
  expect(value).toBe('0');
});
```

## Performance Optimization

### 1. Database Queries

**Batch Operations:**

```go
// Good: Batch insert
tx.Create(&cells) // Insert multiple rows

// Bad: Loop inserts
for _, cell := range cells {
  tx.Create(&cell) // Individual inserts
}
```

### 2. Frontend Rendering

**Debounce Updates:**

```javascript
// Good: Debounce cell updates
const debouncedUpdate = debounce((cell, value) => {
  updateCell(cell, value);
}, 300);

// Bad: Update on every keystroke
onChange={(e) => updateCell(cell, e.target.value)}
```

### 3. Network Optimization

**Compression:**

```nginx
# Enable gzip compression
gzip on;
gzip_types text/plain application/json application/javascript;
gzip_min_length 1000;
```

## Security Best Practices

### 1. Input Validation

**Sanitize User Input:**

```go
// Good: Validate and sanitize
func ValidateCellValue(value string) error {
  if len(value) > 32767 {
    return errors.New("value too long")
  }
  // Sanitize formulas
  if strings.HasPrefix(value, "=") {
    return ValidateFormula(value)
  }
  return nil
}
```

### 2. Rate Limiting

**API Protection:**

```go
// Limit requests per user
limiter := rate.NewLimiter(100, 10) // 100 req/s, burst 10

middleware.Use(func(c *gin.Context) {
  if !limiter.Allow() {
    c.JSON(429, gin.H{"error": "Too many requests"})
    c.Abort()
    return
  }
  c.Next()
})
```

### 3. CORS Configuration

```go
// Good: Specific origins
config := cors.Config{
  AllowOrigins: []string{
    "https://yourdomain.com",
    "https://app.yourdomain.com",
  },
  AllowMethods: []string{"GET", "POST", "PUT", "DELETE"},
}

// Bad: Allow all
AllowOrigins: []string{"*"} // Security risk!
```

## Summary

Following these best practices will help you:

- ✅ Build secure and scalable applications
- ✅ Maintain clean and maintainable code
- ✅ Optimize performance
- ✅ Deploy confidently to production

For more details, see:

- [Security Guide](./security-guide.md)
- [Performance Optimization](./performance-optimization.md)
- [API Reference](../README.md#api-reference)
