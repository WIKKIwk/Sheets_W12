# API Reference Guide

## Authentication Endpoints

### POST /register

Create a new user account.

**Request Body:**

```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "full_name": "John Doe"
}
```

**Response (200):**

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

Authenticate existing user.

**Request Body:**

```json
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

## File Management

### GET /api/files

List all spreadsheet files.

**Headers:**

- `Authorization: Bearer <token>`

**Response:**

```json
{
  "files": [
    {
      "id": "uuid",
      "name": "Sales Report 2024",
      "created_at": "2025-01-01T00:00:00Z"
    }
  ]
}
```

### POST /api/files

Create new spreadsheet.

### PATCH /api/v1/files/:id/cells

Batch update cells.

**Request Body:**

```json
{
  "updates": [
    {"cell": "A1", "value": "Product"},
    {"cell": "B1", "formula": "=SUM(B2:B10)"}
  ]
}
```
