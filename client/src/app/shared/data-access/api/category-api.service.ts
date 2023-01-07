import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  Category,
  CategoryListItem,
  CreateCategoryPayload,
} from '@shared/data-access/models/category.model';
import { ApiParams, GenericApiService } from './generic-api.service';

@Injectable({
  providedIn: 'root',
})
export class CategoryApiService extends GenericApiService<
  Category,
  CategoryListItem,
  Category,
  CreateCategoryPayload,
  Pick<Category, 'id' | 'name' | 'appearance'>
> {
  constructor(http: HttpClient) {
    super(http, '/api/categories');
  }

  override getAll(pagination: ApiParams) {
    return super.getAll({
      orderBy: 'name',
      direction: 'asc',
      ...pagination,
    });
  }
}
