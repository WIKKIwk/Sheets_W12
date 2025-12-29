# Database Optimization

## Indexing

### Identify Slow Queries

```sql
-- Enable query stats
CREATE EXTENSION IF NOT EXISTS pg_stat_statements;

-- View slow queries
SELECT query, calls, total_time, mean_time
FROM pg_stat_statements
ORDER BY total_time DESC
LIMIT 10;
```

### Add Indexes

```sql
-- Index for foreign keys
CREATE INDEX idx_files_owner ON files(owner_id);

-- Composite index
CREATE INDEX idx_cells_file_ref ON cells(file_id, cell_ref);

-- Partial index
CREATE INDEX idx_active_users ON users(id) WHERE deleted_at IS NULL;
```

## Query Optimization

### Use EXPLAIN

```sql
EXPLAIN ANALYZE
SELECT * FROM files WHERE owner_id = 123;
```

### Avoid N+1 Queries

```go
// Bad - N+1 queries
for _, file := range files {
    cells := getCells(file.ID)  // Separate query each time
}

// Good - Single query
fileIDs := extractIDs(files)
allCells := getCellsByFileIDs(fileIDs)
```

### Pagination

```sql
-- With offset (slow for large datasets)
SELECT * FROM files ORDER BY created_at OFFSET 1000 LIMIT 10;

-- Cursor-based (better)
SELECT * FROM files 
WHERE created_at < '2025-01-01' 
ORDER BY created_at DESC 
LIMIT 10;
```

## Connection Pooling

```go
// Configure pool
db.SetMaxOpenConns(25)
db.SetMaxIdleConns(5)
db.SetConnMaxLifetime(5 * time.Minute)
```

## Vacuum

```sql
-- Analyze tables
ANALYZE;

-- Vacuum dead tuples
VACUUM ANALYZE;

-- Full vacuum (locks table)
VACUUM FULL;
```

## Partitioning

```sql
-- Partition by range
CREATE TABLE cells_2024 PARTITION OF cells
FOR VALUES FROM ('2024-01-01') TO ('2025-01-01');
```

## Monitoring

```sql
-- Active connections
SELECT count(*) FROM pg_stat_activity;

--- Database size
SELECT pg_size_pretty(pg_database_size('w12c'));

-- Table sizes
SELECT 
  tablename,
  pg_size_pretty(pg_total_relation_size(tablename::regclass))
FROM pg_tables
WHERE schemaname = 'public'
ORDER BY pg_total_relation_size(tablename::regclass) DESC;
```
