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
  APPEARANCE_ICONS,
  APPEARANCE_ICONS_COLUMNS_COUNT,
} from '@shared/ui/appearance';
import {
  TUI_ARROW,
  TuiBadgedContentModule,
  TuiMarkerIconModule,
} from '@taiga-ui/kit';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiHostedDropdownModule,
  TuiSvgModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { PaletteItemDirective } from '@shared/ui/tui/palette-item.directive';

@Component({
  selector: 'app-appearance-icon-selector',
  standalone: true,
  imports: [
    CommonModule,
    TuiBadgedContentModule,
    TuiDataListModule,
    PaletteItemDirective,
    TuiTextfieldControllerModule,
    TuiHostedDropdownModule,
    TuiButtonModule,
    TuiSvgModule,
    TuiMarkerIconModule,
  ],
  templateUrl: './appearance-icon-selector.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppearanceIconSelectorComponent implements ControlValueAccessor {
  @HostBinding('class') hostClass = 'd-block';
  readonly items = APPEARANCE_ICONS;
  readonly gridSize = APPEARANCE_ICONS_COLUMNS_COUNT;

  readonly arrow = TUI_ARROW;

  dropdownOpened = false;

  disabled = false;
  value: string | null = null;

  constructor(
    @Self() private readonly _ngControl: NgControl,
    private readonly _detector: ChangeDetectorRef
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
