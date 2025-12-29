# Deployment Guide

## Prerequisites

- Docker & Docker Compose
- Domain name (for production)
- SSL certificates
- Server with minimum 2 vCPU, 4GB RAM

## Production Deployment

### 1. Server Setup

```bash
# Update system
sudo apt update && sudo apt upgrade -y

# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Install Docker Compose
sudo apt install docker-compose-plugin
```

### 2. Clone Repository

```bash
cd /opt
sudo git clone https://github.com/WIKKIwk/Sheets_W12.git
cd Sheets_W12
```

### 3. Configure Environment

```bash
# Copy production environment template
cp .env.production.example .env

# Edit configuration
nano .env

# Required changes:
# - DB_PASSWORD
# - JWT_SECRET  
# - ALLOWED_ORIGINS
# - Domain settings
```

### 4. SSL Certificates

```bash
# Using Let's Encrypt
sudo apt install certbot

# Get certificate
sudo certbot certonly --standalone -d yourdomain.com
```

### 5. Deploy

```bash
# Start services
docker compose -f docker-compose.prod.yml up -d

# Check status
docker compose ps

# View logs
docker compose logs -f
```

### 6. Verify Deployment

```bash
# Health check
curl https://yourdomain.com/health

# Test login
curl https://yourdomain.com/api/files
```

## Monitoring

```bash
# Resource usage
docker stats

# Application logs
docker compose logs -f backend-go

# Database logs
docker compose logs -f converter_db
```

## Backup

```bash
# Manual backup
./scripts/auto-backup.sh

# Schedule automatic backups
crontab -e
# Add line:
# 0 2 * * * /opt/Sheets_W12/scripts/auto-backup.sh
```

## Updating

```bash
# Pull latest changes
git pull

# Rebuild
docker compose down
docker compose up -d --build
```

## Rollback

```bash
# Revert to previous version
git checkout <previous-commit>

# Rebuild
docker compose down
docker compose up -d --build

# Restore database
gunzip < backup.sql.gz | docker exec -i converter_db psql
```
