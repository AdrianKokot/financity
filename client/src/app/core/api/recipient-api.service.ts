import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  CreateRecipientPayload,
  Recipient,
  RecipientListItem,
} from '@shared/data-access/models/recipient.model';
import { ApiParams, GenericApiService } from './generic-api.service';

@Injectable({
  providedIn: 'root',
})
export class RecipientApiService extends GenericApiService<
  Recipient,
  RecipientListItem,
  Recipient,
  CreateRecipientPayload,
  Pick<Recipient, 'id' | 'name'>
> {
  constructor(http: HttpClient) {
    super(http, '/api/recipients');
  }

  override getAll(pagination: ApiParams) {
    return super.getAll({
      ...pagination,
      orderBy: 'name',
      direction: 'asc',
    });
  }
}
