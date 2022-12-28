import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { RecipientApiService } from '../../../core/api/recipient-api.service';
import { Recipient } from '@shared/data-access/models/recipient.model';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';

@Component({
  selector: 'app-create-recipient',
  templateUrl: './create-recipient.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateRecipientComponent {
  readonly form = this._fb.form(
    {
      name: ['', [Validators.required, Validators.maxLength(64)]],
      walletId: ['', [Validators.required]],
    },
    {
      submit: payload => this._dataService.create(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  constructor(
    private _dataService: RecipientApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<Recipient, { walletId: string }>
  ) {
    this.form.patchValue({
      walletId: this._context.data.walletId,
    });
  }

  cancel() {
    this._context.$implicit.complete();
  }
}
