#!/usr/bin/env python3
"""
W12C Sheets Python Client Example
Demonstrates how to interact with W12C API using Python
"""

import requests
import json

class W12CClient:
    def __init__(self, base_url="http://localhost:8001"):
        self.base_url = base_url
        self.token = None
    
    def register(self, email, password, full_name):
        """Register a new user"""
        response = requests.post(
            f"{self.base_url}/register",
            json={
                "email": email,
                "password": password,
                "full_name": full_name
            }
        )
        data = response.json()
        self.token = data.get("token")
        return data
    
    def login(self, email, password):
        """Login existing user"""
        response = requests.post(
            f"{self.base_url}/login",
            json={"email": email, "password": password}
        )
        data = response.json()
        self.token = data.get("token")
        return data
    
    def get_files(self):
        """Get all files for authenticated user"""
        headers = {"Authorization": f"Bearer {self.token}"}
        response = requests.get(
            f"{self.base_url}/api/files",
            headers=headers
        )
        return response.json()
    
    def create_file(self, name):
        """Create a new spreadsheet file"""
        headers = {"Authorization": f"Bearer {self.token}"}
        response = requests.post(
            f"{self.base_url}/api/files",
            headers=headers,
            json={"name": name}
        )
        return response.json()

# Example usage
if __name__ == "__main__":
    client = W12CClient()
    
    # Register or login
    client.login("demo@example.com", "password123")
    
    # Get files
    files = client.get_files()
    print("Files:", json.dumps(files, indent=2))
    
    # Create new file
    new_file = client.create_file("Budget 2025")
    print("Created:", new_file)
