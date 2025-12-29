# Caching Strategy

## Cache Layers

### 1. Browser Cache
- Static assets (CSS, JS, images)
- CDN caching
- Service Worker cache

### 2. Application Cache (Redis)
- User sessions
- API responses
- Computed results
- Real-time data

### 3. Database Cache
- Query result cache
- PostgreSQL shared buffers

## Redis Cache Keys

### Naming Convention
```
{resource}:{id}:{field}
```

### Examples
```
user:123:profile
file:uuid:cells
session:token:data
```

## Cache Invalidation

### Time-based (TTL)
```go
// Cache for 10 minutes
cache.Set("user:123", data, 10*time.Minute)
```

### Event-based
```go
// Invalidate on update
onFileUpdate := func(fileID string) {
    cache.Delete("file:" + fileID)
}
```

### Cache-aside Pattern
```go
func GetUser(id int) (*User, error) {
    // Try cache first
    if cached, exists := cache.Get("user:" + id); exists {
        return cached, nil
    }
    
    // Cache miss - query database
    user, err := db.GetUser(id)
    if err != nil {
        return nil, err
    }
    
    // Update cache
    cache.Set("user:" + id, user, 1*time.Hour)
    
    return user, nil
}
```

## Best Practices

1. **Set appropriate TTLs**
   - Static data: 1 hour - 1 day
   - Dynamic data: 1 - 10 minutes
   - Real-time data: 10 - 60 seconds

2. **Use cache for:**
   - Frequently accessed data
   - Expensive computations
   - External API responses
   - Session data

3. **Don't cache:**
   - Rapidly changing data
   - Sensitive information
   - Large binary data

4. **Monitor cache performance:**
   - Hit rate
   - Miss rate
   - Memory usage
   - Eviction rate
