#!/bin/bash
# Configuration Validator
# Checks if all required environment variables are set

echo "🔍 Validating configuration..."

REQUIRED_VARS=(
  "DB_HOST"
  "DB_PORT"
  "DB_USER"
  "DB_PASSWORD"
  "DB_NAME"
  "JWT_SECRET"
  "ALLOWED_ORIGINS"
)

ERRORS=0

# Load .env file
if [ -f .env ]; then
  source .env
else
  echo "❌ .env file not found!"
  exit 1
fi

# Check each required variable
for var in "${REQUIRED_VARS[@]}"; do
  if [ -z "${!var}" ]; then
    echo "❌ Missing: $var"
    ERRORS=$((ERRORS + 1))
  else
    echo "✅ Found: $var"
  fi
done

# Security checks
if [ ${#JWT_SECRET} -lt 32 ]; then
  echo "⚠️  WARNING: JWT_SECRET should be at least 32 characters"
  ERRORS=$((ERRORS + 1))
fi

if [ "$ERRORS" -eq 0 ]; then
  echo ""
  echo "✅ Configuration is valid!"
  exit 0
else
  echo ""
  echo "❌ Found $ERRORS error(s)"
  exit 1
fi
