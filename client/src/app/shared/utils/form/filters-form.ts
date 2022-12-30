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

export class FiltersForm<
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  },
  TKey extends keyof TControl & string
> {
  getFilterKey(key: TKey) {
    return key in this._config
      ? (this._config as Record<string, string>)[key]
      : key;
  }

  readonly filters$ = this.form.valueChanges.pipe(
    startWith(null),
    debounceTime(300),
    map(() => this.form.getRawValue()),
    distinctUntilChangedObject(),
    map(data =>
      (Object.keys(data) as TKey[]).reduce((filters, key) => {
        const filterKey = this.getFilterKey(key);

        if (data[key] === null) {
          return filters;
        }

        if ((data[key] as unknown) instanceof TuiDayRange) {
          if (data[key].from)
            filters[filterKey + '_gte'] = data[key].from.toJSON();

          if (data[key].to) filters[filterKey + '_lte'] = data[key].to.toJSON();
        }

        if (
          (data[key] as unknown) instanceof Array<string> &&
          data[key].length > 0
        ) {
          filters[filterKey + '_in'] = data[key];
        }

        if (typeof data[key] === 'string') {
          filters[filterKey] = data[key].trim();
        }

        return filters;
      }, {} as Record<string, string | string[]>)
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
    public form: FormGroup<TControl>,
    private _config: {
      [K in TKey]?: string;
    } = {}
  ) {}
}
