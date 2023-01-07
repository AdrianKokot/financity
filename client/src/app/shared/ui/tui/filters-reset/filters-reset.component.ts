import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { TuiButtonModule } from '@taiga-ui/core';
import { TuiBadgeModule } from '@taiga-ui/kit';

@Component({
  selector: 'app-filters-reset',
  standalone: true,
  imports: [CommonModule, TuiButtonModule, TuiBadgeModule],
  templateUrl: './filters-reset.component.html',
  styleUrls: ['./filters-reset.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FiltersResetComponent {
  @Input() filtersCount: number | null = null;
  @Output() readonly resetFilters = new EventEmitter<void>();
}
