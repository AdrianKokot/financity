import { Injectable } from '@angular/core';
import { CurrencyListItem } from '../../shared/data-access/models/currency.model';
import { GenericApiService } from './generic-api.service';

@Injectable({
  providedIn: 'root',
})
export class CurrencyApiService extends GenericApiService {
  getList = this.cachedRequest(
    this.http.get<CurrencyListItem[]>('/api/currencies')
  );
}
