import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardShellComponent } from './feature/dashboard-shell/dashboard-shell.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { WalletModule } from '../wallet/wallet.module';
import { TuiForModule, TuiLetModule } from '@taiga-ui/cdk';
import {
  TuiButtonModule,
  TuiHintModule,
  TuiLinkModule,
  TuiLoaderModule,
  TuiScrollbarModule,
  TuiSvgModule,
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
import { BudgetModule } from '../budget/budget.module';
import { SelectModule } from '@shared/ui/tui/select/select.module';
import { MultiSelectModule } from '@shared/ui/tui/multi-select/multi-select.module';
import { DashboardBarChartComponent } from './feature/dashboard-bar-chart/dashboard-bar-chart.component';
import { DashboardTransactionsComponent } from './feature/dashboard-transactions/dashboard-transactions.component';

@NgModule({
  declarations: [
    DashboardShellComponent,
    DashboardBarChartComponent,
    DashboardTransactionsComponent,
  ],
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
    TuiForModule,
    BudgetModule,
    SelectModule,
    MultiSelectModule,
    TuiSvgModule,
  ],
})
export class DashboardModule {}
