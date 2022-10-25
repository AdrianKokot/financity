import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { WalletApiService } from '../../../../core/api/wallet-api.service';
import { ActivatedRoute } from '@angular/router';
import { filter, map, switchMap } from 'rxjs';
import { TransactionApiService } from '../../../../core/api/transaction-api.service';

@Component({
  selector: 'app-wallet-dashboard',
  templateUrl: './wallet-dashboard.component.html',
  styleUrls: ['./wallet-dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletDashboardComponent {
  walletId$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id)
  );

  wallet$ = this.walletId$.pipe(
    switchMap(walletId => this._walletApiService.get(walletId))
  );

  transactions$ = this.walletId$.pipe(
    switchMap(walletId => this._transactionApiService.getList(walletId))
  );

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _walletApiService: WalletApiService,
    private _transactionApiService: TransactionApiService
  ) {}
}
