export interface Transaction {
  id: string;
}

export interface TransactionListItem extends Transaction {
  currencyCode: string;
  currencyName: string;
  currencyId: string;
}
