import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject, finalize, Subject, take, takeUntil, tap } from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { RecipientApiService } from '../../../core/api/recipient-api.service';
import { Recipient } from '@shared/data-access/models/recipient.model';

@Component({
  selector: 'app-update-recipient',
  templateUrl: './update-recipient.component.html',
  styleUrls: ['./update-recipient.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateRecipientComponent implements OnDestroy {
  form = this._fb.nonNullable.group({
    id: ['', [Validators.required]],
    walletId: ['', [Validators.required]],
    name: ['', [Validators.required]],
    appearance: this._fb.group({
      iconName: <(string | null)[]>[null],
      color: <(string | null)[]>[null],
    }),
  });

  loading$ = new BehaviorSubject<boolean>(false);
  private _destroyed$ = new Subject<void>();

  constructor(
    private _recipientService: RecipientApiService,
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Recipient,
      { id: Recipient['id'] }
    >
  ) {
    this._recipientService
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

    this._recipientService
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
