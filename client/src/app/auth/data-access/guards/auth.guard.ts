import { Injectable } from '@angular/core';
import {
  CanActivate,
  CanActivateChild,
  CanLoad,
  Router,
  UrlTree,
} from '@angular/router';
import { AuthService } from '../api/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(
    private readonly _auth: AuthService,
    private readonly _router: Router
  ) {}

  canActivate(): true | UrlTree {
    return this._auth.hasValidToken()
      ? true
      : this._router.createUrlTree(['/auth/login']);
  }

  canActivateChild(): ReturnType<typeof this.canActivate> {
    return this.canActivate();
  }

  canLoad(): ReturnType<typeof this.canActivate> {
    return this.canActivate();
  }
}
