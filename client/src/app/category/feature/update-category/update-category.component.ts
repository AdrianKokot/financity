import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { CategoryApiService } from '../../../core/api/category-api.service';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Category } from '@shared/data-access/models/category.model';
import { exhaustMap, filter, map, startWith, Subject, tap } from 'rxjs';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import {
  getRandomAppearanceColor,
  getRandomAppearanceIcon,
} from '@shared/ui/appearance';

@Component({
  selector: 'app-update-category',
  templateUrl: './update-category.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateCategoryComponent {
  readonly form = this._fb.nonNullable.group({
    id: ['', [Validators.required]],
    walletId: ['', [Validators.required]],
    transactionType: [TransactionType.Income, [Validators.required]],
    name: ['', [Validators.required]],
    appearance: this._fb.group({
      iconName: [getRandomAppearanceIcon(), [Validators.required]],
      color: [getRandomAppearanceColor(), [Validators.required]],
    }),
  });

  readonly initialDataLoading$ = this._categoryService
    .get(this._context.data.id)
    .pipe(
      tap(data => this.form.patchValue(data)),
      map(() => false),
      startWith(true)
    );

  readonly submit$ = new Subject<void>();
  readonly submitLoading$ = this.submit$.pipe(
    tap(() => this.form.markAllAsTouched()),
    filter(() => this.form.valid),
    exhaustMap(() =>
      this._categoryService.update(this.form.getRawValue()).pipe(
        tap(result => this._context.completeWith(result)),
        startWith(null)
      )
    ),
    map(x => x === null),
    startWith(false)
  );

  constructor(
    private _categoryService: CategoryApiService,
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Category,
      { id: Category['id'] }
    >
  ) {}

  cancel(): void {
    this._context.$implicit.complete();
  }
}
