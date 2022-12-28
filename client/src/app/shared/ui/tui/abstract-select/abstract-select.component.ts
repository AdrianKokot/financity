/* eslint-disable @typescript-eslint/no-empty-function */
import { Component, Input } from '@angular/core';
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  distinctUntilChanged,
  EMPTY,
  filter,
  map,
  merge,
  NEVER,
  Observable,
  scan,
  share,
  shareReplay,
  startWith,
  Subject,
  switchMap,
  withLatestFrom,
} from 'rxjs';
import { AUTOCOMPLETE_PAGE_SIZE } from '@shared/data-access/constants/pagination.contants';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';
import { TuiContextWithImplicit, tuiIsString } from '@taiga-ui/cdk';
import { FormControl } from '@angular/forms';

@Component({ template: '' })
export abstract class AbstractSelectComponent<
  T extends { id: string; name: string }
> {
  @Input() control: FormControl | null = null;
  @Input() label = '';
  @Input() preloadedResults: T[] = [];

  @Input() stringify = (item: T) => item.name;

  @Input() getListFunction: (pagination: {
    page: number;
    pageSize: number;
    filters?: Record<string, string | string[]>;
  }) => Observable<T[]> = () => EMPTY;

  externalFilters$ = new BehaviorSubject<Record<string, string | string[]>>({});

  @Input() set externalFilters(value: Record<string, string | string[]>) {
    this.externalFilters$.next(value);
  }

  @Input() searchBy = 'name_ct';

  readonly search$ = new Subject<string>();

  readonly page$ = new BehaviorSubject<number>(1);

  private readonly _filters$ = combineLatest([
    this.search$.pipe(startWith(''), distinctUntilChanged(), debounceTime(300)),
    this.externalFilters$,
  ]).pipe(
    map(([search, external]) => {
      const obj = { ...external };

      if (search) {
        obj[this.searchBy] = search;
      }

      return obj;
    }),
    shareReplay()
  );

  private readonly _api$ = merge(
    this.page$,
    this._filters$.pipe(
      map(() => {
        this.page$.next(1);
        return NEVER;
      })
    )
  ).pipe(
    withLatestFrom(this._filters$, this.page$),
    map(([, filters, page]) => ({
      page,
      filters,
      pageSize: AUTOCOMPLETE_PAGE_SIZE,
    })),
    distinctUntilChangedObject(),
    switchMap(pagination =>
      this.getListFunction(pagination).pipe(startWith(null))
    ),
    shareReplay()
  );

  readonly apiLoading$ = this._api$.pipe(
    map(x => x === null),
    share()
  );

  readonly items$: Observable<(T['id'] | null)[] | null> = merge(
    this._api$.pipe(
      filter((x): x is T[] => x !== null),
      map(x => x.map(y => y.id)),
      scan((acc: T['id'][], data: T['id'][]) => {
        if (this.page$.value === 1) {
          return [...data];
        }
        return [...acc, ...data];
      }, [] as T['id'][])
    ),
    this._api$.pipe(
      filter((x): x is null => x === null && this.page$.value === 1)
    )
  ).pipe(distinctUntilChanged());

  private readonly _allItems$ = this._api$.pipe(
    filter((x): x is T[] => x !== null),
    scan(
      (acc: T[], val: T[]) => [
        ...new Set([...acc, ...val, ...this.preloadedResults]),
      ],
      [] as T[]
    ),
    shareReplay(1)
  );

  stringify$ = this._allItems$.pipe(
    filter((x): x is T[] => x !== null),
    map(
      items =>
        new Map<string, string>(
          items.map(item => [item.id, this.stringify(item)])
        )
    ),
    startWith(new Map<string, string>()),
    map(
      m => (id: TuiContextWithImplicit<string> | string) =>
        (tuiIsString(id) ? m.get(id) : m.get(id.$implicit)) || 'Loading...'
    ),
    shareReplay(1)
  );
}
