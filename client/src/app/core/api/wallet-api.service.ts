import { Injectable } from '@angular/core';
import { GenericApiService } from './generic-api.service';
import {
  CreateWalletPayload,
  Wallet,
  WalletListItem,
} from '@shared/data-access/models/wallet.model';
import { User } from '../../auth/data-access/models/user';
import { map, Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { RecipientApiService } from './recipient-api.service';
import { LabelApiService } from './label-api.service';
import { CategoryApiService } from './category-api.service';

@Injectable({
  providedIn: 'root',
})
export class WalletApiService extends GenericApiService {
  getList = this.cachedRequest(this.http.get<WalletListItem[]>('/api/wallets'));

  constructor(
    http: HttpClient,
    private _recipient: RecipientApiService,
    private _label: LabelApiService,
    private _category: CategoryApiService
  ) {
    super(http);
  }

  get(walletId: Wallet['id']) {
    return this.http.get<Wallet>(`/api/wallets/${walletId}`);
  }

  create(payload: CreateWalletPayload) {
    return this.http.post('/api/wallets', payload);
  }

  update(payload: Pick<Wallet, 'id' | 'startingAmount' | 'name'>) {
    return this.http.put(`/api/wallets/${payload.id}`, payload);
  }

  share(payload: {
    walletId: Wallet['id'];
    userEmail: User['email'];
  }): Observable<boolean> {
    return this.http
      .post(`/api/wallets/${payload.walletId}/share`, payload, {
        observe: 'response',
      })
      .pipe(map(response => response.status === 204));
  }

  revoke(payload: {
    walletId: Wallet['id'];
    userEmail: User['email'];
  }): Observable<boolean> {
    return this.http
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

    return this.http.get<User[]>(`/api/wallets/${walletId}/share`, { params });
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
