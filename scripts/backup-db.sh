#!/bin/bash
set -e

BACKUP_DIR="./backups"
mkdir -p $BACKUP_DIR

TIMESTAMP=$(date +%Y%m%d_%H%M%S)
FILE="backup_$TIMESTAMP.sql.gz"

docker exec converter_db pg_dump -U user -d converter_db | gzip > $BACKUP_DIR/$FILE
echo "Backup created: $FILE"
