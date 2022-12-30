import { Directive, HostBinding, Input, Output, Self } from '@angular/core';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { distinctUntilChanged, filter, map } from 'rxjs';
import { AUTOCOMPLETE_PAGE_SIZE } from '@shared/data-access/constants/pagination.contants';

@Directive({
  selector: 'cdk-virtual-scroll-viewport[appInfiniteVirtualScroll]',
})
export class InfiniteVirtualScrollDirective {
  @Input() pageSize = AUTOCOMPLETE_PAGE_SIZE;

  @HostBinding('style.--viewport-items-count')
  @Input()
  viewportItemsCount = 0;

  @HostBinding('style.--viewport-max-items-count')
  @Input()
  showMax = 1;

  @Input() set fixed(value: number) {
    this.viewportItemsCount = value;
    this.showMax = value;
  }

  get fixed() {
    return this.viewportItemsCount;
  }

  @HostBinding('style.--viewport-items-margin.px')
  @Input()
  itemsMargin = 0;

  @HostBinding('style.--viewport-item-size.px')
  @Input()
  itemSize = 0;

  @HostBinding('attr.data-infinite-viewport')
  @Input()
  calculateViewportHeight = true;

  @HostBinding('class') hostClass = 'tui-zero-scrollbar';

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
