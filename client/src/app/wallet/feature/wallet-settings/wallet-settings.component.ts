import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import {
  BehaviorSubject,
  filter,
  finalize,
  map,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { ActivatedRoute } from '@angular/router';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';

@Component({
  selector: 'app-wallet-settings',
  templateUrl: './wallet-settings.component.html',
  styleUrls: ['./wallet-settings.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletSettingsComponent implements OnDestroy {
  form = this._fb.nonNullable.group({
    id: [''],
    name: ['', [Validators.required]],
    startingAmount: [0, [Validators.required]],
  });

  wallet$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id),
    switchMap(walletId => this._walletService.get(walletId)),
    tap(wallet => {
      this.form.patchValue(wallet);
    })
  );

  constructor(
    private _fb: FormBuilder,
    private _walletService: WalletApiService,
    private _activatedRoute: ActivatedRoute,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService
  ) {}

  destroyed$ = new Subject<void>();

  loading$ = new BehaviorSubject<boolean>(false);

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const payload = this.form.getRawValue();

    this._walletService
      .update(payload)
      .pipe(
        tap(() => {
          this.loading$.next(true);
        }),
        finalize(() => {
          this.loading$.next(false);
        }),
        switchMap(() =>
          this._alertService.open('Wallet saved successfully', {
            label: 'Success!',
            status: TuiNotification.Success,
          })
        )
      )
      .subscribe();
  }
}
