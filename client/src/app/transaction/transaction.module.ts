import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateTransactionComponent } from './feature/create-transaction/create-transaction.component';
import { UpdateTransactionComponent } from './feature/update-transaction/update-transaction.component';
import { ReactiveFormsModule } from '@angular/forms';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiErrorModule,
  TuiGroupModule,
  TuiHintModule,
  TuiLabelModule,
  TuiLinkModule,
  TuiLoaderModule,
  TuiScrollbarModule,
  TuiSvgModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import {
  TuiBadgeModule,
  TuiComboBoxModule,
  TuiDataListWrapperModule,
  TuiFieldErrorPipeModule,
  TuiFilterByInputPipeModule,
  TuiInputDateModule,
  TuiInputNumberModule,
  TuiInputTagModule,
  TuiMarkerIconModule,
  TuiMultiSelectModule,
  TuiSelectModule,
  TuiStringifyContentPipeModule,
  TuiTagModule,
  TuiTextAreaModule,
} from '@taiga-ui/kit';
import { TuiAutoFocusModule, TuiLetModule } from '@taiga-ui/cdk';
import {
  TuiCurrencyPipeModule,
  TuiMoneyModule,
} from '@taiga-ui/addon-commerce';
import { MultiSelectModule } from '@shared/ui/tui/multi-select/multi-select.module';
import { SelectModule } from '@shared/ui/tui/select/select.module';
import { TransactionDetailsComponent } from './feature/transaction-details/transaction-details.component';
import { PolymorpheusModule } from '@tinkoff/ng-polymorpheus';
import { RouterLink } from '@angular/router';

@NgModule({
  declarations: [
    CreateTransactionComponent,
    UpdateTransactionComponent,
    TransactionDetailsComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TuiLabelModule,
    TuiSelectModule,
    TuiTextfieldControllerModule,
    TuiDataListModule,
    TuiFieldErrorPipeModule,
    TuiButtonModule,
    TuiErrorModule,
    TuiInputNumberModule,
    TuiAutoFocusModule,
    TuiCurrencyPipeModule,
    TuiInputDateModule,
    TuiComboBoxModule,
    TuiFilterByInputPipeModule,
    TuiDataListWrapperModule,
    TuiStringifyContentPipeModule,
    TuiLetModule,
    TuiTextAreaModule,
    TuiInputTagModule,
    TuiMultiSelectModule,
    MultiSelectModule,
    SelectModule,
    TuiGroupModule,
    TuiLoaderModule,
    TuiMoneyModule,
    TuiBadgeModule,
    TuiMarkerIconModule,
    PolymorpheusModule,
    TuiScrollbarModule,
    TuiSvgModule,
    TuiHintModule,
    TuiLinkModule,
    RouterLink,
    TuiTagModule,
  ],
  exports: [
    CreateTransactionComponent,
    UpdateTransactionComponent,
    TransactionDetailsComponent,
  ],
})
export class TransactionModule {}
