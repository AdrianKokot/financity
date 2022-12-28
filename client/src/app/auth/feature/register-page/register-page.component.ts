import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Validators } from '@angular/forms';
import { AuthService } from '../../data-access/api/auth.service';
import { Router } from '@angular/router';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterPageComponent {
  readonly form = this._fb.form(
    {
      email: ['', [Validators.email, Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      name: ['', [Validators.required, Validators.maxLength(128)]],
    },
    {
      submit: payload => this._auth.register(payload),
      effect: succeeded =>
        succeeded && this._router.navigateByUrl('/dashboard'),
    }
  );

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _auth: AuthService,
    private readonly _router: Router
  ) {}
}
