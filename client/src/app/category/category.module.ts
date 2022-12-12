import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateCategoryComponent } from './feature/create-category/create-category.component';
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
  TuiInputModule,
  TuiSelectModule,
  TuiSelectOptionModule,
  TuiStringifyContentPipeModule,
} from '@taiga-ui/kit';
import { ReactiveFormsModule } from '@angular/forms';
import { DialogService } from '@shared/utils/services/dialog.service';
import { UpdateCategoryComponent } from './feature/update-category/update-category.component';

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
  ],
  exports: [CreateCategoryComponent, UpdateCategoryComponent],
  providers: [DialogService],
})
export class CategoryModule {}
