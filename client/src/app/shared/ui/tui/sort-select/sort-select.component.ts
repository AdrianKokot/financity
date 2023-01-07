/* eslint-disable @typescript-eslint/no-empty-function */
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  Self,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ControlValueAccessor,
  FormsModule,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { ApiSort } from '@shared/data-access/api/generic-api.service';
import { TuiDataListWrapperModule, TuiSelectModule } from '@taiga-ui/kit';
import { TuiTextfieldControllerModule } from '@taiga-ui/core';

export type SortSelectItem = ApiSort & { label: string };
export const DEFAULT_APP_SORT_SELECT_ITEMS: SortSelectItem[] = [
  { orderBy: 'name', direction: 'asc', label: 'Alphabetical' },
  { orderBy: 'name', direction: 'desc', label: 'Reverse-alphabetical' },
];

@Component({
  selector: 'app-sort-select',
  standalone: true,
  imports: [
    CommonModule,
    TuiSelectModule,
    TuiDataListWrapperModule,
    ReactiveFormsModule,
    TuiTextfieldControllerModule,
    FormsModule,
  ],
  templateUrl: './sort-select.component.html',
  styleUrls: ['./sort-select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SortSelectComponent implements ControlValueAccessor {
  @Input() items = DEFAULT_APP_SORT_SELECT_ITEMS;

  value: SortSelectItem | null = null;
  onTouched: (...args: unknown[]) => void = () => {};
  onChange: (...args: unknown[]) => void = () => {};

  constructor(
    @Self() private readonly _control: NgControl,
    private readonly _detector: ChangeDetectorRef
  ) {
    _control.valueAccessor = this;
  }

  writeValue(value: SortSelectItem | null): void {
    this.value = value;
    this.onChange(this.value);
    this._detector.markForCheck();
  }

  registerOnTouched(onTouched: (...args: unknown[]) => void) {
    this.onTouched = onTouched;
  }

  registerOnChange(onChange: (...args: unknown[]) => void) {
    this.onChange = onChange;
  }
}
