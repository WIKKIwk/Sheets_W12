# Architecture Overview

## System Design

W12C Sheets uses a multi-backend microservices architecture for optimal performance and scalability.

```
┌─────────────────────────────────────────┐
│           Load Balancer (Nginx)         │
└────────┬────────────────────────────────┘
         │
    ┌────┴────┬─────────────┬──────────────┐
    │         │             │              │
┌───▼───┐ ┌──▼───┐  ┌──────▼─────┐  ┌────▼────┐
│React  │ │ Go   │  │  Elixir    │  │  Redis  │
│Frontend│ │Backend│  │  Backend   │  │  Cache  │
└───┬───┘ └──┬───┘  └──────┬─────┘  └────┬────┘
    │        │             │              │
    └────────┴─────────────┴──────────────┘
                           │
                    ┌──────▼──────┐
                    │ PostgreSQL  │
                    └─────────────┘
```

## Components

### Frontend (React + TypeScript)

- **Purpose**: User interface
- **Port**: 8001
- **Tech**: React 19, Vite, TailwindCSS
- **Features**: Spreadsheet grid, AI sidebar, real-time updates

### Go Backend

- **Purpose**: REST API, authentication
- **Port**: 8080
- **Tech**: Gin, GORM, JWT
- **Responsibilities**: CRUD operations, user management

### Elixir Backend

- **Purpose**: Real-time collaboration
- **Port**: 4000
- **Tech**: Phoenix, Channels, CRDT
- **Responsibilities**: WebSocket connections, presence

### PostgreSQL

- **Purpose**: Primary data storage
- **Port**: 5432
- **Data**: Users, files, cells, permissions

### Redis

- **Purpose**: Caching, CRDT sync
- **Port**: 6379
- **Data**: Session cache, real-time state
