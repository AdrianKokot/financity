import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { TuiDialogContext } from '@taiga-ui/core';
import {
  Transaction,
  TransactionListItem,
} from '@shared/data-access/models/transaction.model';
import { Wallet } from '@shared/data-access/models/wallet.model';

@Component({
  selector: 'app-transaction-details',
  templateUrl: './transaction-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionDetailsComponent {
  readonly wallet = this._context.data.wallet;
  readonly transaction = this._context.data.transaction;

  constructor(
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<
      Transaction,
      {
        wallet: Wallet;
        transaction: TransactionListItem;
      }
    >
  ) {}

  close() {
    this._context.$implicit.complete();
  }
}
