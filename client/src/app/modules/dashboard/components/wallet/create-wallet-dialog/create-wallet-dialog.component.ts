import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  ViewEncapsulation,
} from '@angular/core';
import {
  CreateWalletPayload,
  WalletListItem,
} from '@shared/data-access/models/wallet.model';
import { FormBuilder, Validators } from '@angular/forms';
import { TuiDialogContext } from '@taiga-ui/core';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { CurrencyApiService } from '../../../../../core/api/currency-api.service';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { BehaviorSubject, finalize, Subject, takeUntil, tap } from 'rxjs';
import { WalletApiService } from '../../../../../core/api/wallet-api.service';

@Component({
  selector: 'app-create-wallet-dialog',
  templateUrl: './create-wallet-dialog.component.html',
  styleUrls: ['./create-wallet-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateWalletDialogComponent implements OnDestroy {
  form = this._fb.group({
    name: ['', [Validators.required]],
    currencyId: [null as string | null, [Validators.required]],
    startingAmount: [0, [Validators.required]],
  });

  destroyed$ = new Subject<void>();

  currency = this._fb.control<null | CurrencyListItem>(null);

  loading$ = new BehaviorSubject<boolean>(false);
  currencies$ = this._currencyService.getList();
  getCurrencyName = (item: CurrencyListItem) => `${item.name} [${item.id}]`;

  constructor(
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<WalletListItem>,
    private readonly _currencyService: CurrencyApiService,
    private readonly _walletService: WalletApiService
  ) {
    this.currency.valueChanges.pipe(takeUntil(this.destroyed$)).subscribe(v => {
      this.form.controls.currencyId.setValue(v?.id ?? null);
    });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const payload = this.form.getRawValue() as NonNullable<CreateWalletPayload>;

    this._walletService
      .create(payload)
      .pipe(
        tap(() => {
          this.loading$.next(true);
        }),
        finalize(() => {
          this.loading$.next(false);
        })
      )
      .subscribe(() => {
        this._context.completeWith({
          name: payload.name,
          currentState: payload.startingAmount,
          startingAmount: payload.startingAmount,
          currencyId: this.currency.value?.code ?? '',
          currencyName: this.currency.value?.name ?? '',
          id: '',
        });
      });
  }
}
