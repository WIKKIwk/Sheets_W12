
```
██╗    ██╗ ██╗██████╗  ██████╗
██║    ██║███║╚════██╗██╔════╝
██║ █╗ ██║╚██║ █████╔╝██║     
██║███╗██║ ██║██╔═══╝ ██║     
╚███╔███╔╝ ██║███████╗╚██████╗
 ╚══╝╚══╝  ╚═╝╚══════╝ ╚═════╝
                               
███████╗██╗  ██╗███████╗███████╗████████╗███████╗
██╔════╝██║  ██║██╔════╝██╔════╝╚══██╔══╝██╔════╝
███████╗███████║█████╗  █████╗     ██║   ███████╗
╚════██║██╔══██║██╔══╝  ██╔══╝     ██║   ╚════██║
███████║██║  ██║███████╗███████╗   ██║   ███████║
╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝   ╚═╝   ╚══════╝
```

# AI-POWERED SPREADSHEET PLATFORM

```
PROJECT: W12C Sheets - Next-Generation Spreadsheet Engine
ARCHITECTURE: Multi-Backend Microservices (Go + Elixir + React)
AI_ENGINE: Google Gemini 2.5 Flash Integration
REAL_TIME: CRDT (Conflict-free Replicated Data Types)
COLLABORATION: Multi-user simultaneous editing
FORMULAS: 100+ Excel/Google Sheets compatible functions
```

---

## SYSTEM OVERVIEW

```
Enterprise-grade spreadsheet platform combining Google Sheets functionality
with AI-powered intelligence. Multi-backend architecture ensures scalability,
real-time collaboration via CRDT technology, and seamless AI integration for
formula generation, data analysis, and intelligent automation.

DEPLOYMENT ARCHITECTURE
┌──────────────────────────────────────────────────────────────┐
│                    Nginx Reverse Proxy                       │
│               SSL/TLS Termination (Let's Encrypt)            │
└────────┬────────────────┬──────────────┬─────────────────────┘
         │                │              │
    ┌────▼─────┐    ┌────▼──────┐  ┌────▼──────────┐
    │  React   │    │   Go      │  │   Elixir      │
    │ Frontend │    │  Backend  │  │   Backend     │
    │ (8001)   │    │  (8080)   │  │   (4000)      │
    │ Vite+TS  │    │  Gin+JWT  │  │Phoenix+CRDT   │
    └──────┬───┘    └─────┬─────┘  └──────┬────────┘
           │              │                │
           │         ┌────▼────────────────▼────┐
           │         │   PostgreSQL 15          │
           │         │   (Primary Database)     │
           │         └─────────┬────────────────┘
           │                   │
           └───────────────────▼────────────────┐
                          ┌────────────┐        │
                          │  Redis 7   │        │
                          │ CRDT Sync  │        │
                          └────────────┘        │
                                                 │
                          ┌────────────────────┐ │
                          │Google Gemini 2.5   │◄┘
                          │Flash API           │
                          └────────────────────┘

Stack Isolation:
├── Frontend: React 19 + TypeScript + Vite + TailwindCSS
├── REST API: Go 1.21 + Gin + GORM + JWT auth
├── Real-time: Elixir + Phoenix Channels + WebSocket
├── Database: PostgreSQL 15 + Redis 7
└── AI: Google GenAI SDK (@google/genai)
```

---

## WHY W12C SHEETS (OVER GOOGLE SHEETS)?

W12C Sheets is built for speed: you should spend time **making decisions**, not fighting spreadsheets.

### FEATURE HIGHLIGHTS (MVP)

- AI “operator” that executes edits (not just suggestions): fill/update cells, copy/move ranges, sort, clear ranges, delete rows/cols, delete whole category blocks (e.g. “RAM”).
- Safety-first: AI action → auto snapshot (restore anytime) + classic undo/redo.
- In-app versioning (local): snapshots + diff + restore (browser storage).
- Branch mode (local): draft branches, checkout, 3-way merge, conflict picker (Main vs Branch). Branch mode disables realtime + server save.
- Real-time collaboration: WebSocket sync + presence + roles (owner/editor/viewer).
- File workflow: New, Templates, Open recent, Rename (owner), Share, Import (CSV + Excel via `/convert`), Export (CSV).
- Power editing: find/replace (`Ctrl+F` / `Ctrl+H`), command palette (`Ctrl+K`), clipboard cut/copy/paste, context menu row/col ops, merge/unmerge, formatting + format painter.

### POWER SHORTCUTS

- `Ctrl+K` — command palette (quick actions + file switch)
- `Ctrl+F` / `Ctrl+H` — find / replace
- `Ctrl+S` — save (disabled in branch mode / viewer)
- `Ctrl+Z` / `Ctrl+Y` — undo / redo
- `Ctrl+G` — jump to cell (focus name box)
- `Ctrl+Shift+N` — templates

