import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { LoginPageAdapter } from './login-page.adapter';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-page.component.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [LoginPageAdapter],
})
export class LoginPageComponent {
  constructor(public ui: LoginPageAdapter) {}
}
