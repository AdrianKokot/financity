import { Injectable } from '@angular/core';
import {
  CreateWalletPayload,
  Wallet,
  WalletListItem,
} from '@shared/data-access/models/wallet.model';
import { User } from '../../auth/data-access/models/user';
import { map, Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { RecipientApiService } from './recipient-api.service';
import { LabelApiService } from './label-api.service';
import { CategoryApiService } from './category-api.service';
import { UserSettingsService } from '../../user-settings/data-access/services/user-settings.service';
import {
  ApiParams,
  GenericApiService,
  toHttpParams,
} from './generic-api.service';
import { Currency } from '@shared/data-access/models/currency.model';

@Injectable({
  providedIn: 'root',
})
export class WalletApiService extends GenericApiService<
  Wallet,
  WalletListItem,
  Wallet,
  CreateWalletPayload,
  Pick<Wallet, 'id' | 'startingAmount' | 'name'>
> {
  constructor(
    http: HttpClient,
    private readonly _recipient: RecipientApiService,
    private readonly _label: LabelApiService,
    private readonly _category: CategoryApiService,
    private readonly _user: UserSettingsService
  ) {
    super(http, '/api/wallets');
  }

  override getAll(pagination: ApiParams) {
    return super
      .getAll({
        ...pagination,
        orderBy: 'name',
        direction: 'asc',
      })
      .pipe(
        tap(data => {
          if (
            pagination.page === 1 &&
            !(pagination && 'search' in pagination)
          ) {
            this._user.updateSettings({
              showSimplifiedWalletView: data.length < 15,
            });
          }
        })
      );
  }

  getStats(payload: {
    walletIds: Wallet['id'][];
    currencyId: Currency['id'];
    from: Date;
    to: Date;
  }) {
    return this.http.get<{
      expenseStats: Record<string, number>;
      incomeStats: Record<string, number>;
    }>(`${this.api}/stats`, {
      params: toHttpParams({
        walledId_in: payload.walletIds,
        currencyId_eq: payload.currencyId,
        transactionDate_gte: payload.from.toJSON(),
        transactionDate_lte: payload.to.toJSON(),
      }),
    });
  }

  share(payload: {
    walletId: Wallet['id'];
    userEmail: User['email'];
  }): Observable<boolean> {
    return this.http
      .post(`${this.api}/${payload.walletId}/share`, payload, {
        observe: 'response',
      })
      .pipe(map(response => response.status === 204));
  }

  revoke(payload: {
    walletId: Wallet['id'];
    userEmail: User['email'];
  }): Observable<boolean> {
    return this.http
      .put(`${this.api}/${payload.walletId}/share`, payload, {
        observe: 'response',
      })
      .pipe(map(response => response.status === 204));
  }

  resign(walletId: Wallet['id']): Observable<boolean> {
    return this.http
      .post(
        `${this.api}/${walletId}/resign`,
        { walletId },
        {
          observe: 'response',
        }
      )
      .pipe(map(response => response.status === 204));
  }

  getSharedToList(walletId: Wallet['id'], pagination: ApiParams) {
    return this.http.get<User[]>(`${this.api}/${walletId}/share`, {
      params: toHttpParams({
        ...pagination,
        orderBy: 'email',
        direction: 'asc',
      }),
    });
  }

  getConcreteWalletApi(walletId: Wallet['id']) {
    return {
      get: this.get.bind(this, walletId),
      getLabels: (pagination: ApiParams) =>
        this._label.getAll({ ...pagination, walletId_eq: walletId }),
      getRecipients: (pagination: ApiParams) =>
        this._recipient.getAll({ ...pagination, walletId_eq: walletId }),
      getCategories: (pagination: ApiParams) =>
        this._category.getAll({ ...pagination, walletId_eq: walletId }),
    };
  }
}
