import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { merge, Subject, switchMap } from 'rxjs';
import { TransactionListItem } from '@shared/data-access/models/transaction.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { TransactionDetailsComponent } from '../../../transaction/feature/transaction-details/transaction-details.component';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { ActivatedRoute } from '@angular/router';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { TuiDialogService } from '@taiga-ui/core';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';

@Component({
  selector: 'app-search-shell',
  templateUrl: './search-shell.component.html',
  styleUrls: ['./search-shell.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchShellComponent {
  readonly ui = {
    columns: [
      'transactionDate',
      'category',
      'labels',
      'note',
      'recipient',
      'amount',
      'actions',
    ] as const,
    getRemainingLabelsCount: (index: number, labelsCount: number) =>
      labelsCount - (index === 0 ? 1 : index + 2),
    actions: {
      details$: new Subject<TransactionListItem>(),
      applySearch$: new Subject<void>(),
    },
  };

  readonly filters = this._fb.filters(
    {
      search: [''],
    },
    {
      search: 'query',
    }
  );

  readonly searchWasApplied$ = this.ui.actions.applySearch$.pipe(
    toLoadingState()
  );

  readonly data = new ApiDataHandler(
    this._transactionApiService.getAll.bind(this._transactionApiService),
    this.filters,
    this.ui.actions.applySearch$
  );

  readonly dialogs$ = merge(
    this.ui.actions.details$.pipe(
      switchMap(transaction =>
        this._dialog.open(
          new PolymorpheusComponent(
            TransactionDetailsComponent,
            this._injector
          ),
          {
            label: 'Transaction details',
            data: { transaction },
            dismissible: true,
          }
        )
      )
    )
  );

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _walletApiService: WalletApiService,
    private readonly _transactionApiService: TransactionApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  trackById = (index: number, item: TransactionListItem) => item.id;
}
