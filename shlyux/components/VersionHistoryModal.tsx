import React, { useEffect, useMemo, useState } from 'react';
import { Clock, Copy, GitBranch, RotateCcw, Trash2, X } from 'lucide-react';
import { usePresence } from '../utils/usePresence';
import type { SheetState } from '../types';
import type { CellValueDiff, SheetSnapshot } from '../utils/snapshots';
import { diffCellValues } from '../utils/snapshots';
import { getColumnLabel } from '../utils/spreadsheetUtils';

type VersionHistoryModalProps = {
  isOpen: boolean;
  snapshots: SheetSnapshot[];
  currentSheet: SheetState;
  onClose: () => void;
  onCreateSnapshot: (label: string) => void;
  onRestoreSnapshot: (snapshotId: string) => void;
  onDeleteSnapshot: (snapshotId: string) => void;
  onJumpToCell?: (row: number, col: number) => void;
};

const formatTime = (timestampMs: number): string => {
  try {
    return new Date(timestampMs).toLocaleString();
  } catch {
    return `${timestampMs}`;
  }
};

const coordsToLabel = (row: number, col: number): string => `${getColumnLabel(col)}${row + 1}`;

const truncate = (value: string, max = 64) => {
  if (value.length <= max) return value;
  return `${value.slice(0, max - 1)}…`;
};

