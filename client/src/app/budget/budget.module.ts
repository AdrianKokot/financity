import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BudgetsShellComponent } from './feature/budgets-shell/budgets-shell.component';
import { BudgetRoutingModule } from './budget-routing.module';
import { CreateBudgetComponent } from './feature/create-budget/create-budget.component';
import { TuiAutoFocusModule, TuiForModule, TuiLetModule } from '@taiga-ui/cdk';
import {
  TuiBadgeModule,
  TuiBreadcrumbsModule,
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputNumberModule,
  TuiItemsWithMoreModule,
  TuiMarkerIconModule,
} from '@taiga-ui/kit';
import {
  TuiButtonModule,
  TuiErrorModule,
  TuiGroupModule,
  TuiLabelModule,
  TuiLinkModule,
  TuiLoaderModule,
  TuiScrollbarModule,
  TuiSvgModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CdkVirtualForOf, ScrollingModule } from '@angular/cdk/scrolling';
import { InfiniteVirtualScrollModule } from '@shared/ui/infinite-virtual-scroll/infinite-virtual-scroll.module';
import { TuiTableModule } from '@taiga-ui/addon-table';
import {
  TuiCurrencyPipeModule,
  TuiMoneyModule,
} from '@taiga-ui/addon-commerce';
import { SelectModule } from '@shared/ui/tui/select/select.module';
import { MultiSelectModule } from '@shared/ui/tui/multi-select/multi-select.module';
import { RequireConfirmationDirective } from '@shared/utils/directives/require-confirmation.directive';
import { UpdateBudgetComponent } from './feature/update-budget/update-budget.component';
import { BudgetShellComponent } from './feature/budget-shell/budget-shell.component';
import { BudgetListItemComponent } from './ui/budget-list-item/budget-list-item.component';

@NgModule({
  declarations: [
    BudgetsShellComponent,
    CreateBudgetComponent,
    UpdateBudgetComponent,
    BudgetShellComponent,
    BudgetListItemComponent,
  ],
  imports: [
    CommonModule,
    BudgetRoutingModule,
    TuiLetModule,
    TuiInputModule,
    TuiTextfieldControllerModule,
    ReactiveFormsModule,
    TuiButtonModule,
    TuiBadgeModule,
    TuiScrollbarModule,
    ScrollingModule,
    InfiniteVirtualScrollModule,
    TuiTableModule,
    CdkVirtualForOf,
    TuiMoneyModule,
    TuiLoaderModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiLabelModule,
    TuiAutoFocusModule,
    SelectModule,
    TuiGroupModule,
    TuiInputNumberModule,
    TuiCurrencyPipeModule,
    MultiSelectModule,
    RequireConfirmationDirective,
    TuiSvgModule,
    TuiBreadcrumbsModule,
    TuiLinkModule,
    TuiItemsWithMoreModule,
    TuiMarkerIconModule,
    TuiForModule,
  ],
})
export class BudgetModule {}
