import { TestBed } from '@angular/core/testing';

import { AuthGuard } from './auth.guard';
import { AuthService } from '../api/auth.service';
import { Router } from '@angular/router';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let authService: AuthService;

  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService', ['hasValidToken']);

    TestBed.configureTestingModule({
      providers: [
        {
          provide: AuthService,
          useValue: authService,
        },
      ],
    });
    guard = TestBed.inject(AuthGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should proceed when user has valid token', () => {
    authService.hasValidToken = jasmine
      .createSpy('hasValidToken')
      .and.returnValue(true);

    expect(guard.canActivate()).toBeTrue();
    expect(guard.canActivateChild()).toBeTrue();
    expect(guard.canLoad()).toBeTrue();
  });

  it("should return UrlTree to login page when user doesn't have valid token", () => {
    authService.hasValidToken = jasmine
      .createSpy('hasValidToken')
      .and.returnValue(false);

    const urlTree = TestBed.inject(Router).createUrlTree(['/auth/login']);
    expect(guard.canActivate()).toEqual(urlTree);
    expect(guard.canActivateChild()).toEqual(urlTree);
    expect(guard.canLoad()).toEqual(urlTree);
  });
});
