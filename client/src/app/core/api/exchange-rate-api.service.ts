import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Currency } from '@shared/data-access/models/currency.model';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ExchangeRateApiService {
  constructor(private _http: HttpClient) {}

  getExchangeRate(from: Currency['id'], to: Currency['id']) {
    return this._http
      .get<{ rate: number }>('/api/exchange-rate', {
        params: new HttpParams().appendAll({ from, to }),
      })
      .pipe(map(x => x.rate));
  }
}
