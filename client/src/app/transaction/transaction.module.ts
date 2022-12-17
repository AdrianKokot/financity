import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateTransactionComponent } from './feature/create-transaction/create-transaction.component';
import { UpdateTransactionComponent } from './feature/update-transaction/update-transaction.component';
import { ReactiveFormsModule } from '@angular/forms';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiErrorModule,
  TuiLabelModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import {
  TuiComboBoxModule,
  TuiDataListWrapperModule,
  TuiFieldErrorPipeModule,
  TuiFilterByInputPipeModule,
  TuiInputDateModule,
  TuiInputNumberModule,
  TuiInputTagModule,
  TuiMultiSelectModule,
  TuiSelectModule,
  TuiStringifyContentPipeModule,
  TuiTextAreaModule,
} from '@taiga-ui/kit';
import { TuiAutoFocusModule, TuiLetModule } from '@taiga-ui/cdk';
import { TuiCurrencyPipeModule } from '@taiga-ui/addon-commerce';

@NgModule({
  declarations: [CreateTransactionComponent, UpdateTransactionComponent],
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
  ],
  exports: [CreateTransactionComponent, UpdateTransactionComponent],
})
export class TransactionModule {}
