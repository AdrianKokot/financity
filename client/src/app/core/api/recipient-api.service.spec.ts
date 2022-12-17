import { TestBed } from '@angular/core/testing';

import { RecipientApiService } from './recipient-api.service';

describe('RecipientApiService', () => {
  let service: RecipientApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecipientApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
