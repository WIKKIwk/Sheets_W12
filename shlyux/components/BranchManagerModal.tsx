import React, { useEffect, useMemo, useState } from 'react';
import { AlertTriangle, ArrowLeft, GitBranch, GitMerge, Plus, Trash2, X } from 'lucide-react';
import { usePresence } from '../utils/usePresence';
import type { SheetBranch, SheetMergeConflict, SheetMergeResult } from '../utils/branches';
import { getColumnLabel } from '../utils/spreadsheetUtils';

type MergeChoice = 'main' | 'branch';

type MergePreview = {
  branch: SheetBranch;
  result: SheetMergeResult;
};

type BranchManagerModalProps = {
  isOpen: boolean;
  branches: SheetBranch[];
  activeBranchId: string | null;
  onClose: () => void;
  onCreateBranch: (name: string) => void;
  onCheckoutBranch: (branchId: string) => void;
  onCheckoutMain: () => void;
  onDeleteBranch: (branchId: string) => void;
  onPrepareMerge: (branchId: string) => Promise<MergePreview>;
  onApplyMerge: (preview: MergePreview, picks: Record<string, MergeChoice>) => Promise<void>;
};

const formatTime = (timestampMs: number): string => {
  try {
    return new Date(timestampMs).toLocaleString();
  } catch {
    return `${timestampMs}`;
  }
};

const coordsToLabel = (row: number, col: number): string => `${getColumnLabel(col)}${row + 1}`;

const sortConflicts = (conflicts: SheetMergeConflict[]) =>
  [...conflicts].sort((a, b) => (a.row - b.row) || (a.col - b.col) || a.id.localeCompare(b.id));

