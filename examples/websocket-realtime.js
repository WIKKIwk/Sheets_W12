/**
 * WebSocket Real-time Collaboration Example for W12C Sheets
 * 
 * This example demonstrates how to:
 * - Connect to WebSocket for real-time updates
 * - Join a spreadsheet room
 * - Send and receive cell updates
 * - Handle presence (see other users)
 * - Handle disconnection and reconnection
 */

// WebSocket server URL
const WS_URL = 'ws://localhost:4000/socket';

class RealtimeClient {
    constructor(realtimeToken, fileId) {
        this.token = realtimeToken;
        this.fileId = fileId;
        this.socket = null;
        this.channel = null;
        this.userId = null;
    }

    // Connect to WebSocket server
    connect() {
        return new Promise((resolve, reject) => {
            this.socket = new WebSocket(WS_URL);

            this.socket.onopen = () => {
                console.log('WebSocket connected');

                // Send join message
                this.joinRoom();
                resolve();
            };

            this.socket.onmessage = (event) => {
                this.handleMessage(JSON.parse(event.data));
            };

            this.socket.onerror = (error) => {
                console.error('WebSocket error:', error);
                reject(error);
            };

            this.socket.onclose = () => {
                console.log('WebSocket disconnected');
                this.handleDisconnection();
            };
        });
    }

    // Join a spreadsheet room
    joinRoom() {
        const joinMessage = {
            topic: `room:${this.fileId}`,
            event: 'phx_join',
            payload: { token: this.token },
            ref: Date.now().toString(),
        };

        this.socket.send(JSON.stringify(joinMessage));
    }

    // Send cell update
    updateCell(cellId, value, formula = null) {
        const updateMessage = {
            topic: `room:${this.fileId}`,
            event: 'cell_update',
            payload: {
                cell_id: cellId,
                value: value,
                formula: formula,
                user_id: this.userId,
            },
            ref: Date.now().toString(),
        };

        this.socket.send(JSON.stringify(updateMessage));
    }

    // Send bulk updates
    bulkUpdate(updates) {
        const bulkMessage = {
            topic: `room:${this.fileId}`,
            event: 'bulk_update',
            payload: { updates },
            ref: Date.now().toString(),
        };

        this.socket.send(JSON.stringify(bulkMessage));
    }

    // Handle incoming messages
    handleMessage(message) {
        switch (message.event) {
            case 'phx_reply':
                if (message.payload.status === 'ok') {
                    console.log('Successfully joined room');
                    this.userId = message.payload.response.user_id;
                }
                break;

            case 'cell_update':
                console.log('Cell updated:', message.payload);
                this.onCellUpdate(message.payload);
                break;

            case 'bulk_update':
                console.log('Bulk update received:', message.payload);
                this.onBulkUpdate(message.payload);
                break;

            case 'presence_state':
                console.log('Current users in room:', message.payload);
                this.onPresenceState(message.payload);
                break;

            case 'presence_diff':
                console.log('User joined/left:', message.payload);
                this.onPresenceDiff(message.payload);
                break;

            default:
                console.log('Unknown message:', message);
        }
    }

    // Callbacks (override these in your app)
    onCellUpdate(payload) {
        // Update your local state
        console.log(`Cell ${payload.cell_id} updated to: ${payload.value}`);
    }

    onBulkUpdate(payload) {
        // Update multiple cells
        console.log('Multiple cells updated:', payload.updates);
    }

    onPresenceState(payload) {
        // Show all users currently in the room
        console.log('Users online:', Object.keys(payload));
    }

    onPresenceDiff(payload) {
        // Handle user join/leave
        if (payload.joins) {
            console.log('Users joined:', Object.keys(payload.joins));
        }
        if (payload.leaves) {
            console.log('Users left:', Object.keys(payload.leaves));
        }
    }

    handleDisconnection() {
        // Attempt to reconnect after 5 seconds
        console.log('Attempting to reconnect in 5 seconds...');
        setTimeout(() => {
            this.connect();
        }, 5000);
    }

    // Disconnect from WebSocket
    disconnect() {
        if (this.socket) {
            this.socket.close();
        }
    }
}

// Usage Example
async function main() {
    try {
        // First, get realtime token from REST API
        const token = 'YOUR_JWT_TOKEN';
        const fileId = 'YOUR_FILE_ID';

        const response = await fetch(`http://localhost:8080/api/v1/files/${fileId}/realtime/token`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
        });

        const { realtime_token } = await response.json();

        // Create realtime client
        const client = new RealtimeClient(realtime_token, fileId);

        // Connect to WebSocket
        await client.connect();

        // Send a cell update
        setTimeout(() => {
            client.updateCell('A1', 'Hello World');
        }, 2000);

        // Send bulk update
        setTimeout(() => {
            client.bulkUpdate({
                'A1': { value: 'First Cell' },
                'B1': { value: 'Second Cell' },
                'C1': { value: '=SUM(A1:B1)', formula: '=SUM(A1:B1)' },
            });
        }, 4000);

    } catch (error) {
        console.error('Error:', error);
    }
}

// Uncomment to run
// main();

module.exports = RealtimeClient;
