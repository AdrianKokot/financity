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
  TuiTabsModule,
  TuiToggleModule,
} from '@taiga-ui/kit';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [UserSettingsShellComponent, AppSettingsComponent],
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
  ],
})
export class UserSettingsModule {}
