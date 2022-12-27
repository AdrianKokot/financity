import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import { distinctUntilKeyChanged, map } from 'rxjs';
import { RouteDataService } from '@shared/data-access/services/route-data.service';
import { RouteData } from '@shared/utils/toggles/route-data';
import { UserSettingsService } from './user-settings/data-access/services/user-settings.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  isNavbarVisible$ = this._route.data$.pipe(
    distinctUntilKeyChanged(RouteData.NAVBAR_VISIBLE),
    map(x => x[RouteData.NAVBAR_VISIBLE] ?? true)
  );

  settings$ = this._settings.settings$;

  constructor(
    private _route: RouteDataService,
    private _settings: UserSettingsService
  ) {}
}
