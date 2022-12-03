import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from './feature/login-page/login-page.component';
import { RegisterPageComponent } from './feature/register-page/register-page.component';
import { GuestGuard } from './data-access/guards/guest.guard';
import { ResetPasswordPageComponent } from './feature/reset-password-page/reset-password-page.component';
import { AuthShellComponent } from './feature/auth-shell/auth-shell.component';
import { LogoutGuard } from './data-access/guards/logout.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'prefix',
  },
  {
    path: '',
    component: AuthShellComponent,
    canActivateChild: [GuestGuard],
    children: [
      {
        path: 'login',
        component: LoginPageComponent,
      },
      {
        path: 'register',
        component: RegisterPageComponent,
      },
      {
        path: 'reset-password',
        component: ResetPasswordPageComponent,
      },
    ],
  },
  {
    path: 'logout',
    canActivate: [LogoutGuard],
    children: [],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
