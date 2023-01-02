// const appearanceColors = [
//   'slate',
//   'neutral',
//   'yellow',
//   'orange',
//   'rose',
//   'purple',
//   'violet',
//   'indigo',
//   'blue',
//   'cyan',
//   'emerald',
//   'lime',
// ].map(x =>
//   new Array(4)
//     .fill(0)
//     .map((_, i) => (i + 1) * 2 * 100)
//     .map(i => `${x}-${i}`)
// );

const appearanceColors = new Array(5)
  .fill(0)
  .map((_, i) => i)
  .map(i =>
    new Array(4)
      .fill(0)
      .map((_, x) => x + 1)
      .map(x => x + i * 4)
      .map(x => `support-${x < 10 ? '0' + x : x}`)
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
  'fa::solid::bicycle',
  'fa::solid::book',
  'fa::solid::building',
  'fa::solid::car',
  'fa::solid::cart-shopping',
  'fa::solid::certificate',
  'fa::solid::compass',
  'fa::solid::computer',
  'fa::solid::dog',
  'fa::solid::film',
  'fa::solid::futbol',
  'fa::solid::gamepad',
  'fa::solid::gift',
  'fa::solid::hammer',
  'fa::solid::hippo',
  'fa::solid::lemon',
  'fa::solid::location-dot',
  'fa::solid::money-bill',
  'fa::solid::mountain-sun',
  'fa::solid::mug-hot',
  'fa::solid::music',
  'fa::solid::newspaper',
  'fa::solid::pizza-slice',
  'fa::solid::plane',
  'fa::solid::puzzle-piece',
  'fa::solid::receipt',
  'fa::solid::sack',
  'fa::solid::seedling',
  'fa::solid::shirt',
  'fa::solid::soap',
  'fa::solid::tag',
  'fa::solid::tooth',
  'fa::solid::train',
  'fa::solid::tree',
  'fa::solid::utensils',
  'fa::solid::wallet',
];

export const getRandomAppearanceIcon = () => {
  return APPEARANCE_ICONS[Math.floor(Math.random() * APPEARANCE_ICONS.length)];
};
