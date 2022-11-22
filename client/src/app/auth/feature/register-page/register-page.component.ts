import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RegisterPageAdapter } from './register-page.adapter';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [RegisterPageAdapter],
})
export class RegisterPageComponent {
  constructor(public ui: RegisterPageAdapter) {}
}
