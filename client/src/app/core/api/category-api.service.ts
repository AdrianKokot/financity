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

  getList(walletId: Wallet['id']) {
    return this.http.get<CategoryListItem[]>(
      `/api/categories?walletId_eq=${walletId}`
    );
  }

  // get(walletId: Wallet['id']) {
  //   return this.http.get<Wallet>(`/api/wallets/${walletId}`);
  // }
  //
  create(payload: CreateCategoryPayload): Observable<Category> {
    return this.http
      .post<{ id: Category['id'] }>('/api/categories', payload)
      .pipe(map(({ id }) => ({ ...payload, id })));
  }

  //
  // update(payload: Pick<Wallet, 'id' | 'startingAmount' | 'name'>) {
  //   return this.http.put(`/api/wallets/${payload.id}`, payload);
  // }
}
