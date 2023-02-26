import { TestBed } from '@angular/core/testing';

import { BudgetApiService } from './budget-api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('BudgetApiService', () => {
  let service: BudgetApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [HttpClientTestingModule] });
    service = TestBed.inject(BudgetApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
