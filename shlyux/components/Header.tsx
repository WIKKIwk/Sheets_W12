import React, { useState } from 'react';
import { FileText, Save, FolderOpen, FilePlus, LogOut, Download, Trash2, Edit3, Settings, LayoutTemplate, Share2 } from 'lucide-react';
import { usePresence } from '../utils/usePresence';
import Tooltip from './Tooltip';

interface HeaderProps {
    fileName: string;
    currentFileId: number | null;
    currentAccessRole?: 'owner' | 'editor' | 'viewer' | null;
    user: any;
    files: any[];
    onFileNameChange: (name: string) => void;
    onNewFile: () => void;
    onOpenTemplates?: () => void;
    onSaveFile: () => void;
    onShareFile?: () => void;
    onOpenFile: (id: number) => void;
    onDeleteFile?: (id: number) => void;
    onLogout: () => void;
    onExport?: () => void;
    onImportFile?: (file: File) => void;
    onShowProfile?: () => void;
}

const Header: React.FC<HeaderProps> = ({
    fileName,
    currentFileId,
    currentAccessRole,
    user,
    files,
    onFileNameChange,
    onNewFile,
    onOpenTemplates,
    onSaveFile,
    onShareFile,
    onOpenFile,
    onDeleteFile,
    onLogout,
    onExport,
    onImportFile,
    onShowProfile
}) => {
    const [isEditing, setIsEditing] = useState(false);
    const [editValue, setEditValue] = useState(fileName);
    const [filesOpen, setFilesOpen] = useState(false);
    const [userMenuOpen, setUserMenuOpen] = useState(false);

    const isOwner = (currentAccessRole || 'owner') === 'owner';
    const isViewer = currentAccessRole === 'viewer';

    const filesPresence = usePresence(filesOpen, { exitDurationMs: 240 });
    const userMenuPresence = usePresence(userMenuOpen, { exitDurationMs: 240 });

    const handleSaveName = () => {
        onFileNameChange(editValue);
        setIsEditing(false);
    };

    return (
        <div className="modern-header">
            <div className="header-content">
                {/* Logo & File Name */}
                <div className="header-left">
                    <div className="logo-container">
                        <FileText size={20} className="logo-icon" />
                        <span className="logo-text">W12C Sheets</span>
                    </div>

                    {user && (
                        <div className="file-name-container">
                            {isEditing ? (
                                <input
                                    value={editValue}
                                    onChange={(e) => setEditValue(e.target.value)}
                                    onBlur={handleSaveName}
                                    onKeyDown={(e) => {
                                        if (e.key === 'Enter') handleSaveName();
                                        if (e.key === 'Escape') {
                                            setEditValue(fileName);
                                            setIsEditing(false);
                                        }
                                    }}
                                    autoFocus
                                    className="file-name-input"
                                />
                            ) : (
                                <div
                                    className="file-name-display"
                                    onClick={() => {
                                        if (isOwner) setIsEditing(true);
                                    }}
                                    title={isOwner ? "Fayl nomini tahrirlash uchun bosing" : "Shared fayl (faqat owner rename qila oladi)"}
                                    style={{
                                        display: 'flex',
                                        alignItems: 'center',
                                        gap: '0.5rem',
                                        padding: '0.375rem 0.75rem',
                                        border: '1px solid var(--chrome-border)',
                                        borderRadius: 'var(--radius-sm)',
                                        cursor: isOwner ? 'pointer' : 'default',
                                        transition: 'box-shadow 0.15s ease, border-color 0.15s ease',
                                        background: 'var(--chrome-control-bg)',
                                        backdropFilter: 'blur(12px) saturate(160%)',
                                        WebkitBackdropFilter: 'blur(12px) saturate(160%)',
                                    }}
                                    onMouseEnter={(e) => {
                                        if (!isOwner) return;
                                        e.currentTarget.style.boxShadow = 'inset 0 0 0 999px var(--chrome-control-hover)';
                                    }}
                                    onMouseLeave={(e) => {
                                        e.currentTarget.style.boxShadow = 'none';
                                    }}
                                >
                                    <Edit3 size={14} style={{ color: 'var(--text-secondary)', flexShrink: 0 }} />
                                    <span>{fileName}</span>
                                    {!isOwner && currentAccessRole && (
                                        <span
                                            className="text-xs px-2 py-0.5 rounded"
                                            style={{ background: '#e0f2fe', color: '#0369a1' }}
                                        >
                                            {currentAccessRole}
                                        </span>
                                    )}
                                </div>
                            )}
                        </div>
                    )}
                </div>

                {/* Actions */}
                {user && (
                    <div className="header-actions">
                        <Tooltip label="New">
                            <button onClick={onNewFile} className="action-btn" type="button">
                                <FilePlus size={14} />
                                <span>New</span>
                            </button>
                        </Tooltip>

                        {onOpenTemplates && (
                            <Tooltip label="Templates" shortcut="Ctrl+Shift+N">
                                <button onClick={onOpenTemplates} className="action-btn" type="button">
                                    <LayoutTemplate size={14} />
                                    <span>Templates</span>
                                </button>
                            </Tooltip>
                        )}

                        {onShareFile && currentFileId && isOwner && (
                            <Tooltip label="Share">
                                <button onClick={onShareFile} className="action-btn" type="button">
                                    <Share2 size={14} />
                                    <span>Share</span>
                                </button>
                            </Tooltip>
                        )}

                        <Tooltip label="Save" shortcut="Ctrl+S">
                            <button
                                onClick={onSaveFile}
                                disabled={isViewer}
                                className={`action-btn primary btn-lift ${isViewer ? 'opacity-50 cursor-not-allowed' : ''}`}
                                type="button"
                            >
                                <Save size={14} />
                                <span>Save</span>
                            </button>
                        </Tooltip>

                        {onExport && (
                            <Tooltip label="Export">
                                <button onClick={onExport} className="action-btn" type="button">
                                    <Download size={14} />
                                    <span>Export</span>
                                </button>
                            </Tooltip>
                        )}

                        {onImportFile && (
                            <Tooltip label="Import">
                                <label className="action-btn cursor-pointer">
                                    <input
                                        id="file-input"
                                        type="file"
                                        accept=".csv,.xlsx,.xls"
                                        className="hidden"
                                        onChange={(e) => {
                                            const f = e.target.files?.[0];
                                            if (f) {
                                                onImportFile(f);
                                                e.target.value = '';
                                            }
                                        }}
                                    />
                                    <Download size={14} />
                                    <span>Import</span>
                                </label>
                            </Tooltip>
                        )}

                        <div className="relative">
                            <Tooltip label="Open">
                                <button
                                    onClick={() => setFilesOpen(!filesOpen)}
                                    className="action-btn"
                                    type="button"
                                >
                                    <FolderOpen size={14} />
                                    <span>Open</span>
                                </button>
                            </Tooltip>

                            {filesPresence.isMounted && (
                                <>
                                    <div
                                        className="dropdown-overlay ui-overlay"
                                        data-state={filesPresence.state}
                                        onClick={() => setFilesOpen(false)}
                                    />
                                    <div className="dropdown files-dropdown ui-popover" data-state={filesPresence.state}>
                                        <div className="dropdown-header">Recent Files</div>
                                        <div className="files-list">
                                            {files.length === 0 ? (
                                                <div className="empty-files">No files yet</div>
                                            ) : (
                                                files.map((file, index) => {
                                                    const role = (file as any).access_role || 'owner';
                                                    const canDelete = role === 'owner';
                                                    return (
                                                        <div
                                                            key={file.id}
                                                            className={`file-item-wrapper ${currentFileId === file.id ? 'active' : ''}`}
                                                            style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}
                                                        >
                                                            <button
                                                                onClick={() => {
                                                                    onOpenFile(file.id);
                                                                    setFilesOpen(false);
                                                                }}
                                                                className={`file-item ${currentFileId === file.id ? 'active' : ''}`}
                                                                style={{ flex: 1 }}
                                                            >
                                                                <span style={{
                                                                    minWidth: '20px',
                                                                    fontWeight: '500',
                                                                    color: '#6b7280'
                                                                }}>
                                                                    {index + 1}.
                                                                </span>
                                                                <FileText size={14} />
                                                                <span>{file.name}</span>
                                                                {role !== 'owner' && (
                                                                    <span
                                                                        className="text-xs px-2 py-0.5 rounded ml-2"
                                                                        style={{ background: '#e0f2fe', color: '#0369a1' }}
                                                                    >
                                                                        {role}
                                                                    </span>
                                                                )}
                                                            </button>
                                                            {onDeleteFile && canDelete && (
                                                                <button
                                                                    onClick={(e) => {
                                                                        e.stopPropagation();
                                                                        onDeleteFile(file.id);
                                                                    }}
                                                                    className="file-delete-btn"
                                                                    title="Delete file"
                                                                    style={{
                                                                        padding: '0.5rem',
                                                                        background: 'transparent',
                                                                        border: 'none',
                                                                        cursor: 'pointer',
                                                                        borderRadius: '0.25rem',
                                                                        display: 'flex',
                                                                        alignItems: 'center',
                                                                        color: '#ef4444',
                                                                        transition: 'all 0.2s'
                                                                    }}
                                                                    onMouseEnter={(e) => {
                                                                        e.currentTarget.style.background = '#fef2f2';
                                                                    }}
                                                                    onMouseLeave={(e) => {
                                                                        e.currentTarget.style.background = 'transparent';
                                                                    }}
                                                                >
                                                                    <Trash2 size={14} />
                                                                </button>
                                                            )}
                                                        </div>
                                                    );
                                                })
                                            )}
                                        </div>
                                    </div>
                                </>
                            )}
                        </div>
                    </div>
                )}

                {/* Right Side */}
                <div className="header-right">
                    {user && (
                        <div className="relative">
                            <button
                                onClick={() => setUserMenuOpen(!userMenuOpen)}
                                className="user-btn"
                                title="User Menu"
                            >
                                <div className="user-avatar">
                                    {user.name?.charAt(0).toUpperCase() || 'U'}
                                </div>
                                <span className="user-name">{user.name}</span>
                            </button>

                            {userMenuPresence.isMounted && (
                                <>
                                    <div
                                        className="dropdown-overlay ui-overlay"
                                        data-state={userMenuPresence.state}
                                        onClick={() => setUserMenuOpen(false)}
                                    />
                                    <div className="dropdown user-dropdown ui-popover" data-state={userMenuPresence.state}>
                                        <div className="user-info">
                                            <div className="user-avatar large">
                                                {user.name?.charAt(0).toUpperCase() || 'U'}
                                            </div>
                                            <div>
                                                <div className="user-info-name">{user.name}</div>
                                                <div className="user-info-email">{user.email}</div>
                                            </div>
                                        </div>
                                        <div className="dropdown-divider" />
                                        {onShowProfile && (
                                            <button
                                                onClick={() => {
                                                    onShowProfile();
                                                    setUserMenuOpen(false);
                                                }}
                                                className="dropdown-item"
                                            >
                                                <Settings size={16} />
                                                <span>Profile</span>
                                            </button>
                                        )}
                                        <button
                                            onClick={() => {
                                                onLogout();
                                                setUserMenuOpen(false);
                                            }}
                                            className="dropdown-item danger"
                                        >
                                            <LogOut size={16} />
                                            <span>Logout</span>
                                        </button>
                                    </div>
                                </>
                            )}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Header;
