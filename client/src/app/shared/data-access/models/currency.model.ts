export interface Currency {
  id: string;
}

export interface CurrencyListItem extends Currency {
  name: string;
  code: string;
}
