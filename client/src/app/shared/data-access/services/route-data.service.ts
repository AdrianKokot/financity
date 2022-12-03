import { Injectable } from '@angular/core';
import { ActivationEnd, Router } from '@angular/router';
import { filter, map, share } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RouteDataService {
  constructor(private _router: Router) {}

  data$ = this._router.events.pipe(
    filter((event): event is ActivationEnd => event instanceof ActivationEnd),
    filter((event: ActivationEnd) => event.snapshot.firstChild === null),
    map(event => {
      let route = event.snapshot;
      const data = Object.assign({}, route.data);

      while (route.parent) {
        route = route.parent;
        Object.assign(data, route.data);
      }

      return data;
    }),
    share()
  );
}