### AI COMMAND EXAMPLES

These work best when you also select the relevant range (AI context is intentionally limited for huge sheets).

- `RAM kategoriyasini to‘liq o‘chir (A ustuni bo‘yicha).`
- `A2:C200 ni C ustun bo‘yicha sort qil.`
- `A2:C10 ni A20 ga ko‘chir.`
- `D2 ga A2:A200 yig‘indisini yoz (formula bilan).`

## CAPABILITY MATRIX

### SPREADSHEET ENGINE

```
FORMULA PROCESSING (100+ Functions)
├── Mathematical ............. SUM, AVERAGE, COUNT, MAX, MIN, ROUND, ABS, 
│                             SQRT, POWER, MOD, PRODUCT, SUMPRODUCT
├── Statistical .............. MEDIAN, MODE, STDEV, VAR, SUMIF, COUNTIF,
│                             AVERAGEIF, CORREL, COVARIANCE
├── Logical .................. IF, AND, OR, NOT, XOR, IFS, SWITCH, IFERROR,
│                             IFNA, TRUE, FALSE
├── Text ..................... LEN, UPPER, LOWER, TRIM, CONCATENATE, LEFT,
│                             RIGHT, MID, FIND, SEARCH, REPLACE, SUBSTITUTE
├── Date/Time ................ TODAY, NOW, YEAR, MONTH, DAY, HOUR, MINUTE,
│                             SECOND, DATE, TIME, DAYS, NETWORKDAYS
└── Lookup & Reference ....... VLOOKUP, HLOOKUP, INDEX, MATCH, OFFSET,
                              INDIRECT, ROW, COLUMN, ROWS, COLUMNS

CELL OPERATIONS
├── Copy/Paste ............... Full clipboard support with formats
├── Cut/Move ................. Drag-and-drop cell relocation
├── Formatting ............... Bold, italic, underline, fonts, colors
├── Alignment ................ Left, center, right, top, middle, bottom
├── Borders .................. Cell border styling
├── Merge .................... Merge/unmerge cells
└── Number Formats ........... Currency, percentage, date, custom

DATA TOOLS (CURRENT)
├── Sort ..................... Ascending/descending by column
├── Find & Replace ........... Search + bulk replace (Ctrl+F / Ctrl+H)
├── Insert/Delete rows/cols .. Context menu + keyboard workflows
├── Import/Export ............ CSV import/export; Excel import via /convert
├── Templates ................ Start from presets (Ctrl+Shift+N)
└── Command palette .......... Ctrl+K quick actions + file switch

REAL-TIME COLLABORATION
├── Multi-user editing ....... Simultaneous cell editing (CRDT)
├── Presence awareness ....... Live cursors and user indicators
├── Conflict resolution ...... Automatic merge via CRDT algorithm
├── Change broadcasting ...... WebSocket push notifications
└── User permissions ......... Owner, editor, viewer roles

VERSIONING & REVIEW (MVP - local)
├── Version history ........... Snapshots + diff + restore (browser storage)
├── Branch mode (draft) ....... Create local branches without touching server
├── Merge to main ............. 3-way merge + conflict resolver (main/branch pick)
└── Safety .................... Branch mode disables realtime + server save
```

### AI ASSISTANT (GEMINI 2.5 FLASH)

```
AI WORKFLOWS (SHEET EDITOR + CHAT)
├── Natural language → sheet actions (undoable)
│   ├── Update specific cells (fill, formulas, cleanup)
│   ├── Copy/move ranges (sparse or overwrite)
│   ├── Sort a range
│   ├── Clear a range
│   ├── Delete rows/cols (with shifting)
│   └── Delete category blocks (e.g. "RAM" header → next category header)
├── Auto snapshot before apply (restore anytime)
├── Context awareness (smart window)
│   ├── Used range + header preview + active selection + explicit A1 refs
│   └── Keyword search windows so mid-sheet blocks stay visible (e.g. "RAM")
└── Multimodal inputs (optional)
    ├── Attach image (Gemini can read tables/text)
    └── Attach Excel/CSV (Excel auto-converted to CSV via /convert)

AI CAPABILITIES:
├── Model: Google Gemini 2.5 Flash (multimodal)
├── Context: Smart window (large sheets are auto-truncated; select a range for best results)
├── Execution: Structured JSON actions applied by the UI (no “rewrite whole sheet”)
├── Safety: Auto-snapshot + undo/redo
└── Keys: Per-user Gemini API keys (stored via `/api/v1/ai/gemini-key`, cached locally)
```

---

## TECHNICAL REQUIREMENTS

### RUNTIME ENVIRONMENT

