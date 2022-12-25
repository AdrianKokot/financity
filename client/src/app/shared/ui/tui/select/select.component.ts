import { ChangeDetectionStrategy, Component, HostBinding } from '@angular/core';
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
  @HostBinding('class') set hostClass(value: string) {
    console.log(`set classes: ${value}`);
    this.shouldAddGroupClass = value.includes('tui-group__inherit-item');
  }

  shouldAddGroupClass = false;
  value: T['id'] | null = null;

  writeValue(value: T['id'] | null) {
    this.value = value;
    this.onChange(this.value);
  }
}
