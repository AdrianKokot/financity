import { TestBed } from '@angular/core/testing';

import { ConcreteWalletApiService } from './concrete-wallet-api.service';

describe('ConcreteWalletApiService', () => {
  let service: ConcreteWalletApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConcreteWalletApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
