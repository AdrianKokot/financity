import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EMPTY, map, Observable, of, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _basePath = '/api/auth';

  constructor(private _http: HttpClient, private _router: Router) {}

  get isAuthenticated(): boolean {
    return this.token !== null;
  }

  get token(): string | null {
    return 'token' in localStorage ? localStorage.getItem('token') : null;
  }

  set token(value: string | null) {
    if (value === null) {
      localStorage.removeItem('token');
    } else {
      localStorage.setItem('token', value);
    }
  }

  logout(): Observable<void> {
    this.token = null;
    return of(undefined);
  }

  handleUnauthorized(): Observable<never> {
    this.logout();
    this._router.navigateByUrl('/auth/login');
    return EMPTY;
  }

  login(payload: { email: string; password: string }) {
    return this._http
      .post<{ token: string }>(`${this._basePath}/login`, payload, {
        observe: 'response',
      })
      .pipe(
        tap(response => (this.token = response.body?.token ?? null)),
        map(res => res.status === 200)
      );
  }

  register(payload: { email: string; password: string }) {
    return this._http
      .post(`${this._basePath}/register`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 204));
  }

  requestPasswordReset(payload: { email: string }): Observable<boolean> {
    return this._http
      .post(`${this._basePath}/request-reset-password`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 202));
  }

  resetPassword(payload: {
    email: string;
    password: string;
    token: string;
  }): Observable<boolean> {
    return this._http
      .post(`${this._basePath}/reset-password`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 204));
  }
}
