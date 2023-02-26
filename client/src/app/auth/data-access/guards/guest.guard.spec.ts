import { TestBed } from '@angular/core/testing';

import { GuestGuard } from './guest.guard';
import { Router } from '@angular/router';
import { AuthService } from '../api/auth.service';

describe('GuestGuard', () => {
  let guard: GuestGuard;
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

    guard = TestBed.inject(GuestGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it("should proceed when user doesn't have  valid token", () => {
    authService.hasValidToken = jasmine
      .createSpy('hasValidToken')
      .and.returnValue(false);

    expect(guard.canActivate()).toBeTrue();
    expect(guard.canActivateChild()).toBeTrue();
    expect(guard.canLoad()).toBeTrue();
  });

  it('should return UrlTree to root page when user has valid token', () => {
    authService.hasValidToken = jasmine
      .createSpy('hasValidToken')
      .and.returnValue(true);

    const urlTree = TestBed.inject(Router).createUrlTree(['/']);
    expect(guard.canActivate()).toEqual(urlTree);
    expect(guard.canActivateChild()).toEqual(urlTree);
    expect(guard.canLoad()).toEqual(urlTree);
  });
});
