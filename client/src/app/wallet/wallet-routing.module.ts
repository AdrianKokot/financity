import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WalletShellComponent } from './feature/wallet-shell/wallet-shell.component';
import { WalletsShellComponent } from './feature/wallets-shell/wallets-shell.component';
import { WalletTransactionsComponent } from './feature/wallet-transactions/wallet-transactions.component';
import { WalletSettingsComponent } from './feature/wallet-settings/wallet-settings.component';
import { WalletLabelsComponent } from './feature/wallet-labels/wallet-labels.component';
import { WalletCategoriesComponent } from './feature/wallet-categories/wallet-categories.component';
import { WalletShareManagementComponent } from './feature/wallet-share-management/wallet-share-management.component';
import { WalletRecipientsComponent } from './feature/wallet-recipients/wallet-recipients.component';
import { WalletStatsComponent } from './feature/wallet-stats/wallet-stats.component';

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
            title: 'Financity - Wallet transactions',
          },
          {
            path: 'labels',
            component: WalletLabelsComponent,
            title: 'Financity - Wallet labels',
          },
          {
            path: 'categories',
            component: WalletCategoriesComponent,
            title: 'Financity - Wallet categories',
          },
          {
            path: 'transaction-parties',
            component: WalletRecipientsComponent,
            title: 'Financity - Wallet transaction parties',
          },
          {
            path: 'settings',
            component: WalletSettingsComponent,
            title: 'Financity - Wallet settings',
          },
          {
            path: 'access-management',
            component: WalletShareManagementComponent,
            title: 'Financity - Wallet share management',
          },
          {
            path: 'expense-overview',
            component: WalletStatsComponent,
            title: 'Financity - Wallet expense overview',
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
