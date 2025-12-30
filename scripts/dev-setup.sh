#!/bin/bash
# Development Environment Setup

echo "🚀 Setting up W12C Sheets development environment..."

# Check Docker
if ! command -v docker &> /dev/null; then
    echo "❌ Docker not found. Please install Docker first."
    exit 1
fi

# Check Docker Compose
if ! docker compose version &> /dev/null; then
    echo "❌ Docker Compose not found."
    exit 1
fi

# Create .env from example
if [ ! -f .env ]; then
    cp .env.example .env
    echo "✅ Created .env file"
fi

# Start services
echo "Starting services..."
docker compose up -d

echo ""
echo "✅ Setup complete!"
echo "Access: http://localhost:8001"
