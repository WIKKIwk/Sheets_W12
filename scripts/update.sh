#!/bin/bash
set -e

echo "Updating W12C Sheets..."

git pull origin main

echo "Rebuilding images..."
docker compose build --no-cache

echo "Restarting services..."
docker compose down
docker compose up -d

echo "Update complete!"
docker compose ps
