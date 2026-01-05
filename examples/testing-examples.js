/**
 * Testing Examples for W12C Sheets
 * 
 * This file contains examples of:
 * - Unit tests for formula engine
 * - Integration tests for API
 * - E2E tests for user workflows
 */

// ============================================================================
// UNIT TESTS (Jest)
// ============================================================================

describe('Formula Engine', () => {
    test('SUM formula calculates correctly', () => {
        const formula = '=SUM(A1:A3)';
        const cells = {
            'A1': { value: '10' },
            'A2': { value: '20' },
            'A3': { value: '30' },
        };

        const result = evaluateFormula(formula, cells);
        expect(result).toBe(60);
    });

    test('AVERAGE formula handles empty cells', () => {
        const formula = '=AVERAGE(A1:A5)';
        const cells = {
            'A1': { value: '10' },
            'A2': { value: '' },
            'A3': { value: '30' },
            'A4': { value: '' },
            'A5': { value: '50' },
        };

        const result = evaluateFormula(formula, cells);
        expect(result).toBe(30); // (10+30+50)/3
    });

    test('IF formula with conditions', () => {
        const formula = '=IF(A1>100,"High","Low")';
        const cells = { 'A1': { value: '150' } };

        const result = evaluateFormula(formula, cells);
        expect(result).toBe('High');
    });

    test('Nested formulas work correctly', () => {
        const formula = '=SUM(A1:A2)+AVERAGE(B1:B2)';
        const cells = {
            'A1': { value: '10' },
            'A2': { value: '20' },
            'B1': { value: '100' },
            'B2': { value: '200' },
        };

        const result = evaluateFormula(formula, cells);
        expect(result).toBe(180); // 30 + 150
    });
});

describe('Cell Validation', () => {
    test('validates cell ID format', () => {
        expect(isValidCellId('A1')).toBe(true);
        expect(isValidCellId('Z999')).toBe(true);
        expect(isValidCellId('AA1')).toBe(true);
        expect(isValidCellId('1A')).toBe(false);
        expect(isValidCellId('')).toBe(false);
    });

    test('validates cell value length', () => {
        const shortValue = 'test';
        const longValue = 'a'.repeat(40000);

        expect(validateCellValue(shortValue)).toBe(true);
        expect(validateCellValue(longValue)).toBe(false);
    });
});

// ============================================================================
// INTEGRATION TESTS (API)
// ============================================================================

describe('File API', () => {
    let authToken;

    beforeAll(async () => {
        // Login to get token
        const response = await fetch('http://localhost:8080/api/v1/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                email: 'test@example.com',
                password: 'password123',
            }),
        });
        const data = await response.json();
        authToken = data.token;
    });

    test('creates a new file', async () => {
        const response = await fetch('http://localhost:8080/api/v1/files', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${authToken}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name: 'Test File' }),
        });

        expect(response.status).toBe(201);
        const data = await response.json();
        expect(data.name).toBe('Test File');
        expect(data.id).toBeDefined();
    });

    test('retrieves file by ID', async () => {
        // Create a file first
        const createResponse = await fetch('http://localhost:8080/api/v1/files', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${authToken}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name: 'Test File 2' }),
        });
        const created = await createResponse.json();

        // Retrieve it
        const getResponse = await fetch(`http://localhost:8080/api/v1/files/${created.id}`, {
            headers: { 'Authorization': `Bearer ${authToken}` },
        });

        expect(getResponse.status).toBe(200);
        const retrieved = await getResponse.json();
        expect(retrieved.id).toBe(created.id);
        expect(retrieved.name).toBe('Test File 2');
    });

    test('updates cells in file', async () => {
        // Create file
        const createResponse = await fetch('http://localhost:8080/api/v1/files', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${authToken}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name: 'Cell Test' }),
        });
        const file = await createResponse.json();

        // Update cells
        const updateResponse = await fetch(`http://localhost:8080/api/v1/files/${file.id}/cells`, {
            method: 'PATCH',
            headers: {
                'Authorization': `Bearer ${authToken}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                updates: {
                    'A1': { value: 'Hello' },
                    'B1': { value: 'World' },
                },
            }),
        });

        expect(updateResponse.status).toBe(200);
    });

    test('shares file with another user', async () => {
        // Create file
        const createResponse = await fetch('http://localhost:8080/api/v1/files', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${authToken}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name: 'Shared File' }),
        });
        const file = await createResponse.json();

        // Share with user
        const shareResponse = await fetch(`http://localhost:8080/api/v1/files/${file.id}/shares`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${authToken}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                email: 'viewer@example.com',
                role: 'viewer',
            }),
        });

        expect(shareResponse.status).toBe(201);
    });
});

// ============================================================================
// E2E TESTS (Playwright/Puppeteer)
// ============================================================================

