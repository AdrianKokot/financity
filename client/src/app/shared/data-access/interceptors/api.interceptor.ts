import { Inject, Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthService } from '../../../auth/data-access/api/auth.service';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  constructor(
    private readonly _auth: AuthService,
    @Inject(TuiAlertService) private readonly _alert: TuiAlertService
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    if (request.url.includes('/api/')) {
      return next.handle(request).pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            return this._auth.handleUnauthorized();
          }
          if (error.status !== 422) {
            return this._alert
              .open(error.error.title ?? error.message, {
                status: TuiNotification.Error,
                label: 'Something went wrong',
                autoClose: true,
                hasCloseButton: true,
              })
              .pipe(switchMap(() => throwError(() => error)));
          }
          return throwError(() => error);
        })
      );
    }

    return next.handle(request);
  }
}
