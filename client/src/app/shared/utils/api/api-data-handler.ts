import { FiltersForm } from '@shared/utils/form/filters-form';
import {
  BehaviorSubject,
  distinctUntilChanged,
  filter,
  map,
  merge,
  NEVER,
  Observable,
  scan,
  share,
  shareReplay,
  startWith,
  switchMap,
  withLatestFrom,
} from 'rxjs';
import { AUTOCOMPLETE_PAGE_SIZE } from '@shared/data-access/constants/pagination.contants';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';
import { TransactionApiService } from '../../../core/api/transaction-api.service';

export class ApiDataHandler<T> {
  private readonly _page$ = new BehaviorSubject<number>(1);

  private readonly _api$ = merge(
    this._page$,
    this._filters.filters$.pipe(
      map(() => {
        this._page$.next(1);
        return NEVER;
      })
    )
  ).pipe(
    withLatestFrom(this._filters.filters$, this._page$),
    map(([, filters, page]) => ({
      page,
      filters,
      pageSize: AUTOCOMPLETE_PAGE_SIZE,
    })),
    distinctUntilChangedObject(),
    switchMap(pagination => this._getData(pagination).pipe(startWith(null))),
    shareReplay()
  );

  readonly apiLoading$ = this._api$.pipe(
    map(x => x === null),
    startWith(true),
    share()
  );

  readonly items$ = merge(
    this._api$.pipe(
      filter((x): x is T[] => x !== null),
      scan((acc: T[], data: T[]) => {
        if (this._page$.value === 1) {
          return [...data];
        }
        return [...acc, ...data];
      }, [] as T[])
    ),
    this._api$.pipe(
      filter((x): x is null => x === null && this._page$.value === 1)
    )
  ).pipe(distinctUntilChanged());

  constructor(
    private readonly _getData: (
      pagination: Parameters<
        InstanceType<typeof TransactionApiService>['getList']
      >[1]
    ) => Observable<T[]>,
    private readonly _filters: FiltersForm<any, any>
  ) {}

  page(val: number): void {
    this._page$.next(val);
  }

  resetPage(): void {
    this._page$.next(0);
    this._page$.next(1);
  }
}
