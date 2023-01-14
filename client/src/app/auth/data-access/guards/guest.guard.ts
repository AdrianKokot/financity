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
export class GuestGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(
    private readonly _auth: AuthService,
    private readonly _router: Router
  ) {}

  canActivate(): boolean | UrlTree {
    return this._auth.hasValidToken()
      ? this._router.createUrlTree(['/'])
      : true;
  }

  canActivateChild(): ReturnType<typeof this.canActivate> {
    return this.canActivate();
  }

  canLoad(): ReturnType<typeof this.canActivate> {
    return this.canActivate();
  }
}
