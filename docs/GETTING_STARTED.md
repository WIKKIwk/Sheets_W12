# Getting Started with W12C Sheets

## Quick Start Guide

W12C Sheets is an AI-powered spreadsheet platform with real-time collaboration. This guide will help you get started quickly.

### Prerequisites

- Docker 20.10+
- Docker Compose v2.0+
- 4GB RAM minimum
- Internet connection

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/WIKKIwk/Sheets_W12.git
   cd Sheets_W12
   ```

2. **Configure environment**

   ```bash
   cp .env.example .env
   nano .env
   ```

   Update these critical values:
   - `DB_PASSWORD` - Database password
   - `JWT_SECRET` - JWT signing key
   - `ALLOWED_ORIGINS` - CORS origins

3. **Start services**

   ```bash
   docker compose up -d
   ```

4. **Verify installation**

   ```bash
   docker compose ps
   ```

   All services should show "Up" status.

5. **Access application**
   - Frontend: <http://localhost:8001>
   - API: <http://localhost:8080/health>
   - WebSocket: ws://localhost:4000/socket

## First Steps

### 1. Create an Account

Visit <http://localhost:8001> and click "Sign Up":

- Enter your name, email, and password
- Password must be at least 8 characters
- Confirm your email (if SMTP configured)

### 2. Create Your First Spreadsheet

1. Click "New Spreadsheet" or press `Ctrl+N`
2. Choose a template or start blank
3. Name your spreadsheet
4. Start editing!

### 3. Learn the Interface

**Toolbar Buttons:**

- 📄 New - Create new spreadsheet
- 💾 Save - Save changes (Ctrl+S)
- 🔙 Undo / 🔜 Redo - History navigation
- 🎨 Format - Cell formatting options
- ㄓ Merge - Merge/unmerge cells
- 🤖 AI Assistant - Open AI panel

**Keyboard Shortcuts:**

- `Ctrl+K` - Command palette
- `Ctrl+F` - Find
- `Ctrl+H` - Find and replace
- `Ctrl+Z` - Undo
- `Ctrl+Y` - Redo
- `Ctrl+S` - Save
- `Ctrl+G` - Go to cell

### 4. Use Formulas

W12C Sheets supports 100+ Excel-compatible formulas:

```
=SUM(A1:A10)          # Sum values
=AVERAGE(B1:B10)      # Calculate average
=IF(A1>100,"High","Low")  # Conditional logic
=VLOOKUP(A1,B:C,2,FALSE)  # Lookup values
```

See `/examples/formulas-reference.js` for complete list.

### 5. Enable AI Features

1. Get a Gemini API key from Google AI Studio
2. Open Settings → AI Configuration
3. Enter your API key
4. Start using AI commands!

**AI Command Examples:**

- "Sort this data by column C"
- "Calculate the average in D1"
- "Fill A1:A10 with sequential numbers"
- "Clear all cells in B column"

## Real-time Collaboration

### Share Your Spreadsheet

1. Click the Share button
2. Enter collaborator's email
3. Choose role: Viewer or Editor
4. Send invitation

### See Live Edits

When multiple users edit:

- See live cursors with user names
- Changes appear instantly
- No conflicts - CRDT handles merging

## Version Control

### Create Snapshots

1. Click History icon
2. Click "Create Snapshot"
3. Add description
4. Save

### Restore from History

1. Open History
2. Browse snapshots
3. Preview changes
4. Click Restore

### Branch Mode (Advanced)

For draft changes without affecting others:

1. Click GitBranch icon
2. Create new branch
3. Make changes (real-time disabled)
4. Merge back when ready

## Import/Export

### Import Data

**CSV Import:**

- Click File → Import
- Select CSV file
- Data loads automatically

**Excel Import:**

- Upload .xlsx file
- System converts to CSV
- Import the result

### Export Data

**CSV Export:**

- Click File → Export
- Choose CSV format
- Download file

## API Integration

For programmatic access, see:

- `/examples/auth-basic.js` - Authentication
- `/examples/file-operations.js` - CRUD operations
- `/examples/websocket-realtime.js` - Real-time updates
- `/examples/ai-integration.js` - AI features

## Troubleshooting

### Services won't start

```bash
# Check Docker status
systemctl status docker

# View logs
docker compose logs backend-go
docker compose logs frontend
```

### Can't connect to database

```bash
# Check database status
docker compose ps converter_db

# Test connection
docker exec converter_db psql -U user -d converter_db -c "SELECT 1;"
```

### Frontend shows blank page

```bash
# Rebuild frontend
docker compose up -d --build frontend

# Check browser console for errors
```

## Next Steps

- Read the [API Reference](../README.md#api-reference)
- Explore [Formula Examples](../examples/formulas-reference.js)
- Learn [Best Practices](./best-practices.md)
- Check [Performance Tips](./performance-optimization.md)

## Getting Help

- Documentation: See `/docs` folder
- Examples: See `/examples` folder
- Issues: GitHub Issues
- Community: Discord (if available)

Happy spreadsheeting! 🎉
