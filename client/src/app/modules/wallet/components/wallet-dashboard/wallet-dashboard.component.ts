import { ChangeDetectionStrategy, Component, ViewChild } from '@angular/core';
import { WalletApiService } from '../../../../core/api/wallet-api.service';
import { ActivatedRoute } from '@angular/router';
import {
  BehaviorSubject,
  debounceTime,
  distinctUntilChanged,
  exhaustMap,
  filter,
  map,
  scan,
  share,
  startWith,
  switchMap,
  withLatestFrom,
} from 'rxjs';
import { TransactionApiService } from '../../../../core/api/transaction-api.service';
import { TransactionListItem } from '@shared/data-access/models/transaction.model';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-wallet-dashboard',
  templateUrl: './wallet-dashboard.component.html',
  styleUrls: ['./wallet-dashboard.component.scss'],
  // encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletDashboardComponent {
  @ViewChild(CdkVirtualScrollViewport) viewport!: CdkVirtualScrollViewport;

  walletId$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id)
  );

  wallet$ = this.walletId$.pipe(
    switchMap(walletId => this._walletApiService.get(walletId))
  );

  page$ = new BehaviorSubject<number>(1);
  private _pageSize = 50;

  transactions$ = this.page$.pipe(
    distinctUntilChanged(),
    withLatestFrom(this.walletId$),
    exhaustMap(([page, walletId]) =>
      this._transactionApiService
        .getList(walletId, {
          page,
          pageSize: this._pageSize,
        })
        .pipe(startWith(null))
    )
  );
  gotAllResults = false;

  readonly request$ = this.transactions$
    //   combineLatest([
    //   this.sorter$,
    //   this.direction$,
    //   this.page$,
    //   this.size$,
    //   tuiControlValue<number>(this.minAge),
    // ])
    .pipe(
      filter((x): x is TransactionListItem[] => x !== null),
      // zero time debounce for a case when both key and direction change
      debounceTime(0),
      scan((acc, val) => {
        return [...acc, ...val];
      }, [] as TransactionListItem[]),
      // switchMap(query => this.getData(...query).pipe(startWith(null))),
      share()
    );

  readonly loading$ = this.transactions$.pipe(map(value => !value));
  readonly data$ = this.request$.pipe(startWith([] as TransactionListItem[]));

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _walletApiService: WalletApiService,
    private _transactionApiService: TransactionApiService
  ) {}

  log() {
    if (this.gotAllResults) return;
    const { end } = this.viewport.getRenderedRange();
    const total = this.viewport.getDataLength();

    if (end === total) {
      this.page$.next(Math.floor(total / this._pageSize) + 1);
    }
  }

  trackByIdx = (index: number) => index;
}