```
Docker Infrastructure:
├── Docker Engine: 20.10+ (BuildKit enabled)
├── Docker Compose: v2.0+ (plugin-based)
├── System RAM: 4GB minimum, 8GB recommended
└── Disk Space: 10GB minimum, 20GB recommended

Operating System:
├── Ubuntu: 20.04 LTS, 22.04 LTS (production)
├── Debian: 11+, 12+
├── macOS: 12+ with Docker Desktop
└── Windows: 11 with WSL2 + Docker Desktop

Network Requirements:
├── Ports: 80, 443, 8001, 8080, 4000, 5432, 6379
├── External API: api.google.com (Gemini API)
└── SSL: Let's Encrypt (production) or self-signed (dev)
```

### TECHNOLOGY STACK

```
FRONTEND (React Application)
├── Framework: React 19.2.0
├── Language: TypeScript 5.x
├── Build Tool: Vite 6.2.x
├── Styling: TailwindCSS 3.4.x
├── Icons: Lucide React
├── HTTP Client: Fetch API
├── AI SDK: @google/genai
└── WebSocket: Native WebSocket API

BACKEND (Go Microservice)
├── Language: Go 1.21+
├── Web Framework: Gin v1.10
├── ORM: GORM v1.31
├── Authentication: JWT (golang-jwt/jwt)
├── Database Driver: pgx (PostgreSQL)
├── Middleware: CORS, rate limiting
├── Validation: go-playground/validator
└── Server: Gin production mode

BACKEND (Elixir Microservice)
├── Language: Elixir 1.15+
├── Framework: Phoenix 1.7+
├── Real-time: Phoenix Channels
├── CRDT: Custom implementation
├── JSON: Jason library
├── HTTP: Plug/Cowboy
└── Database: Ecto (PostgreSQL adapter)

DATABASE LAYER
├── PostgreSQL: 15.x (Alpine)
│   ├── Connection pooling: pgBouncer (optional)
│   ├── Replication: Streaming (production)
│   └── Backups: pg_dump automated
│
└── Redis: 7.x (Alpine)
    ├── Persistence: AOF enabled
    ├── Max Memory: 512MB (configurable)
    └── Eviction: allkeys-lru

DEVOPS & INFRASTRUCTURE
├── Reverse Proxy: Nginx (Alpine)
├── SSL: Certbot (Let's Encrypt)
├── Container Orchestration: Docker Compose
├── CI/CD: GitHub Actions (optional)
├── Monitoring: Docker stats (built-in)
└── Logging: Centralized via Docker logs
```

---

## DEPLOYMENT PROTOCOLS

### [PROTOCOL 1] RAPID DEVELOPMENT SETUP

```bash
# Prerequisites check
docker --version          # Expected: 20.10+
docker compose version    # Expected: v2.0+
node --version            # Expected: 20+ (optional, for local dev)
go version                # Expected: 1.21+ (optional, for local dev)

# Step 1: Clone repository
git clone https://github.com/WIKKIwk/Sheets_W12.git
cd Sheets_W12

# Step 2: Environment configuration
cp .env.example .env
nano .env
# CRITICAL: Set DB credentials, JWT_SECRET, ALLOWED_ORIGINS

# Step 3: Launch all services
docker compose up -d

# Step 4: Verify deployment
docker compose ps
# All services should show "Up" status

# Step 5: Access application
# Frontend: http://localhost:8001
# API: http://localhost:8080/health
# WebSocket: ws://localhost:4000/socket
```

### IN-APP VERSIONING (MVP)

- Version History: toolbar `History` icon → snapshot yaratish, diff ko‘rish, restore qilish
- Branches: toolbar `GitBranch` icon → draft branch, checkout, merge preview (conflict bo‘lsa `Main/Branch` tanlash)
- AI Safety: AI action qo‘llashdan oldin avtomatik snapshot (xohlagan payt restore qilish mumkin)
- Eslatma: hozircha snapshots/branches browser storage’da (local) saqlanadi, kollaboratsiya bilan sync qilinmaydi

**DOCKER COMPOSE SERVICES:**

```
Service          Port    Description
─────────────────────────────────────────────────────
frontend         8001    React + Nginx (development)
backend-go       8080    Go REST API + JWT auth
backend-elixir   4000    Phoenix WebSocket server
converter_db     5439    PostgreSQL database
redis            6379    Redis (CRDT synchronization)
```

**STARTUP SEQUENCE:**

1. PostgreSQL initialization (schema creation)
2. Redis startup (AOF loading)
3. Go backend (database migrations)
4. Elixir backend (channel setup)
5. Frontend build (Vite compilation)
6. Nginx proxy activation

**VERIFICATION COMMANDS:**

