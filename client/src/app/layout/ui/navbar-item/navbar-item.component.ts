import {
  ChangeDetectionStrategy,
  Component,
  Input,
  ViewEncapsulation,
} from '@angular/core';
import { EMPTY_NAV_ROUTE, NavRoute } from '@layout/data-access/nav-routes';

@Component({
  selector: 'app-navbar-item',
  templateUrl: './navbar-item.component.html',
  styleUrls: ['./navbar-item.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavbarItemComponent {
  @Input() route: NavRoute = EMPTY_NAV_ROUTE;
}
