import React, { useState, useRef, useEffect } from 'react';
import {
  Bold, Italic, Underline, AlignLeft, AlignCenter, AlignRight,
  Type, PaintBucket, Sparkles, Search, X, Undo2, Redo2, Eraser,
  AlignVerticalJustifyStart, AlignVerticalJustifyCenter, AlignVerticalJustifyEnd,
  TextWrap, Printer, Paintbrush, Merge, Square, Plus, Minus, Save, ChevronDown, MoreHorizontal, History
} from 'lucide-react';
import { CellStyle } from '../types';
import { usePresence } from '../utils/usePresence';
import ColorPicker from './ColorPicker';
import Tooltip from './Tooltip';

interface ToolbarProps {
  onStyleChange: (style: Partial<CellStyle>) => void;
  activeStyle: CellStyle;
  isAiOpen?: boolean;
  onToggleAi: () => void;
  onOpenFindReplace?: () => void;
  onUndo?: () => void;
  onRedo?: () => void;
  canUndo?: boolean;
  canRedo?: boolean;
  onPrint?: () => void;
  onFormatPainter?: () => void;
  formatPainterActive?: boolean;
  onMergeCells?: () => void;
  onUnmergeCells?: () => void;
  isMerged?: boolean;
  autoSaveEnabled?: boolean;
  onToggleAutoSave?: () => void;
  onOpenVersionHistory?: () => void;
  density?: 'comfortable' | 'compact';
}

