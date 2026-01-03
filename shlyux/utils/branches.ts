import type { SheetState, CellData, CellStyle } from '../types';
import { safeCloneSheetState } from './snapshots';

export type SheetBranch = {
  id: string;
  name: string;
  createdAt: number;
  updatedAt: number;
  baseState: SheetState;
  state: SheetState;
};

export type SheetMergeConflict = {
  id: string; // "row,col"
  row: number;
  col: number;
  base: string;
  main: string;
  branch: string;
};

export type SheetMergeResult = {
  merged: SheetState;
  conflicts: SheetMergeConflict[];
  applied: number;
  appliedMeta: number;
};

const BRANCH_KEY_PREFIX = 'sheetmaster-branches-v1:';
const ACTIVE_BRANCH_KEY_PREFIX = 'sheetmaster-active-branch-v1:';
const MAIN_SHADOW_KEY_PREFIX = 'sheetmaster-main-shadow-v1:';
const BRANCH_LIMIT = 20;

export function getBranchStorageKey(fileId: number | null): string {
  return `${BRANCH_KEY_PREFIX}${fileId ?? 'local'}`;
}

export function getActiveBranchStorageKey(fileId: number | null): string {
  return `${ACTIVE_BRANCH_KEY_PREFIX}${fileId ?? 'local'}`;
}

export function getMainShadowStorageKey(fileId: number | null): string {
  return `${MAIN_SHADOW_KEY_PREFIX}${fileId ?? 'local'}`;
}

const isPlainObject = (value: unknown): value is Record<string, unknown> =>
  !!value && typeof value === 'object' && !Array.isArray(value);

const deepEqual = (a: any, b: any): boolean => {
  if (a === b) return true;
  if (a == null || b == null) return false;
  if (Array.isArray(a) || Array.isArray(b)) {
    if (!Array.isArray(a) || !Array.isArray(b)) return false;
    if (a.length !== b.length) return false;
    for (let i = 0; i < a.length; i++) {
      if (!deepEqual(a[i], b[i])) return false;
    }
    return true;
  }
  if (typeof a !== 'object' || typeof b !== 'object') return false;
  const aKeys = Object.keys(a);
  const bKeys = Object.keys(b);
  if (aKeys.length !== bKeys.length) return false;
  for (const key of aKeys) {
    if (!Object.prototype.hasOwnProperty.call(b, key)) return false;
    if (!deepEqual(a[key], b[key])) return false;
  }
  return true;
};

type CellInput = { value: string; style?: CellStyle } | null;

const normalizeCellInput = (cell?: CellData): CellInput => {
  if (!cell) return null;
  const value = cell.value ?? '';
  const style = cell.style;
  if (value === '' && (style == null || (isPlainObject(style) && Object.keys(style).length === 0))) {
    return null;
  }
  return style ? { value, style } : { value };
};

const cellInputEqual = (a: CellInput, b: CellInput): boolean => {
  if (a === b) return true;
  if (a == null || b == null) return a == null && b == null;
  if (a.value !== b.value) return false;
  return deepEqual(a.style ?? null, b.style ?? null);
};

export function readBranches(fileId: number | null): SheetBranch[] {
  if (typeof window === 'undefined') return [];
  try {
    const raw = localStorage.getItem(getBranchStorageKey(fileId));
    if (!raw) return [];
    const parsed = JSON.parse(raw) as unknown;
    if (!Array.isArray(parsed)) return [];
    return parsed
      .filter((b): b is SheetBranch => {
        if (!isPlainObject(b)) return false;
        return typeof b.id === 'string'
          && typeof b.name === 'string'
          && typeof b.createdAt === 'number'
          && typeof b.updatedAt === 'number'
          && isPlainObject(b.baseState)
          && isPlainObject(b.state);
      })
      .slice(0, BRANCH_LIMIT);
  } catch {
    return [];
  }
}

export function writeBranches(fileId: number | null, branches: SheetBranch[]) {
  if (typeof window === 'undefined') return;
  try {
    localStorage.setItem(getBranchStorageKey(fileId), JSON.stringify(branches.slice(0, BRANCH_LIMIT)));
  } catch {
    // ignore
  }
}

export function addBranch(fileId: number | null, branch: SheetBranch): SheetBranch[] {
  const current = readBranches(fileId);
  const next = [branch, ...current].slice(0, BRANCH_LIMIT);
  writeBranches(fileId, next);
  return next;
}

export function updateBranch(fileId: number | null, branchId: string, updater: (branch: SheetBranch) => SheetBranch): SheetBranch[] {
  const current = readBranches(fileId);
  const next = current.map((b) => (b.id === branchId ? updater(b) : b));
  writeBranches(fileId, next);
  return next;
}

export function updateBranchState(fileId: number | null, branchId: string, nextState: SheetState): SheetBranch[] {
  return updateBranch(fileId, branchId, (b) => ({
    ...b,
    updatedAt: Date.now(),
    state: safeCloneSheetState(nextState),
  }));
}

export function renameBranch(fileId: number | null, branchId: string, name: string): SheetBranch[] {
  return updateBranch(fileId, branchId, (b) => ({ ...b, name }));
}

