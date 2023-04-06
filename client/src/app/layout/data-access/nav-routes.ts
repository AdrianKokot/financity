export interface NavRoute {
  label: string;
  iconName: string;
  route: string;
}

export const EMPTY_NAV_ROUTE: NavRoute = {
  label: 'Not implemented',
  iconName: '',
  route: '',
};

export const AppNavRoutes: NavRoute[] = [
  {
    label: 'Dashboard',
    iconName: 'tuiIconBookmark',
    route: '/dashboard',
  },
  {
    label: 'Wallets',
    iconName: 'tuiIconFolder',
    route: '/wallets',
  },
  {
    label: 'Budgets',
    iconName: 'tuiIconBarChart',
    route: '/budgets',
  },
  {
    label: 'Search',
    iconName: 'tuiIconSearch',
    route: '/search',
  },
];

export const UserRelatedRoutes: NavRoute[] = [
  { label: 'Settings', iconName: 'tuiIconSettings', route: '/settings' },
  { label: 'Logout', iconName: 'tuiIconToggleOff', route: '/auth/logout' },
];
