import { Injectable } from '@angular/core';
import { ApiParams, GenericApiService } from './generic-api.service';
import {
  CreateTransactionPayload,
  Transaction,
  TransactionDetails,
  TransactionListItem,
} from '@shared/data-access/models/transaction.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TransactionApiService extends GenericApiService<
  Transaction,
  TransactionListItem,
  TransactionDetails,
  CreateTransactionPayload,
  Pick<
    CreateTransactionPayload,
    | 'amount'
    | 'note'
    | 'recipientId'
    | 'categoryId'
    | 'transactionDate'
    | 'labelIds'
  > &
    Pick<Transaction, 'id'>
> {
  constructor(http: HttpClient) {
    super(http, '/api/transactions');
  }

  override getAll(pagination: ApiParams) {
    return super.getAll({
      ...pagination,
      ...('query' in pagination
        ? {}
        : {
            orderBy: 'transactionDate',
            direction: 'desc',
          }),
    });
  }
}
