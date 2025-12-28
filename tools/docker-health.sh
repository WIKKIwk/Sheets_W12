#!/bin/bash
echo "Checking Docker containers..."
docker compose ps
docker stats --no-stream
