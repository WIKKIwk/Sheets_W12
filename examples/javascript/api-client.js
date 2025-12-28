// Simple API client
class W12CClient {
  constructor(baseURL = 'http://localhost:8001') {
    this.baseURL = baseURL;
    this.token = null;
  }
  
  async login(email, password) {
    const res = await fetch(`${this.baseURL}/login`, {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify({email, password})
    });
    const data = await res.json();
    this.token = data.token;
    return data;
  }
}
