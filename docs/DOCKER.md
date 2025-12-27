# Docker Best Practices

## Multi-stage Builds

Use multi-stage builds to reduce image size:

```dockerfile
# Build stage
FROM golang:1.21 AS builder
WORKDIR /app
COPY . .
RUN go build -o server

# Runtime stage
FROM alpine:latest
COPY --from=builder /app/server /server
CMD ["/server"]
```

## Layer Caching

- Order COPY commands for optimal caching
- Install dependencies before copying source code

## Security

- Use non-root users
- Scan images for vulnerabilities
- Keep base images updated

## Image Size

- Use Alpine-based images
- Remove unnecessary files
- Use .dockerignore
