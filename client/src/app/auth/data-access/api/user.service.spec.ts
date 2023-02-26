import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';
import { AuthService } from './auth.service';

describe('UserService', () => {
  let service: UserService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: { userSnapshot: { id: 'TEST_ID' } } },
      ],
    });

    service = TestBed.inject(UserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return proper user id', () => {
    expect(service.userId).toBe('TEST_ID');
  });
});
