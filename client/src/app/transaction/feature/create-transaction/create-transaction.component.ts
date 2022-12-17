import { Component, Inject, OnDestroy, ViewEncapsulation } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import {
  BehaviorSubject,
  distinctUntilChanged,
  filter,
  finalize,
  map,
  of,
  shareReplay,
  startWith,
  Subject,
  switchMap,
  takeUntil,
  tap,
  withLatestFrom,
} from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import {
  TuiContextWithImplicit,
  TuiDay,
  tuiPure,
  TuiStringHandler,
} from '@taiga-ui/cdk';
import { Label } from '@shared/data-access/models/label';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { Transaction } from '@shared/data-access/models/transaction.model';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { HttpClient } from '@angular/common/http';
import { ExchangeRateApiService } from '../../../core/api/exchange-rate-api.service';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { RecipientApiService } from '../../../core/api/recipient-api.service';
import { Category } from '@shared/data-access/models/category.model';
import { Recipient } from '@shared/data-access/models/recipient.model';
import { LabelApiService } from '../../../core/api/label-api.service';

@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class CreateTransactionComponent implements OnDestroy {
  maxDate = TuiDay.currentLocal();
  private _destroyed$ = new Subject<void>();
  walletCurrencyId = this._context.data.walletCurrencyId;

  form = this._fb.nonNullable.group({
    amount: [0, [Validators.required]],
    note: ['', [Validators.maxLength(512)]],
    exchangeRate: [1, [Validators.required, Validators.min(0)]],
    recipientId: [null as string | null],
    walletId: [this._context.data.walletId],
    transactionType: [
      this._context.data.transactionType,
      [Validators.required],
    ],
    categoryId: [null as string | null],
    currencyId: [this.walletCurrencyId, [Validators.required]],
    transactionDate: [this.maxDate, [Validators.required]],
    labelIds: [[] as Label['id'][]],
  });

  currencies$ = this._currencyService.getList().pipe(shareReplay(1));
  getCurrencyName = (item: CurrencyListItem) => `${item.name} [${item.id}]`;

  getTransactionTypeLabel = ({ label }: { label: string }) => label;
  getTransactionTypeId = ({ id }: { id: TransactionType }) => id;
  transactionTypes = [
    { id: TransactionType.Income, label: 'Income' },
    { id: TransactionType.Expense, label: 'Expense' },
  ];
  loading$ = new BehaviorSubject<boolean>(false);

  private _selectedCurrency$ = this.form.controls.currencyId.valueChanges.pipe(
    filter(x => x !== null),
    distinctUntilChanged(),
    startWith(this.form.controls.currencyId.value),
    shareReplay(1)
  );

  shouldExchangeRateBeSpecified$ = this._selectedCurrency$.pipe(
    map(id => id !== this.walletCurrencyId),
    distinctUntilChanged(),
    shareReplay(1)
  );

  fetchExchangeRate$ = this._selectedCurrency$.pipe(
    withLatestFrom(this.shouldExchangeRateBeSpecified$),
    switchMap(([base, should]) => {
      if (!should) return of(1);

      return this._exchangeRate.getExchangeRate(base, this.walletCurrencyId);
    }),
    shareReplay(1),
    tap(rate => {
      this.form.controls.exchangeRate.setValue(rate);
    })
  );

  categories$ = this._categoryService
    .getList(this._context.data.walletId, { pageSize: 250, page: 1 })
    .pipe(shareReplay(1));

  recipients$ = this._recipientService
    .getList(this._context.data.walletId, { pageSize: 250, page: 1 })
    .pipe(shareReplay(1));

  labels$ = this._labelService
    .getList(this._context.data.walletId, { pageSize: 250, page: 1 })
    .pipe(shareReplay(1));

  constructor(
    private _http: HttpClient,
    private _transactionService: TransactionApiService,
    private readonly _fb: FormBuilder,
    private readonly _currencyService: CurrencyApiService,
    private _exchangeRate: ExchangeRateApiService,
    private _categoryService: CategoryApiService,
    private _recipientService: RecipientApiService,
    private _labelService: LabelApiService,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Transaction,
      {
        walletId: Wallet['id'];
        transactionType: TransactionType;
        walletCurrencyId: Wallet['currencyId'];
      }
    >
  ) {
    this.fetchExchangeRate$.pipe(takeUntil(this._destroyed$)).subscribe();
    // combineLatest([
    //   this.form.controls.amount.valueChanges.pipe(distinctUntilChanged()),
    //   this.form.controls.transactionType.valueChanges.pipe(
    //     distinctUntilChanged(),
    //     startWith(this.form.controls.transactionType.value)
    //   ),
    // ])
    //   .pipe(
    //     takeUntil(this._destroyed$),
    //     filter(
    //       ([amount, transactionType]) =>
    //         (transactionType === TransactionType.Expense && amount > 0) ||
    //         (transactionType === TransactionType.Income && amount < 0)
    //     ),
    //     map(([amount]) => amount * -1)
    //   )
    //   .subscribe(val => {
    //     console.log(val);
    //     this.form.controls.amount.setValue(val);
    //   });
  }

  ngOnDestroy(): void {
    this._destroyed$.next();
    this._destroyed$.complete();
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const payload = {
      ...this.form.getRawValue(),
      transactionDate: this.form.controls.transactionDate
        .getRawValue()
        .toUtcNativeDate()
        .toISOString()
        .split('T')[0],
    };

    this._transactionService
      .create(payload)
      .pipe(
        tap(() => {
          this.loading$.next(true);
        }),
        finalize(() => {
          this.loading$.next(false);
        })
      )
      .subscribe(cat => {
        this._context.completeWith(cat);
      });
  }

  getItemName = ({ name }: { name: string }) => name;

  @tuiPure
  stringifyName(
    items: (Category | Recipient)[]
  ): TuiStringHandler<TuiContextWithImplicit<string>> {
    const map = new Map(
      items.map(item => [item.id, item.name] as [string, string])
    );

    return ({ $implicit }: TuiContextWithImplicit<string>) =>
      map.get($implicit) || '';
  }

  @tuiPure
  stringifyCurrencies(
    items: readonly CurrencyListItem[]
  ): TuiStringHandler<TuiContextWithImplicit<string>> {
    const map = new Map(
      items.map(
        item => [item.id, this.getCurrencyName(item)] as [string, string]
      )
    );

    return ({ $implicit }: TuiContextWithImplicit<string>) =>
      map.get($implicit) || '';
  }

  @tuiPure
  stringifyTransactionTypes(
    items: typeof this.transactionTypes
  ): TuiStringHandler<TuiContextWithImplicit<TransactionType>> {
    const map = new Map(items.map(({ id, label }) => [id, label]));

    return ({ $implicit }: TuiContextWithImplicit<TransactionType>) =>
      map.get($implicit) || '';
  }
}
