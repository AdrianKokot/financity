import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject, finalize, tap } from 'rxjs';
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

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateCategoryComponent {
  form = this._fb.nonNullable.group({
    name: ['', [Validators.required]],
    appearance: this._fb.group({
      iconName: [getRandomAppearanceIcon(), [Validators.required]],
      color: [getRandomAppearanceColor(), [Validators.required]],
    }),
    walletId: ['', [Validators.required]],
    transactionType: [TransactionType.Income, [Validators.required]],
  });

  transactionTypes = TRANSACTION_TYPES;
  loading$ = new BehaviorSubject<boolean>(false);

  constructor(
    private _categoryService: CategoryApiService,
    private readonly _fb: FormBuilder,
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

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const payload = this.form.getRawValue();

    this._categoryService
      .create(payload)
      .pipe(
        tap(() => {
          this.loading$.next(true);
        }),
        finalize(() => {
          this.loading$.next(false);
        })
      )
      .subscribe(cat => {
        this._context.completeWith(cat);
      });
  }
}
