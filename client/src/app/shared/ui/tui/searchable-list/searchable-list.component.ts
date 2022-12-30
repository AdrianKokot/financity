import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  TUI_DATA_LIST_HOST,
  TUI_OPTION_CONTENT,
  TuiDataListComponent,
  tuiIsEditingKey,
} from '@taiga-ui/core';
import { AbstractSelectComponent } from '@shared/ui/tui/abstract-select/abstract-select.component';
import { TUI_SELECT_OPTION } from '@taiga-ui/kit';

@Component({
  selector: 'app-searchable-list',
  templateUrl: './searchable-list.component.html',
  styleUrls: ['./searchable-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: TUI_DATA_LIST_HOST,
      useExisting: SearchableListComponent,
    },
    {
      provide: TUI_OPTION_CONTENT,
      useValue: TUI_SELECT_OPTION,
    },
  ],
})
export class SearchableListComponent<
  T extends { id: string; name: string }
> extends AbstractSelectComponent<T> {
  onArrowDown<T>(list: TuiDataListComponent<T>, event: Event): void {
    list.onFocus(event, true);
  }

  onKeyDown(key: string, element: HTMLElement | null): void {
    if (element && tuiIsEditingKey(key)) {
      element.focus({ preventScroll: true });
    }
  }
}
