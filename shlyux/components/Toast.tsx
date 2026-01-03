import React, { useEffect, useMemo, useState } from 'react';
import { AlertCircle, AlertTriangle, CheckCircle2, Info, X } from 'lucide-react';
import { usePresence } from '../utils/usePresence';

export type ToastTone = 'success' | 'info' | 'warning' | 'danger';

export type ToastState = {
  message: string;
  tone: ToastTone;
  title?: string;
  duration?: number;
};

interface ToastProps {
  toast: ToastState | null;
  onClose: () => void;
}

const toneDefaults: Record<ToastTone, { title: string; accent: string; Icon: typeof Info }> = {
  info: { title: 'Xabar', accent: 'var(--brand)', Icon: Info },
  success: { title: 'Tayyor', accent: 'rgba(34, 197, 94, 0.95)', Icon: CheckCircle2 },
  warning: { title: 'Ogohlantirish', accent: 'rgba(245, 158, 11, 0.95)', Icon: AlertTriangle },
  danger: { title: 'Xato', accent: 'rgba(239, 68, 68, 0.95)', Icon: AlertCircle },
};

const Toast: React.FC<ToastProps> = ({ toast, onClose }) => {
  const presence = usePresence(!!toast, { exitDurationMs: 240 });
  const [snapshot, setSnapshot] = useState<ToastState | null>(toast);

  useEffect(() => {
    if (toast) setSnapshot(toast);
  }, [toast]);

  const config = useMemo(() => {
    if (!snapshot) return null;
    return toneDefaults[snapshot.tone];
  }, [snapshot]);

  useEffect(() => {
    if (!toast) return;
    const ms = toast.duration ?? 4500;
    const timer = window.setTimeout(() => onClose(), ms);
    return () => window.clearTimeout(timer);
  }, [toast, onClose]);

  if (!presence.isMounted || !snapshot || !config) return null;

  const title = snapshot.title ?? config.title;
  const Icon = config.Icon;

  return (
    <div
      className="toast ui-toast fixed top-4 right-4 z-50 max-w-md rounded-xl p-4 flex items-start gap-3"
      data-state={presence.state}
      style={{
        background: 'var(--menu-bg)',
        border: `1px solid var(--chrome-border)`,
        borderLeft: `4px solid ${config.accent}`,
        boxShadow: 'var(--shadow-lg)',
        backdropFilter: 'blur(18px) saturate(180%)',
        WebkitBackdropFilter: 'blur(18px) saturate(180%)',
      }}
      role={snapshot.tone === 'danger' ? 'alert' : 'status'}
      aria-live={snapshot.tone === 'danger' ? 'assertive' : 'polite'}
    >
      <Icon size={20} style={{ color: config.accent, flexShrink: 0, marginTop: 2 }} />
      <div className="flex-1 min-w-0">
        <p className="text-sm font-semibold" style={{ color: 'var(--text-primary)' }}>
          {title}
        </p>
        <p className="text-sm mt-1 break-words" style={{ color: 'var(--text-secondary)' }}>
          {snapshot.message}
        </p>
      </div>
      <button
        onClick={onClose}
        className="p-1.5 rounded transition-colors"
        style={{ color: 'var(--text-secondary)' }}
        onMouseEnter={(e) => (e.currentTarget.style.background = 'var(--bg-light)')}
        onMouseLeave={(e) => (e.currentTarget.style.background = 'transparent')}
        aria-label="Yopish"
        type="button"
      >
        <X size={16} />
      </button>
    </div>
  );
};

export default Toast;
