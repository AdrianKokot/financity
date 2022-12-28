import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from './auth-routing.module';
import { RegisterPageComponent } from './feature/register-page/register-page.component';
import { LoginPageComponent } from './feature/login-page/login-page.component';
import { ResetPasswordPageComponent } from './feature/reset-password-page/reset-password-page.component';
import { AuthShellComponent } from './feature/auth-shell/auth-shell.component';
import {
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputPasswordModule,
  TuiIslandModule,
} from '@taiga-ui/kit';
import { ReactiveFormsModule } from '@angular/forms';
import {
  TuiButtonModule,
  TuiErrorModule,
  TuiLabelModule,
  TuiLinkModule,
  TuiNotificationModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';

@NgModule({
  declarations: [
    LoginPageComponent,
    RegisterPageComponent,
    ResetPasswordPageComponent,
    AuthShellComponent,
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    TuiIslandModule,
    ReactiveFormsModule,
    TuiLabelModule,
    TuiInputModule,
    TuiTextfieldControllerModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiInputPasswordModule,
    TuiButtonModule,
    TuiLinkModule,
    TuiNotificationModule,
  ],
})
export class AuthModule {}
