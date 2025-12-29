#!/bin/bash
# Automated Backup Script
# Run via cron for scheduled backups

BACKUP_DIR="./backups"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
DB_CONTAINER="converter_db"
DB_USER="user"
DB_NAME="converter_db"

mkdir -p $BACKUP_DIR

echo "📦 Starting backup at $(date)"

# Database backup
docker exec $DB_CONTAINER pg_dump -U $DB_USER -d $DB_NAME | gzip > "$BACKUP_DIR/db_backup_$TIMESTAMP.sql.gz"

# Cleanup old backups (keep last 30 days)
find $BACKUP_DIR -name "db_backup_*.sql.gz" -mtime +30 -delete

echo "✅ Backup completed: db_backup_$TIMESTAMP.sql.gz"
echo "📊 Backup size: $(du -h $BACKUP_DIR/db_backup_$TIMESTAMP.sql.gz | cut -f1)"
