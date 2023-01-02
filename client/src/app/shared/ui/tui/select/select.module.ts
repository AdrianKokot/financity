import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SelectComponent } from './select.component';
import {
  TuiComboBoxModule,
  TuiDataListWrapperModule,
  TuiMultiSelectModule,
  TuiUnfinishedValidatorModule,
} from '@taiga-ui/kit';
import {
  TuiDataListModule,
  TuiLoaderModule,
  TuiScrollbarModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { TuiLetModule } from '@taiga-ui/cdk';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  CdkFixedSizeVirtualScroll,
  CdkVirtualForOf,
  CdkVirtualScrollViewport,
} from '@angular/cdk/scrolling';
import { InfiniteVirtualScrollModule } from '@shared/ui/infinite-virtual-scroll/infinite-virtual-scroll.module';

@NgModule({
  declarations: [SelectComponent],
  imports: [
    CommonModule,
    TuiComboBoxModule,
    TuiTextfieldControllerModule,
    TuiMultiSelectModule,
    TuiLetModule,
    FormsModule,
    CdkVirtualScrollViewport,
    CdkFixedSizeVirtualScroll,
    InfiniteVirtualScrollModule,
    TuiScrollbarModule,
    TuiDataListModule,
    CdkVirtualForOf,
    TuiLoaderModule,
    TuiDataListWrapperModule,
    ReactiveFormsModule,
    TuiUnfinishedValidatorModule,
  ],
  exports: [SelectComponent],
})
export class SelectModule {}
