import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  catchError,
  exhaustMap,
  filter,
  map,
  of,
  startWith,
  Subject,
  tap,
} from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../data-access/api/auth.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { handleValidationApiError } from '@shared/utils/api/api-error-handler';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginPageComponent {
  form = this._fb.nonNullable.group({
    email: ['', [Validators.email, Validators.required]],
    password: ['', [Validators.required]],
  });
  readonly submit$ = new Subject<void>();
  readonly submitButtonLoading$ = this.submit$.pipe(
    tap(() => this.form.markAllAsTouched()),
    filter(() => this.form.valid),
    exhaustMap(() =>
      this._auth.login(this.form.getRawValue()).pipe(
        startWith(null),
        catchError(err => {
          if (err instanceof HttpErrorResponse) {
            handleValidationApiError(this.form, err);
          }
          return of(undefined);
        })
      )
    ),
    tap(x => Boolean(x) && this._router.navigateByUrl('/dashboard')),
    map(x => x === null),
    startWith(false)
  );

  constructor(
    private readonly _fb: FormBuilder,
    private readonly _auth: AuthService,
    private readonly _router: Router
  ) {}
}
