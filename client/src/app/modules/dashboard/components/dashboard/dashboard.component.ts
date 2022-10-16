import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { of } from 'rxjs';

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
  accounts$ = of([
    { type: AccountType.Cash, name: 'Cash in wallet', currency: Currency.EUR, balance: 2560 },
    { type: AccountType.Bank, name: 'mBank', currency: Currency.EUR, balance: 250 },
  ]);
}
