import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { CurrencyApiService } from '../../../../core/api/currency-api.service';
import { WalletApiService } from '../../../../core/api/wallet-api.service';

enum AccountType {
  Cash,
  Investment,
  Bank,
}

enum Currency {
  EUR,
  PLN,
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent {
  accountTypes = Object.values(AccountType);
  currencies = Object.values(Currency);

  currencies$ = this._currencyApi.getList();
  wallets$ = this._walletApi.getList();

  constructor(
    private _currencyApi: CurrencyApiService,
    private _walletApi: WalletApiService
  ) {}

  // wallets$: Observable<
  //   {
  //     id: number;
  //     type: AccountType;
  //     name: string;
  //     currency: Currency;
  //     balance: number;
  //   }[]
  // > = of([
  //   {
  //     id: 1,
  //     type: AccountType.Cash,
  //     name: 'Mój portfel',
  //     currency: Currency.PLN,
  //     balance: 2560,
  //   },
  //   {
  //     id: 2,
  //     type: AccountType.Bank,
  //     name: 'Santander',
  //     currency: Currency.PLN,
  //     balance: 2560,
  //   },
  //   {
  //     id: 3,
  //     type: AccountType.Investment,
  //     name: 'Obligacje bligacje bligacje',
  //     currency: Currency.PLN,
  //     balance: 2560,
  //   },
  //   {
  //     id: 4,
  //     type: AccountType.Investment,
  //     name: 'Obligacje',
  //     currency: Currency.PLN,
  //     balance: 2560,
  //   },
  //   {
  //     id: 5,
  //     type: AccountType.Investment,
  //     name: 'Obligacje',
  //     currency: Currency.PLN,
  //     balance: 2560,
  //   },
  //   {
  //     id: 6,
  //     type: AccountType.Bank,
  //     name: 'mBank',
  //     currency: Currency.PLN,
  //     balance: 250.12,
  //   },
  // ]);
}
