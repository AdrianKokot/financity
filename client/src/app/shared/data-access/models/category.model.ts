import { Wallet } from '@shared/data-access/models/wallet.model';
import { Appearance } from '@shared/data-access/models/appearance';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';

export interface Category {
  id: string;
  name: string;
  walletId: Wallet['id'];
  appearance: Appearance;
  transactionType: TransactionType;
  walletName?: string;
}

export type CategoryListItem = Category;

export type CreateCategoryPayload = Pick<
  Category,
  'name' | 'appearance' | 'walletId' | 'transactionType'
>;