describe('User Workflows', () => {
    test('complete spreadsheet creation workflow', async () => {
        // Navigate to app
        await page.goto('http://localhost:8001');

        // Login
        await page.click('[data-testid="login-button"]');
        await page.fill('[data-testid="email-input"]', 'test@example.com');
        await page.fill('[data-testid="password-input"]', 'password123');
        await page.click('[data-testid="submit-login"]');

        // Wait for dashboard
        await page.waitForSelector('[data-testid="dashboard"]');

        // Create new file
        await page.click('[data-testid="new-file"]');
        await page.fill('[data-testid="file-name-input"]', 'E2E Test Sheet');
        await page.click('[data-testid="create-file"]');

        // Wait for grid to load
        await page.waitForSelector('[data-testid="grid"]');

        // Enter data in cells
        await page.click('[data-testid="cell-A1"]');
        await page.keyboard.type('Product');
        await page.keyboard.press('Tab');

        await page.keyboard.type('Price');
        await page.keyboard.press('Enter');

        await page.keyboard.type('Apple');
        await page.keyboard.press('Tab');
        await page.keyboard.type('1.50');
        await page.keyboard.press('Enter');

        // Add formula
        await page.click('[data-testid="cell-C1"]');
        await page.keyboard.type('Total');
        await page.keyboard.press('Enter');

        await page.keyboard.type('=B2*10');
        await page.keyboard.press('Enter');

        // Verify result
        const cellValue = await page.textContent('[data-testid="cell-C2"]');
        expect(cellValue).toBe('15.00');

        // Save file
        await page.keyboard.press('Control+s');

        // Wait for save confirmation
        await page.waitForSelector('[data-testid="save-success"]');
    });

    test('real-time collaboration workflow', async () => {
        // Open two browser contexts
        const context1 = await browser.newContext();
        const context2 = await browser.newContext();

        const page1 = await context1.newPage();
        const page2 = await context2.newPage();

        // Both users login
        await Promise.all([
            loginUser(page1, 'user1@example.com', 'password'),
            loginUser(page2, 'user2@example.com', 'password'),
        ]);

        // User 1 creates and shares file
        const fileId = await createAndShareFile(page1, 'user2@example.com');

        // User 2 opens the shared file
        await page2.goto(`http://localhost:8001/files/${fileId}`);

        // User 1 enters data
        await page1.click('[data-testid="cell-A1"]');
        await page1.keyboard.type('Hello from User 1');

        // Verify User 2 sees the update
        await page2.waitForTimeout(1000); // Wait for WebSocket update
        const value = await page2.textContent('[data-testid="cell-A1"]');
        expect(value).toBe('Hello from User 1');

        // Clean up
        await context1.close();
        await context2.close();
    });
});

// ============================================================================
// PERFORMANCE TESTS
// ============================================================================

describe('Performance Tests', () => {
    test('handles large dataset efficiently', async () => {
        const startTime = Date.now();

        // Create 1000 rows of data
        const updates = {};
        for (let i = 1; i <= 1000; i++) {
            updates[`A${i}`] = { value: `Row ${i}` };
            updates[`B${i}`] = { value: Math.random() * 100 };
            updates[`C${i}`] = { value: `=B${i}*2`, formula: `=B${i}*2` };
        }

        // Update cells
        await fetch(`http://localhost:8080/api/v1/files/${fileId}/cells`, {
            method: 'PATCH',
            headers: {
                'Authorization': `Bearer ${authToken}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ updates }),
        });

        const endTime = Date.now();
        const duration = endTime - startTime;

        // Should complete in under 5 seconds
        expect(duration).toBeLessThan(5000);
    });

    test('formula calculation performance', () => {
        const cells = {};
        // Create 100 cells with values
        for (let i = 1; i <= 100; i++) {
            cells[`A${i}`] = { value: i.toString() };
        }

        const startTime = Date.now();

        // Calculate SUM of all cells
        const result = evaluateFormula('=SUM(A1:A100)', cells);

        const endTime = Date.now();
        const duration = endTime - startTime;

        expect(result).toBe(5050); // Sum of 1 to 100
        expect(duration).toBeLessThan(100); // Should be fast
    });
});

// ============================================================================
// HELPER FUNCTIONS
// ============================================================================

async function loginUser(page, email, password) {
    await page.goto('http://localhost:8001');
    await page.click('[data-testid="login-button"]');
    await page.fill('[data-testid="email-input"]', email);
    await page.fill('[data-testid="password-input"]', password);
    await page.click('[data-testid="submit-login"]');
    await page.waitForSelector('[data-testid="dashboard"]');
}

async function createAndShareFile(page, shareWithEmail) {
    await page.click('[data-testid="new-file"]');
    await page.fill('[data-testid="file-name-input"]', 'Shared Test');
    await page.click('[data-testid="create-file"]');
    await page.waitForSelector('[data-testid="grid"]');

    // Get file ID from URL
    const url = page.url();
    const fileId = url.split('/').pop();

    // Share file
    await page.click('[data-testid="share-button"]');
    await page.fill('[data-testid="share-email"]', shareWithEmail);
    await page.click('[data-testid="share-submit"]');

    return fileId;
}

module.exports = {
    // Export for use in other test files
};
