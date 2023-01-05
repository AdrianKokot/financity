import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserSettingsShellComponent } from './feature/user-settings-shell/user-settings-shell.component';
import { AppSettingsComponent } from './feature/app-settings/app-settings.component';
import { AccountSettingsComponent } from './feature/account-settings/account-settings.component';

const routes: Routes = [
  {
    path: '',
    component: UserSettingsShellComponent,
    children: [
      {
        path: '',
        redirectTo: 'account',
        pathMatch: 'full',
      },
      {
        path: 'app',
        component: AppSettingsComponent,
      },
      {
        path: 'account',
        component: AccountSettingsComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserSettingsRoutingModule {}
