/**
 * Basic Authentication Example for W12C Sheets API
 * 
 * This example demonstrates how to:
 * - Register a new user
 * - Login and obtain JWT token
 * - Use the token for authenticated requests
 */

const API_BASE = 'http://localhost:8080/api/v1';

// Example 1: Register a new user
async function registerUser(name, email, password) {
    const response = await fetch(`${API_BASE}/register`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name, email, password }),
    });

    const data = await response.json();
    console.log('Registration response:', data);
    return data;
}

// Example 2: Login and get JWT token
async function loginUser(email, password) {
    const response = await fetch(`${API_BASE}/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
    });

    const data = await response.json();
    console.log('Login response:', data);

    // Store token for future requests
    if (data.token) {
        localStorage.setItem('jwt_token', data.token);
    }

    return data;
}

// Example 3: Get current user profile
async function getCurrentUser(token) {
    const response = await fetch(`${API_BASE}/me`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
    });

    const data = await response.json();
    console.log('Current user:', data);
    return data;
}

// Example 4: Generate API key
async function generateApiKey(token) {
    const response = await fetch(`${API_BASE}/api-key/generate`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
    });

    const data = await response.json();
    console.log('Generated API key:', data);
    return data;
}

// Usage Example
async function main() {
    try {
        // Step 1: Register
        await registerUser('John Doe', 'john@example.com', 'SecurePass123!');

        // Step 2: Login
        const loginData = await loginUser('john@example.com', 'SecurePass123!');
        const token = loginData.token;

        // Step 3: Get user profile
        await getCurrentUser(token);

        // Step 4: Generate API key
        await generateApiKey(token);

    } catch (error) {
        console.error('Error:', error);
    }
}

// Uncomment to run
// main();

module.exports = {
    registerUser,
    loginUser,
    getCurrentUser,
    generateApiKey,
};
