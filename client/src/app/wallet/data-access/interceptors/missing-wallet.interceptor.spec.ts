import { TestBed } from '@angular/core/testing';

import { MissingWalletInterceptor } from './missing-wallet.interceptor';

describe('MissingWalletInterceptor', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      providers: [MissingWalletInterceptor],
    })
  );

  it('should be created', () => {
    const interceptor: MissingWalletInterceptor = TestBed.inject(
      MissingWalletInterceptor
    );
    expect(interceptor).toBeTruthy();
  });
});
