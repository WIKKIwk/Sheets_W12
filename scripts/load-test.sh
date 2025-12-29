#!/bin/bash
# Load Testing Script
# Requires: apache2-utils (ab command)

HOST=${1:-http://localhost:8001}
CONCURRENT=${2:-10}
REQUESTS=${3:-100}

echo "🔥 Load Testing W12C Sheets"
echo "=============================="
echo "Host: $HOST"
echo "Concurrent: $CONCURRENT"
echo "Total Requests: $REQUESTS"
echo ""

# Test health endpoint
echo "Testing /health endpoint..."
ab -n $REQUESTS -c $CONCURRENT $HOST/health

echo ""
echo "✅ Load test complete!"
echo ""
echo "Recommendations:"
echo "- If errors > 0: Increase resources"
echo "- If Time per request > 1000ms: Optimize code"
echo "- If Failed requests > 0: Check logs"
