#!/bin/bash
set -e

echo "Health checking..."
curl -f http://localhost:8080/health || exit 1
echo "OK"
