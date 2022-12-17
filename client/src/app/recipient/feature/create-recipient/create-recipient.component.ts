import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject, finalize, tap } from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { RecipientApiService } from '../../../core/api/recipient-api.service';
import { Recipient } from '@shared/data-access/models/recipient.model';

@Component({
  selector: 'app-create-recipient',
  templateUrl: './create-recipient.component.html',
  styleUrls: ['./create-recipient.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateRecipientComponent {
  form = this._fb.nonNullable.group({
    name: ['', [Validators.required]],
    walletId: ['', [Validators.required]],
  });

  loading$ = new BehaviorSubject<boolean>(false);

  constructor(
    private _recipientService: RecipientApiService,
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<Recipient, { walletId: string }>
  ) {
    this.form.patchValue({
      walletId: this._context.data.walletId,
    });
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }

    this._recipientService
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
