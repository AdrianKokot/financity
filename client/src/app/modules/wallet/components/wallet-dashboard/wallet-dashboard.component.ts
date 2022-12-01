import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  OnInit,
  ViewChild,
} from '@angular/core';
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
  tap,
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
export class WalletDashboardComponent implements OnInit, AfterViewInit {
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
    tap(([page, _]) => {
      console.log('Current page: ' + page);
    }),
    exhaustMap(([page, walletId]) =>
      this._transactionApiService.getList(walletId, {
        page,
        pageSize: this._pageSize,
      })
    ),
    tap(res => {
      this.gotAllResults = res.length < 20;
      console.log(res.map(x => x.transactionDate));
    })
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
      // zero time debounce for a case when both key and direction change
      debounceTime(0),
      scan((acc, val) => {
        return [...acc, ...val];
      }, [] as TransactionListItem[]),
      // switchMap(query => this.getData(...query).pipe(startWith(null))),
      share()
    );

  readonly loading$ = this.request$.pipe(map(value => !value));
  readonly data$ = this.request$.pipe(startWith([] as TransactionListItem[]));

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _walletApiService: WalletApiService,
    private _transactionApiService: TransactionApiService
  ) {}

  log($event: number) {
    if (this.gotAllResults) return;
    const { end } = this.viewport.getRenderedRange();
    const total = this.viewport.getDataLength();

    // console.log($event);
    // console.log(this.viewport.getDataLength());
    // console.log(this.viewport.getRenderedRange().end);
    if (end === total) {
      // console.log({ end, total, $event, res: Math.floor(total / 20) + 1 });
      this.page$.next(Math.floor(total / this._pageSize) + 1);
    }
  }

  trackByIdx = (index: number, i: TransactionListItem) => index; //i.id;

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    // this.viewport.getDataLength();
    // this.viewport.renderedRangeStream.subscribe(i => {
    //   console.log(i);
    // });
  }
}
