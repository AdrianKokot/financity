import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  HostBinding,
  Input,
  Output,
} from '@angular/core';
import { BudgetListItem } from '@shared/data-access/models/budget.model';

@Component({
  selector: 'app-budget-list-item',
  templateUrl: './budget-list-item.component.html',
  styleUrls: ['./budget-list-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BudgetListItemComponent {
  @HostBinding('class') hostClass = 'w-full d-block border-radius-m';
  @Input() budget: BudgetListItem | null = null;
  @Input() showSkeleton = false;
  @Output() edit = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();
}
