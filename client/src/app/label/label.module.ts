import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateLabelComponent } from './feature/create-label/create-label.component';
import { UpdateLabelComponent } from './feature/update-label/update-label.component';
import { AppearanceColorSelectorComponent } from '@shared/ui/tui/appearance-color-selector/appearance-color-selector.component';
import { ReactiveFormsModule } from '@angular/forms';
import {
  TuiButtonModule,
  TuiErrorModule,
  TuiGroupModule,
  TuiLabelModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { TuiFieldErrorPipeModule, TuiInputModule } from '@taiga-ui/kit';
import { TuiAutoFocusModule, TuiLetModule } from '@taiga-ui/cdk';

const exportedComponents = [CreateLabelComponent, UpdateLabelComponent];

@NgModule({
  declarations: [...exportedComponents],
  exports: [...exportedComponents],
  imports: [
    CommonModule,
    AppearanceColorSelectorComponent,
    ReactiveFormsModule,
    TuiGroupModule,
    TuiLabelModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiInputModule,
    TuiTextfieldControllerModule,
    TuiButtonModule,
    TuiLetModule,
    TuiAutoFocusModule,
  ],
})
export class LabelModule {}
