import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { filter, map, merge, Subject, switchMap, tap } from 'rxjs';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { TuiDialogService } from '@taiga-ui/core';
import { UserSettingsService } from '../../../user-settings/data-access/services/user-settings.service';
import { BudgetApiService } from '../../../core/api/budget-api.service';
import { Budget } from '@shared/data-access/models/budget.model';
import { CreateBudgetComponent } from '../create-budget/create-budget.component';
import { UpdateBudgetComponent } from '../update-budget/update-budget.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-budgets-shell',
  templateUrl: './budgets-shell.component.html',
  styleUrls: ['./budgets-shell.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BudgetsShellComponent {
  readonly ui = {
    columns: ['name', 'state', 'actions'] as const,
    actions: {
      create$: new Subject<void>(),
      edit$: new Subject<Budget['id']>(),
      delete$: new Subject<Budget['id']>(),
    },
    showSimplifiedView$: this._user.settings$.pipe(
      map(x => x.showSimplifiedBudgetView)
    ),
  };

  readonly filters = this._fb.filters({
    search: [''],
  });

  readonly data = new ApiDataHandler(
    this._budgetService.getAll.bind(this._budgetService),
    this.filters
  );

  readonly dialogs$ = merge(
    merge(
      this._activatedRoute.queryParamMap.pipe(
        filter(x => x.get('action') === 'create')
      ),
      this.ui.actions.create$
    ).pipe(
      switchMap(() =>
        this._dialog.open(new PolymorpheusComponent(CreateBudgetComponent), {
          label: 'Create budget',
        })
      )
    ),
    this.ui.actions.edit$.pipe(
      switchMap(id =>
        this._dialog.open(
          new PolymorpheusComponent(UpdateBudgetComponent, this._injector),
          {
            label: 'Edit budget',
            data: {
              id,
            },
          }
        )
      )
    ),
    this.ui.actions.delete$.pipe(
      switchMap(id =>
        this._budgetService.delete(id).pipe(filter(success => success))
      )
    )
  ).pipe(tap(() => this.data.resetPage()));

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _budgetService: BudgetApiService,
    @Inject(Injector)
    private readonly _injector: Injector,
    @Inject(TuiDialogService)
    private readonly _dialog: TuiDialogService,
    private readonly _user: UserSettingsService,
    private readonly _activatedRoute: ActivatedRoute
  ) {}

  trackById = (index: number, item: { id: Budget['id'] }) => item.id;
}