export default function BranchManagerModal({
  isOpen,
  branches,
  activeBranchId,
  onClose,
  onCreateBranch,
  onCheckoutBranch,
  onCheckoutMain,
  onDeleteBranch,
  onPrepareMerge,
  onApplyMerge,
}: BranchManagerModalProps) {
  const modalPresence = usePresence(isOpen, { exitDurationMs: 240 });
  const [name, setName] = useState('');
  const [mergePreview, setMergePreview] = useState<MergePreview | null>(null);
  const [mergeLoading, setMergeLoading] = useState(false);
  const [mergePicks, setMergePicks] = useState<Record<string, MergeChoice>>({});

  const activeBranch = useMemo(
    () => (activeBranchId ? branches.find((b) => b.id === activeBranchId) ?? null : null),
    [activeBranchId, branches]
  );

  useEffect(() => {
    if (!isOpen) return;
    setName('');
    setMergePreview(null);
    setMergeLoading(false);
    setMergePicks({});
  }, [isOpen]);

  useEffect(() => {
    if (!mergePreview) return;
    const initial: Record<string, MergeChoice> = {};
    mergePreview.result.conflicts.forEach((c) => {
      initial[c.id] = 'main';
    });
    setMergePicks(initial);
  }, [mergePreview]);

  useEffect(() => {
    if (!isOpen) return;
    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', onKeyDown);
    return () => window.removeEventListener('keydown', onKeyDown);
  }, [isOpen, onClose]);

  const canCreate = !activeBranchId;

  const handleCreate = () => {
    const clean = name.trim();
    const label = clean || `Draft — ${formatTime(Date.now())}`;
    onCreateBranch(label);
    setName('');
  };

  const handleDelete = (branchId: string) => {
    const ok = window.confirm("Branch o‘chiriladi. Davom etamizmi?");
    if (!ok) return;
    onDeleteBranch(branchId);
  };

  const handlePrepareMerge = async (branchId: string) => {
    setMergeLoading(true);
    try {
      const preview = await onPrepareMerge(branchId);
      setMergePreview(preview);
    } finally {
      setMergeLoading(false);
    }
  };

  const handleApplyMerge = async () => {
    if (!mergePreview) return;
    setMergeLoading(true);
    try {
      await onApplyMerge(mergePreview, mergePicks);
      setMergePreview(null);
    } finally {
      setMergeLoading(false);
    }
  };

  const conflicts = useMemo(() => (mergePreview ? sortConflicts(mergePreview.result.conflicts) : []), [mergePreview]);

  if (!modalPresence.isMounted) return null;

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center ui-overlay ui-scrim ui-scrim-strong"
      data-state={modalPresence.state}
      onClick={onClose}
    >
      <div
        className="ui-modal"
        data-state={modalPresence.state}
        onClick={(e) => e.stopPropagation()}
        style={{
          width: 'min(980px, calc(100vw - 24px))',
          height: 'min(720px, calc(100vh - 24px))',
          background: 'var(--card-bg)',
          border: '1px solid var(--border-color)',
          borderRadius: 16,
          boxShadow: 'var(--shadow-lg)',
          overflow: 'hidden',
          display: 'flex',
          flexDirection: 'column',
        }}
      >
        <div
          className="flex items-center justify-between px-5 py-4 border-b"
          style={{ borderColor: 'var(--border-color)', background: 'var(--chrome-bg)' }}
        >
          <div className="flex items-center gap-3">
            <div
              className="w-10 h-10 rounded-xl flex items-center justify-center"
              style={{ background: 'var(--bg-light)', border: '1px solid var(--border-color)' }}
            >
              <GitBranch size={18} style={{ color: 'var(--brand)' }} />
            </div>
            <div>
              <div className="text-sm font-semibold" style={{ color: 'var(--text-primary)' }}>
                Branches
              </div>
              <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                Draft → review → merge (MVP)
              </div>
            </div>
          </div>
          <button className="icon-btn" style={{ width: 32, height: 32 }} onClick={onClose} title="Yopish" type="button">
            <X size={16} />
          </button>
        </div>

        {mergePreview ? (
          <div style={{ flex: 1, minHeight: 0, display: 'flex', flexDirection: 'column' }}>
            <div className="px-5 py-3 border-b" style={{ borderColor: 'var(--border-color)', background: 'var(--bg-light)' }}>
              <div className="flex items-center justify-between gap-3">
                <div className="flex items-center gap-2">
                  <button
                    type="button"
                    className="icon-btn"
                    style={{ width: 32, height: 32 }}
                    onClick={() => setMergePreview(null)}
                    title="Back"
                  >
                    <ArrowLeft size={16} />
                  </button>
                  <div>
                    <div className="text-sm font-semibold" style={{ color: 'var(--text-primary)' }}>
                      Merge preview
                    </div>
                    <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                      {mergePreview.branch.name} → Main
                    </div>
                  </div>
                </div>
                <button
                  type="button"
                  className="action-btn primary"
                  onClick={handleApplyMerge}
                  disabled={mergeLoading}
                  style={{ height: 34 }}
                >
                  <GitMerge size={14} />
                  Apply merge
                </button>
              </div>
            </div>

            <div style={{ padding: 16, display: 'flex', gap: 10, flexWrap: 'wrap' }}>
              <span className="text-xs px-2 py-1 rounded" style={{ background: 'var(--bg-light)', color: 'var(--text-secondary)' }}>
                Applied: {mergePreview.result.applied}
              </span>
              <span className="text-xs px-2 py-1 rounded" style={{ background: 'var(--bg-light)', color: 'var(--text-secondary)' }}>
                Meta: {mergePreview.result.appliedMeta}
              </span>
              <span
                className="text-xs px-2 py-1 rounded flex items-center gap-1"
                style={{
                  background: mergePreview.result.conflicts.length ? 'rgba(245, 158, 11, 0.12)' : 'rgba(34, 197, 94, 0.12)',
                  color: mergePreview.result.conflicts.length ? '#b45309' : '#166534',
                }}
              >
                {mergePreview.result.conflicts.length ? <AlertTriangle size={12} /> : null}
                Conflicts: {mergePreview.result.conflicts.length}
              </span>
            </div>

            <div
              style={{
                flex: 1,
                borderTop: '1px solid var(--border-color)',
                minHeight: 0,
                overflow: 'auto',
              }}
            >
              {conflicts.length === 0 ? (
                <div style={{ padding: 16, color: 'var(--text-secondary)' }}>
                  Konflikt yo‘q. Apply merge bosib main ga qo‘shib yuboring.
                </div>
              ) : (
                <div style={{ padding: 16, display: 'flex', flexDirection: 'column', gap: 10 }}>
                  {conflicts.slice(0, 300).map((c) => {
                    const pick = mergePicks[c.id] || 'main';
                    return (
                      <div
                        key={c.id}
                        style={{
                          border: '1px solid var(--border-color)',
                          borderRadius: 14,
                          padding: 12,
                          background: 'var(--card-bg)',
                          display: 'grid',
                          gridTemplateColumns: '110px 1fr 1fr 160px',
                          gap: 10,
                          alignItems: 'start',
                        }}
                      >
                        <div>
                          <div className="text-xs font-semibold" style={{ color: 'var(--text-primary)' }}>
                            {coordsToLabel(c.row, c.col)}
                          </div>
                          <div className="text-[11px]" style={{ color: 'var(--text-secondary)' }}>
                            Conflict
                          </div>
                        </div>

                        <div style={{ minWidth: 0 }}>
                          <div className="text-[11px] font-semibold mb-1" style={{ color: 'var(--text-secondary)' }}>
                            Main
                          </div>
                          <div
                            className="text-xs"
                            style={{
                              color: 'var(--text-primary)',
                              border: '1px solid var(--border-color)',
                              borderRadius: 10,
                              padding: '6px 8px',
                              background: 'var(--bg-light)',
                              wordBreak: 'break-word',
                            }}
                          >
                            {c.main || <span style={{ color: 'var(--text-tertiary)' }}>(empty)</span>}
                          </div>
                        </div>

                        <div style={{ minWidth: 0 }}>
                          <div className="text-[11px] font-semibold mb-1" style={{ color: 'var(--text-secondary)' }}>
                            Branch
                          </div>
                          <div
                            className="text-xs"
                            style={{
                              color: 'var(--text-primary)',
                              border: '1px solid var(--border-color)',
                              borderRadius: 10,
                              padding: '6px 8px',
                              background: 'var(--bg-light)',
                              wordBreak: 'break-word',
                            }}
                          >
                            {c.branch || <span style={{ color: 'var(--text-tertiary)' }}>(empty)</span>}
                          </div>
                        </div>

                        <div className="flex justify-end">
                          <div
                            className="inline-flex rounded-xl overflow-hidden"
                            style={{ border: '1px solid var(--border-color)', height: 30 }}
                          >
                            <button
                              type="button"
                              className="px-3 text-xs"
                              style={{
                                background: pick === 'main' ? 'var(--bg-light)' : 'transparent',
                                color: 'var(--text-primary)',
                              }}
                              onClick={() => setMergePicks((prev) => ({ ...prev, [c.id]: 'main' }))}
                            >
                              Main
                            </button>
                            <button
                              type="button"
                              className="px-3 text-xs"
                              style={{
                                background: pick === 'branch' ? 'var(--bg-light)' : 'transparent',
                                color: 'var(--text-primary)',
                              }}
                              onClick={() => setMergePicks((prev) => ({ ...prev, [c.id]: 'branch' }))}
                            >
                              Branch
                            </button>
                          </div>
                        </div>
                      </div>
                    );
                  })}
                  {conflicts.length > 300 && (
                    <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                      Juda ko‘p konflikt — faqat birinchi 300 ta ko‘rsatildi.
                    </div>
                  )}
                </div>
              )}
            </div>
          </div>
        ) : (
          <div style={{ flex: 1, display: 'grid', gridTemplateColumns: '1fr', minHeight: 0 }}>
            <div style={{ padding: 16, minHeight: 0, display: 'flex', flexDirection: 'column', gap: 14 }}>
              <div
                style={{
                  border: '1px solid var(--border-color)',
                  borderRadius: 14,
                  padding: 14,
                  background: 'var(--bg-light)',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'space-between',
                  gap: 10,
                }}
              >
                <div style={{ minWidth: 0 }}>
                  <div className="text-sm font-semibold" style={{ color: 'var(--text-primary)' }}>
                    Current: {activeBranch ? activeBranch.name : 'Main'}
                  </div>
                  <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                    Branch rejimida autosave/realtime o‘chadi (xavfsiz draft).
                  </div>
                </div>
                {activeBranch ? (
                  <button type="button" className="action-btn" onClick={onCheckoutMain} style={{ height: 34 }}>
                    Back to main
                  </button>
                ) : null}
              </div>

              <div
                style={{
                  border: '1px solid var(--border-color)',
                  borderRadius: 14,
                  padding: 14,
                  background: 'var(--card-bg)',
                }}
              >
                <div className="text-xs font-semibold mb-2" style={{ color: 'var(--text-secondary)' }}>
                  Create branch
                </div>
                <div style={{ display: 'flex', gap: 8 }}>
                  <input
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder={canCreate ? 'Branch nomi (ixtiyoriy)' : 'Main ga qayting (branch create uchun)'}
                    className="px-3 text-sm"
                    disabled={!canCreate}
                    style={{
                      flex: 1,
                      height: 34,
                      borderRadius: 10,
                      border: '1px solid var(--border-color)',
                      background: canCreate ? 'var(--card-bg)' : 'var(--bg-light)',
                      color: 'var(--text-primary)',
                    }}
                  />
                  <button
                    className="action-btn primary"
                    style={{ height: 34 }}
                    onClick={handleCreate}
                    type="button"
                    disabled={!canCreate}
                  >
                    <Plus size={14} />
                    Create
                  </button>
                </div>
              </div>

              <div
                style={{
                  flex: 1,
                  minHeight: 0,
                  border: '1px solid var(--border-color)',
                  borderRadius: 14,
                  overflow: 'auto',
                  background: 'var(--card-bg)',
                }}
              >
                {branches.length === 0 ? (
                  <div style={{ padding: 16, color: 'var(--text-secondary)' }}>
                    Hozircha branch yo‘q. Birinchi branch yarating.
                  </div>
                ) : (
                  <div style={{ padding: 12, display: 'flex', flexDirection: 'column', gap: 10 }}>
                    {branches.map((b) => {
                      const isActive = b.id === activeBranchId;
                      return (
                        <div
                          key={b.id}
                          style={{
                            border: `1px solid ${isActive ? 'var(--sheet-selection-border)' : 'var(--border-color)'}`,
                            borderRadius: 14,
                            padding: 12,
                            background: isActive ? 'rgba(59, 130, 246, 0.06)' : 'var(--card-bg)',
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'space-between',
                            gap: 10,
                          }}
                        >
                          <div style={{ minWidth: 0 }}>
                            <div className="text-sm font-semibold truncate" style={{ color: 'var(--text-primary)' }}>
                              {b.name}
                            </div>
                            <div className="text-xs mt-0.5" style={{ color: 'var(--text-secondary)' }}>
                              Updated: {formatTime(b.updatedAt)}
                            </div>
                          </div>

                          <div className="flex items-center gap-8">
                            {isActive ? (
                              <button
                                type="button"
                                className="action-btn primary"
                                style={{ height: 32 }}
                                disabled={mergeLoading}
                                onClick={() => handlePrepareMerge(b.id)}
                              >
                                <GitMerge size={14} />
                                Merge
                              </button>
                            ) : (
                              <button
                                type="button"
                                className="action-btn"
                                style={{ height: 32 }}
                                disabled={mergeLoading}
                                onClick={() => onCheckoutBranch(b.id)}
                              >
                                Checkout
                              </button>
                            )}

                            <button
                              type="button"
                              className="icon-btn"
                              style={{ width: 32, height: 32 }}
                              title="Delete"
                              onClick={() => handleDelete(b.id)}
                            >
                              <Trash2 size={14} />
                            </button>
                          </div>
                        </div>
                      );
                    })}
                  </div>
                )}
              </div>

              {mergeLoading ? (
                <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                  Processing…
                </div>
              ) : null}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

