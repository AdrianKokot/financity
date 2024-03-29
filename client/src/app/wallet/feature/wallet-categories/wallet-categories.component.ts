import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { filter, finalize, merge, Subject, switchMap, tap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { TuiDialogService } from '@taiga-ui/core';
import { CategoryApiService } from '@shared/data-access/api/category-api.service';
import { Category } from '@shared/data-access/models/category.model';
import { CreateCategoryComponent } from '../../../category/feature/create-category/create-category.component';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { UpdateCategoryComponent } from 'src/app/category/feature/update-category/update-category.component';
import {
  TRANSACTION_TYPES,
  TransactionType,
} from '@shared/data-access/models/transaction-type.enum';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { ApiParams } from '@shared/data-access/api/generic-api.service';
import { DEFAULT_APP_SORT_SELECT_ITEMS } from '@shared/ui/tui/sort-select/sort-select.component';

@Component({
  selector: 'app-wallet-categories',
  templateUrl: './wallet-categories.component.html',
  styleUrls: ['./wallet-categories.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletCategoriesComponent {
  private readonly _walletId: Wallet['id'] =
    this._activatedRoute.snapshot.params['id'];

  readonly ui = {
    transactionTypes: TRANSACTION_TYPES,
    columns: ['category', 'transactionType', 'actions'] as const,
    actions: {
      edit$: new Subject<Category['id']>(),
      delete$: new Subject<Category['id']>(),
      create$: new Subject<void>(),
    },
    deleteActionAt$: new Subject<Category['id'] | null>(),
  };

  readonly filters = this._fb.filters(
    {
      search: [''],
      sort: [DEFAULT_APP_SORT_SELECT_ITEMS[0]],
      transactionType: ['' as TransactionType | ''],
    },
    {
      transactionType: 'transactionType_eq',
    }
  );

  readonly data = new ApiDataHandler(
    (p: ApiParams) =>
      this._categoryService.getAll({ ...p, walletId_eq: this._walletId }),
    this.filters
  );

  readonly dialogs$ = merge(
    this.ui.actions.edit$.pipe(
      switchMap(id =>
        this._dialog.open(
          new PolymorpheusComponent(UpdateCategoryComponent, this._injector),
          {
            label: 'Edit category',
            data: {
              id,
            },
          }
        )
      )
    ),
    this.ui.actions.create$.pipe(
      switchMap(() =>
        this._dialog.open(
          new PolymorpheusComponent(CreateCategoryComponent, this._injector),
          {
            label: 'Create category',
            data: {
              walletId: this._walletId,
              transactionType:
                this.filters.controls.transactionType.value ===
                TransactionType.Income
                  ? TransactionType.Income
                  : TransactionType.Expense,
            },
          }
        )
      )
    ),
    this.ui.actions.delete$.pipe(
      tap(id => this.ui.deleteActionAt$.next(id)),
      switchMap(id =>
        this._categoryService.delete(id).pipe(
          filter(success => success),
          finalize(() => this.ui.deleteActionAt$.next(null))
        )
      )
    )
  ).pipe(tap(() => this.data.resetPage()));

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _categoryService: CategoryApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  trackById = (index: number, item: { id: Category['id'] }) => item.id;
}
