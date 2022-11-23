import { Injectable, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import {
  BehaviorSubject,
  catchError,
  filter,
  of,
  Subject,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../data-access/api/auth.service';
import { handleValidationApiError } from '@shared/utils/api/api-error-handler';
import { Router } from '@angular/router';

@Injectable()
export class RegisterPageAdapter implements OnDestroy {
  private _destroyed = new Subject<boolean>();

  form = this._fb.nonNullable.group({
    email: ['', [Validators.email, Validators.required]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  submitButtonLoading$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  submit$ = new Subject<void>();

  constructor(
    private _fb: FormBuilder,
    private _auth: AuthService,
    private _router: Router
  ) {
    this.submit$
      .pipe(
        takeUntil(this._destroyed),
        tap(() => {
          this.form.markAllAsTouched();
        }),
        filter(() => this.form.valid),
        tap(() => {
          this.submitButtonLoading$.next(true);
        }),
        switchMap(() =>
          this._auth.register(this.form.getRawValue()).pipe(
            catchError(err => {
              if (err instanceof HttpErrorResponse) {
                handleValidationApiError(this.form, err);
              }
              return of(null);
            })
          )
        ),
        tap(() => {
          this.submitButtonLoading$.next(false);
        }),
        filter(res => !!res)
      )
      .subscribe(() => {
        _router.navigateByUrl('/auth/dashboard');
      });
  }

  ngOnDestroy(): void {
    this._destroyed.next(true);
    this._destroyed.complete();
  }
}
