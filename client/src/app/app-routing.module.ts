import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from '@layout/feature/not-found/not-found.component';
import { RouteData } from '@shared/utils/toggles/route-data';
import { AuthGuard } from './auth/data-access/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard',
  },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule),
    data: {
      [RouteData.NAVBAR_VISIBLE]: false,
    },
  },
  {
    path: 'dashboard',
    loadChildren: () =>
      import('./dashboard/dashboard.module').then(m => m.DashboardModule),
    canLoad: [AuthGuard],
    title: 'Financity - Dashboard',
  },
  {
    path: 'budgets',
    loadChildren: () =>
      import('./budget/budget.module').then(m => m.BudgetModule),
    canLoad: [AuthGuard],
    title: 'Financity - Budgets',
  },
  {
    path: 'settings',
    loadChildren: () =>
      import('./user-settings/user-settings.module').then(
        m => m.UserSettingsModule
      ),
    canLoad: [AuthGuard],
    title: 'Financity - Settings',
  },
  {
    path: 'wallets',
    loadChildren: () =>
      import('./wallet/wallet.module').then(m => m.WalletModule),
    canLoad: [AuthGuard],
    title: 'Financity - Wallets',
  },
  {
    path: 'search',
    loadChildren: () =>
      import('./global-search/global-search.module').then(
        m => m.GlobalSearchModule
      ),
    canLoad: [AuthGuard],
    title: 'Financity - Search',
  },
  {
    path: '**',
    component: NotFoundComponent,
    data: {
      [RouteData.NAVBAR_VISIBLE]: false,
    },
    title: 'Financity - Not Found',
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      paramsInheritanceStrategy: 'always',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
