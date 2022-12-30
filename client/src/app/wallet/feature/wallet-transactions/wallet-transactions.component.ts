import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import {
  BehaviorSubject,
  distinctUntilChanged,
  filter,
  map,
  merge,
  NEVER,
  scan,
  share,
  shareReplay,
  startWith,
  switchMap,
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
import { TuiAlertService, TuiDialogService } from '@taiga-ui/core';
import { UpdateTransactionComponent } from '../../../transaction/feature/update-transaction/update-transaction.component';
import { CreateTransactionComponent } from 'src/app/transaction/feature/create-transaction/create-transaction.component';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';
import { Category } from '@shared/data-access/models/category.model';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { Recipient } from '@shared/data-access/models/recipient.model';
import { Label } from '@shared/data-access/models/label';
import { AUTOCOMPLETE_PAGE_SIZE } from '@shared/data-access/constants/pagination.contants';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { DATE_RANGE_FILTER_GROUPS } from '../../utils/date-range-filter-groups.constants';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';

@Component({
  selector: 'app-wallet-transactions',
  templateUrl: './wallet-transactions.component.html',
  styleUrls: ['./wallet-transactions.component.scss'],
  // encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletTransactionsComponent {
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

  readonly page$ = new BehaviorSubject<number>(1);
  private readonly _walletId: Wallet['id'] =
    this._activatedRoute.snapshot.params['id'];
  private readonly _api$ = merge(
    this.page$,
    this.filters.filters$.pipe(
      map(() => {
        this.page$.next(1);
        return NEVER;
      })
    )
  ).pipe(
    withLatestFrom(this.filters.filters$, this.page$),
    map(([, filters, page]) => ({
      page,
      filters,
      pageSize: AUTOCOMPLETE_PAGE_SIZE,
    })),
    distinctUntilChangedObject(),
    switchMap(pagination =>
      this._transactionApiService
        .getList(this._walletId, pagination)
        .pipe(startWith(null))
    ),
    shareReplay()
  );

  readonly apiLoading$ = this._api$.pipe(
    map(x => x === null),
    startWith(true),
    share()
  );

  readonly dataApis = this._walletApiService.getConcreteWalletApi(
    this._activatedRoute.snapshot.params['id']
  );

  readonly items$ = merge(
    this._api$.pipe(
      filter((x): x is TransactionListItem[] => x !== null),
      scan((acc: TransactionListItem[], data: TransactionListItem[]) => {
        if (this.page$.value === 1) {
          return [...data];
        }
        return [...acc, ...data];
      }, [] as TransactionListItem[])
    ),
    this._api$.pipe(
      filter((x): x is null => x === null && this.page$.value === 1)
    )
  ).pipe(distinctUntilChanged());

  wallet$ = this._walletApiService.get(this._walletId).pipe(shareReplay());

  openCreateDialog(): void {
    this.wallet$
      .pipe(
        switchMap(({ id, currencyId }) => {
          return this._dialog.open<Transaction>(
            new PolymorpheusComponent(
              CreateTransactionComponent,
              this._injector
            ),
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
      )
      .subscribe(() => {
        this.page$.next(0);
        this.page$.next(1);
      });
  }

  openEditDialog(id: Transaction['id']): void {
    this._dialog
      .open<Transaction>(
        new PolymorpheusComponent(UpdateTransactionComponent, this._injector),
        {
          label: 'Edit transaction',
          data: {
            id,
            walletId: this._walletId,
          },
        }
      )
      .subscribe(() => {
        this.page$.next(0);
        this.page$.next(1);
      });
  }

  deleteTransaction(id: Transaction['id']): void {
    this._transactionApiService
      .delete(id)
      .pipe(filter(success => success))
      .subscribe(() => {
        this.page$.next(0);
        this.page$.next(1);
      });
  }

  constructor(
    private _fb: FormWithHandlerBuilder,
    private _activatedRoute: ActivatedRoute,
    private _walletApiService: WalletApiService,
    private _categoryService: CategoryApiService,
    private _transactionApiService: TransactionApiService,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {
    this.filters.form.patchValue(this._activatedRoute.snapshot.queryParams);
  }

  trackByIdx = (index: number) => index;
}