export function deleteBranch(fileId: number | null, branchId: string): SheetBranch[] {
  const current = readBranches(fileId);
  const next = current.filter((b) => b.id !== branchId);
  writeBranches(fileId, next);
  return next;
}

export function readActiveBranchId(fileId: number | null): string | null {
  if (typeof window === 'undefined') return null;
  try {
    const raw = localStorage.getItem(getActiveBranchStorageKey(fileId));
    if (!raw) return null;
    const parsed = JSON.parse(raw) as unknown;
    return typeof parsed === 'string' ? parsed : null;
  } catch {
    return null;
  }
}

export function writeActiveBranchId(fileId: number | null, branchId: string | null) {
  if (typeof window === 'undefined') return;
  try {
    if (branchId) localStorage.setItem(getActiveBranchStorageKey(fileId), JSON.stringify(branchId));
    else localStorage.removeItem(getActiveBranchStorageKey(fileId));
  } catch {
    // ignore
  }
}

export function readMainShadow(fileId: number | null): SheetState | null {
  if (typeof window === 'undefined') return null;
  try {
    const raw = localStorage.getItem(getMainShadowStorageKey(fileId));
    if (!raw) return null;
    const parsed = JSON.parse(raw) as unknown;
    if (!isPlainObject(parsed)) return null;
    return parsed as SheetState;
  } catch {
    return null;
  }
}

export function writeMainShadow(fileId: number | null, sheet: SheetState) {
  if (typeof window === 'undefined') return;
  try {
    localStorage.setItem(getMainShadowStorageKey(fileId), JSON.stringify(sheet));
  } catch {
    // ignore
  }
}

const mergeNumberRecordThreeWay = (
  base: Record<string, number> | undefined,
  main: Record<string, number> | undefined,
  branch: Record<string, number> | undefined,
): { merged: Record<string, number>; applied: number } => {
  const merged: Record<string, number> = { ...(main || {}) };
  const keys = new Set<string>([
    ...Object.keys(base || {}),
    ...Object.keys(main || {}),
    ...Object.keys(branch || {}),
  ]);
  let applied = 0;
  keys.forEach((key) => {
    const b = base?.[key];
    const m = main?.[key];
    const br = branch?.[key];
    if (br === m) return;
    if (m === b && br !== b) {
      if (typeof br === 'number') merged[key] = br;
      else delete merged[key];
      applied++;
      return;
    }
    if (br === b && m !== b) return;
    // conflict -> keep main
  });
  return { merged, applied };
};

export function mergeSheetsThreeWay(base: SheetState, main: SheetState, branch: SheetState): SheetMergeResult {
  const merged = safeCloneSheetState(main);
  const baseData = base.data || {};
  const mainData = main.data || {};
  const branchData = branch.data || {};
  const keys = new Set<string>([
    ...Object.keys(baseData),
    ...Object.keys(mainData),
    ...Object.keys(branchData),
  ]);

  const mergedData = { ...(merged.data || {}) };
  const conflicts: SheetMergeConflict[] = [];
  let applied = 0;

  keys.forEach((id) => {
    const bInput = normalizeCellInput(baseData[id]);
    const mInput = normalizeCellInput(mainData[id]);
    const brInput = normalizeCellInput(branchData[id]);

    if (cellInputEqual(mInput, brInput)) return;

    if (cellInputEqual(mInput, bInput) && !cellInputEqual(brInput, bInput)) {
      if (brInput == null) {
        delete mergedData[id];
      } else {
        mergedData[id] = { value: brInput.value, style: brInput.style };
      }
      applied++;
      return;
    }

    if (cellInputEqual(brInput, bInput) && !cellInputEqual(mInput, bInput)) {
      return;
    }

    const [rowStr, colStr] = id.split(',');
    const row = Number.parseInt(rowStr, 10);
    const col = Number.parseInt(colStr, 10);
    conflicts.push({
      id,
      row: Number.isFinite(row) ? row : -1,
      col: Number.isFinite(col) ? col : -1,
      base: baseData[id]?.value ?? '',
      main: mainData[id]?.value ?? '',
      branch: branchData[id]?.value ?? '',
    });
  });

  merged.data = mergedData;

  let appliedMeta = 0;
  const colWidths = mergeNumberRecordThreeWay(base.columnWidths, main.columnWidths, branch.columnWidths);
  merged.columnWidths = colWidths.merged;
  appliedMeta += colWidths.applied;

  const rowHeights = mergeNumberRecordThreeWay(base.rowHeights, main.rowHeights, branch.rowHeights);
  merged.rowHeights = rowHeights.merged;
  appliedMeta += rowHeights.applied;

  if (deepEqual(main.freezePosition ?? null, base.freezePosition ?? null) && !deepEqual(branch.freezePosition ?? null, base.freezePosition ?? null)) {
    merged.freezePosition = branch.freezePosition;
    appliedMeta++;
  }

  if (deepEqual(main.mergedCells ?? null, base.mergedCells ?? null) && !deepEqual(branch.mergedCells ?? null, base.mergedCells ?? null)) {
    merged.mergedCells = branch.mergedCells;
    appliedMeta++;
  }

  merged.rowCount = Math.max(main.rowCount || 0, branch.rowCount || 0, base.rowCount || 0);

  return { merged, conflicts, applied, appliedMeta };
}

