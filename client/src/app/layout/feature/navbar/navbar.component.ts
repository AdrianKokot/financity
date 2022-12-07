import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
} from '@angular/core';
import {
  AppNavRoutes,
  UserRelatedRoutes,
} from '@layout/data-access/nav-routes';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavbarComponent {
  appRoutes = AppNavRoutes;
  userRoutes = UserRelatedRoutes;
}
