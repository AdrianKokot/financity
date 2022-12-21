import {
  ChangeDetectionStrategy,
  Component,
  Input,
  ViewChild,
} from '@angular/core';
import {
  BehaviorSubject,
  debounceTime,
  distinctUntilChanged,
  EMPTY,
  filter,
  map,
  merge,
  Observable,
  scan,
  share,
  shareReplay,
  startWith,
  Subject,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { TuiContextWithImplicit, tuiIsString } from '@taiga-ui/cdk';
import { FormControl } from '@angular/forms';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-multi-select',
  templateUrl: './multi-select.component.html',
  styleUrls: ['./multi-select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MultiSelectComponent<T extends { id: string; name: string }> {
  @ViewChild(CdkVirtualScrollViewport) viewport!: CdkVirtualScrollViewport;

  @Input() getListFunction: (pagination: {
    page: number;
    pageSize: number;
    filters?: Record<string, string>;
  }) => Observable<T[]> = () => EMPTY;

  readonly search$ = new Subject<string>();

  readonly page$ = new BehaviorSubject<number>(1);
  private readonly _pageSize = 20;

  private readonly _filters$ = this.search$.pipe(
    startWith(''),
    distinctUntilChanged(),
    debounceTime(300),
    map(value => ({ name_ct: value }))
  );

  private readonly _api$ = this.page$.pipe(
    withLatestFrom(this._filters$),
    distinctUntilChangedObject(),
    switchMap(([page, filters]) =>
      this.getListFunction({
        page,
        pageSize: this._pageSize,
        filters,
      }).pipe(startWith(null))
    ),
    shareReplay(1)
  );

  readonly items$ = merge(
    this._api$.pipe(
      filter((x): x is T[] => x !== null),
      map(items => (acc: T[]) => [...acc, ...items])
    ),
    this._filters$.pipe(
      map(() => () => []),
      tap(() => this.page$.next(1))
    )
  ).pipe(
    scan((acc: T[], fn) => fn(acc), [] as T[]),
    startWith([]),
    share()
  );

  private readonly _allItems$ = this._api$.pipe(
    filter((x): x is T[] => x !== null),
    scan((acc: T[], val: T[]) => [...acc, ...val], [] as T[]),
    shareReplay(1)
  );

  gotAllResults = false;

  formControl = new FormControl([]);

  ids$ = this.items$.pipe(
    filter((x): x is T[] => x !== null),
    map(x => x.map(y => y.id)),
    startWith(null)
  );

  stringify$ = this._allItems$.pipe(
    filter((x): x is T[] => x !== null),
    map(
      items => new Map<string, string>(items.map(({ id, name }) => [id, name]))
    ),
    startWith(new Map<string, string>()),
    map(
      m => (id: TuiContextWithImplicit<string> | string) =>
        (tuiIsString(id) ? m.get(id) : m.get(id.$implicit)) || 'Loading...'
    )
  );

  // constructor() {
  //   this.formControl.valueChanges.pipe().subscribe(value => {
  //     console.log({ action: 'valChange', value });
  //   });
  // }

  log() {
    // console.log(this.viewport);
    if (this.gotAllResults) return;
    const { end } = this.viewport.getRenderedRange();
    const total = this.viewport.getDataLength();

    if (end === total) {
      this.page$.next(Math.floor(total / this._pageSize) + 1);
    }
  }

  //
  // readonly items$: Observable<readonly T[] | null> = this.search$.pipe(
  //   map(value => value === null ? '' : value),
  //   switchMap(search =>
  //     this.serverRequest(search).pipe(startWith<readonly User[] | null>(null))
  //   ),
  //   startWith(databaseMockData)
  // );

  // readonly testValue = new FormControl([databaseMockData[0]]);
  //

  //
  // /**
  //  * Server request emulation
  //  */
  // private serverRequest(
  //   searchQuery: string | null
  // ): Observable<readonly User[]> {
  //   const result = databaseMockData.filter(user =>
  //     TUI_DEFAULT_MATCHER(user, searchQuery || '')
  //   );
  //
  //   return of(result).pipe(delay(Math.random() * 1000 + 500));
  // }
}
