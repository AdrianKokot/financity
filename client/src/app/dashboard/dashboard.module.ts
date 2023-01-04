import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardShellComponent } from './feature/dashboard-shell/dashboard-shell.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { WalletModule } from '../wallet/wallet.module';
import { TuiLetModule } from '@taiga-ui/cdk';
import {
  TuiButtonModule,
  TuiHintModule,
  TuiLinkModule,
  TuiLoaderModule,
  TuiScrollbarModule,
} from '@taiga-ui/core';
import {
  TuiAxesModule,
  TuiBarChartModule,
  TuiLineChartModule,
  TuiLineDaysChartModule,
} from '@taiga-ui/addon-charts';
import { CdkVirtualForOf, ScrollingModule } from '@angular/cdk/scrolling';
import { TuiTableModule } from '@taiga-ui/addon-table';
import { InfiniteVirtualScrollModule } from '@shared/ui/infinite-virtual-scroll/infinite-virtual-scroll.module';
import {
  TuiBadgeModule,
  TuiItemsWithMoreModule,
  TuiMarkerIconModule,
} from '@taiga-ui/kit';
import { TuiMoneyModule } from '@taiga-ui/addon-commerce';

@NgModule({
  declarations: [DashboardShellComponent],
  imports: [
    DashboardRoutingModule,
    CommonModule,
    ScrollingModule,
    WalletModule,
    TuiLetModule,
    TuiLoaderModule,
    TuiAxesModule,
    TuiLineDaysChartModule,
    TuiLineChartModule,
    TuiBarChartModule,
    TuiHintModule,
    CdkVirtualForOf,
    TuiTableModule,
    TuiScrollbarModule,
    InfiniteVirtualScrollModule,
    TuiBadgeModule,
    TuiButtonModule,
    TuiMarkerIconModule,
    TuiItemsWithMoreModule,
    TuiLinkModule,
    TuiMoneyModule,
  ],
})
export class DashboardModule {}
