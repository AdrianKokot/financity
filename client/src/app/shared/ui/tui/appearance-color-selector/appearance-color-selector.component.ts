/* eslint-disable @typescript-eslint/no-empty-function */
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  HostBinding,
  Self,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiHostedDropdownModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { TUI_ARROW, TuiBadgedContentModule } from '@taiga-ui/kit';
import { PaletteItemDirective } from '@shared/ui/tui/palette-item.directive';
import {
  APPEARANCE_COLORS,
  APPEARANCE_COLORS_COUNT,
} from '@shared/ui/appearance';

@Component({
  selector: 'app-appearance-color-selector',
  standalone: true,
  imports: [
    CommonModule,
    TuiButtonModule,
    TuiHostedDropdownModule,
    TuiTextfieldControllerModule,
    PaletteItemDirective,
    TuiDataListModule,
    TuiBadgedContentModule,
  ],
  templateUrl: './appearance-color-selector.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppearanceColorSelectorComponent implements ControlValueAccessor {
  @HostBinding('class') hostClass = 'd-block';
  readonly items = APPEARANCE_COLORS;
  readonly gridSize = APPEARANCE_COLORS_COUNT;

  readonly arrow = TUI_ARROW;

  dropdownOpened = false;

  disabled = false;
  value: string | null = null;

  constructor(
    @Self() private _ngControl: NgControl,
    private _detector: ChangeDetectorRef
  ) {
    this._ngControl.valueAccessor = this;
  }

  onChange: (...args: unknown[]) => void = () => {};
  onTouched: (...args: unknown[]) => void = () => {};

  registerOnChange(fn: (...args: unknown[]) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: (...args: unknown[]) => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  writeValue(obj: string | null): void {
    this.value = obj;
    this.dropdownOpened = false;
    this.onChange(this.value);
    this._detector.markForCheck();
  }
}
