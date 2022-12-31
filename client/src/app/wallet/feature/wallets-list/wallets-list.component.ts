import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { map, merge, startWith, Subject, switchMap } from 'rxjs';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { CreateWalletComponent } from '../create-wallet/create-wallet.component';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { TuiDialogService } from '@taiga-ui/core';
import { Wallet } from '@shared/data-access/models/wallet.model';

@Component({
  selector: 'app-wallets-list',
  templateUrl: './wallets-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletsListComponent {
  readonly ui = {
    actions: {
      create$: new Subject<void>(),
    },
  };

  private readonly _dialogs$ = merge(
    this.ui.actions.create$.pipe(
      switchMap(() =>
        this._dialog.open(new PolymorpheusComponent(CreateWalletComponent), {
          label: 'Create wallet',
        })
      )
    )
  ).pipe(map(() => null));

  readonly items$ = this._dialogs$.pipe(
    startWith(null),
    switchMap(() =>
      this._walletService
        .getList({
          page: 1,
          pageSize: 20,
        })
        .pipe(startWith(null))
    )
  );

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _walletService: WalletApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  trackById = (index: number, item: { id: Wallet['id'] }) => item.id;
}
