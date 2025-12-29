# Performance Optimization Guide

## Database Optimization

### Indexing
```sql
-- Add index on frequently queried columns
CREATE INDEX idx_files_owner ON files(owner_id);
CREATE INDEX idx_cells_file ON cells(file_id);
```

### Connection Pooling
In .env:
```
DB_POOL_SIZE=20
DB_MAX_OVERFLOW=10
```

### Query Optimization
- Use EXPLAIN ANALYZE for slow queries
- Avoid SELECT *
- Use batch operations
- Implement pagination

## Backend Optimization

### Go Backend
- Use goroutines efficiently
- Implement caching with Redis
- Use connection pooling
- Enable gzip compression

### Caching Strategy
```go
// Cache frequently accessed data
cache.Set("user:123", userData, 10*time.Minute)
```

## Frontend Optimization

### Code Splitting
- Lazy load routes
- Dynamic imports for heavy components
- Tree shaking

### Asset Optimization
- Compress images
- Minify JavaScript/CSS
- Use CDN for static assets
- Implement service worker

## Infrastructure

### Docker Optimization
- Use multi-stage builds
- .dockerignore unnecessary files
- Limit container resources
- Use Alpine images

### Nginx Configuration
```nginx
gzip on;
gzip_types text/plain text/css application/json;
client_max_body_size 10M;
```

## Monitoring

### Key Metrics
- Response time (p50, p95, p99)
- Database query time
- Memory usage
- CPU usage
- Error rate

### Tools
- Docker stats
- PostgreSQL pg_stat_statements
- Application logs
- Profiling tools
