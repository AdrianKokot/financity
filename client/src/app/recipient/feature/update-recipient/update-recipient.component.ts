import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { RecipientApiService } from '../../../core/api/recipient-api.service';
import { Recipient } from '@shared/data-access/models/recipient.model';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';

@Component({
  selector: 'app-update-recipient',
  templateUrl: './update-recipient.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateRecipientComponent {
  readonly form = this._fb.form(
    {
      id: ['', [Validators.required]],
      name: ['', [Validators.required, Validators.maxLength(64)]],
      walletId: ['', [Validators.required]],
    },
    {
      submit: payload => this._dataService.update(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly initialDataLoading$ = this._dataService
    .get(this._context.data.id)
    .pipe(toLoadingState(data => this.form.patchValue(data)));

  constructor(
    private _dataService: RecipientApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Recipient,
      { id: Recipient['id'] }
    >
  ) {}

  cancel() {
    this._context.$implicit.complete();
  }
}
