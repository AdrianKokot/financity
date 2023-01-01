import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import {
  Category,
  CategoryListItem,
} from '@shared/data-access/models/category.model';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { BudgetApiService } from '../../../core/api/budget-api.service';
import { Budget } from '@shared/data-access/models/budget.model';
import { distinctUntilChanged, tap } from 'rxjs';

@Component({
  selector: 'app-create-budget',
  templateUrl: './create-budget.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateBudgetComponent {
  readonly form = this._fb.form(
    {
      name: ['', [Validators.required, Validators.maxLength(64)]],
      currencyId: ['', [Validators.required]],
      amount: [0, [Validators.required, Validators.min(0.01)]],
      trackedCategoriesId: [new Array<Category['id']>(), [Validators.required]],
    },
    {
      submit: payload => this._dataService.create(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly formEffects$ = this.form.controls.currencyId.valueChanges.pipe(
    distinctUntilChanged(),
    tap(() => this.form.controls.trackedCategoriesId.reset())
  );

  readonly dataApis = {
    getCategories: this._categoryService.getAllList.bind(this._categoryService),
    getCurrencies: this._currencyService.getList.bind(this._currencyService),
    getCurrencyName: (item: CurrencyListItem) => item.id,
    getCategoryName: (item: CategoryListItem) =>
      `${item.name} (${item.walletName})`,
  };

  constructor(
    private readonly _dataService: BudgetApiService,
    private readonly _categoryService: CategoryApiService,
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _currencyService: CurrencyApiService,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<Budget>
  ) {}

  cancel() {
    this._context.$implicit.complete();
  }
}
