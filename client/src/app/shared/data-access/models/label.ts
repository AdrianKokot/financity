import { Wallet } from '@shared/data-access/models/wallet.model';
import { Appearance } from '@shared/data-access/models/appearance';

export interface Label {
  id: string;
  name: string;
  walletId: Wallet['id'];
  appearance: Appearance;
}

export type LabelListItem = Label;

export type CreateLabelPayload = Pick<
  Label,
  'name' | 'appearance' | 'walletId'
>;
