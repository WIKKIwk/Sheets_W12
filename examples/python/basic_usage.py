import requests

# Login
response = requests.post("http://localhost:8001/login", json={
    "email": "user@example.com",
    "password": "password"
})
token = response.json()["token"]

# Get files
files = requests.get("http://localhost:8001/api/files", 
    headers={"Authorization": f"Bearer {token}"})
print(files.json())
