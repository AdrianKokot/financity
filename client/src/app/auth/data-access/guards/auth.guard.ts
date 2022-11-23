import { Injectable } from '@angular/core';
import {
  CanActivate,
  CanActivateChild,
  Router,
  UrlTree,
} from '@angular/router';
import { AuthService } from '../api/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(private _auth: AuthService, private _router: Router) {}

  canActivate(): boolean | UrlTree {
    return this._auth.isAuthenticated
      ? true
      : this._router.createUrlTree(['/login']);
  }

  canActivateChild(): boolean | UrlTree {
    return this.canActivate();
  }
}
