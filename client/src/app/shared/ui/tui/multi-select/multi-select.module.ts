import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MultiSelectComponent } from './multi-select.component';
import {
  TuiDataListModule,
  TuiHostedDropdownModule,
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
  ],
  exports: [MultiSelectComponent],
})
export class MultiSelectModule {}
