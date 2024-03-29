import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../../auth/data-access/api/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private readonly _auth: AuthService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    if (this._auth.token !== null && request.url.startsWith('/api')) {
      return next.handle(
        request.clone({
          setHeaders: {
            Authorization: `Bearer ${this._auth.token}`,
          },
        })
      );
    }

    return next.handle(request);
  }
}
