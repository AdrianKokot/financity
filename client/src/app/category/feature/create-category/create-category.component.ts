import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject, finalize, tap } from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Category } from '@shared/data-access/models/category.model';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import {
  TuiContextWithImplicit,
  tuiPure,
  TuiStringHandler,
} from '@taiga-ui/cdk';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateCategoryComponent {
  form = this._fb.nonNullable.group({
    name: ['', [Validators.required]],
    appearance: this._fb.nonNullable.group({
      iconName: [''],
      color: [''],
    }),
    walletId: ['', [Validators.required]],
    transactionType: [TransactionType.Income, [Validators.required]],
  });

  getTransactionTypeLabel = ({ label }: { label: string }) => label;
  getTransactionTypeId = ({ id }: { id: TransactionType }) => id;
  transactionTypes = [
    { id: TransactionType.Income, label: 'Income' },
    { id: TransactionType.Expense, label: 'Expense' },
  ];
  loading$ = new BehaviorSubject<boolean>(false);

  constructor(
    private _categoryService: CategoryApiService,
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<Category, { walletId: string }>
  ) {
    this.form.patchValue({
      walletId: this._context.data.walletId,
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

  @tuiPure
  stringify(
    items: typeof this.transactionTypes
  ): TuiStringHandler<TuiContextWithImplicit<TransactionType>> {
    const map = new Map(items.map(({ id, label }) => [id, label]));

    return ({ $implicit }: TuiContextWithImplicit<TransactionType>) =>
      map.get($implicit) || '';
  }
}
