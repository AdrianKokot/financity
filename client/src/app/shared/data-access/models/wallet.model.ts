export interface Wallet {
  id: string;
  name: string;
  currencyId: string;
  startingAmount: number;
}

export interface WalletListItem extends Wallet {
  currencyId: string;
  currencyName: string;
  currentState: number;
}

export type CreateWalletPayload = Pick<
  Wallet,
  'name' | 'currencyId' | 'startingAmount'
>;
