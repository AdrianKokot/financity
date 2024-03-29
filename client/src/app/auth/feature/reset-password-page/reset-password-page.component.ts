import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Validators } from '@angular/forms';
import { distinctUntilChanged, map, Subject } from 'rxjs';
import { AuthService } from '../../data-access/api/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { CustomValidators } from '@shared/utils/form/custom-validators';

@Component({
  selector: 'app-reset-password-page',
  templateUrl: './reset-password-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResetPasswordPageComponent {
  readonly form = this._fb.form(
    {
      email: [
        '',
        [Validators.email, Validators.required, Validators.maxLength(256)],
      ],
      password: [{ value: '', disabled: true }, []],
      token: [{ value: '', disabled: true }, []],
    },
    {
      submit: payload => {
        return this._auth[
          this.isResetPasswordRequest ? 'resetPassword' : 'requestPasswordReset'
        ](payload);
      },
      effect: succeeded => {
        if (succeeded && this.isResetPasswordRequest) {
          this._router.navigateByUrl('/auth/login');
        } else {
          this.resultBanner$.next({
            message: succeeded
              ? 'Reset password link was sent to the given email.'
              : 'Something went wrong.',
            succeeded,
          });
        }
      },
    }
  );

  readonly formError$ = this.form.statusChanges.pipe(
    distinctUntilChanged(),
    map(() => this.form.errors),
    map(errors => (errors !== null ? Object.values(errors) : [])),
    map(errors => (errors.length > 0 ? errors[0].message : null)),
    distinctUntilChanged()
  );

  get isResetPasswordRequest(): boolean {
    return this.form.controls.token.enabled;
  }

  readonly resultBanner$ = new Subject<{
    succeeded: boolean;
    message: string;
  }>();

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _auth: AuthService,
    private readonly _route: ActivatedRoute,
    private readonly _router: Router
  ) {
    this.initToken();
  }

  initToken(): void {
    if (this._route.snapshot.queryParamMap.has('token')) {
      this.form.controls.token.enable();
      this.form.controls.token.setValue(
        window.location.search.replace('?token=', '')
      );
      this.form.controls.password.enable();
      this.form.controls.password.setValidators([
        Validators.required,
        CustomValidators.password(),
      ]);
    }
  }
}
