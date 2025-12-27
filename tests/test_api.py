"""
W12C Sheets API Integration Tests
"""

import unittest
import requests

class TestAuthAPI(unittest.TestCase):
    BASE_URL = "http://localhost:8001"
    
    def test_health_endpoint(self):
        """Test health check endpoint"""
        response = requests.get(f"{self.BASE_URL}/health")
        self.assertEqual(response.status_code, 200)
    
    def test_register_new_user(self):
        """Test user registration"""
        payload = {
            "email": "test@example.com",
            "password": "Test123!@#",
            "full_name": "Test User"
        }
        response = requests.post(f"{self.BASE_URL}/register", json=payload)
        # May fail if user exists, that's ok
        self.assertIn(response.status_code, [200, 201, 400])

class TestFilesAPI(unittest.TestCase):
    BASE_URL = "http://localhost:8001"
    
    def setUp(self):
        """Login before each test"""
        response = requests.post(
            f"{self.BASE_URL}/login",
            json={"email": "demo@example.com", "password": "password"}
        )
        if response.status_code == 200:
            self.token = response.json().get("token")
        else:
            self.token = None
    
    def test_get_files_unauthorized(self):
        """Test files endpoint without auth"""
        response = requests.get(f"{self.BASE_URL}/api/files")
        self.assertEqual(response.status_code, 401)

if __name__ == "__main__":
    unittest.main()
