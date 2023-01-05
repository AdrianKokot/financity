import {
  ChangeDetectionStrategy,
  Component,
  HostBinding,
  Input,
} from '@angular/core';
import { WalletListItem } from '@shared/data-access/models/wallet.model';
import { UserService } from '../../../auth/data-access/api/user.service';

@Component({
  selector: 'app-wallet-list-item',
  templateUrl: './wallet-list-item.component.html',
  styleUrls: ['./wallet-list-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletListItemComponent {
  @HostBinding('class') hostClass = 'w-full d-block border-radius-m';
  @Input() wallet: WalletListItem | null = null;
  @Input() showSkeleton = false;

  userId = this._user.userSnapshot?.id;
  constructor(private _user: UserService) {}
}
