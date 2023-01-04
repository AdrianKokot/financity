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
  merge,
  Observable,
  Subject,
  takeUntil,
  throwError,
} from 'rxjs';
import { AuthService } from '../../../auth/data-access/api/auth.service';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';

@Injectable()
export class ApiInterceptor implements HttpInterceptor, OnDestroy {
  private _destroy$ = new Subject<boolean>();
  private _errorAlert$ = new Subject<string>();
  private _sessionExpiredAlert = new Subject<string>();

  constructor(
    private readonly _auth: AuthService,
    @Inject(TuiAlertService) private readonly _alert: TuiAlertService
  ) {
    merge(
      this._errorAlert$.pipe(
        exhaustMap(message =>
          this._alert.open(message, {
            status: TuiNotification.Error,
            label: 'Something went wrong',
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
            this._sessionExpiredAlert.next(
              'Your session expired, please login again.'
            );

            return this._auth.handleUnauthorized();
          }

          if (error.status !== 422) {
            this._errorAlert$.next(error.error.title ?? error.message);
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
