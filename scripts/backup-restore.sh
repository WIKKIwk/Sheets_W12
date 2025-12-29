#!/bin/bash
# Database Backup & Restore Script

BACKUP_DIR="./backups"
DB_CONTAINER="converter_db"
DB_USER="user"
DB_NAME="converter_db"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

function backup() {
    mkdir -p $BACKUP_DIR
    TIMESTAMP=$(date +%Y%m%d_%H%M%S)
    BACKUP_FILE="$BACKUP_DIR/backup_$TIMESTAMP.sql.gz"
    
    echo -e "${YELLOW}Creating backup...${NC}"
    
    if docker exec $DB_CONTAINER pg_dump -U $DB_USER -d $DB_NAME | gzip > $BACKUP_FILE; then
        SIZE=$(du -h $BACKUP_FILE | cut -f1)
        echo -e "${GREEN}✅ Backup created: $BACKUP_FILE ($SIZE)${NC}"
    else
        echo -e "${RED}❌ Backup failed!${NC}"
        exit 1
    fi
}

function restore() {
    BACKUP_FILE=$1
    
    if [ -z "$BACKUP_FILE" ]; then
        echo -e "${RED}Usage: $0 restore <backup_file>${NC}"
        exit 1
    fi
    
    if [ ! -f "$BACKUP_FILE" ]; then
        echo -e "${RED}Backup file not found: $BACKUP_FILE${NC}"
        exit 1
    fi
    
    echo -e "${YELLOW}Restoring from: $BACKUP_FILE${NC}"
    echo -e "${RED}WARNING: This will overwrite the current database!${NC}"
    read -p "Continue? (yes/no): " CONFIRM
    
    if [ "$CONFIRM" != "yes" ]; then
        echo "Restore cancelled"
        exit 0
    fi
    
    if gunzip < $BACKUP_FILE | docker exec -i $DB_CONTAINER psql -U $DB_USER -d $DB_NAME; then
        echo -e "${GREEN}✅ Database restored successfully${NC}"
    else
        echo -e "${RED}❌ Restore failed!${NC}"
        exit 1
    fi
}

function list_backups() {
    echo -e "${YELLOW}Available backups:${NC}"
    ls -lh $BACKUP_DIR/*.sql.gz 2>/dev/null || echo "No backups found"
}

# Main
case "$1" in
    backup)
        backup
        ;;
    restore)
        restore "$2"
        ;;
    list)
        list_backups
        ;;
    *)
        echo "Usage: $0 {backup|restore|list} [backup_file]"
        echo ""
        echo "Examples:"
        echo "  $0 backup"
        echo "  $0 list"
        echo "  $0 restore backups/backup_20250129.sql.gz"
        exit 1
        ;;
esac
