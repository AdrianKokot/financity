import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Wallet } from '@shared/data-access/models/wallet.model';
import {
  CreateLabelPayload,
  Label,
  LabelListItem,
} from '@shared/data-access/models/label';
import { delay, map, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LabelApiService {
  constructor(protected http: HttpClient) {}

  private _getListCache: Record<string, LabelListItem[]> = {};

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

    const cacheKey = params.toString();

    if (cacheKey in this._getListCache) {
      return of(this._getListCache[cacheKey]);
    }

    return this.http.get<LabelListItem[]>('/api/labels', { params }).pipe(
      tap(data => {
        this._getListCache[cacheKey] = data;
      }),
      delay(1000)
    );
  }

  get(walletId: Label['id']) {
    return this.http.get<Label>(`/api/labels/${walletId}`);
  }

  create(payload: CreateLabelPayload): Observable<Label> {
    return this.http
      .post<{ id: Label['id'] }>('/api/labels', payload)
      .pipe(map(({ id }) => ({ ...payload, id })));
  }

  update(payload: Pick<Label, 'id' | 'name' | 'appearance'>) {
    return this.http.put<Label>(`/api/labels/${payload.id}`, payload);
  }

  delete(id: Label['id']): Observable<boolean> {
    return this.http
      .delete(`/api/labels/${id}`, { observe: 'response' })
      .pipe(map(res => res.status >= 200 && res.status < 300));
  }
}
