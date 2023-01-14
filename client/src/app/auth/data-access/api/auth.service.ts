import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  from,
  map,
  NEVER,
  Observable,
  of,
  share,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { Router } from '@angular/router';
import {
  ChangePasswordPayload,
  LoginPayload,
  RegisterPayload,
  ResetPasswordPayload,
  User,
} from '../models/user';
import { ClaimTypes } from '../models/claim-types';
import { decodeJwt } from '../../utils/decode-jwt';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly _basePath = '/api/auth';
  private readonly _logout$ = new Subject<void>();
  readonly loggedOut$ = this._logout$.pipe(share());

  get userSnapshot() {
    return 'user' in localStorage
      ? JSON.parse(localStorage.getItem('user') ?? '{}')
      : this._getUserFromToken();
  }

  get token(): string | null {
    return 'token' in localStorage ? localStorage.getItem('token') : null;
  }

  constructor(
    private readonly _http: HttpClient,
    private readonly _router: Router
  ) {}

  hasValidToken() {
    if (this.token === null) {
      return false;
    }

    const payload = decodeJwt(this.token);

    const currUnixTimestamp = (new Date().getTime() / 1000) | 0;

    return currUnixTimestamp >= payload.nbf && currUnixTimestamp <= payload.exp;
  }

  logout(): Observable<void> {
    this._saveToken(null);
    this._logout$.next(undefined);
    return of(undefined);
  }

  handleUnauthorized(): Observable<never> {
    this.logout();

    return from(this._router.navigate(['/auth/login'])).pipe(
      switchMap(() => NEVER)
    );
  }

  login(payload: LoginPayload) {
    return this._http
      .post<{ token: string }>(`${this._basePath}/login`, payload, {
        observe: 'response',
      })
      .pipe(
        tap(response => {
          this._saveToken(response.body?.token ?? null);
          this._saveUserSnapshot(this._getUserFromToken());
        }),
        map(res => res.status === 200)
      );
  }

  register(payload: RegisterPayload) {
    return this._http
      .post(`${this._basePath}/register`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 200));
  }

  requestPasswordReset(payload: Pick<User, 'email'>): Observable<boolean> {
    return this._http
      .post(`${this._basePath}/request-reset-password`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 202));
  }

  resetPassword(payload: ResetPasswordPayload): Observable<boolean> {
    return this._http
      .post(`${this._basePath}/reset-password`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 204));
  }

  changePassword(payload: ChangePasswordPayload): Observable<boolean> {
    return this._http
      .post(`${this._basePath}/change-password`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 204));
  }

  updateUser(payload: Pick<User, 'name'>) {
    return this._http
      .put<User>(`${this._basePath}/user`, payload)
      .pipe(tap(user => this._saveUserSnapshot(user)));
  }

  private _getUserFromToken() {
    if (this.token === null) {
      return null;
    }

    const payload = decodeJwt(this.token);

    const currUnixTimestamp = (new Date().getTime() / 1000) | 0;

    if (currUnixTimestamp >= payload.nbf && currUnixTimestamp <= payload.exp) {
      return (Object.keys(ClaimTypes) as (keyof User)[]).reduce(
        (user, key) => ({ ...user, [key]: payload[ClaimTypes[key]] }),
        <User>{}
      );
    }

    this.handleUnauthorized();
    return null;
  }

  private _saveUserSnapshot(value: User | null) {
    if (value === null) {
      localStorage.removeItem('user');
    } else {
      localStorage.setItem('user', JSON.stringify(value));
    }
  }

  private _saveToken(value: string | null) {
    if (value === null) {
      localStorage.removeItem('token');
      this._saveUserSnapshot(null);
    } else {
      localStorage.setItem('token', value);
    }
  }
}
