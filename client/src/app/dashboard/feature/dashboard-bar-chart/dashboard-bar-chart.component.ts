import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import {
  Wallet,
  WalletListItem,
} from '@shared/data-access/models/wallet.model';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import {
  debounceTime,
  filter,
  map,
  merge,
  NEVER,
  Observable,
  shareReplay,
  startWith,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import {
  TuiContextWithImplicit,
  TuiDay,
  TuiDayRange,
  TuiMonth,
} from '@taiga-ui/cdk';
import { TUI_MONTHS, TuiDialogService, tuiFormatNumber } from '@taiga-ui/core';
import { tuiFormatCurrency } from '@taiga-ui/addon-commerce';
import { UserService } from '../../../auth/data-access/api/user.service';
import { BudgetApiService } from '../../../core/api/budget-api.service';
import { ActivatedRoute } from '@angular/router';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { ApiParams } from '../../../core/api/generic-api.service';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';

@Component({
  selector: 'app-dashboard-bar-chart',
  templateUrl: './dashboard-bar-chart.component.html',
  styleUrls: ['./dashboard-bar-chart.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardBarChartComponent {
  readonly form = this._fb.nonNullable.group({
    date: [
      new TuiDayRange(
        TuiDay.currentUtc().append({
          day: -TuiDay.currentUtc().day + 1,
          month: -6,
        }),
        TuiDay.currentUtc().append({
          day: -TuiDay.currentUtc().day + 1,
          month: 1,
        })
      ),
    ],
    currencyId: [''],
    wallets: [new Array<Wallet['id']>()],
  });

  chartNotAvailable = false;

  readonly dataApis = {
    getCurrencies: (p: ApiParams) =>
      this._currencyService.getUserCurrencies(p).pipe(
        tap(d => {
          if (d.length > 0) {
            this.form.controls.currencyId.setValue(d[0].id);
          } else {
            this.chartNotAvailable = true;
          }
        })
      ),
    getCurrencyName: (item: CurrencyListItem) => item.id,
    getWalletName: (item: WalletListItem) => item.name,
    getWallets: this._walletService.getAll.bind(this._walletService),
  };

  readonly chart$ = merge(
    this.form.valueChanges,
    this.form.controls.currencyId.valueChanges.pipe(
      tap(() => this.form.patchValue({ wallets: [] })),
      switchMap(() => NEVER)
    )
  ).pipe(
    map(() => this.form.getRawValue()),
    filter(({ currencyId }) => currencyId !== '' && currencyId !== null),
    distinctUntilChangedObject(),
    debounceTime(10),
    map(data => ({
      walletIds: data.wallets,
      currencyId: data.currencyId,
      from: data.date.from.toUtcNativeDate(),
      to: data.date.to.toUtcNativeDate(),
    })),
    switchMap(payload =>
      this._walletService.getStats(payload).pipe(
        map(chartData => [
          Object.values(chartData?.incomeStats ?? {}),
          Object.values(chartData?.expenseStats ?? {}),
        ]),
        map(data => ({
          data,
          max: Math.max(...data[0], ...data[1]),
        })),
        map(({ data, max }) => ({
          data,
          max:
            Math.ceil(max / 10 ** (max.toFixed(0).length - 1)) *
            10 ** (max.toFixed(0).length - 1),
        })),
        startWith(null)
      )
    ),
    shareReplay(1)
  );

  private readonly _chartNotNull$ = this.chart$.pipe(
    filter((x): x is { max: number; data: number[][] } => x !== null)
  );

  readonly isChartLoading$ = this.chart$.pipe(map(chart => chart === null));

  constructor(
    private _user: UserService,
    private readonly _walletService: WalletApiService,
    private readonly _budgetService: BudgetApiService,
    @Inject(TUI_MONTHS)
    private readonly _months$: Observable<readonly string[]>,
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _currencyService: CurrencyApiService,
    private readonly _transactionApiService: TransactionApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  readonly hint$ = this._chartNotNull$.pipe(
    map(
      ({ data }) =>
        ({ $implicit }: TuiContextWithImplicit<number>): string =>
          `Income ${tuiFormatNumber(data[0][$implicit], {
            decimalLimit: 2,
          })} ${tuiFormatCurrency(this.form.controls.currencyId.value)}
          Expenses ${tuiFormatNumber(data[1][$implicit], {
            decimalLimit: 2,
          })} ${tuiFormatCurrency(this.form.controls.currencyId.value)}`.trim()
    ),
    shareReplay(1)
  );

  readonly xLabels$ = this.form.controls.date.valueChanges.pipe(
    startWith(this.form.controls.date.value),
    withLatestFrom(this._months$),
    map(([{ from, to }, months]) =>
      Array.from(
        { length: TuiMonth.lengthBetween(from, to) },
        (_, i) => months[from.append({ month: i }).month]
      )
    ),
    shareReplay()
  );

  readonly yLabels$ = this._chartNotNull$.pipe(
    map(({ max }) => {
      const n = max / 4;

      return new Array(5)
        .fill(0)
        .map((_, i) => i * n)
        .map(
          x =>
            `${tuiFormatNumber(x)} ${tuiFormatCurrency(
              this.form.controls.currencyId.value
            )}`
        );
    })
  );
}
