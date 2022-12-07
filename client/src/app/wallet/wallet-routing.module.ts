import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WalletShellComponent } from './feature/wallet-shell/wallet-shell.component';
import { WalletsShellComponent } from './feature/wallets-shell/wallets-shell.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', component: WalletsShellComponent },
      { path: ':id', component: WalletShellComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WalletRoutingModule {}
