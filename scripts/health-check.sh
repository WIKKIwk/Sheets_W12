#!/bin/bash
# System Health Check

echo "💚 W12C Sheets - Health Check"
echo "============================="

# Backend
echo -n "Backend: "
curl -sf http://localhost:8080/health > /dev/null && echo "✅ OK" || echo "❌ Down"

# Frontend
echo -n "Frontend: "
curl -sf http://localhost:8001 > /dev/null && echo "✅ OK" || echo "❌ Down"

# Database
echo -n "Database: "
docker exec converter_db pg_isready -q && echo "✅ OK" || echo "❌ Down"

# Redis
echo -n "Redis: "
docker exec redis redis-cli ping > /dev/null 2>&1 && echo "✅ OK" || echo "❌ Down"