```bash
# Health checks
curl http://localhost:8080/health
# Expected: {"status":"healthy"}

# Database connection test
docker exec converter_db psql -U user -d converter_db -c "SELECT 1;"

# Redis connectivity
docker exec redis redis-cli ping
# Expected: PONG

# View logs
docker compose logs -f backend-go
```

### [PROTOCOL 2] PRODUCTION DEPLOYMENT

#### SERVER PROVISIONING

```bash
# Target: Ubuntu 22.04 LTS
# Minimum: 2 vCPU, 4GB RAM, 40GB SSD
# Recommended: 4 vCPU, 8GB RAM, 100GB SSD

# Step 1: System update
sudo apt update && sudo apt upgrade -y

# Step 2: Docker installation
sudo apt install -y docker.io docker-compose-v2 git certbot python3-certbot-nginx

# Step 3: Docker configuration
sudo systemctl enable docker
sudo systemctl start docker
sudo usermod -aG docker $USER
newgrp docker

# Step 4: Firewall setup
sudo ufw allow 22/tcp     # SSH
sudo ufw allow 80/tcp     # HTTP
sudo ufw allow 443/tcp    # HTTPS
sudo ufw enable
```

#### APPLICATION DEPLOYMENT

```bash
# Step 1: Clone repository
cd /opt
sudo git clone https://github.com/WIKKIwk/Sheets_W12.git w12c
cd w12c
sudo chown -R $USER:$USER .

# Step 2: Production environment
cp .env.production.example .env.production
nano .env.production

# CRITICAL CONFIGURATION:
# Generate strong secrets:
echo "DB_PASSWORD=$(openssl rand -base64 48)"
echo "JWT_SECRET=$(openssl rand -base64 64)"
echo "INTERNAL_API_SECRET=$(openssl rand -base64 32)"
echo "ELIXIR_SECRET_KEY_BASE=$(openssl rand -base64 64)"
echo "REDIS_PASSWORD=$(openssl rand -base64 32)"

# Set domain and CORS:
DOMAIN=yourdomain.com
ALLOWED_ORIGINS=https://yourdomain.com,https://www.yourdomain.com
```

#### SSL CERTIFICATE ACQUISITION

```bash
# Step 1: Stop containers temporarily
docker compose -f docker-compose.prod.yml down

# Step 2: Certificate request
sudo certbot certonly --standalone \
  -d yourdomain.com \
  -d www.yourdomain.com \
  -d api.yourdomain.com \
  -d realtime.yourdomain.com \
  --non-interactive \
  --agree-tos \
  --email admin@yourdomain.com

# Step 3: Certificate deployment
mkdir -p nginx/ssl
sudo cp /etc/letsencrypt/live/yourdomain.com/fullchain.pem nginx/ssl/
sudo cp /etc/letsencrypt/live/yourdomain.com/privkey.pem nginx/ssl/
sudo chmod 644 nginx/ssl/*.pem

# Step 4: Auto-renewal setup (already configured by certbot)
sudo certbot renew --dry-run  # Test renewal
```

#### PRODUCTION LAUNCH

```bash
# Step 1: Build production images
docker compose -f docker-compose.prod.yml build --no-cache

# Step 2: Start services
docker compose -f docker-compose.prod.yml up -d

# Step 3: Verify deployment
docker compose -f docker-compose.prod.yml ps
docker compose -f docker-compose.prod.yml logs backend-go | grep "Server started"

# Step 4: Health verification
curl -k https://api.yourdomain.com/health
curl -k https://yourdomain.com

# Step 5: Create first admin user (if registration disabled)
docker compose -f docker-compose.prod.yml exec backend-go /app/server create-admin \
  --email admin@yourdomain.com \
  --password YOUR_SECURE_PASSWORD
```

#### DNS CONFIGURATION

```
Add A records in domain registrar:

Type    Name        Value               TTL
─────────────────────────────────────────────
A       @           SERVER_IP_ADDRESS   3600
A       www         SERVER_IP_ADDRESS   3600
A       api         SERVER_IP_ADDRESS   3600
A       realtime    SERVER_IP_ADDRESS   3600

Verification:
dig yourdomain.com +short
# Should return: SERVER_IP_ADDRESS
```

---

## CONFIGURATION MATRIX

### DEVELOPMENT ENVIRONMENT (.env)

```bash
# Database Configuration
DB_HOST=converter_db
DB_PORT=5432
DB_USER=user
DB_PASSWORD=password
DB_NAME=converter_db
DB_DSN=host=converter_db user=user password=password dbname=converter_db port=5432 sslmode=disable

# Go Backend
PORT=8080
JWT_SECRET=your-development-jwt-secret-key-minimum-32-chars
ALLOWED_ORIGINS=http://localhost:8001,http://localhost:5173
INTERNAL_API_SECRET=dev-internal-secret

# Redis
REDIS_HOST=redis
REDIS_PORT=6379
REDIS_PASSWORD=

# Elixir Backend
ELIXIR_SECRET_KEY_BASE=dev-elixir-secret-base-minimum-64-chars
PHX_HOST=localhost
PHX_PORT=4000

# AI Configuration (Optional - user provides via UI)
# GEMINI_API_KEY=your-gemini-api-key-here
```

