import { AbstractControl, FormGroup } from '@angular/forms';
import {
  debounceTime,
  distinctUntilChanged,
  map,
  shareReplay,
  startWith,
} from 'rxjs';
import { distinctUntilChangedObject } from '@shared/utils/rxjs/distinct-until-changed-object';
import { TuiDayRange } from '@taiga-ui/cdk';
import { ApiFilters } from '../../../core/api/generic-api.service';

export class FiltersForm<
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  },
  TKey extends keyof TControl & string
> {
  readonly filters$ = this.form.valueChanges.pipe(
    startWith(null),
    debounceTime(300),
    map(() => this.form.getRawValue()),
    distinctUntilChangedObject(),
    map(data =>
      (Object.keys(data) as TKey[]).reduce((filters, key) => {
        const filterKey = this._getFilterKey(key);

        if (data[key] === null) {
          return filters;
        }

        if ((data[key] as unknown) instanceof TuiDayRange) {
          if (data[key].from)
            filters[`${filterKey}_gte`] = data[key].from.toJSON();

          if (data[key].to) filters[`${filterKey}_lte`] = data[key].to.toJSON();
        }

        if (
          (data[key] as unknown) instanceof Array<string> &&
          data[key].length > 0
        ) {
          filters[`${filterKey}_in`] = data[key];
        }

        if (typeof data[key] === 'string') {
          const trimmed = data[key].trim();
          if (trimmed.length > 0) {
            filters[filterKey] = trimmed;
          }
        }

        return filters;
      }, {} as ApiFilters)
    ),
    distinctUntilChangedObject(),
    shareReplay()
  );

  readonly filtersCount$ = this.filters$.pipe(
    map(
      filters => new Set(Object.keys(filters).map(x => x.split('_')[0])).size
    ),
    distinctUntilChanged(),
    shareReplay()
  );

  constructor(
    public readonly form: FormGroup<TControl>,
    private readonly _config: {
      [K in TKey]?: string;
    } = {}
  ) {}

  get controls() {
    return this.form.controls;
  }

  private _getFilterKey(key: TKey) {
    return key in this._config
      ? (this._config as Record<string, string>)[key]
      : key;
  }

  reset() {
    this.form.reset();
  }
}
