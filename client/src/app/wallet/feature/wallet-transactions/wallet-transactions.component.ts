import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
  ViewChild,
} from '@angular/core';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import {
  BehaviorSubject,
  debounceTime,
  distinctUntilChanged,
  exhaustMap,
  filter,
  map,
  merge,
  scan,
  share,
  shareReplay,
  startWith,
  Subject,
  switchMap,
  take,
  tap,
  withLatestFrom,
} from 'rxjs';
import {
  Transaction,
  TransactionListItem,
} from '@shared/data-access/models/transaction.model';
import { ActivatedRoute } from '@angular/router';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { TuiAlertService, TuiDialogService } from '@taiga-ui/core';
import { UpdateTransactionComponent } from '../../../transaction/feature/update-transaction/update-transaction.component';
import { CreateTransactionComponent } from 'src/app/transaction/feature/create-transaction/create-transaction.component';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import { FormBuilder } from '@angular/forms';
import { TuiDay, TuiDayRange } from '@taiga-ui/cdk';
import { TuiDayRangePeriod } from '@taiga-ui/kit';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';
import { Category } from '@shared/data-access/models/category.model';
import { CategoryApiService } from '../../../core/api/category-api.service';

const createTransactionDateFilterGroups = () => {
  //https://github.com/Tinkoff/taiga-ui/blob/fdde12d70356bf5a018eed3c3e6747fff5adc8b0/projects/kit/utils/miscellaneous/create-default-day-range-periods.ts

  const today = TuiDay.currentLocal();
  const startOfWeek = today.append({ day: -today.dayOfWeek() });

  const endOfWeek = startOfWeek.append({ day: 6 });
  const startOfMonth = today.append({ day: 1 - today.day });
  const endOfMonth = startOfMonth.append({ month: 1, day: -1 });
  const startOfLastMonth = startOfMonth.append({ month: -1 });

  const startOfYear = startOfMonth.append({ month: -startOfMonth.month });

  return [
    new TuiDayRangePeriod(new TuiDayRange(today, today), 'Today'),
    new TuiDayRangePeriod(new TuiDayRange(startOfWeek, endOfWeek), 'This week'),
    new TuiDayRangePeriod(
      new TuiDayRange(
        startOfWeek.append({ day: -7 }),
        endOfWeek.append({ day: -7 })
      ),
      'Last week'
    ),
    new TuiDayRangePeriod(
      new TuiDayRange(startOfMonth, endOfMonth),
      'This month'
    ),
    new TuiDayRangePeriod(
      new TuiDayRange(startOfLastMonth, startOfMonth.append({ day: -1 })),
      'Last month'
    ),
    new TuiDayRangePeriod(
      new TuiDayRange(today.append({ day: -30 }), today),
      'Last 30 days'
    ),
    new TuiDayRangePeriod(
      new TuiDayRange(startOfYear, startOfYear.append({ month: 12, day: -1 })),
      'This year'
    ),
    new TuiDayRangePeriod(
      new TuiDayRange(
        startOfYear.append({ year: -1 }),
        startOfYear.append({ day: -1 })
      ),
      'Last year'
    ),
  ];
};

