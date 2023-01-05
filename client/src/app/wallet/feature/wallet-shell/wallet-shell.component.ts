import { ChangeDetectionStrategy, Component } from '@angular/core';
import { filter, map, shareReplay, switchMap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { UserService } from '../../../auth/data-access/api/user.service';

@Component({
  selector: 'app-wallet-shell',
  templateUrl: './wallet-shell.component.html',
  styleUrls: ['./wallet-shell.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletShellComponent {
  wallet$ = this._activatedRoute.params.pipe(
    filter((params): params is { id: string } => 'id' in params),
    map(params => params.id),
    switchMap(id => this._walletService.get(id)),
    shareReplay(1)
  );

  userId = this._user.userSnapshot?.id;

  constructor(
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _walletService: WalletApiService,
    private readonly _user: UserService
  ) {}
}
