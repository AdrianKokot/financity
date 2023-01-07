import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { WalletApiService } from '@shared/data-access/api/wallet-api.service';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';

@Component({
  selector: 'app-share-wallet',
  templateUrl: './share-wallet.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShareWalletComponent {
  readonly form = this._fb.form(
    {
      userEmail: ['', [Validators.required, Validators.email]],
      walletId: ['', [Validators.required]],
    },
    {
      submit: payload => this._dataService.share(payload),
      effect: success => this._context.completeWith(success),
    }
  );

  constructor(
    private _dataService: WalletApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<boolean, { walletId: string }>
  ) {
    this.form.patchValue({
      walletId: this._context.data.walletId,
    });
  }

  cancel() {
    this._context.$implicit.complete();
  }
}
