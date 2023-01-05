import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { TuiDay } from '@taiga-ui/cdk';
import { Validators } from '@angular/forms';
import { Label, LabelListItem } from '@shared/data-access/models/label';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import {
  POLYMORPHEUS_CONTEXT,
  PolymorpheusComponent,
} from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext, TuiDialogService } from '@taiga-ui/core';
import { Transaction } from '@shared/data-access/models/transaction.model';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import {
  Category,
  CategoryListItem,
} from '@shared/data-access/models/category.model';
import {
  Recipient,
  RecipientListItem,
} from '@shared/data-access/models/recipient.model';
import { merge, Subject, switchMap, tap } from 'rxjs';
import { CreateCategoryComponent } from '../../../category/feature/create-category/create-category.component';
import { CreateRecipientComponent } from '../../../recipient/feature/create-recipient/create-recipient.component';
import { CreateLabelComponent } from '../../../label/feature/create-label/create-label.component';

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
      walletId: [''],
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

  readonly ui = {
    dataApis: this._walletService.getConcreteWalletApi(
      this._context.data.walletId
    ),
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

  readonly dialogs$ = merge(
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

  readonly initialDataLoading$ = this._dataService
    .get(this._context.data.id)
    .pipe(
      toLoadingState(data => {
        this.ui.preloadedResults.labels = data.labels;
        if (data.category !== null)
          this.ui.preloadedResults.categories = [data.category];
        if (data.recipient !== null)
          this.ui.preloadedResults.recipients = [data.recipient];

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
    >,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  cancel() {
    this._context.$implicit.complete();
  }
}
