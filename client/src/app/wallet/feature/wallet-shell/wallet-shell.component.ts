import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-wallet-shell',
  templateUrl: './wallet-shell.component.html',
  styleUrls: ['./wallet-shell.component.scss'],
  // encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletShellComponent {}
