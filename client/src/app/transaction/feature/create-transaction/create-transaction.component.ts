import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { Validators } from '@angular/forms';
import {
  TRANSACTION_TYPES,
  TransactionType,
} from '@shared/data-access/models/transaction-type.enum';
import {
  catchError,
  filter,
  map,
  merge,
  of,
  share,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import {
  POLYMORPHEUS_CONTEXT,
  PolymorpheusComponent,
} from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext, TuiDialogService } from '@taiga-ui/core';
import { TuiDay, tuiIsNumber } from '@taiga-ui/cdk';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { Transaction } from '@shared/data-access/models/transaction.model';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { Label, LabelListItem } from '@shared/data-access/models/label';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { HttpErrorResponse } from '@angular/common/http';
import { handleValidationApiError } from '@shared/utils/api/api-error-handler';
import { CreateCategoryComponent } from '../../../category/feature/create-category/create-category.component';
import {
  Category,
  CategoryListItem,
} from '@shared/data-access/models/category.model';
import { CreateRecipientComponent } from '../../../recipient/feature/create-recipient/create-recipient.component';
import {
  Recipient,
  RecipientListItem,
} from '@shared/data-access/models/recipient.model';
import { CreateLabelComponent } from '../../../label/feature/create-label/create-label.component';

@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateTransactionComponent {
  readonly ui = {
    transactionDate: {
      max: TuiDay.currentLocal(),
      min: new TuiDay(1900, 0, 1),
    } as const,
    transactionTypes: TRANSACTION_TYPES,
    transactionType: TransactionType,
    dataApis: {
      ...this._walletService.getConcreteWalletApi(this._context.data.walletId),
      getCurrencies: this._currencyService.getList.bind(this._currencyService),
      getCurrencyName: (item: CurrencyListItem) => item.id,
    },
    actions: {
      createCategory$: new Subject<string>(),
      createRecipient$: new Subject<string>(),
      createLabel$: new Subject<string>(),
    },
    preloadedResults: {
      categories: new Array<CategoryListItem>(),
      recipients: new Array<RecipientListItem>(),
      labels: new Array<LabelListItem>(),
    },
  };

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
      transactionDate: [this.ui.transactionDate.max, [Validators.required]],
      labelIds: [new Array<Label['id']>()],
    },
    {
      submit: payload =>
        this._dataService.create({
          ...payload,
          transactionDate: payload.transactionDate.toUtcNativeDate().toJSON(),
        }),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly dialogs$ = merge(
    this.form.group.controls.transactionType.valueChanges.pipe(
      tap(() => this.form.group.controls.categoryId.reset())
    ),
    this.ui.actions.createCategory$.pipe(
      switchMap(name =>
        this._dialog
          .open<Category>(
            new PolymorpheusComponent(CreateCategoryComponent, this._injector),
            {
              label: 'Create category',
              data: {
                name,
                walletId: this.form.group.controls.walletId.value,
                transactionType: this.form.group.controls.transactionType.value,
              },
            }
          )
          .pipe(
            tap(item => {
              if (
                item.transactionType ===
                this.form.group.controls.transactionType.value
              ) {
                this.form.group.controls.categoryId.setValue(item.id);
                this.ui.preloadedResults.categories =
                  this.ui.preloadedResults.categories.concat(item);
              }
            })
          )
      )
    ),
    this.ui.actions.createRecipient$.pipe(
      switchMap(name =>
        this._dialog
          .open<Recipient>(
            new PolymorpheusComponent(CreateRecipientComponent, this._injector),
            {
              label: 'Create recipient',
              data: {
                name,
                walletId: this.form.group.controls.walletId.value,
              },
            }
          )
          .pipe(
            tap(item => {
              this.form.group.controls.recipientId.setValue(item.id);
              this.ui.preloadedResults.recipients =
                this.ui.preloadedResults.recipients.concat(item);
            })
          )
      )
    ),
    this.ui.actions.createLabel$.pipe(
      switchMap(name =>
        this._dialog
          .open<Label>(
            new PolymorpheusComponent(CreateLabelComponent, this._injector),
            {
              label: 'Create label',
              data: {
                name,
                walletId: this.form.group.controls.walletId.value,
              },
            }
          )
          .pipe(
            tap(item => {
              this.form.group.controls.labelIds.setValue(
                this.form.group.controls.labelIds.value.concat(item.id)
              );
              this.ui.preloadedResults.labels =
                this.ui.preloadedResults.labels.concat(item);
            })
          )
      )
    )
  );

  readonly shouldExchangeRateBeSpecified$ = merge(
    this.form.controls.currencyId.valueChanges,
    this.form.controls.transactionDate.valueChanges
  ).pipe(
    filter(x => x !== null),
    map(() => this.form.group.getRawValue().currencyId),
    map(x => x !== this._context.data.walletCurrencyId),
    share()
  );

  readonly exchangeRateLoading$ = this.shouldExchangeRateBeSpecified$.pipe(
    switchMap(should => {
      return (
        should
          ? this._currencyService
              .getExchangeRate(
                this.form.controls.currencyId.value,
                this._context.data.walletCurrencyId,
                this.form.controls.transactionDate.value?.toUtcNativeDate()
              )
              .pipe(
                catchError(err => {
                  if (err instanceof HttpErrorResponse) {
                    handleValidationApiError(
                      this.form.group,
                      err,
                      'exchangeRate'
                    );
                  }
                  return of(undefined);
                })
              )
          : of(1)
      ).pipe(
        toLoadingState(
          rate =>
            tuiIsNumber(rate) &&
            this.form.group.controls.exchangeRate.setValue(rate)
        )
      );
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
        transactionType: TransactionType;
        walletCurrencyId: Wallet['currencyId'];
      }
    >,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
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
