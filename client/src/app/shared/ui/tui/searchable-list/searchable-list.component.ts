import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
} from '@angular/core';
import {
  EMPTY_ARRAY,
  TuiFilterPipeModule,
  TuiIdentityMatcher,
  TuiLetModule,
} from '@taiga-ui/cdk';
import {
  TUI_DATA_LIST_HOST,
  TUI_OPTION_CONTENT,
  TuiDataListComponent,
  TuiDataListModule,
  tuiIsEditingKey,
  TuiPrimitiveTextfieldModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { NgForOf, NgIf } from '@angular/common';
import {
  TUI_SELECT_OPTION,
  TuiInputModule,
  TuiMultiSelectModule,
} from '@taiga-ui/kit';
import { ControlValueAccessor, FormsModule, NgControl } from '@angular/forms';
import { TuiHandler } from '@taiga-ui/cdk/types';

@Component({
  selector: 'app-searchable-list',
  templateUrl: './searchable-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    TuiTextfieldControllerModule,
    TuiPrimitiveTextfieldModule,
    TuiDataListModule,
    TuiLetModule,
    TuiFilterPipeModule,
    NgIf,
    NgForOf,
    TuiMultiSelectModule,
    TuiInputModule,
    FormsModule,
  ],
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
export class SearchableListComponent<T extends { id: string; name: string }>
  implements ControlValueAccessor
{
  @Input()
  items: readonly T[] = [];

  value = '';
  onChange: (...args: unknown[]) => void = () => {};
  onTouched: (...args: unknown[]) => void = () => {};

  disabled = false;

  constructor(
    private readonly _detector: ChangeDetectorRef,
    private readonly _control: NgControl
  ) {
    _control.valueAccessor = this;
  }

  identityMatcher?: TuiIdentityMatcher<T> | undefined;
  readonly all = EMPTY_ARRAY;

  readonly filter = (
    item: T,
    search: string,
    stringify?: TuiHandler<T, string>
  ) => {
    return item.name.toLowerCase().includes(search.toLowerCase());
  };

  writeValue() {}

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

  onArrowDown<T>(list: TuiDataListComponent<T>, event: Event): void {
    list.onFocus(event, true);
  }

  onKeyDown(key: string, element: HTMLElement | null): void {
    if (element && tuiIsEditingKey(key)) {
      element.focus({ preventScroll: true });
    }
  }
}
