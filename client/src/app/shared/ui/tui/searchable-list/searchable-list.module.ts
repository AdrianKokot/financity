import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchableListComponent } from '@shared/ui/tui/searchable-list/searchable-list.component';
import {
  CdkFixedSizeVirtualScroll,
  CdkVirtualForOf,
  CdkVirtualScrollViewport,
} from '@angular/cdk/scrolling';
import { InfiniteVirtualScrollModule } from '@shared/ui/infinite-virtual-scroll/infinite-virtual-scroll.module';
import {
  TuiDataListModule,
  TuiLoaderModule,
  TuiPrimitiveTextfieldModule,
  TuiScrollbarModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { TuiMultiSelectModule } from '@taiga-ui/kit';
import {
  TuiActiveZoneModule,
  TuiAutoFocusModule,
  TuiFocusableModule,
  TuiLetModule,
} from '@taiga-ui/cdk';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { A11yModule } from '@angular/cdk/a11y';

@NgModule({
  declarations: [SearchableListComponent],
  imports: [
    CommonModule,
    CdkVirtualScrollViewport,
    InfiniteVirtualScrollModule,
    CdkFixedSizeVirtualScroll,
    TuiDataListModule,
    TuiMultiSelectModule,
    TuiPrimitiveTextfieldModule,
    TuiTextfieldControllerModule,
    TuiScrollbarModule,
    TuiLoaderModule,
    TuiLetModule,
    CdkVirtualForOf,
    FormsModule,
    ReactiveFormsModule,
    TuiAutoFocusModule,
    TuiActiveZoneModule,
    A11yModule,
    TuiFocusableModule,
  ],
  exports: [SearchableListComponent],
})
export class SearchableListModule {}
