import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { WalletApiService } from '../../../../../core/api/wallet-api.service';
import { CreateWalletDialogComponent } from '../create-wallet-dialog/create-wallet-dialog.component';
import { BehaviorSubject, switchMap } from 'rxjs';
import { DialogService } from '@shared/utils/services/dialog.service';

@Component({
  selector: 'app-wallet-list',
  templateUrl: './wallet-list.component.html',
  styleUrls: ['./wallet-list.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletListComponent {
  poll$ = new BehaviorSubject(true);
  wallets$ = this.poll$.pipe(switchMap(() => this._walletApi.getList()));

  constructor(
    private _walletApi: WalletApiService,
    private _dialog: DialogService
  ) {}

  showDialog() {
    this._dialog.open(CreateWalletDialogComponent).subscribe(() => {
      this.poll$.next(true);
    });
  }
}
