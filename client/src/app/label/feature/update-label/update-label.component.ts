import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject, finalize, Subject, take, takeUntil, tap } from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Label } from '@shared/data-access/models/label';
import { LabelApiService } from '../../../core/api/label-api.service';

@Component({
  selector: 'app-update-label',
  templateUrl: './update-label.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateLabelComponent implements OnDestroy {
  form = this._fb.nonNullable.group({
    id: ['', [Validators.required]],
    name: ['', [Validators.required]],
    appearance: this._fb.group({
      color: ['', [Validators.required]],
    }),
    walletId: ['', [Validators.required]],
  });

  loading$ = new BehaviorSubject<boolean>(false);
  private _destroyed$ = new Subject<void>();

  constructor(
    private _labelService: LabelApiService,
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<Label, { id: Label['id'] }>
  ) {
    this._labelService
      .get(this._context.data.id)
      .pipe(take(1), takeUntil(this._destroyed$))
      .subscribe(item => {
        this.form.patchValue(item);
      });
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const payload = this.form.getRawValue();

    this._labelService
      .update(payload)
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

  ngOnDestroy(): void {
    this._destroyed$.next();
    this._destroyed$.complete();
  }
}
