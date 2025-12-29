#!/bin/bash
# Performance Benchmarking Script

echo "🏃 W12C Sheets - Performance Benchmark"
echo "======================================"
echo ""

# Backend response time
echo "1. Backend Health Endpoint:"
time curl -s http://localhost:8080/health > /dev/null
echo ""

# Database query performance
echo "2. Database Connection:"
time docker exec converter_db psql -U user -d converter_db -c "SELECT 1;" > /dev/null 2>&1
echo ""

# Redis performance
echo "3. Redis Ping:"
time docker exec redis redis-cli ping > /dev/null 2>&1
echo ""

# Memory usage
echo "4. Memory Usage:"
docker stats --no-stream --format "{{.Name}}\t{{.MemUsage}}"
echo ""

# Disk I/O
echo "5. Disk Usage:"
df -h | grep -E "Filesystem|/dev/sda"
echo ""

echo "✅ Benchmark complete!"
