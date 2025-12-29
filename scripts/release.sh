#!/bin/bash
# Release Script
# Automates the release process

VERSION=$1

if [ -z "$VERSION" ]; then
  echo "Usage: ./release.sh <version>"
  echo "Example: ./release.sh v1.2.0"
  exit 1
fi

echo "🚀 Preparing release $VERSION"

# Run tests
echo "Running tests..."
./scripts/run-tests.sh || exit 1

# Update CHANGELOG
echo "Update CHANGELOG.md manually, then press Enter"
read

# Create git tag
git tag -a $VERSION -m "Release $VERSION"
git push origin $VERSION

# Build Docker images
echo "Building Docker images..."
docker compose build

# Tag images
docker tag w12c-backend:latest w12c-backend:$VERSION
docker tag w12c-frontend:latest w12c-frontend:$VERSION

echo "✅ Release $VERSION completed!"
echo "Next steps:"
echo "1. Push Docker images to registry"
echo "2. Deploy to production"
echo "3. Monitor deployment"
