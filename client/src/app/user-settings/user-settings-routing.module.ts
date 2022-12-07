import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserSettingsShellComponent } from './feature/user-settings-shell/user-settings-shell.component';
import { AppSettingsComponent } from './feature/app-settings/app-settings.component';

const routes: Routes = [
  {
    path: '',
    component: UserSettingsShellComponent,
    children: [
      {
        path: '',
        redirectTo: 'app',
        pathMatch: 'full',
      },
      {
        path: 'app',
        component: AppSettingsComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserSettingsRoutingModule {}
