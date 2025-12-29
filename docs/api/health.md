# Health Check Endpoints

## GET /health

Basic health check endpoint. Returns 200 if service is running.

**Response:**
```json
{
  "status": "ok",
  "timestamp": "2025-01-01T00:00:00Z"
}
```

**Usage:**
```bash
curl http://localhost:8080/health
```

## GET /health/detailed

Detailed health information including dependencies.

**Response:**
```json
{
  "status": "ok",
  "timestamp": "2025-01-01T00:00:00Z",
  "services": {
    "database": "ok",
    "redis": "ok"
  },
  "uptime": 3600
}
```

## Monitoring

Use health endpoints for:
- Load balancer health checks
- Kubernetes liveness probes
- Monitoring systems
- Automated alerts

### Kubernetes Example

```yaml
livenessProbe:
  httpGet:
    path: /health
    port: 8080
  initialDelaySeconds: 30
  periodSeconds: 10
```

### Docker Compose Example

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
  interval: 30s
  timeout: 10s
  retries: 3
```