### PRODUCTION ENVIRONMENT (.env.production)

```bash
# Database (PostgreSQL)
DB_HOST=converter_db
DB_PORT=5432
DB_USER=w12c_prod
DB_PASSWORD=REPLACE_WITH_GENERATED_PASSWORD_48_CHARS
DB_NAME=w12c_production
DB_DSN=host=converter_db user=w12c_prod password=PASSWORD dbname=w12c_production port=5432 sslmode=require

# Go Backend
PORT=8080
JWT_SECRET=REPLACE_WITH_GENERATED_SECRET_64_CHARS
ALLOWED_ORIGINS=https://yourdomain.com,https://www.yourdomain.com,https://api.yourdomain.com
INTERNAL_API_SECRET=REPLACE_WITH_GENERATED_SECRET_32_CHARS
GIN_MODE=release
TRUSTED_PROXIES=nginx

# Redis
REDIS_HOST=redis
REDIS_PORT=6379
REDIS_PASSWORD=REPLACE_WITH_GENERATED_PASSWORD_32_CHARS

# Elixir Backend
ELIXIR_SECRET_KEY_BASE=REPLACE_WITH_GENERATED_SECRET_64_CHARS
PHX_HOST=realtime.yourdomain.com
PHX_PORT=4000
PHX_SERVER=true
MIX_ENV=prod

# Domain Configuration
DOMAIN=yourdomain.com
API_DOMAIN=api.yourdomain.com
REALTIME_DOMAIN=realtime.yourdomain.com

# Email (SMTP)
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=noreply@yourdomain.com
SMTP_PASSWORD=your-smtp-password
SMTP_FROM=W12C <noreply@yourdomain.com>

# Security
RATE_LIMIT_REQUESTS=100
RATE_LIMIT_WINDOW=3600
BCRYPT_COST=12

# Backup
BACKUP_SCHEDULE=0 2 * * *  # Daily at 2 AM
BACKUP_RETENTION_DAYS=30
```

---

## API REFERENCE

Base URL: `http://localhost:8080`

Notes:
- Preferred API base path: `/api/v1`
- Legacy compatibility path: `/api`
- Protected endpoints require `Authorization: Bearer <jwt_token>`

### AUTHENTICATION

```
POST /api/v1/register   (legacy: /api/register)
Payload: { "name": "John Doe", "email": "user@example.com", "password": "SecurePassword123!" }

POST /api/v1/login      (legacy: /api/login)
Payload: { "email": "user@example.com", "password": "SecurePassword123!" }

GET /api/v1/me          (legacy: /api/me)

POST /api/v1/api-key/generate
```

### FILES

```
GET    /api/v1/files
POST   /api/v1/files              # { id?, name, state }
GET    /api/v1/files/:id
DELETE /api/v1/files/:id

GET    /api/v1/files/:id/cells?range=A1:D20
PATCH  /api/v1/files/:id/cells

GET    /api/v1/files/:id/schema
POST   /api/v1/files/:id/realtime/token
```

### SHARING

```
GET    /api/v1/files/:id/shares
POST   /api/v1/files/:id/shares          # { email, role: "viewer" | "editor" }
DELETE /api/v1/files/:id/shares/:userId
```

### AI

```
GET  /api/v1/ai/gemini-key
POST /api/v1/ai/gemini-key        # { gemini_api_key: "..." }

POST /api/v1/ai/generate
POST /api/v1/ai/stream            # streamed response (SSE)
```

Notes:
- Frontend can also call Gemini directly in the browser via `@google/genai` (key is stored via endpoints above).
- For best AI results on huge sheets, select a smaller range to avoid context truncation.

### CONVERTER (EXCEL → CSV)

```
POST /convert  (multipart/form-data: file=@sheet.xlsx)
```

### HEALTH

```
GET /health  -> {"status":"healthy"}
```

---

## PROJECT STRUCTURE

