import {
  ChangeDetectionStrategy,
  Component,
  HostBinding,
  Input,
} from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-password-input',
  templateUrl: './password-input.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PasswordInputComponent {
  @Input() control: FormControl = new FormControl('');
  @Input() placeholder = '••••••••';
  @Input() label = 'Password';

  @HostBinding('class') hostClass = 'd-block';
}
