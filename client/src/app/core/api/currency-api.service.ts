import { Injectable } from '@angular/core';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { of, tap } from 'rxjs';
import { ExchangeRateApiService } from './exchange-rate-api.service';

@Injectable({
  providedIn: 'root',
})
export class CurrencyApiService {
  constructor(
    protected http: HttpClient,
    private _exchangeRateService: ExchangeRateApiService
  ) {}

  private _getListCache: Record<string, CurrencyListItem[]> = {};

  getExchangeRate(
    ...args: Parameters<
      InstanceType<typeof ExchangeRateApiService>['getExchangeRate']
    >
  ) {
    return this._exchangeRateService.getExchangeRate(...args);
  }

  getList(
    pagination: {
      page: number;
      pageSize: number;
      filters?: Record<string, string | string[]>;
    } = { page: 1, pageSize: 500 }
  ) {
    const filters: Record<string, string | string[]> = {};

    if (pagination.filters && 'name_ct' in pagination.filters) {
      filters['search'] = pagination.filters['name_ct'];
    }

    const params = new HttpParams().appendAll({
      page: pagination.page,
      pageSize: 500,
      orderBy: 'name',
      direction: 'asc',
      ...filters,
    });

    const cacheKey = params.toString();

    if (cacheKey in this._getListCache) {
      return of(this._getListCache[cacheKey]);
    }

    return this.http
      .get<CurrencyListItem[]>('/api/currencies', { params })
      .pipe(
        tap(data => {
          this._getListCache[cacheKey] = data;
        })
      );
  }
}
