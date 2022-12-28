import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { Validators } from '@angular/forms';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Category } from '@shared/data-access/models/category.model';
import {
  TRANSACTION_TYPES,
  TransactionType,
} from '@shared/data-access/models/transaction-type.enum';
import {
  getRandomAppearanceColor,
  getRandomAppearanceIcon,
} from '@shared/ui/appearance';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateCategoryComponent {
  readonly transactionTypes = TRANSACTION_TYPES;

  readonly form = this._fb.form(
    {
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
      walletId: ['', [Validators.required]],
      transactionType: [TransactionType.Income, [Validators.required]],
    },
    {
      submit: payload => this._dataService.create(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  constructor(
    private readonly _dataService: CategoryApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Category,
      { walletId: string; transactionType: TransactionType }
    >
  ) {
    this.form.patchValue({
      walletId: this._context.data.walletId,
      transactionType: this._context.data.transactionType,
    });
  }

  cancel() {
    this._context.$implicit.complete();
  }
}
