# Environment Variables

## Development (.env)

```bash
# Database
DB_HOST=converter_db
DB_PORT=5432
DB_USER=user
DB_PASSWORD=password
DB_NAME=converter_db

# Backend
PORT=8080
JWT_SECRET=dev-secret-key-min-32-chars
ALLOWED_ORIGINS=http://localhost:8001

# Redis
REDIS_HOST=redis
REDIS_PORT=6379
```

## Production (.env.production)

```bash
# Database
DB_HOST=converter_db
DB_USER=w12c_prod
DB_PASSWORD=<STRONG_PASSWORD>
DB_NAME=w12c_production

# Backend
PORT=8080
JWT_SECRET=<STRONG_SECRET_64_CHARS>
ALLOWED_ORIGINS=https://yourdomain.com
GIN_MODE=release

# Redis
REDIS_PASSWORD=<REDIS_PASSWORD>

# Email
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=noreply@yourdomain.com
SMTP_PASSWORD=<SMTP_PASSWORD>
```

## Security Notes

- Never commit `.env` files
- Use strong passwords (64+ characters)
- Rotate secrets regularly
- Use different secrets per environment
