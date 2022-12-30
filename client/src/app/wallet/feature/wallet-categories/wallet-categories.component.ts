import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { filter, merge, Subject, switchMap, tap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { TuiDialogService } from '@taiga-ui/core';
import { CategoryApiService } from '../../../core/api/category-api.service';
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
  };

  readonly filters = this._fb.filters(
    {
      search: [''],
      transactionType: ['' as TransactionType | ''],
    },
    {
      transactionType: 'transactionType_eq',
    }
  );

  readonly data = new ApiDataHandler(
    this._categoryService.getList.bind(this._categoryService, this._walletId),
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
      switchMap(id =>
        this._categoryService.delete(id).pipe(filter(success => success))
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
