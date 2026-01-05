/**
 * File Operations Example for W12C Sheets API
 * 
 * This example demonstrates how to:
 * - Create a new spreadsheet file
 * - List all files
 * - Get file details
 * - Update cells in a spreadsheet
 * - Delete a file
 */

const API_BASE = 'http://localhost:8080/api/v1';

// Helper function to get auth header
function getAuthHeader(token) {
    return {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json',
    };
}

// Example 1: Create a new spreadsheet
async function createSpreadsheet(token, name, initialState = {}) {
    const response = await fetch(`${API_BASE}/files`, {
        method: 'POST',
        headers: getAuthHeader(token),
        body: JSON.stringify({
            name: name,
            state: initialState,
        }),
    });

    const data = await response.json();
    console.log('Created spreadsheet:', data);
    return data;
}

// Example 2: List all spreadsheets
async function listSpreadsheets(token) {
    const response = await fetch(`${API_BASE}/files`, {
        method: 'GET',
        headers: getAuthHeader(token),
    });

    const data = await response.json();
    console.log('All spreadsheets:', data);
    return data;
}

// Example 3: Get specific spreadsheet
async function getSpreadsheet(token, fileId) {
    const response = await fetch(`${API_BASE}/files/${fileId}`, {
        method: 'GET',
        headers: getAuthHeader(token),
    });

    const data = await response.json();
    console.log('Spreadsheet details:', data);
    return data;
}

// Example 4: Get cells in a range
async function getCells(token, fileId, range = 'A1:D20') {
    const response = await fetch(`${API_BASE}/files/${fileId}/cells?range=${range}`, {
        method: 'GET',
        headers: getAuthHeader(token),
    });

    const data = await response.json();
    console.log('Cells in range:', data);
    return data;
}

// Example 5: Update cells
async function updateCells(token, fileId, updates) {
    const response = await fetch(`${API_BASE}/files/${fileId}/cells`, {
        method: 'PATCH',
        headers: getAuthHeader(token),
        body: JSON.stringify({ updates }),
    });

    const data = await response.json();
    console.log('Updated cells:', data);
    return data;
}

// Example 6: Delete spreadsheet
async function deleteSpreadsheet(token, fileId) {
    const response = await fetch(`${API_BASE}/files/${fileId}`, {
        method: 'DELETE',
        headers: getAuthHeader(token),
    });

    console.log('Spreadsheet deleted');
    return response.ok;
}

// Example 7: Get realtime token for WebSocket
async function getRealtimeToken(token, fileId) {
    const response = await fetch(`${API_BASE}/files/${fileId}/realtime/token`, {
        method: 'POST',
        headers: getAuthHeader(token),
    });

    const data = await response.json();
    console.log('Realtime token:', data);
    return data;
}

// Usage Example
async function main() {
    const token = 'YOUR_JWT_TOKEN'; // Get from login

    try {
        // Create a new spreadsheet
        const file = await createSpreadsheet(token, 'My Budget 2026');
        const fileId = file.id;

        // Update some cells
        await updateCells(token, fileId, {
            'A1': { value: 'Category' },
            'B1': { value: 'Amount' },
            'A2': { value: 'Rent' },
            'B2': { value: '1500', formula: null },
            'A3': { value: 'Food' },
            'B3': { value: '500', formula: null },
        });

        // Get cells back
        await getCells(token, fileId, 'A1:B3');

        // List all files
        await listSpreadsheets(token);

        // Get realtime token for collaboration
        await getRealtimeToken(token, fileId);

    } catch (error) {
        console.error('Error:', error);
    }
}

// Uncomment to run
// main();

module.exports = {
    createSpreadsheet,
    listSpreadsheets,
    getSpreadsheet,
    getCells,
    updateCells,
    deleteSpreadsheet,
    getRealtimeToken,
};
