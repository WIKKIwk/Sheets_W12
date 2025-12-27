# Upgrade Guide

## Version 1.0 to 1.1

### Breaking Changes

None

### New Features

- Advanced formulas
- Pivot tables
- Enhanced AI features

### Migration Steps

1. **Backup your data**

```bash
docker exec converter_db pg_dump > backup_pre_upgrade.sql
```

1. **Pull latest changes**

```bash
git pull origin main
```

1. **Rebuild images**

```bash
docker compose down
docker compose build --no-cache
docker compose up -d
```

1. **Run migrations**

```bash
docker compose exec backend-go /app/server migrate
```

1. **Verify upgrade**

```bash
curl http://localhost:8080/health
```

## Rollback Procedure

If upgrade fails:

```bash
docker compose down
git checkout v1.0.0
docker compose up -d
psql -U user -d converter_db < backup_pre_upgrade.sql
```
