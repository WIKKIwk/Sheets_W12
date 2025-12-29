/**
 * WebSocket Client Example for W12C Sheets
 * Demonstrates real-time collaboration
 */

class W12CWebSocketClient {
  constructor(url, token) {
    this.url = url;
    this.token = token;
    this.ws = null;
    this.reconnectDelay = 1000;
  }
  
  connect() {
    this.ws = new WebSocket(this.url);
    
    this.ws.onopen = () => {
      console.log('✅ WebSocket connected');
      // Send authentication
      this.send({
        type: 'auth',
        token: this.token
      });
    };
    
    this.ws.onmessage = (event) => {
      const data = JSON.parse(event.data);
      this.handleMessage(data);
    };
    
    this.ws.onerror = (error) => {
      console.error('❌ WebSocket error:', error);
    };
    
    this.ws.onclose = () => {
      console.log('⚠️ WebSocket closed, reconnecting...');
      setTimeout(() => this.connect(), this.reconnectDelay);
    };
  }
  
  send(data) {
    if (this.ws && this.ws.readyState === WebSocket.OPEN) {
      this.ws.send(JSON.stringify(data));
    }
  }
  
  handleMessage(data) {
    switch(data.type) {
      case 'cell_update':
        console.log('Cell updated:', data.cell, data.value);
        break;
      case 'presence':
        console.log('User cursor:', data.user_id, data.cursor);
        break;
      default:
        console.log('Message:', data);
    }
  }
  
  updateCell(fileId, cell, value) {
    this.send({
      type: 'cell_update',
      file_id: fileId,
      cell: cell,
      value: value
    });
  }
  
  disconnect() {
    if (this.ws) {
      this.ws.close();
    }
  }
}

// Example usage:
// const client = new W12CWebSocketClient('ws://localhost:4000/socket', 'TOKEN');
// client.connect();
// client.updateCell('file-uuid', 'A1', 'Hello World');
