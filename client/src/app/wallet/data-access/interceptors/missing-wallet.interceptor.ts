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
  NEVER,
  Observable,
  Subject,
  switchMap,
  takeUntil,
  throwError,
} from 'rxjs';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';
import { Router } from '@angular/router';

@Injectable()
export class MissingWalletInterceptor implements HttpInterceptor, OnDestroy {
  private readonly _destroy$ = new Subject<boolean>();
  private readonly _errorAlert$ = new Subject<{
    title?: string;
    message: string;
  }>();

  constructor(
    private readonly _router: Router,
    @Inject(TuiAlertService) private readonly _alert: TuiAlertService
  ) {
    this._errorAlert$
      .pipe(
        takeUntil(this._destroy$),
        exhaustMap(({ title, message }) =>
          this._alert.open(message, {
            status: TuiNotification.Error,
            label: title ?? 'Something went wrong',
            autoClose: true,
            hasCloseButton: true,
          })
        )
      )
      .subscribe();
  }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    if (request.url.includes('/api/')) {
      return next.handle(request).pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 422 && 'walletId' in error.error.errors) {
            this._errorAlert$.next({
              title: "Wallet doesn't exist",
              message: "The given wallet doesn't exist.",
            });
            return from(this._router.navigate(['/wallets'])).pipe(
              switchMap(() => NEVER)
            );
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
