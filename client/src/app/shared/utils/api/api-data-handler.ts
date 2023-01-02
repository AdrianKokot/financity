import {
  BehaviorSubject,
  distinctUntilChanged,
  filter,
  map,
  merge,
  NEVER,
  Observable,
  of,
  scan,
  share,
  shareReplay,
  skipUntil,
  startWith,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { ApiParams } from '../../../core/api/generic-api.service';
import { AUTOCOMPLETE_PAGE_SIZE } from '@shared/data-access/constants/pagination.contants';
import { AbstractControl } from '@angular/forms';
import { FiltersForm } from '@shared/utils/form/filters-form';

export class ApiDataHandler<
  T,
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  },
  TKey extends keyof TControl & string
> {
  private readonly _page$ = new BehaviorSubject<number>(1);
  private readonly _reload$ = new BehaviorSubject<boolean>(true);

  constructor(
    private readonly _getData: (pagination: ApiParams) => Observable<T[]>,
    private readonly _filters: FiltersForm<TControl, TKey>,
    private readonly _started$: Observable<unknown> = of(true)
  ) {}

  private readonly _api$ = merge(
    this._page$,
    merge(this._filters.filters$, this._reload$.pipe(filter(x => x))).pipe(
      tap(() => this._page$.next(1)),
      switchMap(() => NEVER)
    )
  ).pipe(
    skipUntil(this._started$),
    withLatestFrom(this._filters.filters$, this._page$),
    map(([, filters, page]) => ({
      pagination: {
        ...filters,
        page,
        pageSize: AUTOCOMPLETE_PAGE_SIZE,
      },
      reload: this._reload$.value,
    })),
    distinctUntilChanged((previous, current) => {
      return current.reload
        ? false
        : JSON.stringify(previous.pagination) ===
            JSON.stringify(current.pagination);
    }),
    tap(() => {
      this._reload$.next(false);
    }),
    switchMap(({ pagination }) =>
      this._getData(pagination).pipe(startWith(null))
    ),
    shareReplay()
  );

  readonly apiLoading$ = this._api$.pipe(
    map(x => x === null),
    startWith(true),
    share()
  );

  readonly items$: Observable<T[] | null> = merge(
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

  page(val: number): void {
    this._page$.next(val);
  }

  resetPage(): void {
    this._reload$.next(true);
  }
}
