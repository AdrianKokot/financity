import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { FormBuilder, Validators } from '@angular/forms';
import { exhaustMap, filter, map, startWith, Subject, tap } from 'rxjs';
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

  readonly submit$ = new Subject<void>();
  readonly submitLoading$ = this.submit$.pipe(
    tap(() => this.form.markAllAsTouched()),
    filter(() => this.form.valid),
    exhaustMap(() =>
      this._categoryService.create(this.form.getRawValue()).pipe(
        tap(result => this._context.completeWith(result)),
        startWith(null)
      )
    ),
    map(x => x === null),
    startWith(false)
  );

  transactionTypes = TRANSACTION_TYPES;

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

  cancel(): void {
    this._context.$implicit.complete();
  }
}
