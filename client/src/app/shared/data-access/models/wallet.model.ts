import { Currency } from './currency.model';
import { Account } from './account.model';

export interface Wallet {
  id: string;
  name: string;
  currencyCode: string;
}

export interface WalletListItem extends Wallet {
  currencyCode: string;
  currencyName: string;
  balance: number;
}

export interface CreateWalletPayload extends Pick<Wallet, 'name'> {
  accountId: Account['id'];
  currencyId: Currency['id'];
  startingBalance: number;
}
