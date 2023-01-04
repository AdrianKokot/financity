import {
  ChangeDetectionStrategy,
  Component,
  HostBinding,
  Input,
} from '@angular/core';
import { BudgetListItem } from '@shared/data-access/models/budget.model';

@Component({
  selector: 'app-budget-list-item-no-actions',
  templateUrl: './budget-list-item-no-actions.component.html',
  styleUrls: ['./budget-list-item-no-actions.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BudgetListItemNoActionsComponent {
  @HostBinding('class') hostClass = 'w-full d-block border-radius-m';
  @Input() budget: BudgetListItem | null = null;
  @Input() showSkeleton = false;
}
