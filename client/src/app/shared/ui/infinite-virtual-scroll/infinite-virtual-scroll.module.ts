import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfiniteVirtualScrollDirective } from './infinite-virtual-scroll.directive';

@NgModule({
  declarations: [InfiniteVirtualScrollDirective],
  imports: [CommonModule],
  exports: [InfiniteVirtualScrollDirective],
})
export class InfiniteVirtualScrollModule {}
