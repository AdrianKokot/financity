import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import {
  filter,
  finalize,
  map,
  merge,
  shareReplay,
  Subject,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import {
  Transaction,
  TransactionListItem,
} from '@shared/data-access/models/transaction.model';
import { ActivatedRoute } from '@angular/router';
import { WalletApiService } from '@shared/data-access/api/wallet-api.service';
import { TransactionApiService } from '@shared/data-access/api/transaction-api.service';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { TuiDialogService } from '@taiga-ui/core';
import { UpdateTransactionComponent } from '../../../transaction/feature/update-transaction/update-transaction.component';
import { CreateTransactionComponent } from 'src/app/transaction/feature/create-transaction/create-transaction.component';
import {
  TRANSACTION_TYPES,
  TransactionType,
} from '@shared/data-access/models/transaction-type.enum';
import { Category } from '@shared/data-access/models/category.model';
import { Recipient } from '@shared/data-access/models/recipient.model';
import { Label } from '@shared/data-access/models/label';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { DATE_RANGE_FILTER_GROUPS } from '../../utils/date-range-filter-groups.constants';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { TuiDay } from '@taiga-ui/cdk';
import { TransactionDetailsComponent } from '../../../transaction/feature/transaction-details/transaction-details.component';
import { ApiParams } from '@shared/data-access/api/generic-api.service';
import { SortSelectItem } from '@shared/ui/tui/sort-select/sort-select.component';
import { getFiltersFromParamMap } from '../../utils/filters-from-param-map';

@Component({
  selector: 'app-wallet-transactions',
  templateUrl: './wallet-transactions.component.html',
  styleUrls: ['./wallet-transactions.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletTransactionsComponent {
  private readonly _walletId: Wallet['id'] =
    this._activatedRoute.snapshot.params['id'];

  readonly ui = {
    transactionDate: {
      items: DATE_RANGE_FILTER_GROUPS,
      min: new TuiDay(1, 0, 1),
      max: new TuiDay(9999, 11, 31),
    } as const,
    transactionTypes: TRANSACTION_TYPES,
    transactionType: TransactionType,
    columns: [
      'transactionDate',
      'category',
      'labels',
      'note',
      'recipient',
      'amount',
      'actions',
    ] as const,
    sort: [
      { orderBy: 'transactionDate', direction: 'desc', label: 'Newest' },
      { orderBy: 'transactionDate', direction: 'asc', label: 'Oldest' },
      { orderBy: 'amount', direction: 'desc', label: 'Biggest amount' },
      { orderBy: 'amount', direction: 'asc', label: 'Smallest amount' },
    ] as SortSelectItem[],
    actions: {
      edit$: new Subject<Transaction['id']>(),
      delete$: new Subject<Transaction['id']>(),
      create$: new Subject<void>(),
      details$: new Subject<TransactionListItem>(),
    },
    deleteActionAt$: new Subject<Transaction['id'] | null>(),
  };

  readonly filters = this._fb.filters(
    {
      transactionDate: [DATE_RANGE_FILTER_GROUPS[5].range],
      transactionType: [''],
      sort: [this.ui.sort[0]],
      search: [''],
      categories: [new Array<Category['id']>()],
      recipients: [new Array<Recipient['id']>()],
      labels: [new Array<Label['id']>()],
    },
    {
      categories: 'categoryId',
      recipients: 'recipientId',
      labels: 'labelId',
      transactionType: 'transactionType_eq',
    }
  );

  readonly data = new ApiDataHandler(
    (pagination: ApiParams) =>
      this._transactionApiService.getAll({
        ...pagination,
        walletId_eq: this._walletId,
      }),
    this.filters
  );

  readonly dataApis = this._walletApiService.getConcreteWalletApi(
    this._activatedRoute.snapshot.params['id']
  );
  readonly wallet$ = this._walletApiService
    .get(this._walletId)
    .pipe(shareReplay());

  readonly dialogs$ = merge(
    this.ui.actions.edit$.pipe(
      switchMap(id =>
        this._dialog.open(
          new PolymorpheusComponent(UpdateTransactionComponent, this._injector),
          {
            label: 'Edit transaction',
            data: {
              id,
              walletId: this._walletId,
            },
          }
        )
      )
    ),
    this.ui.actions.create$.pipe(
      withLatestFrom(this.wallet$),
      switchMap(([, { id, currencyId }]) => {
        return this._dialog.open(
          new PolymorpheusComponent(CreateTransactionComponent, this._injector),
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
    ),
    this.ui.actions.delete$.pipe(
      tap(id => this.ui.deleteActionAt$.next(id)),
      switchMap(id =>
        this._transactionApiService.delete(id).pipe(
          filter(success => success),
          finalize(() => this.ui.deleteActionAt$.next(null))
        )
      )
    ),
    this.ui.actions.details$.pipe(
      withLatestFrom(this.wallet$),
      switchMap(([transaction, wallet]) =>
        this._dialog.open(
          new PolymorpheusComponent(
            TransactionDetailsComponent,
            this._injector
          ),
          {
            label: 'Transaction details',
            data: { transaction, wallet },
            dismissible: true,
          }
        )
      ),
      map(() => false)
    )
  ).pipe(tap(reset => reset !== false && this.data.resetPage()));

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _walletApiService: WalletApiService,
    private readonly _transactionApiService: TransactionApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {
    this.filters.form.patchValue(
      getFiltersFromParamMap(
        this.filters.form,
        this._activatedRoute.snapshot.queryParamMap
      )
    );
  }

  trackById = (index: number, item: TransactionListItem) => item.id;
}
