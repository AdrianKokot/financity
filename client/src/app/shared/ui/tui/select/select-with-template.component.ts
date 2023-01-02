import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { SelectComponent } from '@shared/ui/tui/select/select.component';
import { PolymorpheusContent } from '@tinkoff/ng-polymorpheus';
import { TuiContextWithImplicit, tuiIsString } from '@taiga-ui/cdk';
import { filter, map, shareReplay, startWith } from 'rxjs';

@Component({
  selector: 'app-select-with-template',
  templateUrl: './select-with-template.component.html',
  styleUrls: ['./select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectWithTemplateComponent<
  T extends { id: string; name: string }
> extends SelectComponent<T> {
  @Input() itemTemplate: PolymorpheusContent<TuiContextWithImplicit<T>> =
    item => (item.$implicit !== null ? this.stringify(item.$implicit) : '');

  readonly stringifyWithItem$ = this.allItems$.pipe(
    filter((x): x is T[] => x !== null),
    map(
      items =>
        new Map<string, { item: T; text: string }>(
          items.map(item => {
            return [
              item.id,
              {
                text: this.stringify(item),
                item,
              },
            ];
          })
        )
    ),
    startWith(new Map<string, { item: T; text: string }>()),
    map(m => ({
      text: (id: TuiContextWithImplicit<string> | string) =>
        (tuiIsString(id) ? m.get(id)?.text : m.get(id.$implicit)?.text) ||
        (id === '' ? '' : 'Loading...'),
      item: (id: TuiContextWithImplicit<string> | string): T | null => {
        return tuiIsString(id)
          ? m.get(id)?.item || null
          : m.get(id.$implicit)?.item || null;
      },
    })),
    shareReplay(1)
  );
}
