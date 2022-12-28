import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import {
  TRANSACTION_TYPES,
  TransactionType,
} from '@shared/data-access/models/transaction-type.enum';
import { filter, map, of, share, switchMap } from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { TuiDay } from '@taiga-ui/cdk';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { Transaction } from '@shared/data-access/models/transaction.model';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { Label } from '@shared/data-access/models/label';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { WalletApiService } from '../../../core/api/wallet-api.service';

@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateTransactionComponent {
  readonly transactionTypes = TRANSACTION_TYPES;
  readonly transactionMaxDate = TuiDay.currentLocal();

  readonly form = this._fb.form(
    {
      amount: [0, [Validators.required, Validators.min(0)]],
      note: ['', [Validators.maxLength(512)]],
      exchangeRate: [1, [Validators.required, Validators.min(0)]],
      recipientId: this._fb.control<string | null>(null),
      walletId: [''],
      transactionType: [TransactionType.Income, [Validators.required]],
      categoryId: this._fb.control<string | null>(null),
      currencyId: ['', [Validators.required]],
      transactionDate: [this.transactionMaxDate, [Validators.required]],
      labelIds: [new Array<Label['id']>()],
    },
    {
      submit: payload =>
        this._dataService.create({
          ...payload,
          transactionDate: payload.transactionDate
            .toUtcNativeDate()
            .toISOString(),
        }),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly shouldExchangeRateBeSpecified$ =
    this.form.controls.currencyId.valueChanges.pipe(
      filter(x => x !== null),
      map(id => id !== this._context.data.walletCurrencyId),
      share()
    );

  readonly exchangeRateLoading$ = this.shouldExchangeRateBeSpecified$.pipe(
    switchMap(should =>
      (should
        ? this._currencyService.getExchangeRate(
            this.form.controls.currencyId.value,
            this._context.data.walletCurrencyId
          )
        : of(1)
      ).pipe(
        toLoadingState(rate => this.form.controls.exchangeRate.setValue(rate))
      )
    )
  );

  readonly dataApis = {
    ...this._walletService.getConcreteWalletApi(this._context.data.walletId),
    getCurrencies: this._currencyService.getList.bind(this._currencyService),
    getCurrencyName: (item: CurrencyListItem) => item.id,
  };

  constructor(
    private readonly _dataService: TransactionApiService,
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _currencyService: CurrencyApiService,
    private readonly _walletService: WalletApiService,
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
    this.form.patchValue({
      walletId: this._context.data.walletId,
      currencyId: this._context.data.walletCurrencyId,
      transactionType: this._context.data.transactionType,
    });
  }

  cancel() {
    this._context.$implicit.complete();
  }
}
