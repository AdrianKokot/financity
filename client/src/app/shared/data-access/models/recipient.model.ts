import { Wallet } from '@shared/data-access/models/wallet.model';

export interface Recipient {
  id: string;
  walletId: Wallet['id'];
  name: string;
}

export type RecipientListItem = Recipient;

export type CreateRecipientPayload = Pick<Recipient, 'name' | 'walletId'>;
