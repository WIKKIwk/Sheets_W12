# Architecture Deep Dive

## System Architecture Overview

W12C Sheets is a distributed, microservices-based spreadsheet platform with the following key characteristics:

- **Multi-backend architecture**: Go (REST API) + Elixir (Real-time)
- **Modern frontend**: React 19 with TypeScript
- **AI-powered**: Google Gemini 2.5 Flash integration
- **Real-time collaboration**: CRDT-based conflict resolution
- **Scalable**: Designed for horizontal scaling

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                        CLIENT LAYER                         │
│   ┌──────────────────────────────────────────────────────┐ │
│   │  React Frontend (TypeScript + Vite)                  │ │
│   │  - Grid Component (Virtual Scrolling)                │ │
│   │  - Formula Engine (Client-side calculation)          │ │
│   │  - WebSocket Client (Real-time sync)                 │ │
│   │  - AI Panel (@google/genai SDK)                      │ │
│   └──────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            │
                    ┌───────┴────────┐
                    │                │
           ┌────────▼────────┐ ┌────▼──────────┐
           │  Nginx Proxy    │ │   WebSocket   │
           │  (Port 80/443)  │ │  (Port 4000)  │
           └────────┬────────┘ └────┬──────────┘
                    │                │
      ┌─────────────┴────────┐      │
      │                      │      │
┌─────▼──────┐      ┌───────▼──────▼───────┐
│ Go Backend │      │  Elixir Backend      │
│ (Port 8080)│      │  (Phoenix Channels)  │
│            │      │                      │
│ - REST API │      │ - WebSocket Server   │
│ - JWT Auth │      │ - CRDT Engine        │
│ - Business │      │ - Presence Tracking  │
│   Logic    │      │ - Message Broadcasting│
└─────┬──────┘      └─────────┬────────────┘
      │                       │
      └───────────┬───────────┘
                  │
        ┌─────────▼──────────┐
        │  PostgreSQL 15     │
        │  - Users           │
        │  - Files           │
        │  - Cells           │
        │  - Shares          │
        └────────────────────┘
                  │
        ┌─────────▼──────────┐
        │     Redis 7        │
        │  - Session Cache   │
        │  - Real-time State │
        │  - CRDT Ops        │
        └────────────────────┘
```

## Component Architecture

### 1. Frontend (React)

**Technology Stack:**

- React 19.2 with TypeScript
- Vite for build tooling
- TailwindCSS for styling
- Lucide React for icons

**Key Components:**

```typescript
// Component hierarchy
App
├── AuthProvider
├── Router
│   ├── LoginPage
│   ├── RegisterPage
│   └── SpreadsheetPage
│       ├── Toolbar
│       │   ├── FileMenu
│       │   ├── EditMenu
│       │   └── FormatMenu
│       ├── Grid
│       │   ├── ColumnHeaders
│       │   ├── RowHeaders
│       │   └── CellRenderer (virtualized)
│       ├── FormulaBar
│       ├── AIPanel
│       │   ├── ChatInterface
│       │   └── ActionPreview
│       └── VersionControl
│           ├── HistoryPanel
│           └── BranchManager
└── WebSocketProvider
```

**State Management:**

```typescript
// Global state
interface AppState {
  user: User | null;
  currentFile: File | null;
  cells: Map<string, Cell>;
  selections: Selection[];
  formulas: Map<string, Formula>;
  undoStack: Action[];
  redoStack: Action[];
}

// Real-time state
interface RealtimeState {
  connected: boolean;
  presence: Map<string, UserPresence>;
  pendingOps: Operation[];
}
```

**Data Flow:**

```
User Action → Optimistic Update → Local State Update
     │                                      │
     ├──────────────────────────────────────┘
     │
     ├─→ REST API Call (for persistence)
     └─→ WebSocket Message (for broadcast)
```

### 2. Go Backend (REST API)

**Architecture Pattern:** Clean Architecture / Hexagonal

```
cmd/
└── server/
    └── main.go          # Entry point

internal/
├── config/              # Configuration
├── handlers/            # HTTP handlers (controllers)
│   ├── auth_handler.go
│   ├── file_handler.go
│   ├── cell_handler.go
│   └── share_handler.go
├── services/            # Business logic
│   ├── auth_service.go
│   ├── file_service.go
│   └── spreadsheet_service.go
├── models/              # Domain models
│   ├── user.go
│   ├── file.go
│   └── cell.go
├── repository/          # Data access
│   ├── user_repository.go
│   └── file_repository.go
└── middleware/          # HTTP middleware
    ├── auth.go
    ├── cors.go
    └── ratelimit.go
```

**Request Flow:**

```
HTTP Request
    │
    ▼
Middleware Chain
├── CORS
├── Rate Limiting
├── Authentication
└── Logging
    │
    ▼
Handler (Controller)
    │
    ▼
Service (Business Logic)
    │
    ▼
Repository (Data Access)
    │
    ▼
