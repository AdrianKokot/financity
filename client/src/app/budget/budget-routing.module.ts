import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BudgetsShellComponent } from './feature/budgets-shell/budgets-shell.component';

const routes: Routes = [
  {
    path: '',
    component: BudgetsShellComponent,
    // children: [
    //   {
    //     path: '',
    //     redirectTo: 'app',
    //     pathMatch: 'full',
    //   },
    //   {
    //     path: 'app',
    //     component: AppSettingsComponent,
    //   },
    // ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BudgetRoutingModule {}
