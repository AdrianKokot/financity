import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { UserService } from '../../../auth/data-access/api/user.service';
import { User } from '../../../auth/data-access/models/user';
import {
  filter,
  forkJoin,
  map,
  merge,
  Observable,
  of,
  shareReplay,
  Subject,
  switchMap,
} from 'rxjs';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { BudgetApiService } from '../../../core/api/budget-api.service';
import { TUI_MONTHS, TuiDialogService, tuiFormatNumber } from '@taiga-ui/core';
import {
  TuiContextWithImplicit,
  TuiDay,
  TuiDayRange,
  TuiMonth,
  tuiPure,
} from '@taiga-ui/cdk';
import { Currency } from '@shared/data-access/models/currency.model';
import { tuiFormatCurrency } from '@taiga-ui/addon-commerce';
import { TransactionListItem } from '@shared/data-access/models/transaction.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { TransactionDetailsComponent } from '../../../transaction/feature/transaction-details/transaction-details.component';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { ActivatedRoute } from '@angular/router';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-dashboard-shell',
  templateUrl: './dashboard-shell.component.html',
  styleUrls: ['./dashboard-shell.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardShellComponent {
  user: User | null = null;

  range = new TuiDayRange(
    TuiDay.currentUtc().append({
      day: -TuiDay.currentUtc().day + 1,
      month: -12,
    }),
    TuiDay.currentUtc().append({ day: -TuiDay.currentUtc().day + 1, month: 1 })
  );
  private _currencyId: null | Currency['id'] = null;

  readonly ui$ = forkJoin([
    this._user.user$,
    this._user.getUserCurrencies().pipe(
      switchMap(currencies => {
        if (currencies.length > 0) {
          this._currencyId = currencies[0].id;
          return this._walletService
            .getStats({
              currencyId: currencies[0].id,
              walletIds: [],
              from: this.range.from.toUtcNativeDate(),
              to: this.range.to.toUtcNativeDate(),
            })
            .pipe(map(stats => ({ currencies, stats })));
        }

        return of({ currencies, stats: null });
      })
    ),
    this._walletService
      .getAll({ page: 1, pageSize: 20 })
      .pipe(map(data => data.slice(0, 5))),
    this._budgetService
      .getAll({ page: 1, pageSize: 20 })
      .pipe(map(data => data.slice(0, 5))),
  ]).pipe(
    map(([user, { currencies, stats }, wallets, budgets]) => ({
      user,
      currencies,
      stats,
      wallets,
      budgets,
    })),
    shareReplay(1)
  );

  readonly initialDataLoading$ = this.ui$.pipe(toLoadingState());

  readonly chart$ = this.ui$.pipe(
    map(x => x.stats),
    filter(x => x !== null),
    map(d => {
      const chartData = d ?? {
        incomeStats: {} as Record<string, number>,
        expenseStats: {} as Record<string, number>,
      };

      return [
        Object.values(chartData.incomeStats),
        Object.values(chartData.expenseStats),
      ];
    }),
    map(data => ({
      data,
      max: Math.max(...data[0], ...data[1]),
    })),
    map(({ data, max }) => ({
      data,
      max: max * 1.2,
    }))
  );

  constructor(
    private _user: UserService,
    private readonly _walletService: WalletApiService,
    private readonly _budgetService: BudgetApiService,
    @Inject(TUI_MONTHS)
    private readonly _months$: Observable<readonly string[]>,
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _transactionApiService: TransactionApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  readonly hint$ = this.chart$.pipe(
    map(
      ({ data }) =>
        ({ $implicit }: TuiContextWithImplicit<number>): string =>
          `Income ${tuiFormatNumber(data[0][$implicit], {
            decimalLimit: 2,
          })} ${tuiFormatCurrency(this._currencyId)}
          Expenses ${tuiFormatNumber(data[1][$implicit], {
            decimalLimit: 2,
          })} ${tuiFormatCurrency(this._currencyId)}`.trim()
    ),
    shareReplay(1)
  );

  @tuiPure
  computeLabels$({ from, to }: TuiDayRange): Observable<readonly string[]> {
    return this._months$.pipe(
      map(months =>
        Array.from(
          { length: TuiMonth.lengthBetween(from, to) },
          (_, i) => months[from.append({ month: i }).month]
        )
      )
    );
  }

  readonly ui = {
    columns: [
      'transactionDate',
      'category',
      'labels',
      'note',
      'recipient',
      'amount',
      'actions',
    ] as const,
    actions: {
      details$: new Subject<TransactionListItem>(),
    },
  };

  readonly filters = this._fb.filters({
    search: [''],
  });

  readonly data = new ApiDataHandler<
    TransactionListItem,
    { search: FormControl<string> },
    'search'
  >(
    this._transactionApiService.getAll.bind(this._transactionApiService),
    this.filters
  );

  readonly dialogs$ = merge(
    this.ui.actions.details$.pipe(
      switchMap(transaction =>
        this._dialog.open(
          new PolymorpheusComponent(
            TransactionDetailsComponent,
            this._injector
          ),
          {
            label: 'Transaction details',
            data: { transaction },
            dismissible: true,
          }
        )
      )
    )
  );
  trackById = (index: number, item: TransactionListItem) => item.id;
}
