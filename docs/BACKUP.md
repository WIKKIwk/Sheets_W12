# Backup and Recovery

## Automated Backups

### Database Backup Script

```bash
#!/bin/bash
# Daily backup at 2 AM (configured in docker-compose)
BACKUP_DIR="/backups"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
DB_NAME="converter_db"

pg_dump -U user $DB_NAME | gzip > $BACKUP_DIR/backup_$TIMESTAMP.sql.gz
```

### Retention Policy

- Keep daily backups for 7 days
- Keep weekly backups for 4 weeks
- Keep monthly backups for 12 months

## Manual Backup

```bash
# Create backup
docker exec converter_db pg_dump -U user -d converter_db | gzip > backup.sql.gz

# List backups
ls -lh backups/
```

## Recovery

### Restore from Backup

```bash
# Stop services
docker compose down

# Restore database
gunzip < backup_20250127.sql.gz | docker exec -i converter_db psql -U user -d converter_db

# Restart services
docker compose up -d
```

### Point-in-Time Recovery

For production, enable WAL archiving for PITR capability.
