import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  constructor(http: HttpClient) {
    const start = new Date(2020, 0, 1);
    const end = new Date();

    // for (let i = 0; i < 300; i++)
    //   http
    //     .post('/api/transactions', {
    //       amount: Math.floor(Math.random() * 5000),
    //       recipientId: 'e6687bc7-0005-4d97-a661-f71491e1e0e1',
    //       walletId: '3c0edfc3-6229-4696-821a-dfea882e0f03',
    //       transactionType: 0,
    //       categoryId: '229a4252-a1bd-43a4-8e0b-c6c3f4d1e4ec',
    //       currencyId: '222a9676-2366-4a63-85c1-f892af3635bc',
    //       // labelIds: ['{{labelId}}'],
    //       transactionDate: new Date(
    //         start.getTime() + Math.random() * (end.getTime() - start.getTime())
    //       ).toISOString(),
    //       note: '',
    //     })
    //     .subscribe();
  }
}
