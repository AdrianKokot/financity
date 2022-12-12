import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateLabelComponent } from './feature/create-label/create-label.component';
import { UpdateLabelComponent } from './feature/update-label/update-label.component';
import { TuiFieldErrorPipeModule, TuiInputModule } from '@taiga-ui/kit';
import {
  TuiButtonModule,
  TuiErrorModule,
  TuiLabelModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { ReactiveFormsModule } from '@angular/forms';

const exportedComponents = [CreateLabelComponent, UpdateLabelComponent];

@NgModule({
  declarations: [...exportedComponents],
  exports: [...exportedComponents],
  imports: [
    CommonModule,
    TuiFieldErrorPipeModule,
    TuiErrorModule,
    TuiTextfieldControllerModule,
    TuiInputModule,
    ReactiveFormsModule,
    TuiLabelModule,
    TuiButtonModule,
  ],
})
export class LabelModule {}
