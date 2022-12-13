/* eslint-disable @typescript-eslint/no-empty-function */
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  HostBinding,
  Input,
} from '@angular/core';
import { TUI_SELECT_OPTION, TuiDataListWrapperModule } from '@taiga-ui/kit';
import {
  TUI_DATA_LIST_HOST,
  TUI_OPTION_CONTENT,
  TuiDataListHost,
  TuiHostedDropdownModule,
  TuiSizeL,
  TuiSizeXS,
  TuiValueContentContext,
} from '@taiga-ui/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { PolymorpheusContent } from '@tinkoff/ng-polymorpheus';

@Component({
  selector: 'app-dropdown-select',
  templateUrl: './dropdown-select.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [TuiDataListWrapperModule, TuiHostedDropdownModule],
  providers: [
    {
      provide: TUI_DATA_LIST_HOST,
      useExisting: DropdownSelectComponent,
    },
    {
      provide: TUI_OPTION_CONTENT,
      useValue: TUI_SELECT_OPTION,
    },
  ],
})
export class DropdownSelectComponent<T>
  implements TuiDataListHost<T>, ControlValueAccessor
{
  @Input()
  items: readonly T[] = [];

  @Input() size: TuiSizeXS | TuiSizeL = 'xs';

  @Input() allowDeselect = false;
  @Input() emptyValue: null | '' | undefined = null;

  @Input()
  itemContent: PolymorpheusContent<TuiValueContentContext<T>> = null;

  @HostBinding('attr.data-dropdown-opened')
  open = false;

  onChange: (...args: unknown[]) => void = () => {};
  onTouched: (...args: unknown[]) => void = () => {};

  disabled = false;

  constructor(
    private readonly _detector: ChangeDetectorRef,
    private readonly _control: NgControl
  ) {
    _control.valueAccessor = this;
  }

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

  handleOption(option: T) {
    this.open = false;

    if (this.allowDeselect && this._control.value === option) {
      this.onChange(this.emptyValue);
    } else {
      this.onChange(option);
    }
  }

  onFocusedChange(focused: boolean) {
    if (!focused) {
      this.onTouched();
    }
  }
}
