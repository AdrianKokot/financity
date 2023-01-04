import { ChangeDetectionStrategy, Component } from '@angular/core';
import { forkJoin, map, shareReplay } from 'rxjs';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { BudgetApiService } from '../../../core/api/budget-api.service';

@Component({
  selector: 'app-dashboard-shell',
  templateUrl: './dashboard-shell.component.html',
  styleUrls: ['./dashboard-shell.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardShellComponent {
  readonly ui$ = forkJoin([
    this._walletService
      .getAll({ page: 1, pageSize: 20 })
      .pipe(map(data => data.slice(0, 5))),
    this._budgetService
      .getAll({ page: 1, pageSize: 20 })
      .pipe(map(data => data.slice(0, 5))),
  ]).pipe(
    map(([wallets, budgets]) => ({
      wallets,
      budgets,
    })),
    shareReplay(1)
  );

  constructor(
    private readonly _walletService: WalletApiService,
    private readonly _budgetService: BudgetApiService
  ) {}
}
