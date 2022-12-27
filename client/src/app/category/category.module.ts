import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateCategoryComponent } from './feature/create-category/create-category.component';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiErrorModule,
  TuiGroupModule,
  TuiHostedDropdownModule,
  TuiLabelModule,
  TuiLoaderModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import {
  TuiBadgedContentModule,
  TuiComboBoxModule,
  TuiDataListWrapperModule,
  TuiFieldErrorPipeModule,
  TuiFilterByInputPipeModule,
  TuiInputModule,
  TuiSelectModule,
  TuiSelectOptionModule,
  TuiStringifyContentPipeModule,
} from '@taiga-ui/kit';
import { ReactiveFormsModule } from '@angular/forms';
import { DialogService } from '@shared/utils/services/dialog.service';
import { UpdateCategoryComponent } from './feature/update-category/update-category.component';
import { TuiAutoFocusModule, TuiLetModule } from '@taiga-ui/cdk';
import { PaletteItemDirective } from '@shared/ui/tui/palette-item.directive';
import { AppearanceColorSelectorComponent } from '@shared/ui/tui/appearance-color-selector/appearance-color-selector.component';
import { AppearanceIconSelectorComponent } from '@shared/ui/tui/appearance-icon-selector/appearance-icon-selector.component';

@NgModule({
  declarations: [CreateCategoryComponent, UpdateCategoryComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TuiInputModule,
    TuiButtonModule,
    TuiComboBoxModule,
    TuiDataListModule,
    TuiDataListWrapperModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiTextfieldControllerModule,
    TuiLabelModule,
    TuiStringifyContentPipeModule,
    TuiFilterByInputPipeModule,
    TuiSelectModule,
    TuiSelectOptionModule,
    TuiAutoFocusModule,
    TuiHostedDropdownModule,
    TuiGroupModule,
    PaletteItemDirective,
    TuiBadgedContentModule,
    AppearanceColorSelectorComponent,
    AppearanceIconSelectorComponent,
    TuiLoaderModule,
    TuiLetModule,
  ],
  exports: [CreateCategoryComponent, UpdateCategoryComponent],
  providers: [DialogService],
})
export class CategoryModule {}
