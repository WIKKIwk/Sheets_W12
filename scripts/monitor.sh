#!/bin/bash
# Performance Monitoring Script
# Monitor system resources and service health

echo "📊 W12C Sheets - Performance Monitor"
echo "===================================="
echo ""

# Docker stats
echo "🐳 Docker Container Stats:"
docker stats --no-stream --format "table {{.Name}}\t{{.CPUPerc}}\t{{.MemUsage}}"
echo ""

# Service health
echo "💚 Service Health:"
curl -s http://localhost:8080/health && echo "✅ Backend: Healthy" || echo "❌ Backend: Down"
curl -s http://localhost:8001 > /dev/null && echo "✅ Frontend: Healthy" || echo "❌ Frontend: Down"
echo ""

# Database connections
echo "🗄️  Database Connections:"
docker exec converter_db psql -U user -d converter_db -c "SELECT count(*) as connections FROM pg_stat_activity;" 2>/dev/null || echo "Database not accessible"
echo ""

# Disk usage
echo "💾 Disk Usage:"
df -h | grep -E "Filesystem|/dev/sda"
echo ""

# Memory usage
echo "🧠 Memory Usage:"
free -h
