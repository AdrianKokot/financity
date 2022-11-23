import {
  ChangeDetectionStrategy,
  Component,
  HostBinding,
  Input,
} from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-email-input',
  templateUrl: './email-input.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmailInputComponent {
  @Input() control: FormControl = new FormControl('');
  @Input() placeholder = 'mail@example.com';
  @Input() label = 'Email address';

  @HostBinding('class') hostClass = 'd-block';
}