@Component({
  selector: 'app-wallet-transactions',
  templateUrl: './wallet-transactions.component.html',
  styleUrls: ['./wallet-transactions.component.scss'],
  // encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletTransactionsComponent {
  dayRangeItems = createTransactionDateFilterGroups();

  form = this._fb.nonNullable.group({
    dateRange: [this.dayRangeItems[5].range],
    search: [''],
    categories: [[] as Category['id'][]],
  });

  filters$ = this.form.valueChanges.pipe(
    debounceTime(300),
    startWith({}),
    map(() => this.form.getRawValue()),
    map(({ search, dateRange, categories }) => {
      const obj: Record<string, string> = {};

      console.log(categories);

      if (search) {
        obj['search'] = search.trim();
      }

      if (dateRange && dateRange.from) {
        obj['transactionDate_gte'] = dateRange.from
          .toLocalNativeDate()
          .toISOString()
          .split('T')[0];
      }

      if (dateRange && dateRange.to) {
        obj['transactionDate_lte'] = dateRange.to
          .toLocalNativeDate()
          .toISOString()
          .split('T')[0];
      }

      return obj;
    }),
    distinctUntilChangedObject(),
    shareReplay(1)
  );

  appliedFiltersCount$ = this.filters$.pipe(
    map(x => Object.keys(x).length - ('transactionDate_gte' in x ? 1 : 0)),
    distinctUntilChanged(),
    shareReplay(1)
  );

  @ViewChild(CdkVirtualScrollViewport) viewport!: CdkVirtualScrollViewport;

  walletId$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id)
  );

  wallet$ = this.walletId$.pipe(
    switchMap(walletId => this._walletApiService.get(walletId)),
    shareReplay(1)
  );

  categories$ = this.walletId$.pipe(
    switchMap(walletId =>
      this._categoryService.getList(walletId, {
        page: 1,
        pageSize: 100,
      })
    ),
    shareReplay(1)
  );

  page$ = new BehaviorSubject<number>(1);
  private _pageSize = 20;

  transactions$ = this.page$.pipe(
    withLatestFrom(this.walletId$, this.filters$),
    distinctUntilChangedObject(),
    exhaustMap(([page, walletId, filters]) =>
      this._transactionApiService
        .getList(walletId, {
          page,
          pageSize: this._pageSize,
          filters,
        })
        .pipe(startWith(null))
    ),
    shareReplay(1)
  );

  gotAllResults = false;

  openCreateDialog(): void {
    this.wallet$
      .pipe(
        switchMap(({ id, currencyId }) => {
          return this._dialog.open<Transaction>(
            new PolymorpheusComponent(
              CreateTransactionComponent,
              this._injector
            ),
            {
              label: 'Create transaction',
              data: {
                walletId: id,
                walletCurrencyId: currencyId,
                transactionType: TransactionType.Expense,
              },
            }
          );
        })
      )
      .subscribe(item => {
        this._newTransaction$.next(item);
      });
  }

  openEditDialog(id: Transaction['id']): void {
    this.walletId$
      .pipe(
        take(1),
        switchMap(walletId =>
          this._dialog.open<Transaction>(
            new PolymorpheusComponent(
              UpdateTransactionComponent,
              this._injector
            ),
            {
              label: 'Edit transaction',
              data: {
                id,
                walletId,
              },
            }
          )
        )
      )
      .subscribe(item => this._modifiedTransaction$.next(item));
  }

  deleteTransaction(id: Transaction['id']): void {
    this._transactionApiService
      .delete(id)
      .pipe(filter(success => success))
      .subscribe(() => {
        this._deletedTransaction$.next({ id });
      });
  }

  private _modifiedTransaction$ = new Subject<Transaction>();
  private _deletedTransaction$ = new Subject<Pick<Transaction, 'id'>>();
  private _newTransaction$ = new Subject<Transaction>();

  readonly request$ = merge(
    this.transactions$.pipe(
      filter((x): x is TransactionListItem[] => x !== null),
      map(items => (acc: TransactionListItem[]) => [...acc, ...items])
    ),
    this.filters$.pipe(
      map(() => () => []),
      tap(() => this.page$.next(1))
    )
  )
    //   combineLatest([
    //   this.sorter$,
    //   this.direction$,
    //   this.page$,
    //   this.size$,
    //   tuiControlValue<number>(this.minAge),
    // ])
    .pipe(
      // zero time debounce for a case when both key and direction change
      scan(
        (acc: TransactionListItem[], fn) => fn(acc),
        [] as TransactionListItem[]
      ),
      share()
    );

  readonly loading$ = this.transactions$.pipe(map(value => !value));
  readonly data$ = this.request$.pipe(startWith([] as TransactionListItem[]));

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _walletApiService: WalletApiService,
    private _categoryService: CategoryApiService,
    private _transactionApiService: TransactionApiService,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService,
    private _fb: FormBuilder
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
