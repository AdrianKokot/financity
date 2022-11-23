import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ResetPasswordAdapter } from './reset-password.adapter';

@Component({
  selector: 'app-reset-password-page',
  templateUrl: './reset-password-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ResetPasswordAdapter],
})
export class ResetPasswordPageComponent {
  constructor(public ui: ResetPasswordAdapter) {}
}
