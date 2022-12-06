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
      import('./modules/dashboard/dashboard.module').then(
        m => m.DashboardModule
      ),
    canLoad: [AuthGuard],
  },
  {
    path: 'settings',
    loadChildren: () =>
      import('./user-settings/user-settings.module').then(
        m => m.UserSettingsModule
      ),
    canLoad: [AuthGuard],
  },
  {
    path: 'wallet',
    loadChildren: () =>
      import('./modules/wallet/wallet.module').then(m => m.WalletModule),
    canLoad: [AuthGuard],
  },
  {
    path: '**',
    component: NotFoundComponent,
    data: {
      [RouteData.NAVBAR_VISIBLE]: false,
    },
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
