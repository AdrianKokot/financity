import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateLabelComponent } from './feature/create-label/create-label.component';
import { UpdateLabelComponent } from './feature/update-label/update-label.component';
import {
  TuiBadgedContentModule,
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiMultiSelectModule,
  TuiSelectModule,
} from '@taiga-ui/kit';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiDropdownModule,
  TuiErrorModule,
  TuiGroupModule,
  TuiHostedDropdownModule,
  TuiLabelModule,
  TuiPrimitiveTextfieldModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiFocusableModule } from '@taiga-ui/cdk';
import { PaletteItemDirective } from '@shared/ui/tui/palette-item.directive';

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
    TuiSelectModule,
    TuiDataListModule,
    TuiGroupModule,
    TuiDropdownModule,
    TuiHostedDropdownModule,
    FormsModule,
    TuiMultiSelectModule,
    TuiPrimitiveTextfieldModule,
    TuiFocusableModule,
    TuiBadgedContentModule,
    PaletteItemDirective,
  ],
})
export class LabelModule {}
