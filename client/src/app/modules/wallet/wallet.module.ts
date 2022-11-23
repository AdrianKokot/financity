import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WalletRoutingModule } from './wallet-routing.module';
import { WalletDashboardComponent } from './components/wallet-dashboard/wallet-dashboard.component';

@NgModule({
  declarations: [WalletDashboardComponent],
  imports: [CommonModule, WalletRoutingModule],
})
export class WalletModule {}
