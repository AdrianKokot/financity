import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { BehaviorSubject, switchMap } from 'rxjs';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { DialogService } from '@shared/utils/services/dialog.service';

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
    private _dialog: DialogService
  ) {}

  openCreateWalletDialog() {
    // this._dialog.open(CreateWalletDialogComponent).subscribe(() => {
    //   this.poll$.next(true);
    // });
  }
}
