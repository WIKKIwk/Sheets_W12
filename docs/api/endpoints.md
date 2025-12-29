# API Endpoints Reference

## Authentication

### POST /register
Create new user account

**Request:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "full_name": "John Doe"
}
```

**Response (201):**
```json
{
  "token": "eyJhbGc...",
  "user": {
    "id": 1,
    "email": "user@example.com",
    "full_name": "John Doe"
  }
}
```

### POST /login
Authenticate user

**Request:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

**Response (200):**
```json
{
  "token": "eyJhbGc...",
  "user": {...}
}
```

## Files

### GET /api/files
List all user files

**Headers:**
- `Authorization: Bearer <token>`

**Response (200):**
```json
{
  "files": [
    {
      "id": "uuid",
      "name": "Budget 2024",
      "created_at": "2025-01-01T00:00:00Z"
    }
  ]
}
```

### POST /api/files
Create new spreadsheet file

### GET /api/files/:id
Get file details

### DELETE /api/files/:id
Delete file

## Cells

### GET /api/files/:id/cells
Get all cells in file

### PATCH /api/v1/files/:id/cells
Batch update cells
