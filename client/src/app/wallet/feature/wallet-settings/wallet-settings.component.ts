import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { ActivatedRoute } from '@angular/router';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { filter, map, switchMap } from 'rxjs';

@Component({
  selector: 'app-wallet-settings',
  templateUrl: './wallet-settings.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletSettingsComponent {
  readonly form = this._fb.form(
    {
      id: [''],
      name: ['', [Validators.required]],
      startingAmount: [0, [Validators.required]],
      currencyId: [''],
    },
    {
      submit: payload => this._dataService.update(payload),
      effect$: () =>
        this._alertService.open('Wallet saved successfully', {
          label: 'Success!',
          status: TuiNotification.Success,
        }),
    }
  );

  readonly initialDataLoading$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id),
    switchMap(walletId =>
      this._dataService
        .get(walletId)
        .pipe(toLoadingState(data => this.form.patchValue(data)))
    )
  );

  constructor(
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _dataService: WalletApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService
  ) {}
}
