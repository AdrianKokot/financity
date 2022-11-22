import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-password-input',
  templateUrl: './password-input.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: 'd-block',
  },
})
export class PasswordInputComponent {
  @Input() control: FormControl = new FormControl('');
  @Input() placeholder = '••••••••';
  @Input() label = 'Password';
}
