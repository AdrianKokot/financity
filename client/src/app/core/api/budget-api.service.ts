import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable, of, tap } from 'rxjs';
import {
  Budget,
  BudgetDetails,
  BudgetListItem,
  CreateBudgetPayload,
} from '@shared/data-access/models/budget.model';
import { UserSettingsService } from '../../user-settings/data-access/services/user-settings.service';

@Injectable({
  providedIn: 'root',
})
export class BudgetApiService {
  constructor(protected http: HttpClient, private _user: UserSettingsService) {}

  private _getListCache: Record<string, BudgetListItem[]> = {};

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

    return this.http.get<BudgetListItem[]>('/api/budgets', { params }).pipe(
      tap(data => {
        this._getListCache[cacheKey] = data;

        if (
          pagination.page === 1 &&
          !(pagination.filters && 'search' in pagination.filters)
        ) {
          this._user.updateSettings({
            showSimplifiedBudgetView: data.length < 15,
          });
        }
      })
    );
  }

  get(id: Budget['id']) {
    return this.http.get<BudgetDetails>(`/api/budgets/${id}`);
  }

  create(payload: CreateBudgetPayload): Observable<Budget> {
    return this.http.post<{ id: Budget['id'] }>('/api/budgets', payload).pipe(
      map(({ id }) => ({ ...payload, id, currentPeriodExpenses: 0 })),
      tap(() => (this._getListCache = {}))
    );
  }

  update(
    payload: Pick<
      CreateBudgetPayload,
      'name' | 'amount' | 'trackedCategoriesId'
    > &
      Pick<Budget, 'id'>
  ) {
    return this.http
      .put<Budget>(`/api/budgets/${payload.id}`, payload)
      .pipe(tap(() => (this._getListCache = {})));
  }

  delete(id: Budget['id']): Observable<boolean> {
    return this.http.delete(`/api/budgets/${id}`, { observe: 'response' }).pipe(
      map(res => res.status >= 200 && res.status < 300),
      tap(() => (this._getListCache = {}))
    );
  }
}
