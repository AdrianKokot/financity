import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TuiMoneyModule } from '@taiga-ui/addon-commerce';
import {
  TuiButtonModule,
  TuiLinkModule,
  TuiScrollbarModule,
} from '@taiga-ui/core';
import { TuiIslandModule } from '@taiga-ui/kit';

const tuiModules = [
  TuiButtonModule,
  TuiLinkModule,
  TuiIslandModule,
  TuiMoneyModule,
  TuiScrollbarModule,
];

@NgModule({
  declarations: [],
  imports: [CommonModule, ...tuiModules],
  exports: [...tuiModules],
})
export class SharedModule {}
