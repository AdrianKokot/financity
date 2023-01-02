import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import {
  Category,
  CategoryListItem,
} from '@shared/data-access/models/category.model';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { BudgetApiService } from '../../../core/api/budget-api.service';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { Budget } from '@shared/data-access/models/budget.model';

@Component({
  selector: 'app-update-budget',
  templateUrl: './update-budget.component.html',

  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateBudgetComponent {
  readonly form = this._fb.form(
    {
      id: ['', [Validators.required]],
      name: ['', [Validators.required, Validators.maxLength(64)]],
      currencyId: ['', [Validators.required]],
      amount: [0, [Validators.required, Validators.min(0.01)]],
      trackedCategoriesId: [new Array<Category['id']>(), [Validators.required]],
    },
    {
      submit: payload => this._dataService.update(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly dataApis = {
    getCategories: this._categoryService.getAll.bind(this._categoryService),
    getCategoryName: (item: CategoryListItem) =>
      `${item.name} (${item.walletName})`,
  };

  readonly preloadedData = {
    categories: new Array<Category>(),
  };

  readonly initialDataLoading$ = this._dataService
    .get(this._context.data.id)
    .pipe(
      toLoadingState(data => {
        this.form.patchValue({
          ...data,
          trackedCategoriesId: data.trackedCategories.map(x => x.id),
        });

        this.preloadedData.categories = data.trackedCategories;
      })
    );

  constructor(
    private readonly _dataService: BudgetApiService,
    private readonly _categoryService: CategoryApiService,
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _currencyService: CurrencyApiService,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Pick<Budget, 'id'>,
      Pick<Budget, 'id'>
    >
  ) {}

  cancel() {
    this._context.$implicit.complete();
  }
}