Database
```

**Key Patterns:**

1. **Dependency Injection:**

```go
type FileHandler struct {
  fileService    *services.FileService
  authMiddleware *middleware.AuthMiddleware
}

func NewFileHandler(fs *services.FileService, am *middleware.AuthMiddleware) *FileHandler {
  return &FileHandler{
    fileService:    fs,
    authMiddleware: am,
  }
}
```

1. **Repository Pattern:**

```go
type FileRepository interface {
  Create(file *models.File) error
  FindByID(id string) (*models.File, error)
  Update(id string, updates map[string]interface{}) error
  Delete(id string) error
}
```

### 3. Elixir Backend (Real-time)

**Architecture:** Phoenix Channels + OTP

```
lib/
├── w12c/
│   ├── application.ex      # OTP Application
│   └── crdt/
│       ├── engine.ex       # CRDT implementation
│       └── operations.ex   # CRDT operations
└── w12c_web/
    ├── channels/
    │   └── room_channel.ex # WebSocket handler
    ├── endpoint.ex         # Phoenix endpoint
    └── presence.ex         # User presence
```

**Supervision Tree:**

```
W12C.Application
├── Postgres.Repo
├── Phoenix.PubSub
├── W12CWeb.Endpoint
│   └── Phoenix.Socket
│       └── RoomChannel
└── W12C.Presence
```

**Channel Architecture:**

```elixir
# Client connects
join("room:file_id", %{"token" => token}, socket)
  │
  ▼
# Authenticate & authorize
verify_token(token) → verify_access(user_id, file_id)
  │
  ▼
# Join room & track presence
track_presence(socket, user_id)
  │
  ▼
# Broadcast presence to all
broadcast_presence_diff()

# Client sends update
handle_in("cell_update", payload, socket)
  │
  ▼
# Apply CRDT operation
CRDT.apply_operation(payload)
  │
  ▼
# Broadcast to all users
broadcast("cell_update", payload)
```

### 4. Database Schema

**PostgreSQL Schema:**

```sql
-- Users table
CREATE TABLE users (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name VARCHAR(255) NOT NULL,
  email VARCHAR(255) UNIQUE NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  gemini_key_hash VARCHAR(255),
  created_at TIMESTAMP DEFAULT NOW(),
  updated_at TIMESTAMP DEFAULT NOW()
);

-- Files table
CREATE TABLE files (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name VARCHAR(255) NOT NULL,
  owner_id UUID REFERENCES users(id) ON DELETE CASCADE,
  state JSONB,
  created_at TIMESTAMP DEFAULT NOW(),
  updated_at TIMESTAMP DEFAULT NOW()
);

-- Cells table (for persistence)
CREATE TABLE cells (
  id SERIAL PRIMARY KEY,
  file_id UUID REFERENCES files(id) ON DELETE CASCADE,
  cell_id VARCHAR(10) NOT NULL,
  value TEXT,
  formula TEXT,
  format JSONB,
  created_at TIMESTAMP DEFAULT NOW(),
  updated_at TIMESTAMP DEFAULT NOW(),
  UNIQUE(file_id, cell_id)
);

-- Shares table
CREATE TABLE shares (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  file_id UUID REFERENCES files(id) ON DELETE CASCADE,
  user_id UUID REFERENCES users(id) ON DELETE CASCADE,
  role VARCHAR(20) NOT NULL CHECK (role IN ('viewer', 'editor', 'owner')),
  created_at TIMESTAMP DEFAULT NOW(),
  UNIQUE(file_id, user_id)
);

-- Indexes
CREATE INDEX idx_files_owner_id ON files(owner_id);
CREATE INDEX idx_cells_file_id ON cells(file_id);
CREATE INDEX idx_cells_file_cell ON cells(file_id, cell_id);
CREATE INDEX idx_shares_file_id ON shares(file_id);
CREATE INDEX idx_shares_user_id ON shares(user_id);
```

**Redis Data Structures:**

```
# Session cache
"session:{user_id}" → {user_data}  # 24h TTL

# File cache
"file:{file_id}" → {file_data}     # 5min TTL

# Real-time operations
"crdt:{file_id}" → [operations]    # CRDT op log

# User presence
"presence:{file_id}" → {user_id: timestamp}  # 30s TTL
```

## Data Flow

### 1. File Creation Flow

```
Client                 Go Backend              Database
  │                        │                       │
  │  POST /api/v1/files    │                       │
  ├───────────────────────>│                       │
  │                        │  INSERT INTO files    │
  │                        ├──────────────────────>│
  │                        │<──────────────────────┤
  │                        │  file_id              │
  │  {file_id, name}       │                       │
  │<───────────────────────┤                       │
  │                        │                       │
```

### 2. Real-time Update Flow

```
Client A          WebSocket          Elixir          Database
  │                   │                 │                │
  │  cell_update      │                 │                │
  ├──────────────────>│                 │                │
  │                   │  handle_in()    │                │
  │                   ├────────────────>│                │
  │                   │                 │  CRDT apply    │
  │                   │                 ├───────────────>│
  │                   │                 │<───────────────┤
  │                   │  broadcast()    │                │
  │                   │<────────────────┤                │
  │                   │                 │                │
