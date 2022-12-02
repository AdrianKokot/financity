import { LabelListItem } from '@shared/data-access/models/label';

export interface Transaction {
  id: string;
}

export interface TransactionListItem extends Transaction {
  currencyId: string;
  currencyName: string;
  transactionDate: string;
  amount: number;
  exchangeRate: number;
  labels: LabelListItem[];
  categoryName: string;
  note?: string;
}
