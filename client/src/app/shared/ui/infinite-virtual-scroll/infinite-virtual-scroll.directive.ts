import { Directive, Input, Output, Self } from '@angular/core';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { distinctUntilChanged, filter, map } from 'rxjs';
import { AUTOCOMPLETE_PAGE_SIZE } from '@shared/data-access/constants/pagination.contants';

@Directive({
  selector: 'cdk-virtual-scroll-viewport[appInfiniteVirtualScroll]',
})
export class InfiniteVirtualScrollDirective {
  @Input() pageSize = AUTOCOMPLETE_PAGE_SIZE;

  @Output() pageChange = this._viewport.scrolledIndexChange.pipe(
    distinctUntilChanged(),
    map(() => ({
      end: this._viewport.getRenderedRange().end,
      total: this._viewport.getDataLength(),
    })),
    filter(({ end, total }) => end === total),
    map(({ total }) => Math.floor(total / this.pageSize) + 1),
    distinctUntilChanged()
  );

  constructor(@Self() private _viewport: CdkVirtualScrollViewport) {}
}