Client B              │                 │                │
  │                   │                 │                │
  │  cell_update      │                 │                │
  │<──────────────────┤                 │                │
  │                   │                 │                │
```

### 3. AI Command Flow

```
Client          Frontend        Gemini API       Backend
  │                 │                │               │
  │  "Sort data"    │                │               │
  ├────────────────>│                │               │
  │                 │  Generate      │               │
  │                 ├───────────────>│               │
  │                 │<───────────────┤               │
  │                 │  JSON actions  │               │
  │                 │                │               │
  │                 │  Parse & Apply │               │
  │                 │  (local)       │               │
  │  Optimistic UI  │                │               │
  │<────────────────┤                │               │
  │                 │  PATCH cells   │               │
  │                 ├───────────────────────────────>│
  │                 │<───────────────────────────────┤
  │                 │  Success       │               │
```

## CRDT Implementation

W12C Sheets uses **Last-Write-Wins (LWW) CRDT** for conflict resolution:

```elixir
defmodule W12C.CRDT.Operations do
  @doc """
  Apply operation with timestamp
  """
  def apply_op(state, %{
    cell_id: cell_id,
    value: value,
    timestamp: ts,
    user_id: user_id
  }) do
    current = Map.get(state, cell_id)
    
    if current == nil or ts > current.timestamp do
      # New value wins
      Map.put(state, cell_id, %{
        value: value,
        timestamp: ts,
        user_id: user_id
      })
    else
      # Keep existing (newer) value
      state
    end
  end
end
```

**Conflict Resolution:**

```
User A: A1 = "Hello" (timestamp: 100)
User B: A1 = "World" (timestamp: 105)

Result: A1 = "World" (LWW - timestamp 105 wins)
```

## Scaling Considerations

### Horizontal Scaling

1. **Frontend:**
   - Static hosting (CDN)
   - Multiple frontend instances

2. **Go Backend:**
   - Load balancer → Multiple Go instances
   - Stateless design enables easy scaling

3. **Elixir Backend:**
   - Phoenix PubSub with Redis adapter
   - Distributed Erlang cluster

4. **Database:**
   - PostgreSQL replication (Primary-Replica)
   - Connection pooling (pgBouncer)
   - Read replicas for queries

### Performance Optimizations

1. **Caching:**
   - Redis for frequently accessed data
   - Browser caching for static assets

2. **Database:**
   - Indexes on foreign keys
   - Partitioning for large tables

3. **Network:**
   - gzip compression
   - HTTP/2
   - CDN for static assets

## Security Architecture

### Authentication Flow

```
Client                    Backend
  │                          │
  │  POST /login             │
  ├─────────────────────────>│
  │  {email, password}       │
  │                          │
  │                          │  Verify password
  │                          │  (bcrypt)
  │                          │
  │  {token, user}           │
  │<─────────────────────────┤
  │                          │
  │  Store token locally     │
  │                          │
  │  GET /api/v1/files       │
  │  Authorization: Bearer   │
  ├─────────────────────────>│
  │                          │
  │                          │  Validate JWT
  │                          │
  │  {files}                 │
  │<─────────────────────────┤
```

### Authorization Layers

1. **Network Layer:**
   - Nginx reverse proxy
   - Rate limiting
   - SSL termination

2. **Application Layer:**
   - JWT validation
   - Role-based access control
   - Input validation

3. **Data Layer:**
   - Row-level security (RLS)
   - Encrypted sensitive data

## Monitoring & Observability

### Metrics Collection

```
Application
    │
    ▼
Prometheus Metrics
    │
    ▼
Grafana Dashboard
    │
    ▼
Alerts (if threshold exceeded)
```

### Key Metrics

- Request latency (p50, p95, p99)
- Error rate
- Active connections
- Database query time
- Memory usage
- CPU usage

## Deployment Architecture

### Development

```
docker-compose.yml
├── frontend (hot reload)
├── backend-go
├── backend-elixir
├── postgres
└── redis
```

### Production

```
docker-compose.prod.yml
├── nginx (SSL termination)
├── frontend (optimized build)
├── backend-go (N instances)
├── backend-elixir (N instances)
├── postgres (with replication)
└── redis (with persistence)
```

## Future Improvements

1. **Kubernetes deployment**
2. **GraphQL API**
3. **Offline support (IndexedDB)**
4. **More CRDT types (OR-Set, Counter)**
5. **Message queue (RabbitMQ/Kafka)**
6. **Microservices decomposition**

## References

- [CRDT Paper](https://arxiv.org/abs/1805.06358)
- [Phoenix Channels](https://hexdocs.pm/phoenix/channels.html)
- [PostgreSQL Performance](https://wiki.postgresql.org/wiki/Performance_Optimization)
- [Redis Best Practices](https://redis.io/topics/optimization)
