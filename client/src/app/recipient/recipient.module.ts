import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateRecipientComponent } from './feature/create-recipient/create-recipient.component';
import { UpdateRecipientComponent } from './feature/update-recipient/update-recipient.component';
import {
  TuiButtonModule,
  TuiErrorModule,
  TuiLabelModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TuiFieldErrorPipeModule, TuiInputModule } from '@taiga-ui/kit';

@NgModule({
  declarations: [CreateRecipientComponent, UpdateRecipientComponent],
  imports: [
    CommonModule,
    TuiLabelModule,
    ReactiveFormsModule,
    TuiTextfieldControllerModule,
    TuiInputModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiButtonModule,
  ],
  exports: [CreateRecipientComponent, UpdateRecipientComponent],
})
export class RecipientModule {}
