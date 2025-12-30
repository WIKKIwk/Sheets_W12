/**
 * Authentication API Examples
 */

const API_URL = 'http://localhost:8001'\;

// Register new user
async function register(email, password, fullName) {
  const response = await fetch(`${API_URL}/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password, full_name: fullName })
  });
  return response.json();
}

// Login
async function login(email, password) {
  const response = await fetch(`${API_URL}/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  const data = await response.json();
  return data.token;
}

// Usage
// const token = await login('user@example.com', 'password');
