/**
 * AI Integration Example for W12C Sheets
 * Using Google Gemini 2.5 Flash API
 * 
 * This example demonstrates:
 * - Setting up Gemini API key
 * - Generating formulas using AI
 * - AI-powered data analysis
 * - Natural language to spreadsheet actions
 * - Multimodal AI (image analysis for tables)
 */

const API_BASE = 'http://localhost:8080/api/v1';

// Helper function
function getAuthHeader(token) {
    return {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json',
    };
}

// Example 1: Set Gemini API key
async function setGeminiKey(token, geminiApiKey) {
    const response = await fetch(`${API_BASE}/ai/gemini-key`, {
        method: 'POST',
        headers: getAuthHeader(token),
        body: JSON.stringify({ gemini_api_key: geminiApiKey }),
    });

    const data = await response.json();
    console.log('Gemini key set:', data);
    return data;
}

// Example 2: Get current Gemini key status
async function getGeminiKey(token) {
    const response = await fetch(`${API_BASE}/ai/gemini-key`, {
        method: 'GET',
        headers: getAuthHeader(token),
    });

    const data = await response.json();
    console.log('Gemini key status:', data);
    return data;
}

// Example 3: Generate AI response
async function generateAIResponse(token, prompt, context = {}) {
    const response = await fetch(`${API_BASE}/ai/generate`, {
        method: 'POST',
        headers: getAuthHeader(token),
        body: JSON.stringify({
            prompt: prompt,
            context: context,
        }),
    });

    const data = await response.json();
    console.log('AI response:', data);
    return data;
}

// Example 4: Stream AI response (Server-Sent Events)
async function streamAIResponse(token, prompt, context, onChunk) {
    const response = await fetch(`${API_BASE}/ai/stream`, {
        method: 'POST',
        headers: getAuthHeader(token),
        body: JSON.stringify({
            prompt: prompt,
            context: context,
        }),
    });

    const reader = response.body.getReader();
    const decoder = new TextDecoder();

    while (true) {
        const { done, value } = await reader.read();
        if (done) break;

        const chunk = decoder.decode(value);
        onChunk(chunk);
    }
}

// Example 5: AI Formula Generation
async function generateFormula(token, description, cellContext) {
    const prompt = `Generate an Excel/Google Sheets formula for: ${description}`;

    const context = {
        type: 'formula_generation',
        cells: cellContext,
    };

    return await generateAIResponse(token, prompt, context);
}

// Example 6: AI Data Analysis
async function analyzeData(token, dataRange, question) {
    const prompt = `Analyze this data and answer: ${question}`;

    const context = {
        type: 'data_analysis',
        range: dataRange,
    };

    return await generateAIResponse(token, prompt, context);
}

// Example 7: Natural Language Commands
async function executeNaturalLanguage(token, fileId, command, selectedRange) {
    const prompt = command;

    const context = {
        type: 'sheet_action',
        file_id: fileId,
        selected_range: selectedRange,
    };

    return await generateAIResponse(token, prompt, context);
}

// Example 8: Multimodal - Image to Table
async function imageToTable(token, imageBase64) {
    const prompt = 'Extract table data from this image and convert to spreadsheet format';

    const context = {
        type: 'image_analysis',
        image: imageBase64,
    };

    return await generateAIResponse(token, prompt, context);
}

// Practical Usage Examples
const usageExamples = {
    // Formula generation
    sumIfFormula: async (token) => {
        return await generateFormula(
            token,
            'Sum values in column B where column A is greater than 100',
            { A: [50, 150, 200], B: [10, 20, 30] }
        );
    },

    // Data analysis
    salesAnalysis: async (token) => {
        return await analyzeData(
            token,
            'A1:C100',
            'What is the total sales by category and which category performed best?'
        );
    },

    // Natural language commands
    sortData: async (token, fileId) => {
        return await executeNaturalLanguage(
            token,
            fileId,
            'Sort A2:C10 by column C in descending order',
            'A2:C10'
        );
    },

    fillFormula: async (token, fileId) => {
        return await executeNaturalLanguage(
            token,
            fileId,
            'In D2, write a formula to calculate the sum of A2:C2',
            'D2'
        );
    },

    clearRange: async (token, fileId) => {
        return await executeNaturalLanguage(
            token,
            fileId,
            'Clear all cells in A10:D20',
            'A10:D20'
        );
    },
};

// Complete workflow example
async function completeAIWorkflow() {
    const token = 'YOUR_JWT_TOKEN';
    const geminiKey = 'YOUR_GEMINI_API_KEY';
    const fileId = 'YOUR_FILE_ID';

    try {
        // Step 1: Set API key
        await setGeminiKey(token, geminiKey);

        // Step 2: Generate a formula
        const formula = await generateFormula(
            token,
            'Calculate average of B2:B10 only for values greater than 50',
            {}
        );
        console.log('Generated formula:', formula);

        // Step 3: Analyze data
        const analysis = await analyzeData(
            token,
            'A1:D100',
            'What are the top 5 products by revenue?'
        );
        console.log('Analysis:', analysis);

        // Step 4: Execute natural language command
        const action = await executeNaturalLanguage(
            token,
            fileId,
            'Sort the data by revenue column in descending order',
            'A1:D100'
        );
        console.log('Action result:', action);

    } catch (error) {
        console.error('Error in AI workflow:', error);
    }
}

// Uncomment to run
// completeAIWorkflow();

module.exports = {
    setGeminiKey,
    getGeminiKey,
    generateAIResponse,
    streamAIResponse,
    generateFormula,
    analyzeData,
    executeNaturalLanguage,
    imageToTable,
    usageExamples,
};
