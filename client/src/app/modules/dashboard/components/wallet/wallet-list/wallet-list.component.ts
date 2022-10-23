import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
  ViewEncapsulation,
} from '@angular/core';
import { WalletApiService } from '../../../../../core/api/wallet-api.service';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { CreateWalletDialogComponent } from '../create-wallet-dialog/create-wallet-dialog.component';

@Component({
  selector: 'app-wallet-list',
  templateUrl: './wallet-list.component.html',
  styleUrls: ['./wallet-list.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletListComponent {
  wallets$ = this._walletApi.getList();

  constructor(
    private _walletApi: WalletApiService,
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {}

  showDialog() {
    this._dialog
      .open(
        new PolymorpheusComponent(CreateWalletDialogComponent, this._injector)
      )
      .subscribe();
  }
}
