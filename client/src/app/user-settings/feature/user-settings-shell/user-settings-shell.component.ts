import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-user-settings-shell',
  templateUrl: './user-settings-shell.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserSettingsShellComponent {}
