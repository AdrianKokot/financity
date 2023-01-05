import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { shareReplay } from 'rxjs';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { toHttpParams } from '../../../core/api/generic-api.service';
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

  readonly user$ = this._http.get<User>('/api/auth/user').pipe(shareReplay(1));
  readonly settings$ = this._userSettings.settings$;

  get userSnapshot() {
    return this._auth.user;
  }

  getUserCurrencies() {
    return this._http.get<CurrencyListItem[]>('/api/currencies/used-by-user', {
      params: toHttpParams({
        pageSize: 500,
        page: 1,
        orderBy: 'id',
        direction: 'desc',
      }),
    });
  }
}
