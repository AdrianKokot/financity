import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  TuiBadgeModule,
  TuiInputModule,
  TuiItemsWithMoreModule,
  TuiMarkerIconModule,
} from '@taiga-ui/kit';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  TuiButtonModule,
  TuiGroupModule,
  TuiHintModule,
  TuiLinkModule,
  TuiLoaderModule,
  TuiPrimitiveTextfieldModule,
  TuiScrollbarModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { GlobalSearchRoutingModule } from './global-search-routing.module';
import { SearchShellComponent } from './feature/search-shell/search-shell.component';
import { TuiLetModule } from '@taiga-ui/cdk';
import { InfiniteVirtualScrollModule } from '@shared/ui/infinite-virtual-scroll/infinite-virtual-scroll.module';
import { TuiTableModule } from '@taiga-ui/addon-table';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { TuiMoneyModule } from '@taiga-ui/addon-commerce';

@NgModule({
  declarations: [SearchShellComponent],
  imports: [
    CommonModule,
    GlobalSearchRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    TuiInputModule,
    TuiPrimitiveTextfieldModule,
    TuiTextfieldControllerModule,
    TuiGroupModule,
    TuiButtonModule,
    TuiLetModule,
    ScrollingModule,
    TuiScrollbarModule,
    InfiniteVirtualScrollModule,
    TuiTableModule,
    TuiMarkerIconModule,
    TuiItemsWithMoreModule,
    TuiBadgeModule,
    TuiLinkModule,
    TuiMoneyModule,
    TuiLoaderModule,
    TuiHintModule,
  ],
})
export class GlobalSearchModule {}
