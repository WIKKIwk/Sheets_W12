/**
 * W12C Sheets TypeScript SDK
 * Client library for interacting with W12C API
 */

interface W12CConfig {
  baseURL: string;
  token?: string;
}

interface User {
  id: number;
  email: string;
  full_name: string;
}

interface File {
  id: string;
  name: string;
  created_at: string;
}

export class W12CClient {
  private baseURL: string;
  private token: string | null = null;

  constructor(config: W12CConfig) {
    this.baseURL = config.baseURL;
    this.token = config.token || null;
  }

  private async request<T>(
    method: string,
    path: string,
    body?: any
  ): Promise<T> {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    if (this.token) {
      headers['Authorization'] = `Bearer ${this.token}`;
    }

    const response = await fetch(`${this.baseURL}${path}`, {
      method,
      headers,
      body: body ? JSON.stringify(body) : undefined,
    });

    if (!response.ok) {
      throw new Error(`API Error: ${response.statusText}`);
    }

    return response.json();
  }

  // Authentication
  async register(email: string, password: string, fullName: string) {
    const data = await this.request<{ token: string; user: User }>(
      'POST',
      '/register',
      { email, password, full_name: fullName }
    );
    this.token = data.token;
    return data;
  }

  async login(email: string, password: string) {
    const data = await this.request<{ token: string; user: User }>(
      'POST',
      '/login',
      { email, password }
    );
    this.token = data.token;
    return data;
  }

  // Files
  async getFiles(): Promise<{ files: File[] }> {
    return this.request('GET', '/api/files');
  }

  async createFile(name: string): Promise<File> {
    return this.request('POST', '/api/files', { name });
  }

  async deleteFile(id: string): Promise<void> {
    return this.request('DELETE', `/api/files/${id}`);
  }
}

// Example usage:
// const client = new W12CClient({ baseURL: 'http://localhost:8001' });
// await client.login('user@example.com', 'password');
// const files = await client.getFiles();
