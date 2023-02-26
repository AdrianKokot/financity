import { TestBed } from '@angular/core/testing';

import { LogoutGuard } from './logout.guard';
import { AuthService } from '../api/auth.service';
import { NEVER, of } from 'rxjs';
import { Router } from '@angular/router';

describe('LogoutGuard', () => {
  let guard: LogoutGuard;
  let authService: AuthService;

  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService', ['logout']);

    TestBed.configureTestingModule({
      providers: [
        {
          provide: AuthService,
          useValue: authService,
        },
      ],
    });
    guard = TestBed.inject(LogoutGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should call AuthService logout method', () => {
    authService.logout = jasmine.createSpy('logout').and.returnValue(NEVER);

    guard.canActivate();

    expect(authService.logout).toHaveBeenCalled();
  });

  it('should return UrlTree to login page', done => {
    authService.logout = jasmine.createSpy('logout').and.returnValue(of(null));

    const expectedUrlTree = TestBed.inject(Router).createUrlTree([
      '/auth/login',
    ]);

    guard.canActivate().subscribe(urlTree => {
      expect(urlTree).toEqual(expectedUrlTree);
      done();
    });
  });
});
