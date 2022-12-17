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
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TransactionApiService extends GenericApiService {
  getList(
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
      orderBy: 'transactionDate',
      direction: 'desc',
      walletId_eq: walletId,
      ...pagination.filters,
    });

    return this.http.get<TransactionListItem[]>('/api/transactions', {
      params,
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
    return this.http.put<Transaction>(
      `/api/transactions/${payload.id}`,
      payload
    );
  }

  delete(id: Transaction['id']): Observable<boolean> {
    return this.http
      .delete(`/api/transactions/${id}`, { observe: 'response' })
      .pipe(map(res => res.status >= 200 && res.status < 300));
  }
}
