import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TuiButtonModule, TuiLinkModule } from '@taiga-ui/core';
import { TuiIslandModule } from '@taiga-ui/kit';

const tuiModules = [TuiButtonModule, TuiLinkModule, TuiIslandModule];

@NgModule({
  declarations: [],
  imports: [CommonModule, ...tuiModules],
  exports: [...tuiModules],
})
export class SharedModule {}
