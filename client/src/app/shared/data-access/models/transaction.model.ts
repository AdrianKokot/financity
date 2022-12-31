import { Label } from '@shared/data-access/models/label';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { Category } from '@shared/data-access/models/category.model';
import { Recipient } from '@shared/data-access/models/recipient.model';

export interface Transaction {
  id: string;
  currencyId: string;
  transactionDate: string;
  transactionType: TransactionType;
  recipientId: Recipient['id'] | null;
  amount: number;
  exchangeRate: number;
  note: string | null;
  categoryId: Category['id'] | null;
  walletId: Wallet['id'];
}

export interface TransactionDetails extends Transaction {
  wallet: Wallet;
  labels: Label[];
  category: Category | null;
  recipient: Recipient | null;
}

export interface TransactionListItem extends Transaction {
  recipient: Pick<Recipient, 'id' | 'name'> | null;
  category: Pick<Category, 'id' | 'name' | 'appearance'> | null;
  labels: Pick<Label, 'id' | 'name' | 'appearance'>[];
}

export interface CreateTransactionPayload
  extends Pick<
    Transaction,
    | 'amount'
    | 'note'
    | 'exchangeRate'
    | 'recipientId'
    | 'walletId'
    | 'transactionType'
    | 'categoryId'
    | 'currencyId'
    | 'transactionDate'
  > {
  labelIds: Label['id'][];
}
