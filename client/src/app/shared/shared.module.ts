import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TuiMoneyModule } from '@taiga-ui/addon-commerce';
import {
  TuiButtonModule,
  TuiLinkModule,
  TuiScrollbarModule,
} from '@taiga-ui/core';
import { TuiIslandModule } from '@taiga-ui/kit';
import { ReactiveFormsModule } from '@angular/forms';

const tuiModules = [
  TuiButtonModule,
  TuiLinkModule,
  TuiIslandModule,
  TuiMoneyModule,
  TuiScrollbarModule,
];

const otherModules = [ReactiveFormsModule];

const modules = [...tuiModules, ...otherModules];

@NgModule({
  declarations: [],
  imports: [CommonModule, ...modules],
  exports: [...modules],
})
export class SharedModule {}
