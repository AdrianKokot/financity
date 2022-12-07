import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SharedModule } from '@shared/shared.module';
import { CreateWalletDialogComponent } from './components/wallet/create-wallet-dialog/create-wallet-dialog.component';
import { WalletListComponent } from './components/wallet/wallet-list/wallet-list.component';
import {
  TuiComboBoxModule,
  TuiDataListWrapperModule,
  TuiFieldErrorPipeModule,
  TuiFilterByInputPipeModule,
  TuiInputModule,
  TuiInputNumberModule,
  TuiStringifyContentPipeModule,
} from '@taiga-ui/kit';
import { TuiErrorModule } from '@taiga-ui/core';
import { TuiCurrencyPipeModule } from '@taiga-ui/addon-commerce';

@NgModule({
  declarations: [
    DashboardComponent,
    CreateWalletDialogComponent,
    WalletListComponent,
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    SharedModule,
    TuiInputModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiComboBoxModule,
    TuiDataListWrapperModule,
    TuiFilterByInputPipeModule,
    TuiStringifyContentPipeModule,
    TuiInputNumberModule,
    TuiCurrencyPipeModule,
  ],
  exports: [WalletListComponent],
})
export class DashboardModule {}
