import { Injectable } from '@angular/core';
import {
  CreateWalletPayload,
  Wallet,
  WalletListItem,
} from '@shared/data-access/models/wallet.model';
import { User } from '../../auth/data-access/models/user';
import { map, Observable, of, tap } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { RecipientApiService } from './recipient-api.service';
import { LabelApiService } from './label-api.service';
import { CategoryApiService } from './category-api.service';
import { UserSettingsService } from '../../user-settings/data-access/services/user-settings.service';

@Injectable({
  providedIn: 'root',
})
export class WalletApiService {
  private _getListCache: Record<string, WalletListItem[]> = {};

  constructor(
    private readonly _http: HttpClient,
    private readonly _recipient: RecipientApiService,
    private readonly _label: LabelApiService,
    private readonly _category: CategoryApiService,
    private readonly _user: UserSettingsService
  ) {}

  getList(pagination: {
    page: number;
    pageSize: number;
    filters?: Record<string, string | string[]>;
  }) {
    const params = new HttpParams().appendAll({
      page: pagination.page,
      pageSize: pagination.pageSize,
      orderBy: 'name',
      direction: 'asc',
      ...pagination.filters,
    });

    const cacheKey = params.toString();

    if (cacheKey in this._getListCache) {
      return of(this._getListCache[cacheKey]);
    }

    return this._http.get<WalletListItem[]>('/api/wallets', { params }).pipe(
      tap(data => {
        this._getListCache[cacheKey] = data;

        if (
          pagination.page === 1 &&
          !(pagination.filters && 'search' in pagination.filters)
        ) {
          this._user.updateSettings({
            showSimplifiedWalletView: data.length < 15,
          });
        }
      })
    );
  }

  get(walletId: Wallet['id']) {
    return this._http.get<Wallet>(`/api/wallets/${walletId}`);
  }

  create(payload: CreateWalletPayload) {
    return this._http.post<{ id: Wallet['id'] }>('/api/wallets', payload).pipe(
      map(({ id }) => ({ ...payload, id })),
      tap(() => (this._getListCache = {}))
    );
  }

  update(payload: Pick<Wallet, 'id' | 'startingAmount' | 'name'>) {
    return this._http.put(`/api/wallets/${payload.id}`, payload).pipe(
      tap(() => (this._getListCache = {})),
      tap(() => (this._getListCache = {}))
    );
  }

  share(payload: {
    walletId: Wallet['id'];
    userEmail: User['email'];
  }): Observable<boolean> {
    return this._http
      .post(`/api/wallets/${payload.walletId}/share`, payload, {
        observe: 'response',
      })
      .pipe(map(response => response.status === 204));
  }

  revoke(payload: {
    walletId: Wallet['id'];
    userEmail: User['email'];
  }): Observable<boolean> {
    return this._http
      .put(`/api/wallets/${payload.walletId}/share`, payload, {
        observe: 'response',
      })
      .pipe(map(response => response.status === 204));
  }

  getSharedToList(
    walletId: Wallet['id'],
    pagination: {
      page: number;
      pageSize: number;
      filters?: Record<string, string | string[]>;
    }
  ) {
    const params = new HttpParams().appendAll({
      page: pagination.page,
      pageSize: pagination.pageSize,
      orderBy: 'email',
      direction: 'asc',
      ...pagination.filters,
    });

    return this._http.get<User[]>(`/api/wallets/${walletId}/share`, { params });
  }

  getConcreteWalletApi(walletId: Wallet['id']) {
    return {
      get: this.get.bind(this, walletId),
      getLabels: this._label.getList.bind(this._label, walletId),
      getRecipients: this._recipient.getList.bind(this._recipient, walletId),
      getCategories: this._category.getList.bind(this._category, walletId),
    };
  }
}
