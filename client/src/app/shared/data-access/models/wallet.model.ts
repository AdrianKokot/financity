import { User } from '../../../auth/data-access/models/user';
import { Currency } from '@shared/data-access/models/currency.model';

export interface Wallet {
  id: string;
  name: string;
  currencyId: Currency['id'];
  startingAmount: number;
  ownerId: User['id'];
}

export interface WalletListItem extends Wallet {
  currentState?: number;
  ownerName: User['name'];
}

export type CreateWalletPayload = Pick<
  Wallet,
  'name' | 'currencyId' | 'startingAmount'
>;
