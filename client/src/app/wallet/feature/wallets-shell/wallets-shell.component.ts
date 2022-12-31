import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { map, merge, Subject, switchMap, tap } from 'rxjs';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { CreateWalletComponent } from '../create-wallet/create-wallet.component';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { UserSettingsService } from '../../../user-settings/data-access/services/user-settings.service';

@Component({
  selector: 'app-wallets-shell',
  templateUrl: './wallets-shell.component.html',
  styleUrls: ['./wallets-shell.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletsShellComponent {
  readonly ui = {
    columns: ['name', 'amount', 'actions'] as const,
    actions: {
      create$: new Subject<void>(),
    },
    showSimplifiedView$: this._user.settings$.pipe(
      map(x => x.showSimplifiedWalletView)
    ),
  };

  readonly filters = this._fb.filters({
    search: [''],
  });

  readonly data = new ApiDataHandler(
    this._walletService.getList.bind(this._walletService),
    this.filters
  );

  readonly dialogs$ = merge(
    this.ui.actions.create$.pipe(
      switchMap(() =>
        this._dialog.open(new PolymorpheusComponent(CreateWalletComponent), {
          label: 'Create wallet',
        })
      )
    )
  ).pipe(tap(() => this.data.resetPage()));

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _walletService: WalletApiService,
    @Inject(Injector)
    private readonly _injector: Injector,
    @Inject(TuiDialogService)
    private readonly _dialog: TuiDialogService,
    private readonly _user: UserSettingsService
  ) {}

  trackById = (index: number, item: { id: Wallet['id'] }) => item.id;
}
