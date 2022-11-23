import { Injectable, OnDestroy } from '@angular/core';
import {
  BehaviorSubject,
  catchError,
  distinctUntilChanged,
  filter,
  map,
  of,
  share,
  Subject,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../data-access/api/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { handleValidationApiError } from '@shared/utils/api/api-error-handler';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable()
export class ResetPasswordAdapter implements OnDestroy {
  private _destroyed = new Subject<boolean>();

  form = this._fb.nonNullable.group({
    email: ['', [Validators.email, Validators.required]],
    password: [{ value: '', disabled: true }, []],
    token: [{ value: '', disabled: true }, []],
  });

  formError$ = this.form.statusChanges.pipe(
    distinctUntilChanged(),
    map(() => this.form.errors),
    map(errors => (errors !== null ? Object.values(errors) : [])),
    map(errors => (errors.length > 0 ? errors[0].message : null)),
    distinctUntilChanged()
  );

  submitButtonLoading$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  submit$ = new Subject<void>();
  resultBanner$ = new Subject<{ succeeded: boolean; message: string }>();

  constructor(
    private _fb: FormBuilder,
    private _auth: AuthService,
    private _route: ActivatedRoute,
    private _router: Router
  ) {
    this.initToken();

    const startRequest$ = this.submit$.pipe(
      takeUntil(this._destroyed),
      tap(() => {
        this.form.markAllAsTouched();
      }),
      filter(() => this.form.valid),
      tap(() => {
        this.submitButtonLoading$.next(true);
      }),
      share()
    );

    startRequest$
      .pipe(
        filter(() => this.form.controls.token.enabled),
        switchMap(() =>
          this._auth.resetPassword(this.form.getRawValue()).pipe(
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
        filter((res): res is boolean => typeof res === 'boolean')
      )
      .subscribe(succeeded => {
        if (succeeded) {
          _router.navigateByUrl('/auth/login');
        } else {
          this.resultBanner$.next({
            message: 'Something went wrong.',
            succeeded,
          });
        }
      });

    startRequest$
      .pipe(
        filter(() => !this.form.controls.token.enabled),
        switchMap(() =>
          this._auth
            .requestPasswordReset({
              email: this.form.getRawValue().email,
            })
            .pipe(
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
        filter((res): res is boolean => typeof res === 'boolean')
      )
      .subscribe(succeeded => {
        this.resultBanner$.next({
          message: succeeded
            ? 'Reset password link was sent to the given email.'
            : 'Something went wrong.',
          succeeded,
        });
      });
  }

  ngOnDestroy(): void {
    this._destroyed.next(true);
    this._destroyed.complete();
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
