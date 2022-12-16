import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WalletRoutingModule } from './wallet-routing.module';
import { WalletShellComponent } from './feature/wallet-shell/wallet-shell.component';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiErrorModule,
  TuiHostedDropdownModule,
  TuiLabelModule,
  TuiLinkModule,
  TuiLoaderModule,
  TuiScrollbarModule,
  TuiSvgModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import {
  TuiCurrencyPipeModule,
  TuiMoneyModule,
} from '@taiga-ui/addon-commerce';
import { TuiTableModule } from '@taiga-ui/addon-table';
import {
  TuiActionModule,
  TuiBadgeModule,
  TuiComboBoxModule,
  TuiDataListWrapperModule,
  TuiFieldErrorPipeModule,
  TuiInputDateRangeModule,
  TuiInputModule,
  TuiInputNumberModule,
  TuiIslandModule,
  TuiMarkerIconModule,
  TuiMultiSelectModule,
  TuiSelectModule,
  TuiTabsModule,
  TuiTagModule,
} from '@taiga-ui/kit';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { TuiForModule, TuiLetModule } from '@taiga-ui/cdk';
import { WalletsShellComponent } from './feature/wallets-shell/wallets-shell.component';
import { WalletListItemComponent } from './ui/wallet-list-item/wallet-list-item.component';
import { WalletSettingsComponent } from './feature/wallet-settings/wallet-settings.component';
import { WalletTransactionsComponent } from './feature/wallet-transactions/wallet-transactions.component';
import { WalletLabelsComponent } from './feature/wallet-labels/wallet-labels.component';
import { WalletCategoriesComponent } from './feature/wallet-categories/wallet-categories.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CategoryModule } from '../category/category.module';
import { RequireConfirmationDirective } from '@shared/utils/directives/require-confirmation.directive';
import { WalletShareManagementComponent } from './feature/wallet-share-management/wallet-share-management.component';
import { ShareWalletComponent } from './ui/share-wallet/share-wallet.component';
import { LabelModule } from '../label/label.module';
import { DropdownSelectComponent } from '@shared/ui/tui/dropdown-select/dropdown-select.component';
import { TransactionModule } from '../transaction/transaction.module';

@NgModule({
  declarations: [
    WalletShellComponent,
    WalletsShellComponent,
    WalletListItemComponent,
    WalletSettingsComponent,
    WalletTransactionsComponent,
    WalletLabelsComponent,
    WalletCategoriesComponent,
    WalletShareManagementComponent,
    ShareWalletComponent,
  ],
  imports: [
    CommonModule,
    TuiForModule,
    WalletRoutingModule,
    TuiLoaderModule,
    TuiMoneyModule,
    TuiTableModule,
    TuiBadgeModule,
    ScrollingModule,
    TuiLetModule,
    TuiScrollbarModule,
    TuiIslandModule,
    TuiButtonModule,
    TuiSvgModule,
    TuiTabsModule,
    ReactiveFormsModule,
    TuiFieldErrorPipeModule,
    TuiInputNumberModule,
    TuiErrorModule,
    TuiComboBoxModule,
    TuiDataListWrapperModule,
    TuiInputModule,
    TuiCurrencyPipeModule,
    TuiTextfieldControllerModule,
    TuiLabelModule,
    TuiLinkModule,
    TuiTagModule,
    FormsModule,
    CategoryModule,
    RequireConfirmationDirective,
    TuiMarkerIconModule,
    LabelModule,
    TuiHostedDropdownModule,
    TuiDataListModule,
    TuiMultiSelectModule,
    TuiSelectModule,
    DropdownSelectComponent,
    TuiActionModule,
    TransactionModule,
    TuiInputDateRangeModule,
  ],
})
export class WalletModule {}
