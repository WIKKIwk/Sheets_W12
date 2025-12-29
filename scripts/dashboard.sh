#!/bin/bash
# Real-time Monitoring Dashboard

watch -n 2 '
echo "=== W12C Sheets - Live Dashboard ==="
echo ""
echo "📊 Container Status:"
docker compose ps --format "table {{.Name}}\t{{.Status}}"
echo ""
echo "💻 Resource Usage:"
docker stats --no-stream --format "table {{.Name}}\t{{.CPUPerc}}\t{{.MemUsage}}"
echo ""
echo "🌐 Network:"
curl -s http://localhost:8080/health && echo "✅ Backend OK" || echo "❌ Backend Down"
echo ""
echo "🗄️  Database:"
docker exec converter_db pg_isready && echo "✅ DB Ready" || echo "❌ DB Down"
echo ""
echo "Last updated: $(date)"
'
