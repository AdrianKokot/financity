import { Currency } from '@shared/data-access/models/currency.model';
import { Category } from '@shared/data-access/models/category.model';

export interface Budget {
  id: string;
  name: string;
  amount: number;
  currentPeriodExpenses: number;
  currencyId: Currency['id'];
}

export interface BudgetDetails extends Budget {
  trackedCategories: Pick<
    Category,
    'id' | 'name' | 'walletId' | 'appearance' | 'transactionType'
  >[];
}

export type BudgetListItem = Budget;

export interface CreateBudgetPayload
  extends Pick<Budget, 'name' | 'amount' | 'currencyId'> {
  trackedCategoriesId: Category['id'][];
}