export default function VersionHistoryModal({
  isOpen,
  snapshots,
  currentSheet,
  onClose,
  onCreateSnapshot,
  onRestoreSnapshot,
  onDeleteSnapshot,
  onJumpToCell,
}: VersionHistoryModalProps) {
  const modalPresence = usePresence(isOpen, { exitDurationMs: 240 });
  const [label, setLabel] = useState('');
  const [selectedId, setSelectedId] = useState<string | null>(null);
  const [query, setQuery] = useState('');

  useEffect(() => {
    if (!isOpen) return;
    setQuery('');
    setLabel('');
  }, [isOpen]);

  useEffect(() => {
    if (!snapshots.length) {
      setSelectedId(null);
      return;
    }
    setSelectedId((prev) => {
      if (prev && snapshots.some((s) => s.id === prev)) return prev;
      return snapshots[0].id;
    });
  }, [snapshots]);

  useEffect(() => {
    if (!isOpen) return;
    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', onKeyDown);
    return () => window.removeEventListener('keydown', onKeyDown);
  }, [isOpen, onClose]);

  const selected = useMemo(
    () => (selectedId ? snapshots.find((s) => s.id === selectedId) ?? null : null),
    [selectedId, snapshots]
  );

  const diffs = useMemo<CellValueDiff[]>(() => {
    if (!selected) return [];
    return diffCellValues(selected.state, currentSheet);
  }, [selected, currentSheet]);

  const filteredDiffs = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return diffs;
    return diffs.filter((d) => {
      const label = coordsToLabel(d.row, d.col).toLowerCase();
      return label.includes(q) || d.before.toLowerCase().includes(q) || d.after.toLowerCase().includes(q);
    });
  }, [diffs, query]);

  const handleCreate = () => {
    const clean = label.trim();
    const auto = `Snapshot — ${formatTime(Date.now())}`;
    onCreateSnapshot(clean || auto);
    setLabel('');
  };

  const handleDelete = (id: string) => {
    const ok = window.confirm("Snapshot o‘chiriladi. Davom etamizmi?");
    if (!ok) return;
    onDeleteSnapshot(id);
    if (selectedId === id) setSelectedId(null);
  };

  const handleRestore = (id: string) => {
    onRestoreSnapshot(id);
  };

  const copyDiffLine = (diff: CellValueDiff) => {
    const line = `${coordsToLabel(diff.row, diff.col)}: "${diff.before}" -> "${diff.after}"`;
    navigator.clipboard?.writeText(line).catch(() => {});
  };

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
                Version History
              </div>
              <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                Snapshot + diff + restore (MVP)
              </div>
            </div>
          </div>
          <button className="icon-btn" style={{ width: 32, height: 32 }} onClick={onClose} title="Yopish" type="button">
            <X size={16} />
          </button>
        </div>

        <div style={{ flex: 1, display: 'grid', gridTemplateColumns: '320px 1fr', minHeight: 0 }}>
          <div
            style={{
              borderRight: '1px solid var(--border-color)',
              background: 'var(--bg-light)',
              minHeight: 0,
              display: 'flex',
              flexDirection: 'column',
            }}
          >
            <div style={{ padding: 14, borderBottom: '1px solid var(--border-color)' }}>
              <div className="text-xs font-semibold mb-2" style={{ color: 'var(--text-secondary)' }}>
                Create snapshot
              </div>
              <div style={{ display: 'flex', gap: 8 }}>
                <input
                  value={label}
                  onChange={(e) => setLabel(e.target.value)}
                  placeholder="Label (ixtiyoriy)"
                  className="px-3 text-sm"
                  style={{
                    flex: 1,
                    height: 32,
                    borderRadius: 10,
                    border: '1px solid var(--border-color)',
                    background: 'var(--card-bg)',
                    color: 'var(--text-primary)',
                  }}
                />
                <button className="action-btn primary" style={{ height: 32 }} onClick={handleCreate} type="button">
                  <Clock size={14} />
                  Snapshot
                </button>
              </div>
            </div>

            <div style={{ padding: 10, overflow: 'auto' }}>
              {snapshots.length === 0 ? (
                <div className="text-sm" style={{ color: 'var(--text-secondary)', padding: 12 }}>
                  Hozircha snapshot yo‘q. Birinchi snapshot yarating.
                </div>
              ) : (
                <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
                  {snapshots.map((s) => {
                    const active = s.id === selectedId;
                    return (
                      <button
                        key={s.id}
                        type="button"
                        onClick={() => setSelectedId(s.id)}
                        className="w-full text-left"
                        style={{
                          padding: 12,
                          borderRadius: 14,
                          border: `1px solid ${active ? 'var(--sheet-selection-border)' : 'var(--border-color)'}`,
                          background: active ? 'var(--card-bg)' : 'rgba(255,255,255,0.55)',
                          boxShadow: active ? '0 10px 30px rgba(0,0,0,0.10)' : 'none',
                        }}
                      >
                        <div className="flex items-center justify-between gap-2">
                          <div style={{ minWidth: 0 }}>
                            <div className="text-sm font-semibold truncate" style={{ color: 'var(--text-primary)' }}>
                              {s.label || 'Snapshot'}
                            </div>
                            <div className="text-xs mt-0.5" style={{ color: 'var(--text-secondary)' }}>
                              {formatTime(s.createdAt)}
                            </div>
                          </div>
                          <div className="flex items-center gap-6">
                            <button
                              type="button"
                              className="icon-btn"
                              style={{ width: 30, height: 30 }}
                              title="Restore"
                              onClick={(e) => {
                                e.stopPropagation();
                                handleRestore(s.id);
                              }}
                            >
                              <RotateCcw size={14} />
                            </button>
                            <button
                              type="button"
                              className="icon-btn"
                              style={{ width: 30, height: 30 }}
                              title="Delete"
                              onClick={(e) => {
                                e.stopPropagation();
                                handleDelete(s.id);
                              }}
                            >
                              <Trash2 size={14} />
                            </button>
                          </div>
                        </div>
                      </button>
                    );
                  })}
                </div>
              )}
            </div>
          </div>

          <div style={{ padding: 16, minHeight: 0, display: 'flex', flexDirection: 'column', gap: 12 }}>
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: 12 }}>
              <div>
                <div className="text-sm font-semibold" style={{ color: 'var(--text-primary)' }}>
                  Compare to current
                </div>
                <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                  {selected ? `${selected.label || 'Snapshot'} → Current` : 'Snapshot tanlang'}
                </div>
              </div>

              <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                  {diffs.length} ta o‘zgarish
                </div>
                <input
                  value={query}
                  onChange={(e) => setQuery(e.target.value)}
                  placeholder="Search (A1, SUM, 100...)"
                  className="px-3 text-sm"
                  style={{
                    width: 260,
                    height: 32,
                    borderRadius: 10,
                    border: '1px solid var(--border-color)',
                    background: 'var(--card-bg)',
                    color: 'var(--text-primary)',
                  }}
                />
              </div>
            </div>

            <div
              style={{
                flex: 1,
                border: '1px solid var(--border-color)',
                borderRadius: 14,
                overflow: 'hidden',
                background: 'var(--card-bg)',
                minHeight: 0,
              }}
            >
              <div
                className="grid"
                style={{
                  gridTemplateColumns: '120px 1fr 1fr 44px',
                  gap: 0,
                  padding: '10px 12px',
                  borderBottom: '1px solid var(--border-color)',
                  background: 'var(--bg-light)',
                  fontSize: 12,
                  fontWeight: 700,
                  color: 'var(--text-secondary)',
                }}
              >
                <div>Cell</div>
                <div>Before</div>
                <div>After</div>
                <div />
              </div>

              <div style={{ overflow: 'auto', maxHeight: '100%' }}>
                {selected == null ? (
                  <div style={{ padding: 14, color: 'var(--text-secondary)' }}>Snapshot tanlang.</div>
                ) : filteredDiffs.length === 0 ? (
                  <div style={{ padding: 14, color: 'var(--text-secondary)' }}>O‘zgarish topilmadi.</div>
                ) : (
                  filteredDiffs.slice(0, 300).map((d) => (
                    <div
                      key={d.id}
                      className="grid"
                      style={{
                        gridTemplateColumns: '120px 1fr 1fr 44px',
                        gap: 0,
                        padding: '10px 12px',
                        borderBottom: '1px solid var(--border-color)',
                        cursor: onJumpToCell ? 'pointer' : 'default',
                      }}
                      onClick={() => onJumpToCell?.(d.row, d.col)}
                      title={coordsToLabel(d.row, d.col)}
                    >
                      <div className="font-semibold" style={{ color: 'var(--text-primary)' }}>
                        {coordsToLabel(d.row, d.col)}
                      </div>
                      <div style={{ color: 'var(--text-secondary)', fontFamily: 'var(--font-family)' }}>
                        {truncate(d.before)}
                      </div>
                      <div style={{ color: 'var(--text-primary)', fontFamily: 'var(--font-family)' }}>
                        {truncate(d.after)}
                      </div>
                      <div className="flex justify-end">
                        <button
                          type="button"
                          className="icon-btn"
                          style={{ width: 30, height: 30 }}
                          title="Copy diff"
                          onClick={(e) => {
                            e.stopPropagation();
                            copyDiffLine(d);
                          }}
                        >
                          <Copy size={14} />
                        </button>
                      </div>
                    </div>
                  ))
                )}
              </div>
            </div>

            {filteredDiffs.length > 300 && (
              <div className="text-xs" style={{ color: 'var(--text-secondary)' }}>
                Juda ko‘p o‘zgarish bor — faqat birinchi 300 ta ko‘rsatildi.
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

