import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-wallet-labels',
  templateUrl: './wallet-labels.component.html',
  styleUrls: ['./wallet-labels.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class WalletLabelsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}