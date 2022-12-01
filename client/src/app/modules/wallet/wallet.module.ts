import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WalletRoutingModule } from './wallet-routing.module';
import { WalletDashboardComponent } from './components/wallet-dashboard/wallet-dashboard.component';
import {
  TuiFormatDatePipeModule,
  TuiLoaderModule,
  TuiScrollbarModule,
} from '@taiga-ui/core';
import { TuiTableModule } from '@taiga-ui/addon-table';
import { TuiLetModule } from '@taiga-ui/cdk';
import { TuiMoneyModule } from '@taiga-ui/addon-commerce';
import { TuiBadgeModule, TuiTagModule } from '@taiga-ui/kit';
import { ScrollingModule } from '@angular/cdk/scrolling';

@NgModule({
  declarations: [WalletDashboardComponent],
  imports: [
    CommonModule,
    WalletRoutingModule,
    TuiLoaderModule,
    TuiTableModule,
    TuiLetModule,
    TuiFormatDatePipeModule,
    TuiMoneyModule,
    TuiTagModule,
    TuiBadgeModule,
    TuiScrollbarModule,
    ScrollingModule,
  ],
})
export class WalletModule {}
