import { Injectable } from '@angular/core';
import {
  CanActivate,
  CanActivateChild,
  CanLoad,
  Router,
  UrlTree,
} from '@angular/router';
import { AuthService } from '../api/auth.service';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(
    private readonly _auth: AuthService,
    private readonly _router: Router
  ) {}

  canActivate(): Observable<boolean | UrlTree> {
    return this._auth.isAuthenticated$.pipe(
      map(isAuthenticated =>
        isAuthenticated ? true : this._router.createUrlTree(['/auth/login'])
      )
    );
  }

  canActivateChild(): ReturnType<typeof this.canActivate> {
    return this.canActivate();
  }

  canLoad(): ReturnType<typeof this.canActivate> {
    return this.canActivate();
  }
}
