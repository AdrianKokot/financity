import { Injectable } from '@angular/core';
import { GenericApiService } from './generic-api.service';
import { Wallet } from '../../shared/data-access/models/wallet.model';
import { TransactionListItem } from '../../shared/data-access/models/transaction.model';

@Injectable({
  providedIn: 'root',
})
export class TransactionApiService extends GenericApiService {
  getList(
    walletId: Wallet['id'],
    pagination: { page: number; pageSize: number }
  ) {
    return this.http.get<TransactionListItem[]>(
      `/api/transactions?page=${pagination.page}&pageSize=${pagination.pageSize}&orderBy=transactionDate&direction=desc&walletId_eq=${walletId}`
    );
  }
}
