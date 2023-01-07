import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { filter, map, merge, Subject, switchMap, tap } from 'rxjs';
import { WalletApiService } from '@shared/data-access/api/wallet-api.service';
import { CreateWalletComponent } from '../create-wallet/create-wallet.component';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../auth/data-access/api/user.service';
import { DEFAULT_APP_SORT_SELECT_ITEMS } from '@shared/ui/tui/sort-select/sort-select.component';

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
    userId: this._user.userSnapshot?.id,
  };

  readonly filters = this._fb.filters({
    search: [''],
    sort: [DEFAULT_APP_SORT_SELECT_ITEMS[0]],
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
    private readonly _user: UserService,
    private readonly _activatedRoute: ActivatedRoute
  ) {}

  trackById = (index: number, item: { id: Wallet['id'] }) => item.id;
}
