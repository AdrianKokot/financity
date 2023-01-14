import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserSettingsService } from '../../../user-settings/data-access/services/user-settings.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(
    private readonly _http: HttpClient,
    private readonly _userSettings: UserSettingsService,
    private readonly _auth: AuthService
  ) {}

  readonly settings$ = this._userSettings.settings$;

  get userId() {
    return this._auth.userSnapshot?.id;
  }
}
