import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  TuiButtonModule,
  TuiErrorModule,
  TuiLabelModule,
  TuiLinkModule,
  TuiNotificationModule,
  TuiTextfieldControllerModule,
  TuiTooltipModule,
} from '@taiga-ui/core';
import {
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputPasswordModule,
  TuiIslandModule,
} from '@taiga-ui/kit';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthRoutingModule } from './auth-routing.module';
import { RegisterPageComponent } from './feature/register-page/register-page.component';
import { LoginPageComponent } from './feature/login-page/login-page.component';
import { ResetPasswordPageComponent } from './feature/reset-password-page/reset-password-page.component';
import { AuthShellComponent } from './feature/auth-shell/auth-shell.component';

@NgModule({
  declarations: [
    LoginPageComponent,
    RegisterPageComponent,
    ResetPasswordPageComponent,
    AuthShellComponent,
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
    TuiNotificationModule,
    TuiIslandModule,
  ],
})
export class AuthModule {}
