import { Injectable } from '@angular/core';
import { CurrencyListItem } from '@shared/data-access/models/currency.model';
import { HttpClient } from '@angular/common/http';
import { of, tap } from 'rxjs';
import { ExchangeRateApiService } from './exchange-rate-api.service';
import { ApiParams, toHttpParams } from './generic-api.service';

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

  getList(pagination: ApiParams) {
    const temp = { ...pagination };

    if (temp && 'name_ct' in temp && temp['name_ct']) {
      temp['search'] = temp['name_ct'];
      delete temp['name_ct'];
    }

    const params = toHttpParams({
      ...temp,
      pageSize: 500,
      orderBy: 'name',
      direction: 'asc',
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
