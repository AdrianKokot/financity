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
import { TuiAutoFocusModule, TuiLetModule } from '@taiga-ui/cdk';

@NgModule({
  declarations: [CreateRecipientComponent, UpdateRecipientComponent],
  imports: [
    CommonModule,
    TuiLabelModule,
    ReactiveFormsModule,
    TuiTextfieldControllerModule,
    TuiInputModule,
    TuiLetModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiButtonModule,
    TuiAutoFocusModule,
  ],
  exports: [CreateRecipientComponent, UpdateRecipientComponent],
})
export class RecipientModule {}
