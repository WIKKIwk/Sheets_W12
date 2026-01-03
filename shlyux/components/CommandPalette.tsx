import React, { useEffect, useMemo, useRef, useState } from 'react';
import { Search } from 'lucide-react';
import { usePresence } from '../utils/usePresence';

export type CommandPaletteItem = {
  id: string;
  label: string;
  shortcut?: string;
  group?: string;
  keywords?: string;
  disabled?: boolean;
  run: () => void;
};

type CommandPaletteProps = {
  isOpen: boolean;
  commands: CommandPaletteItem[];
  onClose: () => void;
};

const normalize = (value: string) => value.toLowerCase().replace(/\s+/g, ' ').trim();

const CommandPalette: React.FC<CommandPaletteProps> = ({ isOpen, commands, onClose }) => {
  const presence = usePresence(isOpen, { exitDurationMs: 240 });
  const inputRef = useRef<HTMLInputElement | null>(null);

  const [query, setQuery] = useState('');
  const [activeIndex, setActiveIndex] = useState(0);

  useEffect(() => {
    if (!isOpen) return;
    setQuery('');
    setActiveIndex(0);
  }, [isOpen]);

  useEffect(() => {
    if (!isOpen) return;
    const t = window.setTimeout(() => inputRef.current?.focus(), 0);
    return () => window.clearTimeout(t);
  }, [isOpen]);

  const filtered = useMemo(() => {
    const q = normalize(query);
    if (!q) return commands;
    return commands.filter((cmd) => {
      const hay = normalize([cmd.label, cmd.keywords, cmd.group].filter(Boolean).join(' '));
      return hay.includes(q);
    });
  }, [commands, query]);

  useEffect(() => {
    setActiveIndex((prev) => {
      if (filtered.length === 0) return 0;
      return Math.min(prev, filtered.length - 1);
    });
  }, [filtered.length]);

  useEffect(() => {
    if (!isOpen) return;
    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', onKeyDown);
    return () => window.removeEventListener('keydown', onKeyDown);
  }, [isOpen, onClose]);

  const runActive = () => {
    const cmd = filtered[activeIndex];
    if (!cmd || cmd.disabled) return;
    onClose();
    window.requestAnimationFrame(() => cmd.run());
  };

  if (!presence.isMounted) return null;

  return (
    <div
      className="fixed inset-0 z-50 flex items-start justify-center px-4 pt-24 ui-overlay ui-scrim ui-scrim-strong"
      data-state={presence.state}
      onClick={onClose}
      role="dialog"
      aria-modal="true"
      aria-label="Command palette"
    >
      <div
        className="w-full max-w-xl rounded-2xl overflow-hidden ui-modal"
        data-state={presence.state}
        style={{ background: 'var(--card-bg)', border: '1px solid var(--border-color)', boxShadow: 'var(--shadow-lg)' }}
        onClick={(e) => e.stopPropagation()}
      >
        <div className="flex items-center gap-2 px-4 py-3 border-b" style={{ borderColor: 'var(--border-color)' }}>
          <Search size={16} style={{ color: 'var(--text-secondary)' }} />
          <input
            ref={inputRef}
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === 'ArrowDown') {
                e.preventDefault();
                setActiveIndex((i) => (filtered.length ? (i + 1) % filtered.length : 0));
                return;
              }
              if (e.key === 'ArrowUp') {
                e.preventDefault();
                setActiveIndex((i) => (filtered.length ? (i - 1 + filtered.length) % filtered.length : 0));
                return;
              }
              if (e.key === 'Enter') {
                e.preventDefault();
                runActive();
                return;
              }
            }}
            placeholder="Buyruq qidiring… (masalan: save, open, find)"
            className="w-full bg-transparent text-sm focus:outline-none"
            style={{ color: 'var(--text-primary)' }}
            autoComplete="off"
          />
          <span className="text-[11px] px-2 py-1 rounded border" style={{ borderColor: 'var(--border-color)', color: 'var(--text-secondary)' }}>
            Esc
          </span>
        </div>

        <div className="max-h-[52vh] overflow-y-auto">
          {filtered.length === 0 ? (
            <div className="px-4 py-10 text-center text-sm" style={{ color: 'var(--text-secondary)' }}>
              Hech narsa topilmadi
            </div>
          ) : (
            filtered.map((cmd, idx) => {
              const active = idx === activeIndex;
              const disabled = !!cmd.disabled;
              return (
                <button
                  key={cmd.id}
                  type="button"
                  disabled={disabled}
                  onMouseEnter={() => setActiveIndex(idx)}
                  onClick={() => {
                    if (disabled) return;
                    onClose();
                    window.requestAnimationFrame(() => cmd.run());
                  }}
                  className="w-full text-left px-4 py-3 flex items-center justify-between"
                  style={{
                    background: active ? 'var(--bg-light)' : 'transparent',
                    color: disabled ? 'var(--text-secondary)' : 'var(--text-primary)',
                    opacity: disabled ? 0.6 : 1,
                  }}
                >
                  <div className="min-w-0">
                    {cmd.group && (
                      <div className="text-[11px] font-semibold uppercase tracking-wide mb-1" style={{ color: 'var(--text-secondary)' }}>
                        {cmd.group}
                      </div>
                    )}
                    <div className="text-sm font-medium truncate">{cmd.label}</div>
                  </div>
                  {cmd.shortcut && (
                    <kbd
                      className="ml-3 text-[11px] px-2 py-1 rounded border"
                      style={{ borderColor: 'var(--border-color)', color: 'var(--text-secondary)', background: 'transparent' }}
                    >
                      {cmd.shortcut}
                    </kbd>
                  )}
                </button>
              );
            })
          )}
        </div>
      </div>
    </div>
  );
};

export default CommandPalette;
