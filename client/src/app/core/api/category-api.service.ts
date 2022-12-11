import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Wallet } from '@shared/data-access/models/wallet.model';
import {
  Category,
  CategoryListItem,
  CreateCategoryPayload,
} from '@shared/data-access/models/category.model';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CategoryApiService {
  constructor(protected http: HttpClient) {}

  getList(
    walletId: Wallet['id'],
    pagination: { page: number; pageSize: number }
  ) {
    return this.http.get<CategoryListItem[]>(
      `/api/categories?page=${pagination.page}&pageSize=${pagination.pageSize}&walletId_eq=${walletId}&orderBy=name&direction=asc`
    );
  }

  get(walletId: Category['id']) {
    return this.http.get<Category>(`/api/categories/${walletId}`);
  }

  create(payload: CreateCategoryPayload): Observable<Category> {
    return this.http
      .post<{ id: Category['id'] }>('/api/categories', payload)
      .pipe(map(({ id }) => ({ ...payload, id })));
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
