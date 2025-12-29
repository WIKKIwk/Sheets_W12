#!/bin/bash
# Environment Switcher
# Quickly switch between dev/staging/prod configurations

ENV=${1:-development}

case $ENV in
  dev|development)
    cp .env.development .env
    echo "✅ Switched to development environment"
    ;;
  staging)
    cp .env.staging .env
    echo "✅ Switched to staging environment"
    ;;
  prod|production)
    cp .env.production .env
    echo "✅ Switched to production environment"
    ;;
  *)
    echo "❌ Unknown environment: $ENV"
    echo "Usage: ./switch-env.sh [dev|staging|prod]"
    exit 1
    ;;
esac

echo ""
echo "Current .env file:"
head -5 .env
