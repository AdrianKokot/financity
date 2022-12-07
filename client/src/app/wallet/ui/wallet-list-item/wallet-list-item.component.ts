import {
  ChangeDetectionStrategy,
  Component,
  HostBinding,
  Input,
  ViewEncapsulation,
} from '@angular/core';
import { WalletListItem } from '@shared/data-access/models/wallet.model';

@Component({
  selector: 'app-wallet-list-item',
  templateUrl: './wallet-list-item.component.html',
  styleUrls: ['./wallet-list-item.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletListItemComponent {
  @HostBinding('class') hostClass = 'w-full d-block border-radius-m';
  @Input() wallet: WalletListItem | null = null;
  @Input() showSkeleton = false;
}
