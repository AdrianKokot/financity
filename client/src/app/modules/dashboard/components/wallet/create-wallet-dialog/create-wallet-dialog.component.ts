import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  ViewEncapsulation,
} from '@angular/core';
import { CreateWalletPayload } from '../../../../../core/models/wallet.model';
import { ModelFormBuilder } from '../../../../../core/utility/services/model-form.builder';
import { Validators } from '@angular/forms';
import { TuiDialogContext } from '@taiga-ui/core';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TUI_VALIDATION_ERRORS } from '@taiga-ui/kit';
import { CurrencyApiService } from '../../../../../core/api/currency-api.service';
import { CurrencyListItem } from '../../../../../core/models/currency.model';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-create-wallet-dialog',
  templateUrl: './create-wallet-dialog.component.html',
  styleUrls: ['./create-wallet-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: TUI_VALIDATION_ERRORS,
      useValue: {
        required: 'This field is required',
      },
    },
  ],
})
export class CreateWalletDialogComponent implements OnDestroy {
  form = this._fb.from<CreateWalletPayload>({
    name: ['', [Validators.required]],
    accountId: [null, [Validators.required]],
    currencyId: [null, [Validators.required]],
    startingBalance: [0],
  });

  destroyed$ = new Subject<void>();

  currency = this._fb.control<null | CurrencyListItem>(null);

  currencies$ = this._currencyService.getList();
  getCurrencyName = (item: CurrencyListItem) => `${item.name} [${item.code}]`;
  getCurrencyId = (item: CurrencyListItem) => item.id;
  currencyMatcher = (a: CurrencyListItem, b: CurrencyListItem) => a.id === b.id;

  constructor(
    private readonly _fb: ModelFormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<boolean>,
    private readonly _currencyService: CurrencyApiService
  ) {
    this.currency.valueChanges.pipe(takeUntil(this.destroyed$)).subscribe(v => {
      this.form.controls.currencyId.setValue(v?.id ?? null);
    });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
