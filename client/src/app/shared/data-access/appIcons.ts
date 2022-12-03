export const appIcons: Record<string, string> = {
  appIconHome: 'home-24px',
};

export function iconsPath(name: string): string {
  return `assets/icons/${appIcons[name]}.svg`;
}
