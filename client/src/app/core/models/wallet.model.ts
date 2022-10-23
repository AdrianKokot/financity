export interface Wallet {
  id: string;
  name: string;
}

export interface WalletListItem extends Wallet {
  currencyCode: string;
  currencyName: string;
  balance: number;
}
