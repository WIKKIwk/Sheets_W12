# Database Schema

## Users Table
```sql
CREATE TABLE users (
  id SERIAL PRIMARY KEY,
  email VARCHAR(255) UNIQUE NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  full_name VARCHAR(255),
  created_at TIMESTAMP DEFAULT NOW()
);
```

## Files Table
```sql
CREATE TABLE files (
  id UUID PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  owner_id INTEGER REFERENCES users(id),
  created_at TIMESTAMP DEFAULT NOW(),
  updated_at TIMESTAMP DEFAULT NOW()
);
```

## Cells Table
```sql
CREATE TABLE cells (
  file_id UUID REFERENCES files(id),
  cell_ref VARCHAR(10),
  value TEXT,
  formula TEXT,
  PRIMARY KEY (file_id, cell_ref)
);
```
