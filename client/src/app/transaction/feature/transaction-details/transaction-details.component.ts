import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import {
  Transaction,
  TransactionListItem,
} from '@shared/data-access/models/transaction.model';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { WalletApiService } from '@shared/data-access/api/wallet-api.service';
import { of, startWith } from 'rxjs';
import { UserService } from '../../../auth/data-access/api/user.service';
import { TransactionType } from '@shared/data-access/models/transaction-type.enum';

@Component({
  selector: 'app-transaction-details',
  templateUrl: './transaction-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionDetailsComponent {
  readonly transaction = this._context.data.transaction;
  readonly wallet$ =
    this._context.data.wallet !== undefined
      ? of(this._context.data.wallet)
      : this._walletService
          .get(this.transaction.walletId)
          .pipe(startWith(null));

  readonly userId = this._user.userId;
  readonly transactionType = TransactionType;

  constructor(
    private readonly _walletService: WalletApiService,
    private readonly _user: UserService,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Transaction,
      {
        wallet?: Wallet;
        transaction: TransactionListItem;
      }
    >
  ) {}

  close() {
    this._context.$implicit.complete();
  }
}
