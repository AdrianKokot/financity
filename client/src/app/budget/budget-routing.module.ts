import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BudgetsShellComponent } from './feature/budgets-shell/budgets-shell.component';
import { BudgetShellComponent } from './feature/budget-shell/budget-shell.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', component: BudgetsShellComponent },
      {
        path: ':id',
        component: BudgetShellComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BudgetRoutingModule {}
