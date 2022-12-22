import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
} from '@angular/core';
import {
  BehaviorSubject,
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
import { TuiContextWithImplicit, tuiIsString } from '@taiga-ui/cdk';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';
import { AUTOCOMPLETE_PAGE_SIZE } from '@shared/data-access/constants/pagination.contants';

@Component({
  selector: 'app-multi-select',
  templateUrl: './multi-select.component.html',
  styleUrls: ['./multi-select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MultiSelectComponent<T extends { id: string; name: string }>
  implements ControlValueAccessor
{
  @Input() label = '';

  @Input() preloadedResults: T[] = [];

  @Input() getListFunction: (pagination: {
    page: number;
    pageSize: number;
    filters?: Record<string, string>;
  }) => Observable<T[]> = () => EMPTY;

  readonly search$ = new Subject<string>();

  readonly page$ = new BehaviorSubject<number>(1);

  private readonly _filters$ = this.search$.pipe(
    startWith(''),
    distinctUntilChanged(),
    debounceTime(300),
    map(value => {
      const obj: Record<string, string> = {};

      if (value) {
        obj['name_ct'] = value;
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
    map(([_, filters, page]) => ({
      page,
      filters,
      pageSize: AUTOCOMPLETE_PAGE_SIZE,
    })),
    distinctUntilChangedObject(),
    switchMap(pagination =>
      this.getListFunction(pagination).pipe(startWith(null))
    ),
    share()
  );

  readonly items$ = this._api$.pipe(
    filter((x): x is T[] => x !== null),
    scan((acc: T[], data: T[]) => {
      if (this.page$.value === 1) {
        return [...data];
      }
      return [...acc, ...data];
    }, [] as T[]),
    shareReplay()
  );

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

  onChange: (...args: unknown[]) => void = () => {};
  onTouched: (...args: unknown[]) => void = () => {};

  disabled = false;

  value: T['id'][] = [];

  constructor(
    private readonly _detector: ChangeDetectorRef,
    private readonly _control: NgControl
  ) {
    _control.valueAccessor = this;
  }

  writeValue(value: T['id'][]) {
    this.value = value ?? [];
    this.onChange(this.value);
  }

  registerOnTouched(onTouched: (...args: unknown[]) => void) {
    this.onTouched = onTouched;
  }

  registerOnChange(onChange: (...args: unknown[]) => void) {
    this.onChange = onChange;
  }

  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
    this._detector.markForCheck();
  }
}
