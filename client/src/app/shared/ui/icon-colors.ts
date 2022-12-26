export const ICON_COLORS = [
  'slate',
  'neutral',
  'yellow',
  'orange',
  'rose',
  'purple',
  'violet',
  'indigo',
  'blue',
  'cyan',
  'emerald',
  'lime',
].map(x =>
  new Array(4)
    .fill(0)
    .map((_, i) => i + 1)
    .map(x => x * 2)
    .map(x => x * 100)
    .map(i => x + '-' + i)
);
