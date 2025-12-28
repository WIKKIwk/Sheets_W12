# Backup Strategy

## Automated Backups
- Daily at 2 AM UTC
- Retention: 30 days
- Storage: Local + Cloud

## Manual Backups
```bash
./scripts/backup-db.sh
```

## Restore Process
```bash
docker compose down
gunzip < backup.sql.gz | docker exec -i converter_db psql
docker compose up -d
```

## Disaster Recovery
1. Restore from latest backup
2. Verify data integrity
3. Test critical functions
4. Resume operations
