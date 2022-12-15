import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
  ViewChild,
} from '@angular/core';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import {
  BehaviorSubject,
  debounceTime,
  distinctUntilChanged,
  exhaustMap,
  filter,
  map,
  scan,
  share,
  shareReplay,
  startWith,
  Subject,
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

@Component({
  selector: 'app-wallet-transactions',
  templateUrl: './wallet-transactions.component.html',
  styleUrls: ['./wallet-transactions.component.scss'],
  // encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletTransactionsComponent {
  @ViewChild(CdkVirtualScrollViewport) viewport!: CdkVirtualScrollViewport;

  walletId$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id)
  );

  wallet$ = this.walletId$.pipe(
    switchMap(walletId => this._walletApiService.get(walletId)),
    shareReplay(1)
  );

  page$ = new BehaviorSubject<number>(1);
  private _pageSize = 250;

  transactions$ = this.page$.pipe(
    distinctUntilChanged(),
    withLatestFrom(this.walletId$),
    exhaustMap(([page, walletId]) =>
      this._transactionApiService
        .getList(walletId, {
          page,
          pageSize: this._pageSize,
        })
        .pipe(startWith(null))
    ),
    shareReplay(1)
  );
  gotAllResults = false;

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
      .subscribe(item => {
        this._newTransaction$.next(item);
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
          },
        }
      )
      .subscribe(item => this._modifiedTransaction$.next(item));
  }

  deleteTransaction(id: Transaction['id']): void {
    this._transactionApiService
      .delete(id)
      .pipe(filter(success => success))
      .subscribe(() => {
        this._deletedTransaction$.next({ id });
      });
  }

  private _modifiedTransaction$ = new Subject<Transaction>();
  private _deletedTransaction$ = new Subject<Pick<Transaction, 'id'>>();
  private _newTransaction$ = new Subject<Transaction>();

  readonly request$ = this.transactions$
    //   combineLatest([
    //   this.sorter$,
    //   this.direction$,
    //   this.page$,
    //   this.size$,
    //   tuiControlValue<number>(this.minAge),
    // ])
    .pipe(
      filter((x): x is TransactionListItem[] => x !== null),
      // zero time debounce for a case when both key and direction change
      debounceTime(0),
      scan((acc, val) => {
        return [...acc, ...val];
      }, [] as TransactionListItem[]),
      // switchMap(query => this.getData(...query).pipe(startWith(null))),
      share()
    );

  readonly loading$ = this.transactions$.pipe(map(value => !value));
  readonly data$ = this.request$.pipe(startWith([] as TransactionListItem[]));

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _walletApiService: WalletApiService,
    private _transactionApiService: TransactionApiService,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {}

  log() {
    if (this.gotAllResults) return;
    const { end } = this.viewport.getRenderedRange();
    const total = this.viewport.getDataLength();

    if (end === total) {
      this.page$.next(Math.floor(total / this._pageSize) + 1);
    }
  }

  trackByIdx = (index: number) => index;
}
