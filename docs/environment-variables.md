# Environment Variables Reference

## Database

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `DB_HOST` | Yes | - | Database hostname |
| `DB_PORT` | Yes | `5432` | Database port |
| `DB_USER` | Yes | - | Database username |
| `DB_PASSWORD` | Yes | - | Database password |
| `DB_NAME` | Yes | - | Database name |

## Backend

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `PORT` | No | `8080` | Server port |
| `JWT_SECRET` | Yes | - | JWT signing key (min 32 chars) |
| `ALLOWED_ORIGINS` | Yes | - | CORS allowed origins |
| `GIN_MODE` | No | `debug` | Gin framework mode (debug/release) |
| `LOG_LEVEL` | No | `info` | Logging level |

## Redis

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `REDIS_HOST` | Yes | `localhost` | Redis hostname |
| `REDIS_PORT` | No | `6379` | Redis port |
| `REDIS_PASSWORD` | No | - | Redis password |

## Email

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `SMTP_HOST` | No | - | SMTP server hostname |
| `SMTP_PORT` | No | `587` | SMTP server port |
| `SMTP_USER` | No | - | SMTP username |
| `SMTP_PASSWORD` | No | - | SMTP password |

## AI Features

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `GEMINI_API_KEY` | No | - | Google Gemini API key |

## Rate Limiting

| Variable | Required | Default | Description |
|----------|----------|---------|-------------|
| `RATE_LIMIT_ENABLED` | No | `true` | Enable rate limiting |
| `RATE_LIMIT_REQUESTS` | No | `100` | Max requests per window |
| `RATE_LIMIT_WINDOW` | No | `3600` | Time window in seconds |

## Example .env File

```bash
# Database
DB_HOST=converter_db
DB_PORT=5432
DB_USER=user
DB_PASSWORD=strong_password_here
DB_NAME=w12c_production

# Backend
PORT=8080
JWT_SECRET=your_super_secret_key_minimum_32_characters_long
ALLOWED_ORIGINS=https://yourdomain.com
GIN_MODE=release
LOG_LEVEL=info

# Redis
REDIS_HOST=redis
REDIS_PORT=6379
```
