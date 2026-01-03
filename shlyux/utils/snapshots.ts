import type { SheetState } from '../types';

export type SheetSnapshot = {
  id: string;
  label: string;
  createdAt: number;
  state: SheetState;
};

export type CellValueDiff = {
  id: string; // "row,col"
  row: number;
  col: number;
  before: string;
  after: string;
};

const SNAPSHOT_KEY_PREFIX = 'sheetmaster-snapshots-v1:';
const SNAPSHOT_LIMIT = 20;

export function getSnapshotStorageKey(fileId: number | null): string {
  return `${SNAPSHOT_KEY_PREFIX}${fileId ?? 'local'}`;
}

export function safeCloneSheetState(sheet: SheetState): SheetState {
  if (typeof structuredClone === 'function') {
    return structuredClone(sheet);
  }
  return JSON.parse(JSON.stringify(sheet)) as SheetState;
}

export function readSnapshots(fileId: number | null): SheetSnapshot[] {
  if (typeof window === 'undefined') return [];
  try {
    const raw = localStorage.getItem(getSnapshotStorageKey(fileId));
    if (!raw) return [];
    const parsed = JSON.parse(raw) as unknown;
    if (!Array.isArray(parsed)) return [];
    return parsed
      .filter((s): s is SheetSnapshot => {
        if (!s || typeof s !== 'object') return false;
        const anyS = s as any;
        return typeof anyS.id === 'string'
          && typeof anyS.label === 'string'
          && typeof anyS.createdAt === 'number'
          && typeof anyS.state === 'object'
          && anyS.state;
      })
      .slice(0, SNAPSHOT_LIMIT);
  } catch {
    return [];
  }
}

export function writeSnapshots(fileId: number | null, snapshots: SheetSnapshot[]) {
  if (typeof window === 'undefined') return;
  try {
    localStorage.setItem(getSnapshotStorageKey(fileId), JSON.stringify(snapshots.slice(0, SNAPSHOT_LIMIT)));
  } catch {
    // ignore
  }
}

export function addSnapshot(fileId: number | null, snapshot: SheetSnapshot): SheetSnapshot[] {
  const current = readSnapshots(fileId);
  const next = [snapshot, ...current].slice(0, SNAPSHOT_LIMIT);
  writeSnapshots(fileId, next);
  return next;
}

export function deleteSnapshot(fileId: number | null, snapshotId: string): SheetSnapshot[] {
  const current = readSnapshots(fileId);
  const next = current.filter((s) => s.id !== snapshotId);
  writeSnapshots(fileId, next);
  return next;
}

export function diffCellValues(before: SheetState, after: SheetState): CellValueDiff[] {
  const diffs: CellValueDiff[] = [];
  const keys = new Set<string>([...Object.keys(before.data || {}), ...Object.keys(after.data || {})]);
  keys.forEach((id) => {
    const b = before.data?.[id]?.value ?? '';
    const a = after.data?.[id]?.value ?? '';
    if (b === a) return;
    const [rowStr, colStr] = id.split(',');
    const row = Number.parseInt(rowStr, 10);
    const col = Number.parseInt(colStr, 10);
    if (Number.isNaN(row) || Number.isNaN(col)) return;
    diffs.push({ id, row, col, before: b, after: a });
  });
  diffs.sort((x, y) => (x.row - y.row) || (x.col - y.col));
  return diffs;
}
