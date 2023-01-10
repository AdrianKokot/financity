import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';
import { toLoadingState } from '@shared/utils/rxjs/to-loading-state';
import { AuthService } from '../../../auth/data-access/api/auth.service';
import { WalletApiService } from '@shared/data-access/api/wallet-api.service';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { CustomValidators } from '@shared/utils/form/custom-validators';
import { filter, of, tap } from 'rxjs';
import { Router } from '@angular/router';
import { User } from '../../../auth/data-access/models/user';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountSettingsComponent {
  readonly userForm = this._fb.form(
    {
      name: ['', [Validators.required, Validators.maxLength(128)]],
    },
    {
      submit: payload => this._authService.updateUser(payload),
      effect$: () =>
        this._alertService.open('Account settings saved successfully', {
          label: 'Success!',
          status: TuiNotification.Success,
        }),
    }
  );

  readonly passwordChangeForm = this._fb.form(
    {
      password: ['', [Validators.required]],
      newPassword: ['', [Validators.required, CustomValidators.password()]],
    },
    {
      submit: payload => this._authService.changePassword(payload),
      effect$: () =>
        of(null).pipe(
          tap(() =>
            this._alertService
              .open('Password changed saved successfully', {
                label: 'Success!',
                status: TuiNotification.Success,
              })
              .subscribe()
          ),
          tap(() => this._router.navigateByUrl('/auth/logout'))
        ),
    }
  );

  readonly initialDataLoading$ = this._authService.user$.pipe(
    filter((x): x is User => x !== null),
    toLoadingState(data => this.userForm.patchValue(data))
  );

  constructor(
    private readonly _authService: AuthService,
    private readonly _dataService: WalletApiService,
    private readonly _router: Router,
    private readonly _fb: FormWithHandlerBuilder,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService
  ) {}
}
