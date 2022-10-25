import { Injectable } from '@angular/core';
import { GenericApiService } from './generic-api.service';
import { Wallet } from '../models/wallet.model';
import { TransactionListItem } from '../models/transaction.model';

@Injectable({
  providedIn: 'root',
})
export class TransactionApiService extends GenericApiService {
  getList(walletId: Wallet['id']) {
    return this.http.get<TransactionListItem[]>(
      '/api/transactions?walletId=' + walletId
    );
  }
}
