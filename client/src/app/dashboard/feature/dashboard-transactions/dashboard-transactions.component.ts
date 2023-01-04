import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { merge, Subject, switchMap } from 'rxjs';
import { TransactionListItem } from '@shared/data-access/models/transaction.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { FormControl } from '@angular/forms';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { TransactionDetailsComponent } from '../../../transaction/feature/transaction-details/transaction-details.component';
import { TuiDialogService } from '@taiga-ui/core';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { TransactionApiService } from '../../../core/api/transaction-api.service';

@Component({
  selector: 'app-dashboard-transactions',
  templateUrl: './dashboard-transactions.component.html',
  styleUrls: ['./dashboard-transactions.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardTransactionsComponent {
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
    actions: {
      details$: new Subject<TransactionListItem>(),
    },
  };

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _transactionApiService: TransactionApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  readonly filters = this._fb.filters({
    search: [''],
  });

  readonly data = new ApiDataHandler<
    TransactionListItem,
    { search: FormControl<string> },
    'search'
  >(
    this._transactionApiService.getAll.bind(this._transactionApiService),
    this.filters
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
  trackById = (index: number, item: TransactionListItem) => item.id;
}
