import { LabelListItem } from '@shared/data-access/models/label';

export interface Transaction {
  id: string;
}

export interface TransactionListItem extends Transaction {
  currencyCode: string;
  currencyName: string;
  currencyId: string;
  transactionDate: string;
  amount: number;
  exchangeRate: number;
  labels: LabelListItem[];
  categoryName: string;
  note?: string;
}