```
Sheets_W12/
├── backend-go/                 # Go REST API microservice
│   ├── cmd/server/            # Application entry point
│   │   └── main.go            # Server initialization
│   ├── internal/              # Internal packages
│   │   ├── config/            # Configuration management
│   │   ├── handlers/          # HTTP request handlers
│   │   │   ├── auth_handler.go
│   │   │   ├── file_handler.go
│   │   │   ├── ai_handler.go
│   │   │   └── share_handler.go
│   │   ├── models/            # Database models
│   │   │   ├── user.go
│   │   │   ├── file.go
│   │   │   ├── spreadsheet.go
│   │   │   └── share.go
│   │   ├── services/          # Business logic
│   │   │   ├── spreadsheet_service.go
│   │   │   └── email_service.go
│   │   ├── middleware/        # HTTP middleware
│   │   │   └── ratelimit.go
│   │   └── logger/            # Structured logging
│   ├── Dockerfile             # Development container
│   ├── Dockerfile.prod        # Production container
│   ├── go.mod                 # Go module definition
│   └── go.sum                 # Dependency checksums
│
├── backend-elixir/            # Elixir/Phoenix real-time backend
│   ├── lib/                   # Application code
│   │   ├── w12c/      # Core application
│   │   │   ├── application.ex
│   │   │   └── crdt/         # CRDT implementation
│   │   └── w12c_web/  # Web layer
│   │       ├── channels/     # WebSocket channels
│   │       │   └── room_channel.ex
│   │       ├── controllers/
│   │       └── presence.ex   # User presence tracking
│   ├── config/                # Configuration
│   │   ├── dev.exs
│   │   └── prod.exs
│   ├── Dockerfile
│   ├── Dockerfile.prod
│   └── mix.exs               # Elixir project config
│
├── shlyux/                    # React frontend (Turkish: "shlyuz" = gateway)
│   ├── components/            # React components (Grid, Toolbar, AI, modals)
│   ├── utils/                 # API client, formulas engine, realtime client, etc.
│   ├── App.tsx                # Root component
│   ├── index.tsx              # Entry point
│   ├── types.ts               # TypeScript types
│   ├── index.css              # Theme + UI styles
│   ├── animations.css         # UI animations
│   ├── Dockerfile
│   ├── Dockerfile.prod
│   ├── package.json          # Dependencies
│   ├── vite.config.ts        # Vite configuration
│   ├── tailwind.config.cjs   # TailwindCSS config
│   └── tsconfig.json         # TypeScript config
│
├── nginx/                     # Nginx reverse proxy
│   └── nginx.prod.conf       # Production configuration
│
├── scripts/                   # Utility scripts
│   ├── backup.sh             # Database backup
│   ├── restore.sh            # Database restore
│   └── deploy-production.sh  # Production deployment
│
├── docker-compose.yml         # Development orchestration
├── docker-compose.prod.yml    # Production orchestration
├── .env.example               # Development environment template
├── .env.production.example    # Production environment template
├── Makefile                   # Build automation
├── QUICK_START.md             # Quick start guide
├── PRODUCTION_DEPLOYMENT.md   # Production deployment guide
├── INTEGRATIONS.md            # API integration guide
└── README.md                  # This documentation
```

---

## OPERATIONAL PROCEDURES

### MAKEFILE COMMANDS

```bash
# Development
make up ................... Start all services
make down ................. Stop all services
make restart .............. Restart all services
make logs ................. View logs (all services)
make logs-backend ......... View Go backend logs
make logs-elixir .......... View Elixir logs
make logs-frontend ........ View frontend logs

# Build & Deploy
make rebuild-backend ...... Rebuild Go backend
make rebuild-elixir ....... Rebuild Elixir backend
make rebuild-frontend ..... Rebuild React frontend
make rebuild-all .......... Rebuild all services

# Database
make db-shell ............. PostgreSQL interactive shell
make db-migrate ........... Run database migrations (if applicable)
make db-backup ............ Create database backup
make db-restore ........... Restore from backup

# Cleanup
make clean ................ Stop and remove all containers/volumes
make prune ................ Remove unused Docker resources
```

### DOCKER COMPOSE COMMANDS

```bash
# Development
docker compose up -d
docker compose down
docker compose restart backend-go
docker compose logs -f
docker compose ps

# Production
docker compose -f docker-compose.prod.yml up -d --build
docker compose -f docker-compose.prod.yml down
docker compose -f docker-compose.prod.yml logs -f backend-go
docker compose -f docker-compose.prod.yml restart
```

### DATABASE BACKUP & RESTORE

```bash
# Automated backup (runs daily at 2 AM)
docker exec converter_db /backup.sh

# Manual backup
docker exec converter_db pg_dump -U user -d converter_db | gzip > backup_$(date +%Y%m%d_%H%M%S).sql.gz

# List backups
ls -lh backups/

# Restore from backup
gunzip < backup_20250127_020000.sql.gz | docker exec -i converter_db psql -U user -d converter_db

# Or using script
docker exec converter_db /restore.sh /backups/backup_20250127_020000.sql.gz
```

---

## DIAGNOSTIC PROCEDURES

### ISSUE: Service Fails to Start

