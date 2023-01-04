import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { filter, map, merge, Subject, switchMap, tap } from 'rxjs';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { CreateWalletComponent } from '../create-wallet/create-wallet.component';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { UserSettingsService } from '../../../user-settings/data-access/services/user-settings.service';
import { ActivatedRoute } from '@angular/router';

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
    this._walletService.getAll.bind(this._walletService),
    this.filters
  );

  readonly dialogs$ = merge(
    merge(
      this._activatedRoute.queryParamMap.pipe(
        filter(x => x.get('action') === 'create')
      ),
      this.ui.actions.create$
    ).pipe(
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
    private readonly _user: UserSettingsService,
    private readonly _activatedRoute: ActivatedRoute
  ) {}

  trackById = (index: number, item: { id: Wallet['id'] }) => item.id;
}
