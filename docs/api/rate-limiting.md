# API Rate Limiting

## Limits

### Default Limits
- **Authentication**: 10 requests/minute
- **File operations**: 100 requests/hour
- **AI features**: 50 requests/hour
- **General API**: 1000 requests/hour

## Response Headers

```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1640995200
```

## Error Response

**Status**: 429 Too Many Requests

```json
{
  "error": "Rate limit exceeded",
  "retry_after": 60
}
```

## Implementation

### Backend Configuration

In `.env`:
```
RATE_LIMIT_ENABLED=true
RATE_LIMIT_REQUESTS=100
RATE_LIMIT_WINDOW=3600
```

### Bypass Rate Limiting

For trusted clients, add to whitelist:
```
RATE_LIMIT_WHITELIST=192.168.1.0/24,10.0.0.1
```

## Best Practices

### Client-side
- Cache responses when possible
- Batch requests
- Monitor rate limit headers
- Implement exponential backoff

### Example Client Code

```javascript
async function makeRequest(url) {
  const response = await fetch(url);
  
  if (response.status === 429) {
    const retryAfter = response.headers.get('Retry-After');
    await sleep(retryAfter * 1000);
    return makeRequest(url);  // Retry
  }
  
  return response;
}
```

## Monitoring

Track rate limit hits:
```bash
docker compose logs backend-go | grep "rate limit"
```
