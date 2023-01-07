import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  CreateLabelPayload,
  Label,
  LabelListItem,
} from '@shared/data-access/models/label';
import { ApiParams, GenericApiService } from './generic-api.service';

@Injectable({
  providedIn: 'root',
})
export class LabelApiService extends GenericApiService<
  Label,
  LabelListItem,
  Label,
  CreateLabelPayload,
  Pick<Label, 'id' | 'name' | 'appearance'>
> {
  constructor(http: HttpClient) {
    super(http, '/api/labels');
  }

  override getAll(pagination: ApiParams) {
    return super.getAll({
      orderBy: 'name',
      direction: 'asc',
      ...pagination,
    });
  }
}
