import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject, finalize, tap } from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { LabelApiService } from '../../../core/api/label-api.service';
import { Label } from '@shared/data-access/models/label';
import { getRandomAppearanceColor } from '@shared/ui/appearance';

@Component({
  selector: 'app-create-label',
  templateUrl: './create-label.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateLabelComponent {
  form = this._fb.nonNullable.group({
    name: ['', [Validators.required]],
    appearance: this._fb.group({
      color: [getRandomAppearanceColor(), [Validators.required]],
    }),
    walletId: ['', [Validators.required]],
  });

  loading$ = new BehaviorSubject<boolean>(false);

  constructor(
    private _labelService: LabelApiService,
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<Label, { walletId: string }>
  ) {
    this.form.patchValue({
      walletId: this._context.data.walletId,
    });
  }

  submit(): void {
    if (!this.form.valid) {
      return this.form.markAllAsTouched();
    }

    this._labelService
      .create(this.form.getRawValue())
      .pipe(
        tap(() => {
          this.loading$.next(true);
        }),
        finalize(() => {
          this.loading$.next(false);
        })
      )
      .subscribe(item => {
        this._context.completeWith(item);
      });
  }
}
