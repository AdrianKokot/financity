import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { WalletApiService } from '@shared/data-access/api/wallet-api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { AuthService } from '../../../auth/data-access/api/auth.service';
import {
  BehaviorSubject,
  filter,
  finalize,
  merge,
  Subject,
  switchMap,
  tap,
} from 'rxjs';

@Component({
  selector: 'app-wallet-settings',
  templateUrl: './wallet-settings.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletSettingsComponent {
  readonly form = this._fb.form(
    {
      id: [''],
      name: ['', [Validators.required, Validators.maxLength(64)]],
      startingAmount: [0, [Validators.required]],
      currencyId: [''],
      ownerId: [''],
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

  readonly initialDataLoading$ = this._dataService
    .get(this._activatedRoute.snapshot.params['id'])
    .pipe(toLoadingState(data => this.form.patchValue(data)));

  readonly user = this._user.user;

  readonly ui = {
    actions: {
      delete$: new Subject<void>(),
      resign$: new Subject<void>(),
    },
    deleteButtonLoading$: new BehaviorSubject<boolean>(false),
  };

  readonly dialogs$ = merge(
    this.ui.actions.delete$.pipe(
      tap(() => this.ui.deleteButtonLoading$.next(true)),
      switchMap(() =>
        this._dataService.delete(this.form.controls.id.value).pipe(
          filter(success => success),
          finalize(() => this.ui.deleteButtonLoading$.next(false))
        )
      )
    ),
    this.ui.actions.resign$.pipe(
      tap(() => this.ui.deleteButtonLoading$.next(true)),
      switchMap(() =>
        this._dataService.resign(this.form.controls.id.value).pipe(
          filter(success => success),
          finalize(() => this.ui.deleteButtonLoading$.next(false))
        )
      )
    )
  ).pipe(
    tap(reset => reset !== false && this._router.navigateByUrl('/wallets'))
  );

  constructor(
    private readonly _user: AuthService,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _router: Router,
    private readonly _dataService: WalletApiService,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService
  ) {}
}