```bash
# Diagnosis Sequence
[1] Check Docker daemon
    systemctl status docker
    docker info

[2] View service logs
    docker compose logs backend-go
    docker compose logs converter_db
    docker compose logs redis

[3] Check port conflicts
    sudo lsof -i :8080  # Go backend
    sudo lsof -i :4000  # Elixir backend
    sudo lsof -i :5432  # PostgreSQL
    sudo lsof -i :6379  # Redis

[4] Verify disk space
    df -h
    docker system df

Resolution:
# Kill conflicting processes
sudo kill -9 $(sudo lsof -t -i:8080)

# Clean Docker resources
docker compose down -v
docker system prune -af
docker compose up -d --build
```

### ISSUE: Database Connection Failure

```bash
# Diagnosis:
[1] Check PostgreSQL logs
    docker compose logs converter_db | grep ERROR

[2] Test connection
    docker exec converter_db psql -U user -d converter_db -c "SELECT 1;"

[3] Verify credentials
    cat .env | grep DB_

[4] Check network
    docker network inspect w12c_default

Resolution:
# Restart database
docker compose restart converter_db

# Wait for startup (30 seconds)
sleep 30

# Verify health
docker compose exec converter_db pg_isready -U user

# Re-run migrations (if needed)
docker compose exec backend-go /app/server migrate
```

### ISSUE: Real-time Collaboration Not Working

```bash
# Diagnosis:
[1] Check Elixir backend logs
    docker compose logs backend-elixir | grep -i websocket

[2] Test WebSocket connection
    wscat -c ws://localhost:4000/socket
    # Should connect successfully

[3] Verify Redis
    docker exec redis redis-cli ping
    # Expected: PONG

[4] Check CORS settings
    curl -H "Origin: http://localhost:8001" -I http://localhost:4000/socket

Resolution:
# Restart Elixir backend
docker compose restart backend-elixir

# Restart Redis
docker compose restart redis

# Verify ALLOWED_ORIGINS in .env includes frontend URL
nano .env
# Ensure: ALLOWED_ORIGINS=http://localhost:8001,...

# Rebuild and restart
docker compose down
docker compose up -d --build
```

### ISSUE: AI Features Not Working

```bash
# Diagnosis:
# AI requires user to set their own Gemini API key in profile

Resolution Steps:
[1] User must obtain Gemini API key
    - Visit: https://aistudio.google.com/apikey
    - Create new API key

[2] Set API key in W12C
    - Navigate: Profile → Settings → AI Configuration
    - Paste API key → Save

[3] Verify in backend logs
    docker compose logs backend-go | grep "Gemini API"

Common Errors:
- "API key not set" → User needs to add key in profile
- "Invalid API key" → Check key is correct (starts with "AIza...")
- "Quota exceeded" → User hit Gemini API rate limit
- "Request failed" → Network issue or Gemini service down
```

---

## SECURITY CONSIDERATIONS

```
AUTHENTICATION & AUTHORIZATION
├── JWT token-based authentication (HS256)
├── Token expiration: 24 hours (configurable)
├── Password hashing: bcrypt (cost: 12)
├── API key generation: cryptographically secure random
├── Role-based access: Owner, Editor, Viewer
└── Session management: Stateless JWT

DATA PROTECTION
├── Database encryption at rest (PostgreSQL TDE recommended)
├── SSL/TLS encryption in transit (HTTPS mandatory in production)
├── CORS policy enforcement
├── Input validation and sanitization
├── SQL injection prevention (GORM parameterized queries)
└── XSS protection (React auto-escaping)

RATE LIMITING
├── Global: 100 requests/hour per IP
├── Authentication: 10 requests/minute per IP
├── AI endpoints: 50 requests/hour per user
└── File operations: 200 requests/hour per user

API KEY SECURITY
├── User-provided Gemini keys stored encrypted
├── Internal API keys: 256-bit random
├── Key rotation recommended every 90 days
├── Automatic key revocation on suspicious activity
└── No API keys in logs or error messages

PRODUCTION CHECKLIST
┌───────────────────────────────────────────────────┐
│ [X] Strong JWT_SECRET (64+ characters)          │
│ [X] Unique DB_PASSWORD per environment           │
│ [X] SSL certificates installed (Let's Encrypt)   │
│ [X] ALLOWED_ORIGINS restricted to actual domains │
│ [X] GIN_MODE=release in production               │
│ [X] Database backups automated                   │
│ [X] Firewall rules configured (UFW)              │
│ [X] SSH key-based auth (password login disabled) │
│ [X] Regular system updates (unattended-upgrades) │
│ [X] Monitoring enabled (logs, health checks)     │
└───────────────────────────────────────────────────┘
```

---

## PERFORMANCE OPTIMIZATION

