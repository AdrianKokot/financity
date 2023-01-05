import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { LabelApiService } from '../../../core/api/label-api.service';
import { Label } from '@shared/data-access/models/label';
import { getRandomAppearanceColor } from '@shared/ui/appearance';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';

@Component({
  selector: 'app-create-label',
  templateUrl: './create-label.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateLabelComponent {
  readonly form = this._fb.form(
    {
      name: ['', [Validators.required, Validators.maxLength(64)]],
      appearance: this._fb.group({
        color: [
          getRandomAppearanceColor(),
          [Validators.required, Validators.maxLength(64)],
        ],
      }),
      walletId: ['', [Validators.required]],
    },
    {
      submit: payload => this._dataService.create(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  constructor(
    private _dataService: LabelApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Label,
      Pick<Label, 'walletId'> & { name?: string }
    >
  ) {
    this.form.patchValue(this._context.data);
  }

  cancel() {
    this._context.$implicit.complete();
  }
}
