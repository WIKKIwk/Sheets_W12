#!/bin/bash
# Test Runner
# Runs all test suites

set -e

echo "🧪 Running test suites..."

# Backend tests
if [ -d "backend-go" ]; then
  echo "Running Go tests..."
  cd backend-go
  go test ./... -v
  cd ..
fi

# Frontend tests
if [ -d "shlyux" ]; then
  echo "Running frontend tests..."
  cd shlyux
  npm test
  cd ..
fi

echo "✅ All tests passed!"
