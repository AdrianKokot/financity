import { Inject, Injectable, OnDestroy } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import {
  catchError,
  exhaustMap,
  from,
  merge,
  NEVER,
  Observable,
  Subject,
  switchMap,
  takeUntil,
  throwError,
} from 'rxjs';
import { AuthService } from '../../../auth/data-access/api/auth.service';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';
import { Router } from '@angular/router';

@Injectable()
export class ApiInterceptor implements HttpInterceptor, OnDestroy {
  private readonly _destroy$ = new Subject<boolean>();
  private readonly _errorAlert$ = new Subject<{
    title?: string;
    message: string;
  }>();
  private readonly _sessionExpiredAlert = new Subject<string>();

  constructor(
    private readonly _auth: AuthService,
    private readonly _router: Router,
    @Inject(TuiAlertService) private readonly _alert: TuiAlertService
  ) {
    merge(
      this._errorAlert$.pipe(
        exhaustMap(({ title, message }) =>
          this._alert.open(message, {
            status: TuiNotification.Error,
            label: title ?? 'Something went wrong',
            autoClose: true,
            hasCloseButton: true,
          })
        )
      ),
      this._sessionExpiredAlert.pipe(
        exhaustMap(message =>
          this._alert.open(message, {
            status: TuiNotification.Warning,
            label: 'Session expired',
            autoClose: true,
            hasCloseButton: true,
          })
        )
      )
    )
      .pipe(takeUntil(this._destroy$))
      .subscribe();
  }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    if (request.url.includes('/api/')) {
      return next.handle(request).pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            if (!request.url.includes('/api/auth/user')) {
              this._sessionExpiredAlert.next(
                'Your session expired, please login again.'
              );
            }
            return this._auth.handleUnauthorized();
          }

          if (error.status === 404) {
            return from(this._router.navigate(['/not-found'])).pipe(
              switchMap(() => NEVER)
            );
          }

          if (error.status === 400) {
            this._errorAlert$.next({
              title: 'Invalid operation',
              message: "The action you've tried to perform is invalid.",
            });
            return NEVER;
          }

          if (error.status !== 422) {
            this._errorAlert$.next({
              title: error.error.title,
              message: error.message.replace(window.location.origin, ''),
            });
          }

          return throwError(() => error);
        })
      );
    }

    return next.handle(request);
  }

  ngOnDestroy(): void {
    this._destroy$.next(true);
    this._destroy$.complete();
  }
}
