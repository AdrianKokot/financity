import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';

@Component({
  selector: 'app-wallet-categories',
  templateUrl: './wallet-categories.component.html',
  styleUrls: ['./wallet-categories.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletCategoriesComponent {}
