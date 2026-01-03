import React, { useState } from 'react';
import { X, Code2, Key, Copy, Check, Terminal } from 'lucide-react';
import { generateApiKey } from '../utils/api';

interface ProfileProps {
    isOpen: boolean;
    onClose: () => void;
    onSaved?: (prefs: { theme: string; font: string; density: 'comfortable' | 'compact' }) => void;
    apiBase: string;
    authToken: string | null;
    currentFileId: number | null;
    currentAccessRole?: 'owner' | 'editor' | 'viewer' | null;
}

const Profile: React.FC<ProfileProps> = ({ isOpen, onClose, onSaved, apiBase, authToken, currentFileId, currentAccessRole }) => {
    const [activeTab, setActiveTab] = useState<'settings' | 'developer'>('settings');
    const [selectedTheme, setSelectedTheme] = useState(() => {
        return localStorage.getItem('app-theme') || 'light';
    });

    const [selectedFont, setSelectedFont] = useState(() => {
        return localStorage.getItem('app-font') || 'Inter, sans-serif';
    });

    const [selectedDensity, setSelectedDensity] = useState<'comfortable' | 'compact'>(() => {
        return localStorage.getItem('app-density') === 'compact' ? 'compact' : 'comfortable';
    });

    const [generatedSnippet, setGeneratedSnippet] = useState<string | null>(null);
    const [copyStatus, setCopyStatus] = useState<string | null>(null);
    const [apiKey, setApiKey] = useState<string | null>(null);
    const [apiKeyStatus, setApiKeyStatus] = useState<string | null>(null);

    const isViewer = currentAccessRole === 'viewer';
    const apiBaseV1 = `${apiBase}/api/v1`;
    const fileIdForSnippet = currentFileId ? `${currentFileId}` : ':id';
    const apiKeyHeader = apiKey ? `-H "X-API-Key: ${apiKey}"` : `-H "X-API-Key: YOUR_API_KEY"`;
    const placeholderSnippet = `curl ${apiKeyHeader} ${apiBaseV1}/files`;

    const handleSave = () => {
        // Save to localStorage
        localStorage.setItem('app-theme', selectedTheme);
        localStorage.setItem('app-font', selectedFont);
        localStorage.setItem('app-density', selectedDensity);

        // Apply theme
        document.documentElement.setAttribute('data-theme', selectedTheme);

        // Apply density
        document.documentElement.setAttribute('data-density', selectedDensity);

        // Apply font
        document.documentElement.style.setProperty('--font-family', selectedFont);

        onSaved?.({ theme: selectedTheme, font: selectedFont, density: selectedDensity });

        // Close modal
        onClose();
    };

    const setSnippet = (snippet: string) => {
        setGeneratedSnippet(snippet);
        setCopyStatus(null);
    };

    const handleGenerateApiSnippet = () => {
        setSnippet(placeholderSnippet);
    };

    const copyText = (text: string) => {
        navigator.clipboard?.writeText(text)
            .then(() => setCopyStatus("Nusxa olindi"))
            .catch(() => setCopyStatus("Nusxa olishda xato"));
    };

    const handleCopyPlaceholder = () => {
        if (!generatedSnippet) return;
        copyText(generatedSnippet);
    };

    const handleCopyWithApiKey = () => {
        if (!apiKey) {
            setCopyStatus("API key yo'q, avval yaratib oling");
            return;
        }
        copyText(`curl -H "X-API-Key: ${apiKey}" ${apiBaseV1}/files`);
    };

    const handleCopyApiKeyOnly = () => {
        if (!apiKey) {
            setCopyStatus("API key yo'q, avval yaratib oling");
            return;
        }
        copyText(apiKey);
    };

    const handleGenerateApiKeyClick = async () => {
        if (!authToken) {
            setApiKeyStatus("Token yo'q, login qiling");
            return;
        }
        setApiKeyStatus("Yaratilmoqda...");
        try {
            const res = await generateApiKey(authToken);
            setApiKey(res.api_key);
            setApiKeyStatus("Yaratildi");
            setSnippet(`curl -H "X-API-Key: ${res.api_key}" ${apiBaseV1}/files`);
        } catch (err) {
            const msg = err instanceof Error ? err.message : 'API key yaratilmadi';
            setApiKeyStatus(msg);
        }
    };

    const handleSnippetListFiles = () => setSnippet(`curl ${apiKeyHeader} ${apiBaseV1}/files`);
    const handleSnippetGetFile = () => setSnippet(`curl ${apiKeyHeader} ${apiBaseV1}/files/${fileIdForSnippet}`);
    const handleSnippetSchema = () => setSnippet(`curl ${apiKeyHeader} ${apiBaseV1}/files/${fileIdForSnippet}/schema`);
    const handleSnippetRange = () => setSnippet(`curl ${apiKeyHeader} "${apiBaseV1}/files/${fileIdForSnippet}/cells?range=A1:D20&format=grid"`);
    const handleSnippetPatch = () => setSnippet(
        `curl -X PATCH ${apiKeyHeader} -H "Content-Type: application/json" -d '{"edits":[{"cell":"A2","value":"Hello"}]}' ${apiBaseV1}/files/${fileIdForSnippet}/cells`
    );
    const handleSnippetRealtimeToken = () => setSnippet(`curl -X POST ${apiKeyHeader} ${apiBaseV1}/realtime/token`);

    if (!isOpen) return null;

    return (
        <div className="profile-overlay" onClick={onClose}>
            <div className="profile-modal" onClick={(e) => e.stopPropagation()}>
                <div className="profile-header">
                    <h1>Profile Settings</h1>
                    <button onClick={onClose} className="profile-close">
                        <X size={18} />
                    </button>
                </div>

                <div className="profile-content">
                    <div className="profile-tabs-row">
                        <div className="profile-tabs" role="tablist" aria-label="Profile Tabs">
                            <button
                                type="button"
                                className={`profile-tab ${activeTab === 'settings' ? 'active' : ''}`}
                                onClick={() => setActiveTab('settings')}
                                role="tab"
                                aria-selected={activeTab === 'settings'}
                            >
                                Settings
                            </button>
                            <button
                                type="button"
                                className={`profile-tab ${activeTab === 'developer' ? 'active' : ''}`}
                                onClick={() => setActiveTab('developer')}
                                role="tab"
                                aria-selected={activeTab === 'developer'}
                            >
                                Developer
                            </button>
                        </div>
                    </div>

                    {activeTab === 'settings' ? (
                        <>
                            <h2 className="profile-title">Settings</h2>
                            <p className="profile-subtitle">Customize your app appearance</p>

                            {/* Theme Selector */}
                            <div className="profile-section">
                                <label className="profile-label">Theme</label>
                                <div className="theme-options">
                                    <button
                                        type="button"
                                        className={`theme-option ${selectedTheme === 'light' ? 'active' : ''}`}
                                        onClick={() => setSelectedTheme('light')}
                                    >
                                        <div className="theme-preview light">
                                            <div className="theme-preview-header"></div>
                                            <div className="theme-preview-body"></div>
                                        </div>
                                        <span>Light</span>
                                    </button>
                                    <button
                                        type="button"
                                        className={`theme-option ${selectedTheme === 'dark' ? 'active' : ''}`}
                                        onClick={() => setSelectedTheme('dark')}
                                    >
                                        <div className="theme-preview dark">
                                            <div className="theme-preview-header"></div>
                                            <div className="theme-preview-body"></div>
                                        </div>
                                        <span>Dark</span>
                                    </button>
                                </div>
                            </div>

                            {/* Font Selector */}
                            <div className="profile-section">
                                <label className="profile-label" htmlFor="font-select">Font Family</label>
                                <select
                                    id="font-select"
                                    value={selectedFont}
                                    onChange={(e) => setSelectedFont(e.target.value)}
                                    className="font-select"
                                >
                                    <option value="Inter, sans-serif">Inter</option>
                                    <option value="Roboto, sans-serif">Roboto</option>
                                    <option value="'Open Sans', sans-serif">Open Sans</option>
                                    <option value="Lato, sans-serif">Lato</option>
                                    <option value="Arial, sans-serif">Arial</option>
                                    <option value="'Segoe UI', sans-serif">Segoe UI</option>
                                </select>
                                <p className="font-preview" style={{ fontFamily: selectedFont }}>
                                    The quick brown fox jumps over the lazy dog
                                </p>
                            </div>

                            {/* Density Selector */}
                            <div className="profile-section">
                                <label className="profile-label" htmlFor="density-select">Density</label>
                                <select
                                    id="density-select"
                                    value={selectedDensity}
                                    onChange={(e) => setSelectedDensity(e.target.value as 'comfortable' | 'compact')}
                                    className="font-select"
                                >
                                    <option value="comfortable">Comfortable</option>
                                    <option value="compact">Compact</option>
                                </select>
                                <p className="profile-subtitle" style={{ marginTop: '0.5rem' }}>
                                    Jadval va toolbar zichligini sozlash
                                </p>
                            </div>

                            {/* Action Buttons */}
                            <div className="profile-actions">
                                <button type="button" onClick={onClose} className="profile-btn cancel">
                                    Cancel
                                </button>
                                <button type="button" onClick={handleSave} className="profile-btn save">
                                    Save Changes
                                </button>
                            </div>
                        </>
                    ) : (
                        <>
                            <h2 className="profile-title">Developer</h2>
                            <p className="profile-subtitle">API key va cURL snippetlar</p>

                            <div className="developer-card">
                                <div className="developer-card-header">
                                    <Code2 size={18} />
                                    <span>Developer API</span>
                                </div>

                                <div className="dev-section">
                                    <div className="dev-info-row">
                                        <span className="dev-label">Base URL:</span>
                                        <code className="dev-value">{apiBaseV1}</code>
                                    </div>
                                    <div className="dev-info-row">
                                        <span className="dev-label">File ID:</span>
                                        <code className="dev-value">{currentFileId ?? '—'}</code>
                                    </div>
                                    <div className="dev-info-row">
                                        <span className="dev-label">Auth:</span>
                                        <span className="dev-value">Bearer JWT (login/register orqali)</span>
                                    </div>
                                </div>

                                <div className="dev-divider" />

                                <div className="dev-section">
                                    <div className="dev-subsection-header">
                                        <Key size={16} />
                                        <span>API Key</span>
                                    </div>

                                    <div className="api-key-display">
                                        {apiKey ? (
                                            <>
                                                <code className="api-key-text">
                                                    {apiKey.slice(0, 8)}...{apiKey.slice(-8)}
                                                </code>
                                                <button
                                                    type="button"
                                                    className="icon-copy-btn"
                                                    onClick={handleCopyApiKeyOnly}
                                                    title="Copy API Key"
                                                >
                                                    {copyStatus === "Nusxa olindi" ? <Check size={14} /> : <Copy size={14} />}
                                                </button>
                                            </>
                                        ) : (
                                            <span className="dev-placeholder">Yaratilmagan</span>
                                        )}
                                    </div>

                                    <button
                                        type="button"
                                        className="dev-action-btn"
                                        onClick={handleGenerateApiKeyClick}
                                    >
                                        <Key size={16} />
                                        API key yaratish
                                    </button>

                                    {apiKeyStatus && (
                                        <div className="dev-status">{apiKeyStatus}</div>
                                    )}
                                </div>

                                <div className="dev-divider" />

                                <div className="dev-section">
                                    <div className="snippet-actions" style={{ flexWrap: 'wrap' }}>
                                        <button type="button" className="snippet-action-btn" onClick={handleSnippetListFiles}>
                                            <Terminal size={14} />
                                            files
                                        </button>
                                        <button type="button" className="snippet-action-btn" onClick={handleSnippetGetFile} disabled={!currentFileId}>
                                            <Terminal size={14} />
                                            file
                                        </button>
                                        <button type="button" className="snippet-action-btn" onClick={handleSnippetSchema} disabled={!currentFileId}>
                                            <Terminal size={14} />
                                            schema
                                        </button>
                                        <button type="button" className="snippet-action-btn" onClick={handleSnippetRange} disabled={!currentFileId}>
                                            <Terminal size={14} />
                                            range
                                        </button>
                                        <button type="button" className="snippet-action-btn" onClick={handleSnippetPatch} disabled={!currentFileId || isViewer}>
                                            <Terminal size={14} />
                                            patch
                                        </button>
                                        <button type="button" className="snippet-action-btn" onClick={handleSnippetRealtimeToken}>
                                            <Terminal size={14} />
                                            realtime
                                        </button>
                                    </div>

                                    <button
                                        type="button"
                                        className="dev-action-btn primary"
                                        onClick={handleGenerateApiSnippet}
                                    >
                                        <Code2 size={16} />
                                        cURL yaratish
                                    </button>

                                    {generatedSnippet && (
                                        <div className="code-snippet-container">
                                            <div className="code-snippet-header">
                                                <Terminal size={14} />
                                                <span>cURL Command</span>
                                            </div>
                                            <div className="code-snippet">
                                                <code>{generatedSnippet}</code>
                                                <button
                                                    type="button"
                                                    className="snippet-copy-btn"
                                                    onClick={handleCopyPlaceholder}
                                                    title="Copy to clipboard"
                                                >
                                                    {copyStatus === "Nusxa olindi" ? <Check size={14} /> : <Copy size={14} />}
                                                </button>
                                            </div>

                                            <div className="snippet-actions">
                                                <button
                                                    type="button"
                                                    className="snippet-action-btn"
                                                    onClick={handleCopyWithApiKey}
                                                >
                                                    <Copy size={14} />
                                                    Nusxa (mening API key)
                                                </button>
                                            </div>

                                            {copyStatus && (
                                                <div className="copy-feedback">
                                                    <Check size={14} />
                                                    {copyStatus}
                                                </div>
                                            )}
                                        </div>
                                    )}
                                </div>
                            </div>
                        </>
                    )}
                </div>

                <style>{`
                    .profile-overlay {
                        position: fixed;
                        inset: 0;
                        background: rgba(0, 0, 0, 0.5);
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        z-index: 1000;
                        animation: fadeIn 0.2s ease-out;
                    }

                    @keyframes fadeIn {
                        from { opacity: 0; }
                        to { opacity: 1; }
                    }

                    @keyframes ripple {
                        0% {
                            transform: scale(0);
                            opacity: 0.6;
                        }
                        100% {
                            transform: scale(2.5);
                            opacity: 0;
                        }
                    }

                    @keyframes bouncePress {
                        0% { transform: perspective(500px) rotateX(0deg) translateZ(0); }
                        50% { transform: perspective(500px) rotateX(-5deg) translateZ(-10px); }
                        100% { transform: perspective(500px) rotateX(0deg) translateZ(0); }
                    }

                    @keyframes glowPulse {
                        0%, 100% { box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.2), 0 4px 12px rgba(102, 126, 234, 0.25); }
                        50% { box-shadow: 0 0 0 5px rgba(102, 126, 234, 0.3), 0 6px 20px rgba(102, 126, 234, 0.4); }
                    }

	                    .profile-modal {
	                        background: var(--card-bg, #ffffff);
	                        border-radius: 12px;
	                        width: 92%;
	                        max-width: 560px;
                            max-height: min(760px, 90vh);
	                        box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
	                        animation: slideUp 0.3s ease-out;
	                        overflow: hidden;
                            display: flex;
                            flex-direction: column;
	                    }

                    @keyframes slideUp {
                        from { transform: translateY(20px); opacity: 0; }
                        to { transform: translateY(0); opacity: 1; }
                    }

                    .profile-header {
                        display: flex;
                        align-items: center;
                        justify-content: space-between;
                        padding: 1.5rem;
                        border-bottom: 1px solid var(--border-color, #e5e7eb);
                    }

                    .profile-header h1 {
                        font-size: 1.25rem;
                        font-weight: 600;
                        color: var(--text-primary, #111827);
                        margin: 0;
                    }

                    .profile-close {
                        background: transparent;
                        border: none;
                        cursor: pointer;
                        padding: 0.5rem;
                        border-radius: 6px;
                        color: var(--text-secondary, #6b7280);
                        transition: all 0.2s;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                    }

                    .profile-close:hover {
                        background: var(--bg-light, #f3f4f6);
                        color: var(--text-primary, #111827);
                    }

	                    .profile-content {
	                        padding: 1.5rem;
                            overflow: auto;
                            -webkit-overflow-scrolling: touch;
	                    }

                        .profile-tabs-row {
                            display: flex;
                            justify-content: flex-start;
                            margin-bottom: 1.25rem;
                        }

                        .profile-tabs {
                            display: inline-flex;
                            gap: 0.25rem;
                            padding: 0.25rem;
                            border-radius: 999px;
                            border: 1px solid var(--border-color, #e5e7eb);
                            background: var(--bg-light, #f3f4f6);
                        }

                        .profile-tab {
                            border: none;
                            background: transparent;
                            border-radius: 999px;
                            padding: 0.5rem 0.875rem;
                            font-size: 0.8125rem;
                            font-weight: 600;
                            color: var(--text-secondary, #6b7280);
                            cursor: pointer;
                            transition: background 0.2s, color 0.2s, transform 0.2s, box-shadow 0.2s;
                        }

                        .profile-tab:hover {
                            color: var(--text-primary, #111827);
                        }

                        .profile-tab.active {
                            background: var(--card-bg, #ffffff);
                            color: var(--text-primary, #111827);
                            box-shadow: 0 1px 2px rgba(0, 0, 0, 0.12);
                        }

                        .profile-tab:active {
                            transform: scale(0.98);
                        }

                        .developer-card {
                            border: 1px solid var(--chrome-border);
                            border-radius: 14px;
                            overflow: hidden;
                            background: var(--card-bg, #ffffff);
                            box-shadow: 0 16px 40px rgba(0, 0, 0, 0.18);
                        }

                        .developer-card-header {
                            display: flex;
                            align-items: center;
                            gap: 0.625rem;
                            padding: 1rem 1.25rem;
                            border-bottom: 1px solid var(--chrome-border);
                            background: linear-gradient(135deg, rgba(10, 132, 255, 0.16), rgba(10, 132, 255, 0.06));
                            color: var(--text-primary, #111827);
                            font-weight: 600;
                            letter-spacing: 0.01em;
                        }

                        .developer-card-header svg {
                            color: var(--brand, #0a84ff);
                        }

                        .developer-card .dev-value {
                            word-break: break-all;
                        }

                    .profile-title {
                        font-size: 1.125rem;
                        font-weight: 600;
                        color: var(--text-primary, #111827);
                        margin: 0 0 0.5rem 0;
                    }

                    .profile-subtitle {
                        font-size: 0.875rem;
                        color: var(--text-secondary, #6b7280);
                        margin: 0 0 1.5rem 0;
                    }

                    .profile-section {
                        margin-bottom: 1.5rem;
                    }

                    .profile-label {
                        display: block;
                        font-size: 0.875rem;
                        font-weight: 500;
                        color: var(--text-primary, #111827);
                        margin-bottom: 0.75rem;
                    }

                    .theme-options {
                        display: grid;
                        grid-template-columns: 1fr 1fr;
                        gap: 1rem;
                    }

                    .theme-option {
                        position: relative;
                        background: transparent;
                        border: 2px solid var(--border-color, #e5e7eb);
                        border-radius: 8px;
                        padding: 1rem;
                        cursor: pointer;
                        transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                        display: flex;
                        flex-direction: column;
                        align-items: center;
                        gap: 0.75rem;
                        overflow: hidden;
                        transform-style: preserve-3d;
                    }

                    .theme-option::before {
                        content: '';
                        position: absolute;
                        top: 50%;
                        left: 50%;
                        width: 0;
                        height: 0;
                        border-radius: 50%;
                        background: radial-gradient(circle, rgba(10, 132, 255, 0.25), transparent);
                        transform: translate(-50%, -50%);
                        transition: width 0.6s, height 0.6s;
                    }

                    .theme-option:hover {
                        border-color: var(--brand, #0a84ff);
                        transform: perspective(500px) translateY(-4px) rotateX(2deg);
                        box-shadow: 0 8px 20px rgba(10, 132, 255, 0.18);
                    }

                    .theme-option.active {
                        border-color: var(--brand, #0a84ff);
                        background: rgba(10, 132, 255, 0.10);
                        border-width: 3px;
                        animation: glowPulse 2s ease-in-out infinite;
                    }

                    .theme-option:active::before {
                        width: 300%;
                        height: 300%;
                        transition: width 0s, height 0s;
                    }

                    .theme-option:active {
                        animation: bouncePress 0.4s ease-out;
                    }

                    .theme-option span {
                        font-size: 0.875rem;
                        font-weight: 500;
                        color: var(--text-primary, #111827);
                        position: relative;
                        z-index: 1;
                    }

                    .theme-preview {
                        width: 100%;
                        height: 80px;
                        border-radius: 6px;
                        overflow: hidden;
                        border: 1px solid #ddd;
                        position: relative;
                        z-index: 1;
                        transition: transform 0.3s ease;
                    }

                    .theme-option:hover .theme-preview {
                        transform: scale(1.02);
                    }

                    .theme-preview.light {
                        background: #ffffff;
                    }

                    .theme-preview.dark {
                        background: #1a1a1a;
                    }

                    .theme-preview-header {
                        height: 20px;
                    }

                    .theme-preview.light .theme-preview-header {
                        background: #f3f4f6;
                    }

                    .theme-preview.dark .theme-preview-header {
                        background: #2d2d2d;
                    }

                    .theme-preview-body {
                        height: 60px;
                        padding: 0.5rem;
                        display: flex;
                        flex-direction: column;
                        gap: 0.25rem;
                    }

                    .theme-preview.light .theme-preview-body::before,
                    .theme-preview.light .theme-preview-body::after {
                        content: '';
                        height: 6px;
                        background: #d1d5db;
                        border-radius: 2px;
                    }

                    .theme-preview.dark .theme-preview-body::before,
                    .theme-preview.dark .theme-preview-body::after {
                        content: '';
                        height: 6px;
                        background: #4b5563;
                        border-radius: 2px;
                    }

                    .font-select {
                        width: 100%;
                        padding: 0.75rem;
                        border: 1px solid var(--border-color, #e5e7eb);
                        border-radius: 6px;
                        background: var(--card-bg, #ffffff);
                        color: var(--text-primary, #111827);
                        font-size: 0.875rem;
                        cursor: pointer;
                        transition: all 0.2s;
                    }

                    .font-select:focus {
                        outline: none;
                        border-color: var(--brand, #0a84ff);
                        box-shadow: 0 0 0 3px var(--focus-ring);
                    }

                    .font-preview {
                        margin-top: 0.75rem;
                        padding: 0.75rem;
                        background: var(--bg-light, #f9fafb);
                        border-radius: 6px;
                        font-size: 0.875rem;
                        color: var(--text-primary, #111827);
                    }

                    .profile-actions {
                        display: flex;
                        gap: 0.75rem;
                        margin-top: 2rem;
                    }

                    .profile-btn {
                        position: relative;
                        flex: 1;
                        padding: 0.75rem 1.5rem;
                        border: none;
                        border-radius: 6px;
                        font-size: 0.875rem;
                        font-weight: 500;
                        cursor: pointer;
                        transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                        overflow: hidden;
                        transform-style: preserve-3d;
                    }

                    .profile-btn::after {
                        content: '';
                        position: absolute;
                        top: 50%;
                        left: 50%;
                        width: 0;
                        height: 0;
                        border-radius: 50%;
                        background: rgba(255, 255, 255, 0.5);
                        transform: translate(-50%, -50%);
                        transition: width 0.6s, height 0.6s;
                    }

                    .profile-btn:active::after {
                        width: 300%;
                        height: 300%;
                        transition: width 0s, height 0s;
                    }

                    .profile-btn.cancel {
                        background: var(--bg-light, #f3f4f6);
                        color: var(--text-primary, #111827);
                        border: 1px solid var(--border-color, #e5e7eb);
                    }

                    .profile-btn.cancel:hover {
                        background: #e5e7eb;
                        border-color: #d1d5db;
                        transform: perspective(500px) translateY(-2px);
                        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                    }

                    .profile-btn.cancel:active {
                        animation: bouncePress 0.3s ease-out;
                    }

                    .profile-btn.save {
                        background: linear-gradient(135deg, var(--brand, #0a84ff) 0%, #0070e0 100%);
                        color: white;
                        box-shadow: 0 4px 15px rgba(10, 132, 255, 0.25);
                        background-size: 200% 200%;
                        background-position: left center;
                        transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                    }

                    .profile-btn.save:hover {
                        background-position: right center;
                        box-shadow: 0 6px 20px rgba(10, 132, 255, 0.35);
                        transform: perspective(500px) translateY(-3px) rotateX(2deg);
                    }

                    .profile-btn.save:active {
                        animation: bouncePress 0.3s ease-out;
                        box-shadow: 0 2px 8px rgba(10, 132, 255, 0.28);
                    }
                `}</style>
            </div>
        </div>
    );
};

export default Profile;
