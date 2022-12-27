const appearanceColors = [
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

export const APPEARANCE_COLORS_COUNT = appearanceColors.length;
export const APPEARANCE_COLOR_VARIANTS_COUNT = appearanceColors[0].length;
export const APPEARANCE_COLORS = new Array(APPEARANCE_COLOR_VARIANTS_COUNT)
  .fill(0)
  .flatMap((_, i) =>
    new Array(APPEARANCE_COLORS_COUNT)
      .fill(i)
      .map((y, x) => appearanceColors[x][y])
  );

export const getRandomAppearanceColor = () => {
  return APPEARANCE_COLORS[
    Math.floor(Math.random() * APPEARANCE_COLORS.length)
  ];
};

export const APPEARANCE_ICONS = [
  'fa::address-card',
  'fa::pen',
  'fa::question',
  'fa::save',
];
