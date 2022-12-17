import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { map, Observable } from 'rxjs';
import {
  CreateRecipientPayload,
  Recipient,
  RecipientListItem,
} from '@shared/data-access/models/recipient.model';

@Injectable({
  providedIn: 'root',
})
export class RecipientApiService {
  constructor(protected http: HttpClient) {}

  getList(
    walletId: Wallet['id'],
    pagination: {
      page: number;
      pageSize: number;
      filters?: Record<string, string>;
    }
  ) {
    const params = new HttpParams().appendAll({
      page: pagination.page,
      pageSize: pagination.pageSize,
      orderBy: 'name',
      direction: 'asc',
      walletId_eq: walletId,
      ...pagination.filters,
    });

    return this.http.get<RecipientListItem[]>('/api/recipients', { params });
  }

  get(walletId: Recipient['id']) {
    return this.http.get<Recipient>(`/api/recipients/${walletId}`);
  }

  create(payload: CreateRecipientPayload): Observable<Recipient> {
    return this.http
      .post<{ id: Recipient['id'] }>('/api/recipients', payload)
      .pipe(map(({ id }) => ({ ...payload, id })));
  }

  update(payload: Pick<Recipient, 'id' | 'name'>) {
    return this.http.put<Recipient>(`/api/recipients/${payload.id}`, payload);
  }

  delete(id: Recipient['id']): Observable<boolean> {
    return this.http
      .delete(`/api/recipients/${id}`, { observe: 'response' })
      .pipe(map(res => res.status >= 200 && res.status < 300));
  }
}
