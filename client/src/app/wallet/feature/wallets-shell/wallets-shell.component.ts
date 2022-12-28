import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  ViewEncapsulation,
} from '@angular/core';
import { BehaviorSubject, switchMap } from 'rxjs';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { CreateWalletComponent } from '../create-wallet/create-wallet.component';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';

@Component({
  selector: 'app-wallets-shell',
  templateUrl: './wallets-shell.component.html',
  styleUrls: ['./wallets-shell.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletsShellComponent {
  poll$ = new BehaviorSubject(true);
  wallets$ = this.poll$.pipe(switchMap(() => this._walletApi.getList()));

  constructor(
    private _walletApi: WalletApiService,
    @Inject(TuiDialogService) private _dialog: TuiDialogService
  ) {}

  openCreateWalletDialog() {
    this._dialog
      .open(new PolymorpheusComponent(CreateWalletComponent), {
        label: 'Create wallet',
      })
      .subscribe(() => {
        this.poll$.next(true);
      });
  }
}
