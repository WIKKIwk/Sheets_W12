import React, { useEffect, useMemo, useRef, useState } from 'react';
import ReactDOM from 'react-dom';
import { usePresence } from '../utils/usePresence';

type TooltipSide = 'top' | 'bottom' | 'auto';

type TooltipProps = {
  label: string;
  shortcut?: string;
  side?: TooltipSide;
  children: React.ReactNode;
};

const clamp = (value: number, min: number, max: number) => Math.min(max, Math.max(min, value));

const Tooltip: React.FC<TooltipProps> = ({ label, shortcut, side = 'top', children }) => {
  const [open, setOpen] = useState(false);
  const presence = usePresence(open, { exitDurationMs: 120 });

  const triggerRef = useRef<HTMLSpanElement | null>(null);
  const [pos, setPos] = useState<{ top: number; left: number; side: Exclude<TooltipSide, 'auto'> }>({
    top: 0,
    left: 0,
    side: 'top',
  });

  const resolvedSide = useMemo(() => {
    if (side !== 'auto') return side;
    const rect = triggerRef.current?.getBoundingClientRect();
    if (!rect) return 'top';
    return rect.top < 56 ? 'bottom' : 'top';
  }, [side, open]);

  useEffect(() => {
    if (!open) return;

    const update = () => {
      const rect = triggerRef.current?.getBoundingClientRect();
      if (!rect) return;
      const centerX = rect.left + rect.width / 2;
      const clampedLeft = clamp(centerX, 12, window.innerWidth - 12);
      const top = resolvedSide === 'top' ? rect.top : rect.bottom;
      setPos({ top, left: clampedLeft, side: resolvedSide });
    };

    update();
    window.addEventListener('scroll', update, true);
    window.addEventListener('resize', update);
    return () => {
      window.removeEventListener('scroll', update, true);
      window.removeEventListener('resize', update);
    };
  }, [open, resolvedSide]);

  return (
    <span
      ref={triggerRef}
      className="relative inline-flex"
      onMouseEnter={() => setOpen(true)}
      onMouseLeave={() => setOpen(false)}
      onFocusCapture={() => setOpen(true)}
      onBlurCapture={() => setOpen(false)}
    >
      {children}
      {presence.isMounted &&
        ReactDOM.createPortal(
          <div
            className="ui-tooltip"
            data-state={presence.state}
            data-side={pos.side}
            style={{
              top: pos.top,
              left: pos.left,
            }}
          >
            <div className="ui-tooltip-inner">
              <span className="ui-tooltip-label">{label}</span>
              {shortcut && (
                <kbd className="ui-tooltip-kbd">{shortcut}</kbd>
              )}
            </div>
          </div>,
          document.body
        )}
    </span>
  );
};

export default Tooltip;
