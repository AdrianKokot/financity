import { Component, Inject, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import {
  TRANSACTION_TYPES,
  TransactionType,
} from '@shared/data-access/models/transaction-type.enum';
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
import { TuiDay } from '@taiga-ui/cdk';
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
import { LabelApiService } from '../../../core/api/label-api.service';

@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.scss'],
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

  getCurrencyName = (item: CurrencyListItem) => item.id;

  transactionTypes = TRANSACTION_TYPES;
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

  getLabelsFunction = this._labelService.getList.bind(
    this._labelService,
    this._context.data.walletId
  );

  getCategoriesFunction = this._categoryService.getList.bind(
    this._categoryService,
    this._context.data.walletId
  );

  getRecipientsFunction = this._recipientService.getList.bind(
    this._recipientService,
    this._context.data.walletId
  );

  getCurrenciesFunction = this._currencyService.getList.bind(
    this._currencyService
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
}
