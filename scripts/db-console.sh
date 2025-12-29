#!/bin/bash
# Database Console Helper
# Quick access to PostgreSQL console

set -e

DB_CONTAINER=${1:-converter_db}
DB_USER=${2:-user}
DB_NAME=${3:-converter_db}

echo "Connecting to database..."
echo "Container: $DB_CONTAINER"
echo "User: $DB_USER"
echo "Database: $DB_NAME"
echo ""

docker exec -it $DB_CONTAINER psql -U $DB_USER -d $DB_NAME
