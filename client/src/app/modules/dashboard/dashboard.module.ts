import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { CreateWalletDialogComponent } from './components/wallet/create-wallet-dialog/create-wallet-dialog.component';
import { WalletListComponent } from './components/wallet/wallet-list/wallet-list.component';
import {
  TuiComboBoxModule,
  TuiDataListWrapperModule,
  TuiFieldErrorPipeModule,
  TuiFilterByInputPipeModule,
  TuiInputModule,
  TuiInputNumberModule,
  TuiIslandModule,
  TuiStringifyContentPipeModule,
} from '@taiga-ui/kit';
import { TuiButtonModule, TuiErrorModule } from '@taiga-ui/core';
import {
  TuiCurrencyPipeModule,
  TuiMoneyModule,
} from '@taiga-ui/addon-commerce';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    DashboardComponent,
    CreateWalletDialogComponent,
    WalletListComponent,
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    TuiInputModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiComboBoxModule,
    TuiDataListWrapperModule,
    TuiFilterByInputPipeModule,
    TuiStringifyContentPipeModule,
    TuiInputNumberModule,
    TuiCurrencyPipeModule,
    TuiIslandModule,
    TuiMoneyModule,
    TuiButtonModule,
    ReactiveFormsModule,
  ],
  exports: [WalletListComponent],
})
export class DashboardModule {}