const Toolbar: React.FC<ToolbarProps> = ({
  onStyleChange,
  activeStyle,
  isAiOpen,
  onToggleAi,
  onOpenFindReplace,
  onUndo,
  onRedo,
  canUndo,
  canRedo,
  onPrint,
  onFormatPainter,
  formatPainterActive,
  onMergeCells,
  onUnmergeCells,
  isMerged,
  autoSaveEnabled,
  onToggleAutoSave,
  onOpenVersionHistory,
  density
}) => {
  const [showFontDropdown, setShowFontDropdown] = useState(false);
  const fontDropdownRef = useRef<HTMLDivElement>(null);
  const fontDropdownPresence = usePresence(showFontDropdown, { exitDurationMs: 240 });
  const [moreOpen, setMoreOpen] = useState(false);
  const moreRef = useRef<HTMLDivElement>(null);
  const morePresence = usePresence(moreOpen, { exitDurationMs: 240 });

  const controlSize = density === 'compact' ? 28 : 32;
  const iconBtnStyle: React.CSSProperties = { width: controlSize, height: controlSize };
  const iconBtnClass = (active: boolean) => `icon-btn${active ? ' is-active' : ''}`;

  const FONT_FAMILIES = [
    'Inter',
    'Poppins',
    'Roboto',
    'Open Sans',
    'Lato',
    'Montserrat',
    'Raleway',
    'Outfit',
    'Space Grotesk',
    'JetBrains Mono',
    'Fira Code',
    'Source Code Pro',
    'Playfair Display',
    'Merriweather',
    'Crimson Text',
    'Lora',
    'Bebas Neue',
    'Oswald',
    'Ubuntu',
    'Quicksand',
    'Nunito',
    'Comfortaa',
    'Arial',
    'Georgia',
    'Times New Roman',
    'Courier New'
  ];

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (fontDropdownRef.current && !fontDropdownRef.current.contains(event.target as Node)) {
        setShowFontDropdown(false);
      }
      if (moreRef.current && !moreRef.current.contains(event.target as Node)) {
        setMoreOpen(false);
      }
    };

    if (showFontDropdown || moreOpen) {
      document.addEventListener('mousedown', handleClickOutside);
      return () => document.removeEventListener('mousedown', handleClickOutside);
    }
  }, [showFontDropdown, moreOpen]);
  const FONT_SIZES = [8, 10, 12, 13, 14, 16, 18, 24, 32, 48];
  const NUMBER_FORMATS: { label: string; value: CellStyle['numberFormat'] }[] = [
    { label: 'General', value: 'general' },
    { label: 'Number', value: 'number' },
    { label: 'Currency', value: 'currency' },
    { label: 'Percent', value: 'percent' }
  ];

  const currentFont = activeStyle.fontFamily || FONT_FAMILIES[0];
  const currentSize = activeStyle.fontSize || 13;
  const currentFormat = activeStyle.numberFormat || 'general';
  const currentDecimals = activeStyle.decimalPlaces ?? 2;
  const currentVertical = activeStyle.verticalAlign || 'middle';
  const currentWrap = activeStyle.wrapMode || 'overflow';
  const toolbarHeight = density === 'compact' ? 44 : 48;

  const handleDecimalChange = (delta: number) => {
    const base = Math.max(0, currentDecimals + delta);
    onStyleChange({
      numberFormat: activeStyle.numberFormat || 'number',
      decimalPlaces: base
    });
  };

  return (
    <div
      className="border-b flex items-center px-4 space-x-2 z-20"
      style={{
        height: toolbarHeight,
        borderColor: 'var(--chrome-border)',
        background: 'var(--chrome-bg)',
        backdropFilter: 'blur(18px) saturate(180%)',
        WebkitBackdropFilter: 'blur(18px) saturate(180%)',
      }}
    >
      {/* Undo/Redo */}
      <div className="flex items-center space-x-1 border-r pr-2" style={{ borderColor: 'var(--border-color)' }}>
        <Tooltip label="Undo" shortcut="Ctrl+Z">
          <button
            className="icon-btn"
            onClick={onUndo}
            disabled={!canUndo}
            aria-label="Undo"
            style={iconBtnStyle}
            type="button"
          >
            <Undo2 size={16} />
          </button>
        </Tooltip>
        <Tooltip label="Redo" shortcut="Ctrl+Y">
          <button
            className="icon-btn"
            onClick={onRedo}
            disabled={!canRedo}
            aria-label="Redo"
            style={iconBtnStyle}
            type="button"
          >
            <Redo2 size={16} />
          </button>
        </Tooltip>
        <Tooltip label="Print" shortcut="Ctrl+P">
          <button
            className="icon-btn"
            onClick={onPrint}
            aria-label="Print"
            style={iconBtnStyle}
            type="button"
          >
            <Printer size={16} />
          </button>
        </Tooltip>
      </div>

      {/* Format Painter */}
      <div className="flex items-center border-r pr-2" style={{ borderColor: 'var(--border-color)' }}>
        <Tooltip label="Format Painter">
          <button
            className={iconBtnClass(!!formatPainterActive)}
            onClick={onFormatPainter}
            aria-label="Format Painter"
            style={iconBtnStyle}
            type="button"
          >
            <Paintbrush size={16} />
          </button>
        </Tooltip>
      </div>

      <div className="flex items-center space-x-2 border-r pr-2" style={{ borderColor: 'var(--border-color)' }}>
        {/* Custom Animated Font Dropdown */}
        <div className="relative" ref={fontDropdownRef}>
          <button
            onClick={() => setShowFontDropdown(!showFontDropdown)}
            className="action-btn"
            style={{
              height: controlSize,
              minWidth: 140,
              position: 'relative',
              justifyContent: 'flex-start',
              paddingRight: 32,
              borderColor: 'var(--chrome-border)',
              background: 'var(--chrome-control-bg)',
              color: 'var(--text-primary)',
              fontFamily: currentFont
            }}
            type="button"
          >
            <span className="truncate">{currentFont}</span>
            <ChevronDown
              size={16}
              className="absolute right-2 top-1/2 -translate-y-1/2 transition-transform duration-200"
              style={{ transform: showFontDropdown ? 'translateY(-50%) rotate(180deg)' : 'translateY(-50%)' }}
            />
          </button>

          {fontDropdownPresence.isMounted && (
            <>
              <style>{`
	                .font-dropdown-item {
	                  transition: all 0.15s ease;
	                }
	                .font-dropdown-item:hover {
	                  background: var(--bg-light);
	                  padding-left: 16px;
	                }
	                .font-dropdown-menu-container {
	                  position: absolute !important;
	                  top: 100% !important;
                  left: 0 !important;
                  z-index: 999999 !important;
                  margin-top: 4px !important;
                }
              `}</style>
              <div
                className="font-dropdown-menu-container ui-popover w-64 max-h-96 overflow-y-auto border rounded-lg shadow-xl"
                data-state={fontDropdownPresence.state}
                style={{
                  background: 'var(--card-bg)',
                  borderColor: 'var(--border-color)',
                  boxShadow: '0 10px 40px rgba(0, 0, 0, 0.15), 0 0 0 1px rgba(0, 0, 0, 0.05)',
                  transformOrigin: 'top left'
                }}
              >
                {FONT_FAMILIES.map((font) => {
                  const isSelected = currentFont === font;
                  return (
                  <button
                    key={font}
                    onClick={(e) => {
                      e.preventDefault();
                      e.stopPropagation();
                      onStyleChange({ fontFamily: font });
                      setShowFontDropdown(false);
                    }}
                    className="font-dropdown-item w-full text-left px-3 py-2.5 text-sm hover:bg-blue-50 border-b last:border-b-0"
                    style={{
                      fontFamily: font,
                      borderColor: 'rgba(0,0,0,0.05)',
                      background: isSelected ? 'var(--sheet-selection-fill-weak)' : 'transparent',
                      color: isSelected ? 'var(--brand)' : 'var(--text-primary)',
                      fontWeight: isSelected ? 600 : 400,
                      cursor: 'pointer'
                    }}
                  >
                    <div className="flex items-center justify-between">
                      <span>{font}</span>
                      {isSelected && (
                        <span className="text-xs" style={{ color: 'var(--brand)' }}>✓</span>
                      )}
                    </div>
                  </button>
                  );
                })}
              </div>
            </>
          )}
        </div>
        <div className="flex items-center space-x-1">
          <Tooltip label="Decrease font size">
            <button
              className="icon-btn"
              onClick={() => onStyleChange({ fontSize: Math.max(8, currentSize - 1) })}
              aria-label="Decrease font size"
              style={iconBtnStyle}
              type="button"
            >
              <Minus size={14} />
            </button>
          </Tooltip>
          <select
            value={currentSize}
            onChange={(e) => onStyleChange({ fontSize: parseInt(e.target.value, 10) })}
            className="px-2 text-sm border w-16"
            style={{
              height: controlSize,
              borderColor: 'var(--chrome-border)',
              background: 'var(--chrome-control-bg)',
              borderRadius: 'var(--radius-sm)',
              color: 'var(--text-primary)'
            }}
          >
            {FONT_SIZES.map(size => (
              <option key={size} value={size}>{size}</option>
            ))}
          </select>
          <Tooltip label="Increase font size">
            <button
              className="icon-btn"
              onClick={() => onStyleChange({ fontSize: Math.min(72, currentSize + 1) })}
              aria-label="Increase font size"
              style={iconBtnStyle}
              type="button"
            >
              <Plus size={14} />
            </button>
          </Tooltip>
        </div>
      </div>

      <div className="flex items-center space-x-1 border-r pr-2" style={{ borderColor: 'var(--border-color)' }}>
        <Tooltip label="Bold" shortcut="Ctrl+B">
          <button className={iconBtnClass(!!activeStyle.bold)} style={iconBtnStyle} onClick={() => onStyleChange({ bold: !activeStyle.bold })} aria-label="Bold" type="button">
            <Bold size={16} />
          </button>
        </Tooltip>
        <Tooltip label="Italic" shortcut="Ctrl+I">
          <button className={iconBtnClass(!!activeStyle.italic)} style={iconBtnStyle} onClick={() => onStyleChange({ italic: !activeStyle.italic })} aria-label="Italic" type="button">
            <Italic size={16} />
          </button>
        </Tooltip>
        <Tooltip label="Underline" shortcut="Ctrl+U">
          <button className={iconBtnClass(!!activeStyle.underline)} style={iconBtnStyle} onClick={() => onStyleChange({ underline: !activeStyle.underline })} aria-label="Underline" type="button">
            <Underline size={16} />
          </button>
        </Tooltip>
      </div>

      <div className="flex items-center space-x-1 border-r pr-2" style={{ borderColor: 'var(--border-color)' }}>
        <Tooltip label="Align left">
          <button className={iconBtnClass(activeStyle.textAlign === 'left')} style={iconBtnStyle} onClick={() => onStyleChange({ textAlign: 'left' })} aria-label="Align left" type="button">
            <AlignLeft size={16} />
          </button>
        </Tooltip>
        <Tooltip label="Align center">
          <button className={iconBtnClass(activeStyle.textAlign === 'center')} style={iconBtnStyle} onClick={() => onStyleChange({ textAlign: 'center' })} aria-label="Align center" type="button">
            <AlignCenter size={16} />
          </button>
        </Tooltip>
        <Tooltip label="Align right">
          <button className={iconBtnClass(activeStyle.textAlign === 'right')} style={iconBtnStyle} onClick={() => onStyleChange({ textAlign: 'right' })} aria-label="Align right" type="button">
            <AlignRight size={16} />
          </button>
        </Tooltip>
      </div>

      <div className="flex items-center space-x-2 border-r pr-2" style={{ borderColor: 'var(--border-color)' }}>
        <div className="flex items-center space-x-1">
          <Type size={16} style={{ color: 'var(--text-secondary)' }} />
          <ColorPicker
            title="Matn rangi"
            value={activeStyle.color}
            defaultLabel="Avto"
            defaultSwatch="var(--text-primary)"
            onChange={(next) => onStyleChange({ color: next })}
          />
        </div>
        <div className="flex items-center space-x-1">
          <PaintBucket size={16} style={{ color: 'var(--text-secondary)' }} />
          <ColorPicker
            title="Katak rangi"
            value={activeStyle.backgroundColor}
            defaultLabel="Standart"
            defaultSwatch="var(--sheet-cell-bg)"
            onChange={(next) => onStyleChange({ backgroundColor: next })}
          />
        </div>
      </div>

      {/* More */}
      <div className="relative flex items-center border-r pr-2" style={{ borderColor: 'var(--border-color)' }} ref={moreRef}>
        <Tooltip label="More">
          <button
            type="button"
            onClick={() => setMoreOpen((v) => !v)}
            className={iconBtnClass(moreOpen)}
            style={iconBtnStyle}
            aria-label="More"
          >
            <MoreHorizontal size={16} />
          </button>
        </Tooltip>

        {morePresence.isMounted && (
          <div
            className="ui-popover"
            data-state={morePresence.state}
            style={{
              position: 'absolute',
              right: 0,
              top: '100%',
              marginTop: 8,
              width: 360,
              background: 'var(--card-bg)',
              border: '1px solid var(--border-color)',
              borderRadius: 12,
              boxShadow: 'var(--shadow-lg)',
              padding: 12,
              zIndex: 9999,
            }}
          >
            <div className="text-[11px] font-semibold uppercase tracking-wide mb-2" style={{ color: 'var(--text-secondary)' }}>
              Cells
            </div>
            <div className="grid grid-cols-2 gap-2">
              <button
                type="button"
                onClick={() => {
                  if (isMerged) onUnmergeCells?.();
                  else onMergeCells?.();
                  setMoreOpen(false);
                }}
                className="flex items-center gap-2 px-3 py-2 rounded border hover:bg-gray-100 text-sm"
                style={{ borderColor: 'var(--border-color)', color: 'var(--text-primary)' }}
              >
                <Merge size={16} />
                <span>{isMerged ? 'Unmerge' : 'Merge'}</span>
              </button>

              <button
                type="button"
                onClick={() => {
                  const hasAllBorders = activeStyle.borders?.top && activeStyle.borders?.right && activeStyle.borders?.bottom && activeStyle.borders?.left;
                  onStyleChange({
                    borders: hasAllBorders
                      ? {}
                      : {
                          top: true,
                          right: true,
                          bottom: true,
                          left: true,
                          color: 'var(--text-primary)',
                          style: 'solid',
                        },
                  });
                }}
                className="flex items-center gap-2 px-3 py-2 rounded border hover:bg-gray-100 text-sm"
                style={{ borderColor: 'var(--border-color)', color: 'var(--text-primary)' }}
              >
                <Square size={16} />
                <span>All borders</span>
              </button>
            </div>

            <div className="mt-4 text-[11px] font-semibold uppercase tracking-wide mb-2" style={{ color: 'var(--text-secondary)' }}>
              Alignment
            </div>
            <div className="flex items-center gap-2">
              <button
                type="button"
                className={iconBtnClass(currentVertical === 'top')}
                style={iconBtnStyle}
                onClick={() => onStyleChange({ verticalAlign: 'top' })}
              >
                <AlignVerticalJustifyStart size={16} />
              </button>
              <button
                type="button"
                className={iconBtnClass(currentVertical === 'middle')}
                style={iconBtnStyle}
                onClick={() => onStyleChange({ verticalAlign: 'middle' })}
              >
                <AlignVerticalJustifyCenter size={16} />
              </button>
              <button
                type="button"
                className={iconBtnClass(currentVertical === 'bottom')}
                style={iconBtnStyle}
                onClick={() => onStyleChange({ verticalAlign: 'bottom' })}
              >
                <AlignVerticalJustifyEnd size={16} />
              </button>

              <div className="ml-auto flex items-center gap-1">
                {(['overflow', 'wrap', 'clip'] as const).map((mode) => (
                  <button
                    key={mode}
                    type="button"
                    className="px-2 py-1 text-xs rounded border hover:opacity-80 transition-opacity"
                    style={{
                      borderColor: currentWrap === mode ? 'var(--sheet-selection-border)' : 'var(--border-color)',
                      background: currentWrap === mode ? 'var(--sheet-selection-fill-weak)' : 'var(--card-bg)',
                      color: currentWrap === mode ? 'var(--brand)' : 'var(--text-primary)',
                    }}
                    onClick={() => onStyleChange({ wrapMode: mode })}
                  >
                    {mode === 'wrap' ? <TextWrap size={16} /> : mode.charAt(0).toUpperCase() + mode.slice(1)}
                  </button>
                ))}
              </div>
            </div>

            <div className="mt-4 text-[11px] font-semibold uppercase tracking-wide mb-2" style={{ color: 'var(--text-secondary)' }}>
              Number format
            </div>
            <div className="flex items-center gap-2">
              <select
                value={currentFormat}
                onChange={(e) => onStyleChange({ numberFormat: e.target.value as CellStyle['numberFormat'] })}
                className="h-9 px-2 text-sm border rounded flex-1"
                style={{ borderColor: 'var(--border-color)', background: 'var(--card-bg)', color: 'var(--text-primary)' }}
              >
                {NUMBER_FORMATS.map((format) => (
                  <option key={format.value || 'general'} value={format.value || 'general'}>
                    {format.label}
                  </option>
                ))}
              </select>
              <button
                type="button"
                className="px-2 py-1 rounded border text-sm hover:bg-gray-100"
                style={{ borderColor: 'var(--border-color)', background: 'var(--card-bg)', color: 'var(--text-primary)' }}
                onClick={() => handleDecimalChange(-1)}
              >
                -.0
              </button>
              <button
                type="button"
                className="px-2 py-1 rounded border text-sm hover:bg-gray-100"
                style={{ borderColor: 'var(--border-color)', background: 'var(--card-bg)', color: 'var(--text-primary)' }}
                onClick={() => handleDecimalChange(1)}
              >
                +.0
              </button>
              <button
                type="button"
                className="px-2 py-1 rounded border text-sm hover:bg-gray-100"
                style={{ borderColor: 'var(--border-color)', background: 'var(--card-bg)', color: 'var(--text-primary)' }}
                onClick={() => onStyleChange({ numberFormat: 'percent', decimalPlaces: currentDecimals })}
              >
                %
              </button>
            </div>

            <div className="mt-4 flex justify-end">
              <button
                type="button"
                className="flex items-center gap-2 px-3 py-2 rounded border hover:bg-gray-100 text-sm"
                style={{ borderColor: 'var(--border-color)', color: 'var(--text-primary)' }}
                onClick={() => {
                  onStyleChange({ __reset: true } as any);
                  setMoreOpen(false);
                }}
              >
                <Eraser size={16} />
                Clear
              </button>
            </div>
          </div>
        )}
      </div>

      <div className="flex-1"></div>

      {onOpenVersionHistory && (
        <Tooltip label="Version history">
          <button
            type="button"
            onClick={onOpenVersionHistory}
            className="icon-btn"
            style={iconBtnStyle}
            aria-label="Version history"
          >
            <History size={16} />
          </button>
        </Tooltip>
      )}

      {/* Auto Save Toggle */}
      {onToggleAutoSave && (
        <button
          type="button"
          onClick={onToggleAutoSave}
          className="action-btn"
          style={{
            height: controlSize,
            borderColor: autoSaveEnabled ? '#22c55e' : 'var(--chrome-border)',
            background: autoSaveEnabled ? 'rgba(34, 197, 94, 0.14)' : 'var(--chrome-control-bg)',
          }}
          title={autoSaveEnabled ? "Auto Save yoqilgan" : "Auto Save o'chirilgan"}
        >
          <Save size={14} style={{ color: autoSaveEnabled ? '#16a34a' : 'var(--text-secondary)' }} />
          <span style={{ fontSize: '0.75rem', fontWeight: 600, color: autoSaveEnabled ? 'var(--text-primary)' : 'var(--text-secondary)' }}>
            Auto Save
          </span>
        </button>
      )}

      {onOpenFindReplace && (
        <Tooltip label="Qidirish" shortcut="Ctrl+F">
          <button
            onClick={onOpenFindReplace}
            className="icon-btn"
            style={iconBtnStyle}
            aria-label="Qidirish"
            type="button"
          >
            <Search size={16} />
          </button>
        </Tooltip>
      )}

      <Tooltip label={isAiOpen ? 'AI (yopish)' : 'AI'}>
        <button
          onClick={onToggleAi}
          className={iconBtnClass(!!isAiOpen)}
          style={iconBtnStyle}
          aria-label="AI"
          type="button"
        >
          {isAiOpen ? <X size={16} /> : <Sparkles size={16} />}
        </button>
      </Tooltip>
    </div>
  );
};

export default Toolbar;
