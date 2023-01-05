import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
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
  loadingValue = '';
  @Output() addClick = new EventEmitter<string>();
  @Input() showAddButton = false;
}
