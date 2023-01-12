import { User } from '../../../auth/data-access/models/user';
import { Currency } from '@shared/data-access/models/currency.model';
import { Category } from '@shared/data-access/models/category.model';

export interface Wallet {
  id: string;
  name: string;
  currencyId: Currency['id'];
  startingAmount: number;
  ownerId: User['id'];
  ownerName: User['name'];
}

export interface WalletListItem extends Wallet {
  currentState?: number;
}

export type CreateWalletPayload = Pick<
  Wallet,
  'name' | 'currencyId' | 'startingAmount'
>;

export interface WalletStats {
  currencyId: Currency['id'];
  expensesByCategory: {
    id: Category['id'] | null;
    name: string;
    expenses: number;
  }[];
}

export interface WalletsStats {
  expenseStats: Record<string, number>;
  incomeStats: Record<string, number>;
}
