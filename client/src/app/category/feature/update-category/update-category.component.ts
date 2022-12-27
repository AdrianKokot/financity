import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import { BehaviorSubject, finalize, Subject, take, takeUntil, tap } from 'rxjs';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Category } from '@shared/data-access/models/category.model';
import {
  getRandomAppearanceColor,
  getRandomAppearanceIcon,
} from '@shared/ui/appearance';

@Component({
  selector: 'app-update-category',
  templateUrl: './update-category.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateCategoryComponent implements OnDestroy {
  form = this._fb.nonNullable.group({
    id: ['', [Validators.required]],
    walletId: ['', [Validators.required]],
    transactionType: [TransactionType.Income, [Validators.required]],
    name: ['', [Validators.required]],
    appearance: this._fb.group({
      iconName: [getRandomAppearanceIcon(), [Validators.required]],
      color: [getRandomAppearanceColor(), [Validators.required]],
    }),
  });

  loading$ = new BehaviorSubject<boolean>(false);
  private _destroyed$ = new Subject<void>();

  constructor(
    private _categoryService: CategoryApiService,
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Category,
      { id: Category['id'] }
    >
  ) {
    this._categoryService
      .get(this._context.data.id)
      .pipe(take(1), takeUntil(this._destroyed$))
      .subscribe(category => {
        this.form.patchValue(category);
      });
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const payload = this.form.getRawValue();

    this._categoryService
      .update(payload)
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

  ngOnDestroy(): void {
    this._destroyed$.next();
    this._destroyed$.complete();
  }
}