```
DATABASE TUNING
├── PostgreSQL configuration
│   ├── shared_buffers: 25% of RAM
│   ├── effective_cache_size: 50% of RAM
│   ├── work_mem: 64MB (per connection)
│   ├── max_connections: 200
│   └── checkpoint_completion_target: 0.9
│
├── Indexing strategy
│   ├── Primary keys: All tables
│   ├── Foreign keys: All relationships
│   ├── Email (users): Unique index
│   ├── File ownership: Composite index (owner_id, created_at)
│   └── Cell lookups: GIN index on JSONB (if used)
│
└── Query optimization
    ├── Use EXPLAIN ANALYZE for slow queries
    ├── Avoid N+1 queries (eager loading)
    ├── Limit result sets with pagination
    └── Cache frequently accessed data (Redis)

FRONTEND OPTIMIZATION
├── Code splitting (Vite automatic)
├── Lazy loading components (React.lazy)
├── Virtual scrolling for large grids
├── Debounced cell updates (300ms)
├── Memoization (React.memo, useMemo)
└── Asset compression (gzip, brotli)

BACKEND OPTIMIZATION
├── Connection pooling (GORM default: 10 connections)
├── Request compression (Gin middleware)
├── Response caching (Redis)
├── Goroutine pooling for concurrent operations
└── Efficient serialization (JSON encoding)

REDIS OPTIMIZATION
├── AOF persistence (fsync every second)
├── Max memory: 512MB with LRU eviction
├── Key expiration for temporary data
└── Connection pooling (go-redis default)

NETWORK OPTIMIZATION
├── HTTP/2 enabled (Nginx)
├── Gzip compression (level 6)
├── Static asset caching (1 year)
├── CDN distribution (optional, Cloudflare)
└── WebSocket compression enabled
```

---

## ROADMAP & ENHANCEMENTS

```
Recently Added (MVP):
├── Version History ............. Snapshots + diff + restore (local)
├── Branches & Merge ............ Draft branches + merge preview (local)
├── AI Sheet Operator ........... Update/copy/move/sort/clear/delete + auto-snapshot
└── AI Context Retrieval ........ Keyword search windows for mid-sheet accuracy

Planned Features:
├── Advanced Formulas ........... Array formulas, lambda functions
├── Pivot Tables ................ Drag-and-drop pivot table builder
├── Macros & Scripts ............ Custom automation (JavaScript)
├── Import/Export ............... XLSX export, Google Sheets sync, PDF export
├── Comments & Annotations ...... Cell-level comments
├── Data Validation Rules ....... Advanced validation (regex, custom)
├── Conditional Formatting ...... Visual rule builder
├── Mobile Apps ................. iOS & Android native apps
├── Offline Mode ................ Service worker + IndexedDB
├── Plugin System ............... Third-party extensions
├── Advanced Charts ............. Gantt, heatmaps, geo maps
└── AI Enhancements ............. Predictive analytics, anomaly detection

Performance Improvements:
├── GraphQL API ................. More efficient data fetching
├── Server-side rendering ....... Faster initial load
├── WebAssembly formulas ........ 10x faster calculations
├── Distributed Redis ........... Multi-instance sync
└── Horizontal scaling .......... Load balancer + multiple backends
```

---

## LICENSE

```
Apache License 2.0

Copyright 2025 W12C Sheets Development Team

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

Full license text: See LICENSE file in repository root.
```

---

## SUPPORT & CONTRIBUTIONS

```
Issue Reporting:
Platform: GitHub Issues
URL: https://github.com/WIKKIwk/Sheets_W12/issues

Required Information:
├── Environment: Development or Production
├── OS: uname -a
├── Docker version: docker --version
├── Error logs: docker compose logs <service>
├── Steps to reproduce
├── Expected vs actual behavior
└── Browser (if frontend issue): version, console errors

Response SLA:
├── Critical (data loss, security): 4 hours
├── High (broken core feature): 24 hours
├── Medium (degraded performance): 72 hours
└── Low (enhancement, minor bug): Best effort

Pull Request Guidelines:
[1] Fork repository
[2] Create feature branch (feature/pivot-tables)
[3] Write tests for new features
[4] Ensure all tests pass
[5] Update documentation
[6] Follow code style (gofmt, prettier, mix format)
[7] Submit PR with detailed description

Development Setup:
# Backend (Go)
cd backend-go
go test ./...

# Backend (Elixir)
cd backend-elixir
mix test

# Frontend (React)
cd shlyux
npm test
```

---

```
PROJECT: W12C Sheets
TYPE: AI-Powered Spreadsheet Platform
VERSION: 1.0.0
LAST_UPDATED: 2025-12-27
STACK: React + Go + Elixir + PostgreSQL + Redis + Gemini AI
ARCHITECTURE: Multi-Backend Microservices
STATUS: PRODUCTION_READY
```

**END DOCUMENTATION**
