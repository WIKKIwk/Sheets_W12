#!/bin/bash
set -e

echo "Installing W12C Sheets..."

# Check Docker
if ! command -v docker &> /dev/null; then
  echo "Error: Docker not found"
  exit 1
fi

# Clone repo
if [ ! -d "Sheets_W12" ]; then
  git clone https://github.com/WIKKIwk/Sheets_W12.git
  cd Sheets_W12
else
  cd Sheets_W12
  git pull
fi

# Setup env
cp .env.example .env

# Start services
docker compose up -d

echo "Installation complete!"
echo "Access at: http://localhost:8001"
