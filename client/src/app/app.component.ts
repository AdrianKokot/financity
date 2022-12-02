import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { combineLatest, filter, map } from 'rxjs';
import { WalletListItem } from '@shared/data-access/models/wallet.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  constructor(private _http: HttpClient) {
    this._http
      .get<unknown[]>('/api/transactions?pageSize=1')
      .pipe(filter(x => x.length == 0))
      .subscribe(() => this.seedData());
  }

  seedData(): void {
    const start = new Date(2020, 0, 1);
    const end = new Date();

    combineLatest([
      this._http.get<WalletListItem[]>('/api/wallets?pageSize=1').pipe(
        map(x => ({
          walletId: x[0].id,
          currencyId: x[0].currencyId,
        }))
      ),

      this._http
        .get<{ id: string }[]>('/api/categories?pageSize=1')
        .pipe(map(x => x[0].id)),

      this._http
        .get<{ id: string }[]>('/api/recipients?pageSize=1')
        .pipe(map(x => x[0].id)),

      this._http
        .get<{ id: string }[]>('/api/labels?pageSize=1')
        .pipe(map(x => x[0].id)),
    ]).subscribe(
      ([{ walletId, currencyId }, categoryId, recipientId, labelId]) => {
        for (let i = 0; i < 300; i++)
          this._http
            .post('/api/transactions', {
              amount: Math.floor(Math.random() * 500),
              recipientId,
              walletId,
              transactionType: 0,
              categoryId,
              currencyId,
              labelIds: [labelId],
              transactionDate: new Date(
                start.getTime() +
                  Math.random() * (end.getTime() - start.getTime())
              ).toISOString(),
              note: '',
            })
            .subscribe();
      }
    );
  }
}
