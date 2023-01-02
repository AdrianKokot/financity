import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { TuiDay } from '@taiga-ui/cdk';
import { Validators } from '@angular/forms';
import { Label } from '@shared/data-access/models/label';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Transaction } from '@shared/data-access/models/transaction.model';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { Category } from '@shared/data-access/models/category.model';
import { Recipient } from '@shared/data-access/models/recipient.model';

@Component({
  selector: 'app-update-transaction',
  templateUrl: './update-transaction.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateTransactionComponent {
  readonly transactionMaxDate = TuiDay.currentLocal();

  readonly form = this._fb.form(
    {
      id: ['', [Validators.required]],
      amount: [0, [Validators.required, Validators.min(0)]],
      note: ['', [Validators.maxLength(512)]],
      recipientId: this._fb.control<string | null>(null),
      categoryId: this._fb.control<string | null>(null),
      currencyId: [''],
      transactionType: [''],
      transactionDate: [this.transactionMaxDate, [Validators.required]],
      labelIds: [new Array<Label['id']>()],
    },
    {
      submit: payload =>
        this._dataService.update({
          ...payload,
          transactionDate: payload.transactionDate.toUtcNativeDate().toJSON(),
        }),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly dataApis = this._walletService.getConcreteWalletApi(
    this._context.data.walletId
  );

  readonly preloadedData = {
    labels: new Array<Label>(),
    categories: new Array<Category>(),
    recipients: new Array<Recipient>(),
  };

  readonly initialDataLoading$ = this._dataService
    .get(this._context.data.id)
    .pipe(
      toLoadingState(data => {
        this.preloadedData.labels = data.labels;
        if (data.category !== null)
          this.preloadedData.categories = [data.category];
        if (data.recipient !== null)
          this.preloadedData.recipients = [data.recipient];

        this.form.patchValue({
          ...data,
          note: data.note ?? '',
          transactionDate: TuiDay.fromLocalNativeDate(
            new Date(data.transactionDate)
          ),
          labelIds: data.labels.map(x => x.id),
        });
      })
    );

  constructor(
    private readonly _dataService: TransactionApiService,
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _currencyService: CurrencyApiService,
    private readonly _walletService: WalletApiService,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Pick<Transaction, 'id'>,
      {
        walletId: Wallet['id'];
        id: Transaction['id'];
      }
    >
  ) {}

  cancel() {
    this._context.$implicit.complete();
  }
}
