import { Wallet } from '@shared/data-access/models/wallet.model';
import { Appearance } from '@shared/data-access/models/appearance';

export interface LabelListItem {
  id: string;
  name: string;
  walletId: Wallet['id'];
  appearance: Appearance;
}
