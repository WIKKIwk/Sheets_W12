"""
W12C Sheets - JavaScript / Node.js Client
"""

const axios = require('axios');

class W12CClient {
    constructor(baseURL = 'http://localhost:8001') {
        this.baseURL = baseURL;
        this.token = null;
    }

    async register(email, password, fullName) {
        const response = await axios.post(`${this.baseURL}/register`, {
            email,
            password,
            full_name: fullName
        });
        this.token = response.data.token;
        return response.data;
    }

    async login(email, password) {
        const response = await axios.post(`${this.baseURL}/login`, {
            email,
            password
        });
        this.token = response.data.token;
        return response.data;
    }

    async getFiles() {
        const response = await axios.get(`${this.baseURL}/api/files`, {
            headers: { Authorization: `Bearer ${this.token}` }
        });
        return response.data;
    }

    async createFile(name) {
        const response = await axios.post(
            `${this.baseURL}/api/files`,
            { name },
            { headers: { Authorization: `Bearer ${this.token}` } }
        );
        return response.data;
    }
}

module.exports = W12CClient;

// Example usage
async function example() {
    const client = new W12CClient();
    await client.login('demo@example.com', 'password');
    const files = await client.getFiles();
    console.log('Files:', files);
}
