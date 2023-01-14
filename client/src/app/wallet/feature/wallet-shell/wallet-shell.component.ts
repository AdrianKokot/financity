import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WalletApiService } from '@shared/data-access/api/wallet-api.service';
import { UserService } from '../../../auth/data-access/api/user.service';

@Component({
  selector: 'app-wallet-shell',
  templateUrl: './wallet-shell.component.html',
  styleUrls: ['./wallet-shell.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletShellComponent {
  readonly wallet$ = this._walletService.get(
    this._activatedRoute.snapshot.params['id']
  );
  readonly userId = this._user.userId;

  constructor(
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _walletService: WalletApiService,
    private readonly _user: UserService
  ) {}
}
