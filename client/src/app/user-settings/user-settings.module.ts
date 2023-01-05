import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserSettingsRoutingModule } from './user-settings-routing.module';
import { UserSettingsShellComponent } from './feature/user-settings-shell/user-settings-shell.component';
import { AppSettingsComponent } from './feature/app-settings/app-settings.component';
import {
  TuiButtonModule,
  TuiErrorModule,
  TuiLabelModule,
  TuiLinkModule,
  TuiSvgModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import {
  TuiFieldErrorPipeModule,
  TuiInputCountModule,
  TuiInputModule,
  TuiInputPasswordModule,
  TuiTabsModule,
  TuiToggleModule,
} from '@taiga-ui/kit';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccountSettingsComponent } from './feature/account-settings/account-settings.component';

@NgModule({
  declarations: [
    UserSettingsShellComponent,
    AppSettingsComponent,
    AccountSettingsComponent,
  ],
  imports: [
    CommonModule,
    UserSettingsRoutingModule,
    TuiSvgModule,
    TuiTabsModule,
    TuiInputCountModule,
    FormsModule,
    TuiTextfieldControllerModule,
    TuiLabelModule,
    TuiInputModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiButtonModule,
    TuiLinkModule,
    TuiToggleModule,
    ReactiveFormsModule,
    TuiInputPasswordModule,
  ],
})
export class UserSettingsModule {}
