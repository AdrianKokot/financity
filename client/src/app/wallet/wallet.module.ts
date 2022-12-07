import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WalletRoutingModule } from './wallet-routing.module';
import { WalletShellComponent } from './feature/wallet-shell/wallet-shell.component';
import {
  TuiButtonModule,
  TuiLoaderModule,
  TuiScrollbarModule,
  TuiSvgModule,
} from '@taiga-ui/core';
import { TuiMoneyModule } from '@taiga-ui/addon-commerce';
import { TuiTableModule } from '@taiga-ui/addon-table';
import { TuiBadgeModule, TuiIslandModule } from '@taiga-ui/kit';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { TuiForModule, TuiLetModule } from '@taiga-ui/cdk';
import { WalletsShellComponent } from './feature/wallets-shell/wallets-shell.component';
import { WalletListItemComponent } from './ui/wallet-list-item/wallet-list-item.component';

@NgModule({
  declarations: [
    WalletShellComponent,
    WalletsShellComponent,
    WalletListItemComponent,
  ],
  imports: [
    CommonModule,
    TuiForModule,
    WalletRoutingModule,
    TuiLoaderModule,
    TuiMoneyModule,
    TuiTableModule,
    TuiBadgeModule,
    ScrollingModule,
    TuiLetModule,
    TuiScrollbarModule,
    TuiIslandModule,
    TuiButtonModule,
    TuiSvgModule,
  ],
})
export class WalletModule {}
