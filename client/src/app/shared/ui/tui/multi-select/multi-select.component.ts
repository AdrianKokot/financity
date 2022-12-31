import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { AbstractSelectComponent } from '@shared/ui/tui/abstract-select/abstract-select.component';
import { filter, map, share, shareReplay, startWith } from 'rxjs';
import { TuiContextWithImplicit, tuiIsString } from '@taiga-ui/cdk';

@Component({
  selector: 'app-multi-select',
  templateUrl: './multi-select.component.html',
  styleUrls: ['./multi-select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MultiSelectComponent<
  T extends { id: string; name: string }
> extends AbstractSelectComponent<T> {
  @Input() tagStringify = (item: T) => item.name;

  private _multipleStringify$ = this.allItems$.pipe(
    filter((x): x is T[] => x !== null),
    map(
      items =>
        new Map<string, { stringify: string; tagStringify: string }>(
          items.map(item => [
            item.id,
            {
              stringify: this.stringify(item),
              tagStringify: this.tagStringify(item),
            },
          ])
        )
    ),
    startWith(new Map<string, { stringify: string; tagStringify: string }>()),
    share()
  );

  override stringify$ = this._multipleStringify$.pipe(
    map(
      m => (id: TuiContextWithImplicit<string> | string) =>
        (tuiIsString(id)
          ? m.get(id)?.stringify
          : m.get(id.$implicit)?.stringify) || (id === '' ? '' : 'Loading...')
    ),
    shareReplay(1)
  );

  tagStringify$ = this._multipleStringify$.pipe(
    map(
      m => (id: TuiContextWithImplicit<string> | string) =>
        (tuiIsString(id)
          ? m.get(id)?.tagStringify
          : m.get(id.$implicit)?.tagStringify) ||
        (id === '' ? '' : 'Loading...')
    ),
    shareReplay(1)
  );
}
