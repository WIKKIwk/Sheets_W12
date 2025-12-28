#!/bin/bash
# Register
curl -X POST http://localhost:8001/register \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"pass123","full_name":"User"}'

# Login
curl -X POST http://localhost:8001/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"pass123"}'
