# Deployment Guide

## Local Development

```bash
# 1. Clone repository
git clone https://github.com/WIKKIwk/Sheets_W12.git
cd Sheets_W12

# 2. Configure environment
cp .env.example .env
nano .env

# 3. Start services
docker compose up -d

# 4. Access application
open http://localhost:8001
```

## Production Deployment

### Prerequisites

- Ubuntu 22.04 LTS server
- Docker & Docker Compose installed
- Domain name configured
- SSL certificates

### Steps

1. **Server Setup**

```bash
sudo apt update && sudo apt upgrade -y
sudo apt install docker.io docker-compose-v2
```

1. **Configure DNS**

```
A record: @ → SERVER_IP
A record: api → SERVER_IP
A record: realtime → SERVER_IP
```

1. **SSL Certificates**

```bash
sudo certbot certonly --standalone -d yourdomain.com
```

1. **Deploy**

```bash
docker compose -f docker-compose.prod.yml up -d
```
