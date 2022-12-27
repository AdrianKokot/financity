import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import {
  catchError,
  distinctUntilChanged,
  exhaustMap,
  filter,
  map,
  of,
  startWith,
  Subject,
  tap,
} from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { handleValidationApiError } from '@shared/utils/api/api-error-handler';
import { AuthService } from '../../data-access/api/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-reset-password-page',
  templateUrl: './reset-password-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResetPasswordPageComponent {
  readonly form = this._fb.nonNullable.group({
    email: ['', [Validators.email, Validators.required]],
    password: [{ value: '', disabled: true }, []],
    token: [{ value: '', disabled: true }, []],
  });

  readonly formError$ = this.form.statusChanges.pipe(
    distinctUntilChanged(),
    map(() => this.form.errors),
    map(errors => (errors !== null ? Object.values(errors) : [])),
    map(errors => (errors.length > 0 ? errors[0].message : null)),
    distinctUntilChanged()
  );

  get isResetPasswordRequest() {
    return this.form.controls.token.enabled;
  }

  readonly submit$ = new Subject<void>();
  readonly resultBanner$ = new Subject<{
    succeeded: boolean;
    message: string;
  }>();
  readonly submitButtonLoading$ = this.submit$.pipe(
    tap(() => this.form.markAllAsTouched()),
    filter(() => this.form.valid),
    exhaustMap(() => {
      const observableFn = this._auth[
        this.isResetPasswordRequest ? 'resetPassword' : 'requestPasswordReset'
      ].bind(this._auth);

      return observableFn(this.form.getRawValue()).pipe(
        startWith(null),
        catchError(err => {
          if (err instanceof HttpErrorResponse) {
            handleValidationApiError(this.form, err);
          }
          return of(undefined);
        })
      );
    }),
    tap(succeeded => {
      if (typeof succeeded !== 'boolean') {
        return;
      }

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
    }),
    map(x => x === null),
    startWith(false)
  );

  constructor(
    private readonly _fb: FormBuilder,
    private readonly _auth: AuthService,
    private readonly _route: ActivatedRoute,
    private readonly _router: Router
  ) {
    this.initToken();
  }

  initToken(): void {
    const resetToken = this._route.snapshot.queryParamMap.get('token');

    if (resetToken !== null) {
      this.form.controls.token.enable();
      this.form.controls.token.setValue(resetToken);
      this.form.controls.password.enable();
      this.form.controls.password.setValidators([
        Validators.required,
        Validators.minLength(8),
      ]);
    }
  }
}
