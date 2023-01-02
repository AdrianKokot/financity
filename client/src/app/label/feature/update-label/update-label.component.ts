import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { Label } from '@shared/data-access/models/label';
import { LabelApiService } from '../../../core/api/label-api.service';
import { getRandomAppearanceColor } from '@shared/ui/appearance';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';

@Component({
  selector: 'app-update-label',
  templateUrl: './update-label.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateLabelComponent {
  readonly form = this._fb.form(
    {
      id: ['', [Validators.required]],
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
      submit: payload => this._dataService.update(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly initialDataLoading$ = this._dataService
    .get(this._context.data.id)
    .pipe(toLoadingState(data => this.form.patchValue(data)));

  constructor(
    private _dataService: LabelApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Pick<Label, 'id'>,
      Pick<Label, 'id'>
    >
  ) {}

  cancel() {
    this._context.$implicit.complete();
  }
}
