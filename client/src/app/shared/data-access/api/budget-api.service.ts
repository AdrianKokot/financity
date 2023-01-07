import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import {
  Budget,
  BudgetDetails,
  BudgetListItem,
  CreateBudgetPayload,
} from '@shared/data-access/models/budget.model';
import { UserSettingsService } from '../../../user-settings/data-access/services/user-settings.service';
import { ApiParams, GenericApiService } from './generic-api.service';

@Injectable({
  providedIn: 'root',
})
export class BudgetApiService extends GenericApiService<
  Budget,
  BudgetListItem,
  BudgetDetails,
  CreateBudgetPayload,
  Pick<CreateBudgetPayload, 'name' | 'amount' | 'trackedCategoriesId'> &
    Pick<Budget, 'id'>
> {
  constructor(http: HttpClient, private _user: UserSettingsService) {
    super(http, '/api/budgets');
  }

  override getAll(pagination: ApiParams) {
    return super
      .getAll({
        orderBy: 'name',
        direction: 'asc',
        ...pagination,
      })
      .pipe(
        tap(data => {
          if (pagination.page === 1 && !('search' in pagination)) {
            this._user.updateSettings({
              showSimplifiedBudgetView: data.length < 15,
            });
          }
        })
      );
  }
}
