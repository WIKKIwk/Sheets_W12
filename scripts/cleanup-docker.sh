#!/bin/bash
# Docker Cleanup Script
# Removes unused containers, images, and volumes

echo "🧹 Cleaning up Docker resources..."

# Stop all containers
echo "Stopping containers..."
docker compose down

# Remove unused images
echo "Removing unused images..."
docker image prune -f

# Remove unused volumes
echo "Removing unused volumes..."
docker volume prune -f

echo "✅ Cleanup complete!"
docker system df
