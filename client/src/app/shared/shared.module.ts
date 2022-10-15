import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TuiButtonModule, TuiLinkModule } from '@taiga-ui/core';

const sharedModules = [TuiButtonModule, TuiLinkModule];

@NgModule({
  declarations: [],
  imports: [CommonModule, ...sharedModules],
  exports: [...sharedModules],
})
export class SharedModule {}
