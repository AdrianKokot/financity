import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import {
  filter,
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
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { TuiDialogService } from '@taiga-ui/core';
import { UpdateTransactionComponent } from '../../../transaction/feature/update-transaction/update-transaction.component';
import { CreateTransactionComponent } from 'src/app/transaction/feature/create-transaction/create-transaction.component';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import { Category } from '@shared/data-access/models/category.model';
import { Recipient } from '@shared/data-access/models/recipient.model';
import { Label } from '@shared/data-access/models/label';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { DATE_RANGE_FILTER_GROUPS } from '../../utils/date-range-filter-groups.constants';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { TuiDay } from '@taiga-ui/cdk';

@Component({
  selector: 'app-wallet-transactions',
  templateUrl: './wallet-transactions.component.html',
  styleUrls: ['./wallet-transactions.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletTransactionsComponent {
  private readonly _walletId: Wallet['id'] =
    this._activatedRoute.snapshot.params['id'];
  readonly minDate = new TuiDay(1, 0, 1);
  readonly maxDate = new TuiDay(9999, 11, 31);
  readonly columns = [
    'transactionDate',
    'category',
    'labels',
    'note',
    'recipient',
    'amount',
    'actions',
  ];

  readonly dayRangeItems = DATE_RANGE_FILTER_GROUPS;

  readonly filters = this._fb.filters(
    {
      transactionDate: [this.dayRangeItems[5].range],
      search: [''],
      categories: [new Array<Category['id']>()],
      recipients: [new Array<Recipient['id']>()],
      labels: [new Array<Label['id']>()],
    },
    {
      categories: 'categoryId',
      recipients: 'recipientId',
      labels: 'labelId',
    }
  );

  readonly data = new ApiDataHandler(
    this._transactionApiService.getList.bind(
      this._transactionApiService,
      this._walletId
    ),
    this.filters
  );

  readonly dataApis = this._walletApiService.getConcreteWalletApi(
    this._activatedRoute.snapshot.params['id']
  );
  wallet$ = this._walletApiService.get(this._walletId).pipe(shareReplay());

  readonly edit$ = new Subject<Transaction['id']>();
  readonly delete$ = new Subject<Transaction['id']>();
  readonly create$ = new Subject<void>();
  readonly dialogs$ = merge(
    this.edit$.pipe(
      switchMap(id =>
        this._dialog.open<Transaction>(
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
    this.create$.pipe(
      withLatestFrom(this.wallet$),
      switchMap(([, { id, currencyId }]) => {
        return this._dialog.open<Transaction>(
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
    this.delete$.pipe(
      switchMap(id =>
        this._transactionApiService.delete(id).pipe(filter(success => success))
      )
    )
  ).pipe(tap(() => this.data.resetPage()));

  constructor(
    private _fb: FormWithHandlerBuilder,
    private _activatedRoute: ActivatedRoute,
    private _walletApiService: WalletApiService,
    private _transactionApiService: TransactionApiService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {
    this.filters.form.patchValue(this._getFiltersFromQueryParams());
  }

  private _getFiltersFromQueryParams() {
    return (
      Object.keys(
        this.filters.form.controls
      ) as (keyof typeof this.filters.form.controls)[]
    ).reduce((obj, key) => {
      if (this._activatedRoute.snapshot.queryParamMap.has(key)) {
        const value = this._activatedRoute.snapshot.queryParamMap.get(key);

        if (this.filters.form.controls[key].value instanceof Array<string>) {
          obj[key] = [value];
        } else {
          obj[key] = value === 'null' ? null : value;
        }
      }

      return obj;
    }, {} as Record<string, unknown>);
  }

  trackByIdx = (index: number, item: TransactionListItem) => item.id;
}
