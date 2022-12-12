import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject, finalize, tap } from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import { WalletApiService } from '../../../core/api/wallet-api.service';

@Component({
  selector: 'app-share-wallet',
  templateUrl: './share-wallet.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShareWalletComponent {
  form = this._fb.nonNullable.group({
    userEmail: ['', [Validators.required, Validators.email]],
    walletId: ['', [Validators.required]],
  });

  loading$ = new BehaviorSubject<boolean>(false);

  constructor(
    private _walletService: WalletApiService,
    private readonly _fb: FormBuilder,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<boolean, { walletId: string }>
  ) {
    this.form.patchValue({
      walletId: this._context.data.walletId,
    });
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const payload = this.form.getRawValue();

    this._walletService
      .share(payload)
      .pipe(
        tap(() => {
          this.loading$.next(true);
        }),
        finalize(() => {
          this.loading$.next(false);
        })
      )
      .subscribe(success => {
        this._context.completeWith(success);
      });
  }
}
