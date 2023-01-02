import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { map, merge, shareReplay, Subject, switchMap, tap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { DATE_RANGE_FILTER_GROUPS_MAP } from '../../../wallet/utils/date-range-filter-groups.constants';
import { TransactionListItem } from '@shared/data-access/models/transaction.model';
import { Category } from '@shared/data-access/models/category.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { TransactionDetailsComponent } from '../../../transaction/feature/transaction-details/transaction-details.component';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { TransactionApiService } from '../../../core/api/transaction-api.service';
import { TuiDialogService } from '@taiga-ui/core';
import { Budget } from '@shared/data-access/models/budget.model';
import { BudgetApiService } from '../../../core/api/budget-api.service';

@Component({
  selector: 'app-budget-shell',
  templateUrl: './budget-shell.component.html',
  styleUrls: ['./budget-shell.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BudgetShellComponent {
  private readonly _budgetId: Budget['id'] =
    this._activatedRoute.snapshot.params['id'];

  readonly budget$ = this._budgetService.get(this._budgetId).pipe(
    tap(budget => {
      this.filters.form.patchValue({
        categories: budget.trackedCategories.map(x => x.id),
        currencyId: budget.currencyId,
      });
    }),
    shareReplay(1)
  );

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
    },
  };

  readonly filters = this._fb.filters(
    {
      transactionDate: [DATE_RANGE_FILTER_GROUPS_MAP['this month'].range],
      search: [''],
      categories: [new Array<Category['id']>()],
      currencyId: [''],
    },
    {
      categories: 'categoryId',
      currencyId: 'exchangedCurrencyId_eq',
    }
  );

  readonly data = new ApiDataHandler(
    this._transactionApiService.getAll.bind(this._transactionApiService),
    this.filters,
    this.budget$
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
      ),
      map(() => false)
    )
  ).pipe(tap(reset => reset !== false && this.data.resetPage()));

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _budgetService: BudgetApiService,
    private readonly _transactionApiService: TransactionApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  trackById = (index: number, item: TransactionListItem) => item.id;
}
