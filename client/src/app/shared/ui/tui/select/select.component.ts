import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AbstractSelectComponent } from '@shared/ui/tui/abstract-select/abstract-select.component';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectComponent<
  T extends { id: string; name: string }
> extends AbstractSelectComponent<T> {
  value: T['id'] | null = null;

  writeValue(value: T['id'] | null) {
    this.value = value;
    this.onChange(this.value);
  }
}
