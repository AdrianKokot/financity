import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Wallet } from '@shared/data-access/models/wallet.model';
import {
  Category,
  CategoryListItem,
  CreateCategoryPayload,
} from '@shared/data-access/models/category.model';
import { delay, map, Observable, of, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CategoryApiService {
  constructor(protected http: HttpClient) {}

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
      orderBy: 'name',
      direction: 'asc',
      walletId_eq: walletId,
      ...pagination.filters,
    });

    return this.http.get<CategoryListItem[]>('/api/categories', { params });
  }

  get(walletId: Category['id']) {
    return this.http.get<Category>(`/api/categories/${walletId}`);
  }

  create(payload: CreateCategoryPayload): Observable<Category> {
    return of(null).pipe(
      switchMap(() =>
        this.http.post<{ id: Category['id'] }>('/api/categories', payload).pipe(
          map(({ id }) => ({ ...payload, id })),
          delay(3000)
        )
      )
    );
    // return this.http
    //   .post<{ id: Category['id'] }>('/api/categories', payload)
    //   .pipe(map(({ id }) => ({ ...payload, id })));
  }

  update(payload: Pick<Category, 'id' | 'name' | 'appearance'>) {
    return this.http.put<Category>(`/api/categories/${payload.id}`, payload);
  }

  delete(id: Category['id']): Observable<boolean> {
    return this.http
      .delete(`/api/categories/${id}`, { observe: 'response' })
      .pipe(map(res => res.status >= 200 && res.status < 300));
  }
}
