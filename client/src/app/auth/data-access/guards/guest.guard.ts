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
export class GuestGuard implements CanActivate, CanActivateChild {
  constructor(private _auth: AuthService, private _router: Router) {}

  canActivate(): boolean | UrlTree {
    return this._auth.isAuthenticated
      ? this._router.createUrlTree(['/'])
      : true;
  }

  canActivateChild(): boolean | UrlTree {
    return this.canActivate();
  }
}
