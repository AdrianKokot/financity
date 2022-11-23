import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  TuiButtonModule,
  TuiErrorModule,
  TuiLabelModule,
  TuiLinkModule,
  TuiTextfieldControllerModule,
  TuiTooltipModule,
} from '@taiga-ui/core';
import {
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputPasswordModule,
} from '@taiga-ui/kit';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthRoutingModule } from './auth-routing.module';
import { RegisterPageComponent } from './feature/register-page/register-page.component';
import { LoginPageComponent } from './feature/login-page/login-page.component';
import { EmailInputComponent } from './ui/email-input/email-input.component';
import { PasswordInputComponent } from './ui/password-input/password-input.component';
import { ResetPasswordPageComponent } from './feature/reset-password-page/reset-password-page.component';

@NgModule({
  declarations: [
    LoginPageComponent,
    RegisterPageComponent,
    EmailInputComponent,
    PasswordInputComponent,
    ResetPasswordPageComponent,
  ],
  imports: [
    CommonModule,
    TuiLabelModule,
    TuiInputModule,
    ReactiveFormsModule,
    TuiTooltipModule,
    TuiTextfieldControllerModule,
    AuthRoutingModule,
    TuiInputPasswordModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiButtonModule,
    TuiLinkModule,
  ],
})
export class AuthModule {}
