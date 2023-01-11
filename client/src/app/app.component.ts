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
  readonly isNavbarVisible$ = this._route.data$.pipe(
    distinctUntilKeyChanged(RouteData.NAVBAR_VISIBLE),
    map(x => x[RouteData.NAVBAR_VISIBLE] ?? true)
  );

  readonly settings$ = this._settings.settings$;

  constructor(
    private readonly _route: RouteDataService,
    private readonly _settings: UserSettingsService
  ) {}
}
