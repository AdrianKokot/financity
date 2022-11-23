import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _basePath = '/api/auth';

  constructor(private _http: HttpClient) {}

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

  login(payload: { email: string; password: string }) {
    return this._http
      .post<{ token: string }>(`${this._basePath}/login`, payload)
      .pipe(tap(({ token }) => (this.token = token)));
  }

  register(payload: { email: string; password: string }) {
    return this._http.post<void>(`${this._basePath}/register`, payload);
  }
}
