#!/bin/bash
set -e

echo "Running linters..."

# Go
if [ -d "backend-go" ]; then
  echo "Running gofmt..."
  gofmt -l backend-go/
fi

# TypeScript
if [ -d "shlyux" ]; then
  echo "Running eslint..."
  cd shlyux && npm run lint
fi

echo "Linting complete!"
