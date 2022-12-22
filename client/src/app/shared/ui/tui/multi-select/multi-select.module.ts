import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MultiSelectComponent } from './multi-select.component';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiHostedDropdownModule,
  TuiLoaderModule,
  TuiScrollbarModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import {
  TuiDataListWrapperModule,
  TuiInputTagModule,
  TuiMultiSelectModule,
} from '@taiga-ui/kit';
import {
  TuiActiveZoneModule,
  TuiLetModule,
  TuiMapperPipeModule,
} from '@taiga-ui/cdk';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PolymorpheusModule } from '@tinkoff/ng-polymorpheus';
import { EventPluginsModule } from '@tinkoff/ng-event-plugins';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { InfiniteVirtualScrollModule } from '@shared/ui/infinite-virtual-scroll/infinite-virtual-scroll.module';

@NgModule({
  declarations: [MultiSelectComponent],
  imports: [
    CommonModule,
    EventPluginsModule,
    TuiActiveZoneModule,
    TuiHostedDropdownModule,
    TuiInputTagModule,
    TuiTextfieldControllerModule,
    TuiMapperPipeModule,
    FormsModule,
    PolymorpheusModule,
    TuiMultiSelectModule,
    TuiDataListWrapperModule,
    ReactiveFormsModule,
    TuiLetModule,
    ScrollingModule,
    TuiScrollbarModule,
    TuiDataListModule,
    InfiniteVirtualScrollModule,
    TuiLoaderModule,
    TuiButtonModule,
  ],
  exports: [MultiSelectComponent],
})
export class MultiSelectModule {}
