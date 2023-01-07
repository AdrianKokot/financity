import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Category } from '@shared/data-access/models/category.model';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import {
  getRandomAppearanceColor,
  getRandomAppearanceIcon,
} from '@shared/ui/appearance';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { CategoryApiService } from '@shared/data-access/api/category-api.service';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';

@Component({
  selector: 'app-update-category',
  templateUrl: './update-category.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateCategoryComponent {
  readonly form = this._fb.form(
    {
      id: ['', [Validators.required]],
      walletId: ['', [Validators.required]],
      transactionType: [TransactionType.Income, [Validators.required]],
      name: ['', [Validators.required, Validators.maxLength(64)]],
      appearance: this._fb.group({
        iconName: [
          getRandomAppearanceIcon(),
          [Validators.required, Validators.maxLength(64)],
        ],
        color: [
          getRandomAppearanceColor(),
          [Validators.required, Validators.maxLength(64)],
        ],
      }),
    },
    {
      submit: payload => this._dataService.update(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly initialDataLoading$ = this._dataService
    .get(this._context.data.id)
    .pipe(toLoadingState(data => this.form.patchValue(data)));

  constructor(
    private readonly _dataService: CategoryApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Pick<Category, 'id'>,
      Pick<Category, 'id'>
    >
  ) {}

  cancel() {
    this._context.$implicit.complete();
  }
}
