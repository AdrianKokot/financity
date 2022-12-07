import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '../api/auth.service';
import { map, Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LogoutGuard implements CanActivate {
  constructor(private _auth: AuthService, private _router: Router) {}

  canActivate(): Observable<UrlTree> {
    return this._auth.logout().pipe(
      take(1),
      map(() => this._router.createUrlTree(['/auth/login']))
    );
  }
}
