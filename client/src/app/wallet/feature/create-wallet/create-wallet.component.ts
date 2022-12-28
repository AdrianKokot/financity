import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { CurrencyApiService } from '../../../core/api/currency-api.service';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { Wallet } from '@shared/data-access/models/wallet.model';

@Component({
  selector: 'app-create-wallet',
  templateUrl: './create-wallet.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateWalletComponent {
  readonly form = this._fb.form(
    {
      name: ['', [Validators.required, Validators.maxLength(64)]],
      currencyId: ['', [Validators.required]],
      startingAmount: [0, [Validators.required]],
    },
    {
      submit: payload => this._dataService.create(payload),
      effect: item => this._context.completeWith(item),
    }
  );

  readonly dataApis = {
    getCurrencies: this._currencyService.getList.bind(this._currencyService),
    getCurrencyName: (item: CurrencyListItem) => item.id,
  };

  constructor(
    private readonly _dataService: WalletApiService,
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _currencyService: CurrencyApiService,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<Wallet>
  ) {}

  cancel() {
    this._context.$implicit.complete();
  }
}
