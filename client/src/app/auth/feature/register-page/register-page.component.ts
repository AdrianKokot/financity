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
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterPageComponent {
  readonly form = this._fb.nonNullable.group({
    email: ['', [Validators.email, Validators.required]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    name: ['', [Validators.required, Validators.maxLength(128)]],
  });

  readonly submit$ = new Subject<void>();
  readonly submitButtonLoading$ = this.submit$.pipe(
    tap(() => this.form.markAllAsTouched()),
    filter(() => this.form.valid),

    exhaustMap(() =>
      this._auth.register(this.form.getRawValue()).pipe(
        startWith(null),
        catchError(err => {
          if (err instanceof HttpErrorResponse) {
            handleValidationApiError(this.form, err);
          }
          return of(undefined);
        })
      )
    ),
    tap(console.log),
    tap(x => Boolean(x) && this._router.navigateByUrl('/auth/dashboard')),
    map(x => x === null),
    startWith(false)
  );

  constructor(
    private readonly _fb: FormBuilder,
    private readonly _auth: AuthService,
    private readonly _router: Router
  ) {}
}
