import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  BehaviorSubject,
  catchError,
  filter,
  from,
  map,
  merge,
  NEVER,
  Observable,
  of,
  share,
  shareReplay,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { Router } from '@angular/router';
import { User } from '../models/user';
import { ClaimTypes } from '../models/claim-types';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly _basePath = '/api/auth';
  private readonly _logout$ = new Subject<void>();
  private readonly _updatedUser$ = new BehaviorSubject<User | null>(null);
  private _userSnapshot: User | null = null;

  readonly user$ = merge(
    this._logout$.pipe(
      tap(() => (this.token = null)),
      map(() => null)
    ),
    this._updatedUser$.pipe(
      switchMap(user =>
        user === null
          ? this._http
              .get<User>(`${this._basePath}/user`)
              .pipe(catchError(() => of(null)))
          : of(user)
      )
    )
  ).pipe(
    tap(user => (this._userSnapshot = user)),
    shareReplay(1)
  );

  constructor(
    private readonly _http: HttpClient,
    private readonly _router: Router
  ) {}

  readonly loggedOut$ = this.user$.pipe(
    filter(x => x === null),
    share()
  );

  readonly isAuthenticated$ = this.user$.pipe(
    map(x => x !== null),
    share()
  );

  get userSnapshot(): User | null {
    return this._userSnapshot;
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
    this._logout$.next(undefined);
    return of(undefined);
  }

  handleUnauthorized(): Observable<never> {
    this.logout();

    return from(this._router.navigate(['/auth/login'])).pipe(
      switchMap(() => NEVER)
    );
  }

  login(payload: { email: string; password: string }) {
    return this._http
      .post<{ token: string }>(`${this._basePath}/login`, payload, {
        observe: 'response',
      })
      .pipe(
        tap(response => {
          this.token = response.body?.token ?? null;

          const user = this._getUserFromToken();
          this._userSnapshot = user;
          this._updatedUser$.next(user);
        }),
        map(res => res.status === 200)
      );
  }

  register(payload: { email: string; password: string }) {
    return this._http
      .post(`${this._basePath}/register`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 200));
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

  changePassword(payload: {
    password: string;
    newPassword: string;
  }): Observable<boolean> {
    return this._http
      .post(`${this._basePath}/change-password`, payload, {
        observe: 'response',
      })
      .pipe(map(res => res.status === 204));
  }

  updateUser(payload: Pick<User, 'name'>) {
    return this._http
      .put<User>(`${this._basePath}/user`, payload)
      .pipe(tap(user => this._updatedUser$.next(user)));
  }

  private _getUserFromToken() {
    if (this.token === null) {
      return null;
    }

    const payload = JSON.parse(
      decodeURIComponent(
        window
          .atob(this.token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))
          .split('')
          .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      )
    );

    return (Object.keys(ClaimTypes) as (keyof User)[]).reduce(
      (user, key) => ({ ...user, [key]: payload[ClaimTypes[key]] }),
      <User>{}
    );
  }
}
