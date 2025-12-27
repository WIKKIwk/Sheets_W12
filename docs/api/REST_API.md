# API Documentation

## Authentication

### POST /register

Register a new user account.

**Request:**

```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "full_name": "John Doe"
}
```

**Response:**

```json
{
  "token": "eyJhbGc...",
  "user": {
    "id": 1,
    "email": "user@example.com"
  }
}
```

### POST /login

Authenticate existing user.

**Request:**

```json
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

## Files API

### GET /api/files

List all spreadsheet files for authenticated user.

**Headers:**

- `Authorization: Bearer <token>`

**Response:**

```json
{
  "files": [
    {
      "id": "uuid",
      "name": "My Spreadsheet",
      "created_at": "2025-01-01T00:00:00Z"
    }
  ]
}
```

### POST /api/files

Create new spreadsheet file.

### GET /api/files/:id/cells

Get all cells in a file.

### PATCH /api/v1/files/:id/cells

Batch update cells.
