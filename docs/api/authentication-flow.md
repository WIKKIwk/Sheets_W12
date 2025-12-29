# Authentication Flow

## Registration Flow

```
User                    Backend                 Database
  |                        |                        |
  |--1. POST /register---->|                        |
  |   {email, password}    |                        |
  |                        |--2. Hash password----->|
  |                        |                        |
  |                        |--3. Insert user------->|
  |                        |                        |
  |                        |<--4. User created------|
  |                        |                        |
  |                        |--5. Generate JWT------>|
  |                        |                        |
  |<--6. Return token------|                        |
  |   {token, user}        |                        |
```

## Login Flow

```
User                    Backend                 Database
  |                        |                        |
  |--1. POST /login------->|                        |
  |   {email, password}    |                        |
  |                        |--2. Query user-------->|
  |                        |                        |
  |                        |<--3. User data---------|
  |                        |                        |
  |                        |--4. Verify password--->|
  |                        |                        |
  |                        |--5. Generate JWT------>|
  |                        |                        |
  |<--6. Return token------|                        |
  |   {token, user}        |                        |
```

## Protected Request Flow

```
User                    Backend                 Database
  |                        |                        |
  |--1. GET /api/files---->|                        |
  |   Authorization: Bearer|                        |
  |                        |--2. Validate JWT------>|
  |                        |                        |
  |                        |--3. Extract user ID--->|
  |                        |                        |
  |                        |--4. Query files------->|
  |                        |                        |
  |                        |<--5. User files--------|
  |                        |                        |
  |<--6. Return files------|                        |
  |   {files: [...]}       |                        |
```

## Token Refresh (Optional)

```
User                    Backend
  |                        |
  |--1. POST /refresh----->|
  |   {refresh_token}      |
  |                        |--2. Validate refresh-->|
  |                        |                        |
  |                        |--3. Generate new JWT-->|
  |                        |                        |
  |<--4. New tokens--------|
  |   {access, refresh}    |
```

## Security Notes

- Passwords hashed with bcrypt (cost 10)
- JWT expires in 24 hours
- Refresh tokens expire in 30 days
- Use HTTPS in production
- Implement rate limiting on auth endpoints
