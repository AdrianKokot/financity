import { Injectable } from '@angular/core';
import { GenericApiService } from './generic-api.service';
import {
  CreateWalletPayload,
  Wallet,
  WalletListItem,
} from '../models/wallet.model';

@Injectable({
  providedIn: 'root',
})
export class WalletApiService extends GenericApiService {
  getList = this.cachedRequest(this.http.get<WalletListItem[]>('/api/wallets'));

  get(walletId: Wallet['id']) {
    return this.http.get('/api/wallets/' + walletId);
  }

  create(payload: CreateWalletPayload) {
    return this.http.post('/api/wallets', payload);
  }
}