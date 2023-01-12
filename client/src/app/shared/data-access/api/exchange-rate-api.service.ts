import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Currency } from '@shared/data-access/models/currency.model';
import { map, of, tap } from 'rxjs';
import { toHttpParams } from './generic-api.service';

@Injectable({
  providedIn: 'root',
})
export class ExchangeRateApiService {
  constructor(private readonly _http: HttpClient) {}

  private _getListCache: Record<string, { rate: number }> = {};

  getExchangeRate(from: Currency['id'], to: Currency['id'], date?: Date) {
    const params = toHttpParams({
      from,
      to,
      date: date?.toJSON(),
    });

    const stringKey = params.toString();

    if (stringKey in this._getListCache) {
      return of(this._getListCache[stringKey].rate);
    }

    return this._http
      .get<{ rate: number }>('/api/exchange-rate', { params })
      .pipe(
        tap(data => {
          this._getListCache[stringKey] = data;
        }),
        map(x => x.rate)
      );
  }
}
