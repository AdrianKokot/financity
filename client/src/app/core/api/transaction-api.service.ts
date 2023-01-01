import { Injectable } from '@angular/core';
import { GenericApiService } from './generic-api.service';
import { Wallet } from '@shared/data-access/models/wallet.model';
import {
  CreateTransactionPayload,
  Transaction,
  TransactionDetails,
  TransactionListItem,
} from '@shared/data-access/models/transaction.model';
import { HttpParams } from '@angular/common/http';
import { map, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TransactionApiService extends GenericApiService {
  private _getListCache: Record<string, TransactionListItem[]> = {};

  getAllList(pagination: {
    page: number;
    pageSize: number;
    filters?: Record<string, string | string[]>;
  }) {
    const params = new HttpParams().appendAll({
      page: pagination.page,
      pageSize: pagination.pageSize,
      orderBy: 'transactionDate',
      direction: 'desc',
      ...pagination.filters,
    });

    const cacheKey = params.toString();

    if (cacheKey in this._getListCache) {
      return of(this._getListCache[cacheKey]);
    }

    return this.http
      .get<TransactionListItem[]>('/api/transactions', { params })
      .pipe(
        tap(data => {
          this._getListCache[cacheKey] = data;
        })
      );
  }

  getList(
    walletId: Wallet['id'],
    pagination: {
      page: number;
      pageSize: number;
      filters?: Record<string, string | string[]>;
    }
  ) {
    return this.getAllList({
      ...pagination,
      filters: { ...pagination.filters, walletId_eq: walletId },
    });
  }

  get(walletId: Transaction['id']) {
    return this.http.get<TransactionDetails>(`/api/transactions/${walletId}`);
  }

  create(payload: CreateTransactionPayload): Observable<Transaction> {
    return this.http
      .post<{ id: Transaction['id'] }>('/api/transactions', payload)
      .pipe(map(({ id }) => ({ ...payload, id })));
  }

  update(
    payload: Pick<
      CreateTransactionPayload,
      | 'amount'
      | 'note'
      | 'recipientId'
      | 'categoryId'
      | 'transactionDate'
      | 'labelIds'
    > &
      Pick<Transaction, 'id'>
  ) {
    return this.http
      .put<Transaction>(`/api/transactions/${payload.id}`, payload)
      .pipe(tap(() => (this._getListCache = {})));
  }

  delete(id: Transaction['id']): Observable<boolean> {
    return this.http
      .delete(`/api/transactions/${id}`, { observe: 'response' })
      .pipe(
        map(res => res.status >= 200 && res.status < 300),
        tap(() => (this._getListCache = {}))
      );
  }
}
