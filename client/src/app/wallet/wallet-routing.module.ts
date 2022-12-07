import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WalletShellComponent } from './feature/wallet-shell/wallet-shell.component';
import { WalletsShellComponent } from './feature/wallets-shell/wallets-shell.component';
import { WalletTransactionsComponent } from './feature/wallet-transactions/wallet-transactions.component';
import { WalletSettingsComponent } from './feature/wallet-settings/wallet-settings.component';
import { WalletLabelsComponent } from './feature/wallet-labels/wallet-labels.component';
import { WalletCategoriesComponent } from './feature/wallet-categories/wallet-categories.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', component: WalletsShellComponent },
      {
        path: ':id',
        component: WalletShellComponent,

        children: [
          {
            path: '',
            redirectTo: 'transactions',
            pathMatch: 'full',
          },
          {
            path: 'transactions',
            component: WalletTransactionsComponent,
          },
          {
            path: 'labels',
            component: WalletLabelsComponent,
          },
          {
            path: 'categories',
            component: WalletCategoriesComponent,
          },
          {
            path: 'settings',
            component: WalletSettingsComponent,
          },
        ],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WalletRoutingModule {}
