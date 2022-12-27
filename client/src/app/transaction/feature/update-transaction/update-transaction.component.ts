import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  ViewEncapsulation,
} from '@angular/core';
import {
  TuiContextWithImplicit,
  TuiDay,
  TuiHandler,
  tuiIsString,
  tuiPure,
  TuiStringHandler,
} from '@taiga-ui/cdk';
import {
  BehaviorSubject,
  finalize,
  map,
  Observable,
  shareReplay,
  Subject,
  take,
  takeUntil,
  tap,
} from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';
import { Label } from '@shared/data-access/models/label';
import { HttpClient } from '@angular/common/http';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { ExchangeRateApiService } from '../../../core/api/exchange-rate-api.service';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { RecipientApiService } from '../../../core/api/recipient-api.service';
import { LabelApiService } from '../../../core/api/label-api.service';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Transaction } from '@shared/data-access/models/transaction.model';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { Category } from '@shared/data-access/models/category.model';
import { Recipient } from '@shared/data-access/models/recipient.model';

@Component({
  selector: 'app-update-transaction',
  templateUrl: './update-transaction.component.html',
  styleUrls: ['./update-transaction.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateTransactionComponent implements OnDestroy {
  maxDate = TuiDay.currentLocal();
  private _destroyed$ = new Subject<void>();

  form = this._fb.nonNullable.group({
    id: [this._context.data.id, [Validators.required]],
    amount: [0, [Validators.required]],
    note: ['', [Validators.maxLength(512)]],
    recipientId: [null as string | null],
    categoryId: [null as string | null],
    currencyId: ['', [Validators.required]],
    transactionDate: [TuiDay.currentLocal(), [Validators.required]],
    labelIds: [[] as Label['id'][]],
    exchangeRate: [1],
  });

  loading$ = new BehaviorSubject<boolean>(false);

  categories$ = this._categoryService
    .getList(this._context.data.walletId, { pageSize: 250, page: 1 })
    .pipe(shareReplay(1));

  recipients$ = this._recipientService
    .getList(this._context.data.walletId, { pageSize: 250, page: 1 })
    .pipe(shareReplay(1));

  labels$ = this._labelService
    .getList(this._context.data.walletId, { pageSize: 250, page: 1 })
    .pipe(shareReplay(1));

  labelIds$ = this.labels$.pipe(map(data => data.map(y => y.id)));
  stringifiedLabels$: Observable<
    TuiHandler<TuiContextWithImplicit<string> | string, string>
  > = this.labels$.pipe(
    map(data => new Map(data.map(({ id, name }) => [id, name]))),
    map(
      m => (id: TuiContextWithImplicit<string> | string) =>
        (tuiIsString(id) ? m.get(id) : m.get(id.$implicit)) || 'Loading'
    )
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
        id: Transaction['id'];
        walletId: Wallet['id'];
      }
    >
  ) {
    this._transactionService
      .get(this._context.data.id)
      .pipe(takeUntil(this._destroyed$), take(1))
      .subscribe(transaction => {
        this.form.patchValue({
          note: transaction.note ?? '',
          transactionDate: TuiDay.jsonParse(transaction.transactionDate),
          recipientId: transaction.recipientId,
          categoryId: transaction.categoryId,
          labelIds: transaction.labels.map(x => x.id),
          currencyId: transaction.currencyId,
          exchangeRate: transaction.exchangeRate,
          amount: transaction.amount,
        });
      });
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
      .update(payload)
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
}
