#!/bin/bash
# API Example Commands using cURL

BASE_URL="http://localhost:8001"
TOKEN=""

# Register new user
echo "1. Register User"
curl -X POST $BASE_URL/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "demo@example.com",
    "password": "SecurePass123!",
    "full_name": "Demo User"
  }'

echo -e "\n\n2. Login"
RESPONSE=$(curl -s -X POST $BASE_URL/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "demo@example.com",
    "password": "SecurePass123!"
  }')

TOKEN=$(echo $RESPONSE | jq -r '.token')
echo "Token: $TOKEN"

echo -e "\n\n3. Get Files"
curl -X GET $BASE_URL/api/files \
  -H "Authorization: Bearer $TOKEN"

echo -e "\n\n4. Create New File"
curl -X POST $BASE_URL/api/files \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Budget 2024"
  }'
