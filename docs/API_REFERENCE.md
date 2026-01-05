# API Reference Guide

Complete reference for W12C Sheets API endpoints.

## Base URL

```
Development: http://localhost:8080
Production:  https://api.yourdomain.com
```

All API requests should use the `/api/v1` prefix (e.g., `/api/v1/login`).

## Authentication

### POST /api/v1/register

Register a new user account.

**Request:**

```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "SecurePassword123!"
}
```

**Response (201 Created):**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "John Doe",
    "email": "john@example.com",
    "created_at": "2026-01-05T10:00:00Z"
  }
}
```

### POST /api/v1/login

Login to existing account.

**Request:**

```json
{
  "email": "john@example.com",
  "password": "SecurePassword123!"
}
```

**Response (200 OK):**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "John Doe",
    "email": "john@example.com"
  }
}
```

### GET /api/v1/me

Get current user profile.

**Headers:**

```
Authorization: Bearer {token}
```

**Response (200 OK):**

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "John Doe",
  "email": "john@example.com",
  "created_at": "2026-01-05T10:00:00Z"
}
```

## Files (Spreadsheets)

### GET /api/v1/files

List all files accessible to the user.

**Headers:**

```
Authorization: Bearer {token}
```

**Query Parameters:**

- `page` (optional): Page number (default: 1)
- `limit` (optional): Items per page (default: 20)

**Response (200 OK):**

```json
{
  "files": [
    {
      "id": "file-123",
      "name": "Budget 2026",
      "owner_id": "user-456",
      "created_at": "2026-01-05T10:00:00Z",
      "updated_at": "2026-01-05T11:30:00Z"
    }
  ],
  "total": 15,
  "page": 1,
  "limit": 20
}
```

### POST /api/v1/files

Create a new spreadsheet file.

**Headers:**

```
Authorization: Bearer {token}
```

**Request:**

```json
{
  "name": "My Spreadsheet",
  "state": {
    "cells": {
      "A1": { "value": "Hello" }
    }
  }
}
```

**Response (201 Created):**

```json
{
  "id": "file-789",
  "name": "My Spreadsheet",
  "owner_id": "user-456",
  "state": { "cells": {} },
  "created_at": "2026-01-05T12:00:00Z"
}
```

### GET /api/v1/files/:id

Get specific file details.

**Headers:**

```
Authorization: Bearer {token}
```

**Response (200 OK):**

```json
{
  "id": "file-123",
  "name": "Budget 2026",
  "owner_id": "user-456",
  "state": {
    "cells": {
      "A1": { "value": "Category" },
      "B1": { "value": "Amount" }
    }
  },
  "created_at": "2026-01-05T10:00:00Z",
  "updated_at": "2026-01-05T11:30:00Z"
}
```

### DELETE /api/v1/files/:id

Delete a file (owner only).

**Headers:**

```
Authorization: Bearer {token}
```

**Response (204 No Content)**

### GET /api/v1/files/:id/cells

Get cells in a specific range.

**Headers:**

```
Authorization: Bearer {token}
```

**Query Parameters:**

- `range`: Cell range (e.g., "A1:D20")

**Response (200 OK):**

```json
{
  "cells": {
    "A1": { "value": "Product", "format": {} },
    "B1": { "value": "Price", "format": {} },
    "A2": { "value": "Apple", "format": {} },
    "B2": { "value": "1.50", "format": {} }
  }
}
```

### PATCH /api/v1/files/:id/cells

Update multiple cells.

**Headers:**

```
Authorization: Bearer {token}
```

**Request:**

```json
{
  "updates": {
    "A1": { "value": "Name" },
    "B1": { "value": "Age" },
    "C1": { "value": "=A1&\" is \"&B1", "formula": "=A1&\" is \"&B1" }
  }
}
```

**Response (200 OK):**

```json
{
  "success": true,
  "updated": 3
}
```

## Sharing

### GET /api/v1/files/:id/shares

List all users with access to the file.

**Headers:**

```
Authorization: Bearer {token}
```

**Response (200 OK):**

```json
{
  "shares": [
    {
      "user_id": "user-123",
      "email": "viewer@example.com",
      "role": "viewer",
      "created_at": "2026-01-05T10:00:00Z"
    }
  ]
}
```

### POST /api/v1/files/:id/shares

Share file with another user.

**Headers:**

```
Authorization: Bearer {token}
```

**Request:**

```json
{
  "email": "colleague@example.com",
  "role": "editor"
}
```

**Roles:**

- `viewer`: Read-only access
- `editor`: Can edit cells
- `owner`: Full control (including delete)

**Response (201 Created):**

```json
{
  "user_id": "user-789",
  "email": "colleague@example.com",
  "role": "editor",
  "created_at": "2026-01-05T12:00:00Z"
}
```

### DELETE /api/v1/files/:id/shares/:userId

Remove user's access to file.

**Headers:**

```
Authorization: Bearer {token}
```

**Response (204 No Content)**

## AI Integration

### POST /api/v1/ai/gemini-key

Store user's Gemini API key.

**Headers:**

```
Authorization: Bearer {token}
```

**Request:**

```json
{
  "gemini_api_key": "AIza..."
}
```

**Response (200 OK):**

```json
{
  "success": true,
  "message": "API key stored successfully"
}
```

### GET /api/v1/ai/gemini-key

Check if user has set Gemini API key.

**Headers:**

```
Authorization: Bearer {token}
```

**Response (200 OK):**

```json
{
  "has_key": true
}
```

### POST /api /v1/ai/generate

Generate AI response (non-streaming).

**Headers:**

```
Authorization: Bearer {token}
```

**Request:**

```json
{
  "prompt": "Sort this data by column B",
  "context": {
    "file_id": "file-123",
    "selected_range": "A1:C10",
    "cells": {
      "A1": { "value": "Name" }
    }
  }
}
```

**Response (200 OK):**

```json
{
  "response": "I'll sort the data...",
  "actions": [
    {
      "type": "sort",
      "range": "A1:C10",
      "column": "B",
      "order": "asc"
    }
  ]
}
```

### POST /api/v1/ai/stream

Generate AI response (streaming via SSE).

**Headers:**

```
Authorization: Bearer {token}
```

**Request:** Same as `/api/v1/ai/generate`

**Response:** Server-Sent Events stream

## Real-time

### POST /api/v1/files/:id/realtime/token

Get WebSocket authentication token.

**Headers:**

```
Authorization: Bearer {token}
```

**Response (200 OK):**

```json
{
  "realtime_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "ws_url": "ws://localhost:4000/socket"
}
```

## Health Check

### GET /health

Check API health status (no auth required).

**Response (200 OK):**

```json
{
  "status": "healthy",
  "timestamp": "2026-01-05T12:00:00Z"
}
```

## File Conversion

### POST /convert

Convert Excel file to CSV.

**Headers:**

```
Content-Type: multipart/form-data
```

**Request:**

```
Form data:
- file: <excel_file.xlsx>
```

**Response (200 OK):**

```
CSV content as text/plain
```

## Error Responses

### 400 Bad Request

```json
{
  "error": "Invalid input",
  "details": "Email is required"
}
```

### 401 Unauthorized

```json
{
  "error": "Unauthorized",
  "details": "Invalid or expired token"
}
```

### 403 Forbidden

```json
{
  "error": "Forbidden",
  "details": "You don't have permission to access this resource"
}
```

### 404 Not Found

```json
{
  "error": "Not found",
  "details": "File not found"
}
```

### 429 Too Many Requests

```json
{
  "error": "Rate limit exceeded",
  "details": "Too many requests, please try again later"
}
```

### 500 Internal Server Error

```json
{
  "error": "Internal server error",
  "details": "An unexpected error occurred"
}
```

## Rate Limiting

- Default: 100 requests per user per second
- Burst: 10 requests
- Headers returned:
  - `X-RateLimit-Limit`: Maximum requests allowed
  - `X-RateLimit-Remaining`: Requests remaining
  - `X-RateLimit-Reset`: Time when limit resets

## Best Practices

1. **Always include Authorization header** for protected endpoints
2. **Handle errors gracefully** - check status codes
3. **Use pagination** for large datasets
4. **Implement retry logic** with exponential backoff
5. **Cache responses** when appropriate
6. **Use HTTPS** in production

## Code Examples

See `/examples` directory for complete working examples in JavaScript.
