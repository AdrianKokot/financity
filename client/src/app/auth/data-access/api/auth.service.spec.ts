import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { LoginPayload, RegisterPayload, User } from '../models/user';
import { JwtHelper } from '../../utils/decode-jwt';
import { JwtToken } from '../models/token';
import { ClaimTypes } from '../models/claim-types';
import { Router } from '@angular/router';

describe('AuthService', () => {
  let service: AuthService;
  const apiPaths = {
    register: '/api/auth/register',
    login: '/api/auth/login',
    requestResetPassword: '/api/auth/request-reset-password',
    resetPassword: '/api/auth/reset-password',
    changePassword: '/api/auth/change-password',
    updateUser: '/api/auth/user',
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(AuthService);

    spyOn(TestBed.inject(Router), 'navigate').and.returnValue(
      new Promise(resolve => resolve(true))
    );
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('HTTP Calls', () => {
    let httpController: HttpTestingController;

    beforeEach(() => {
      httpController = TestBed.inject(HttpTestingController);
    });

    describe('Register', () => {
      const payload: RegisterPayload = {
        name: 'TEST_USER',
        email: 'TEST_USER@MAIL.COM',
        password: 'TEST_PASSWORD',
      };

      it('should call proper api url', done => {
        service.register(payload).subscribe(value => {
          expect(value).toBeTrue();
          done();
        });

        httpController
          .expectOne({
            method: 'POST',
            url: apiPaths.register,
          })
          .flush(payload, { status: 200, statusText: '' });
      });
    });

    describe('Login', () => {
      const payload: LoginPayload = {
        email: 'TEST_USER@MAIL.COM',
        password: 'TEST_PASSWORD',
      };

      it('should call proper api url', done => {
        service.login(payload).subscribe(value => {
          expect(value).toBeTrue();
          done();
        });

        httpController
          .expectOne({
            method: 'POST',
            url: apiPaths.login,
          })
          .flush({ token: null }, { status: 200, statusText: '' });
      });

      describe('Token validation', () => {
        const flushFakeToken = () => {
          httpController
            .expectOne({
              method: 'POST',
              url: apiPaths.login,
            })
            .flush({ token: 'token' }, { status: 200, statusText: '' });
        };

        it('should reflect login result in user snapshot', done => {
          const fakeToken: JwtToken = {
            nbf: (new Date().getTime() / 1000) | 0,
            exp: (new Date().getTime() / 1000 + 60 * 60) | 0,
            // eslint-disable-next-line @typescript-eslint/naming-convention
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress':
              payload.email,
            // eslint-disable-next-line @typescript-eslint/naming-convention
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name':
              'test',
            // eslint-disable-next-line @typescript-eslint/naming-convention
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier':
              'test',
          };

          spyOn(JwtHelper, 'decode').and.returnValue(fakeToken);
          spyOn(localStorage, 'setItem').and.callThrough();

          service.login(payload).subscribe(() => {
            expect(service.userSnapshot).toEqual({
              email: fakeToken[ClaimTypes.email],
              id: fakeToken[ClaimTypes.id],
              name: fakeToken[ClaimTypes.name],
            });

            expect(localStorage.setItem).toHaveBeenCalledWith('token', 'token');

            expect(service.token).not.toBeNull();

            done();
          });

          flushFakeToken();
        });

        it('should check token exp', done => {
          const fakeToken: JwtToken = {
            nbf: 0,
            exp: (new Date().getTime() / 1000 - 60 * 60) | 0,
          } as JwtToken;

          spyOn(JwtHelper, 'decode').and.returnValue(fakeToken);

          service.login(payload).subscribe(() => {
            expect(service.userSnapshot).toBeNull();
            expect(service.hasValidToken()).toBeFalse();

            done();
          });

          flushFakeToken();
        });

        it('should check token nbf', done => {
          const fakeToken: JwtToken = {
            nbf: (new Date().getTime() / 1000 + 60 * 60) | 0,
            exp: (new Date().getTime() / 1000 + 60 * 60 * 2) | 0,
          } as JwtToken;

          spyOn(JwtHelper, 'decode').and.returnValue(fakeToken);

          service.login(payload).subscribe(() => {
            expect(service.userSnapshot).toBeNull();
            expect(service.hasValidToken()).toBeFalse();

            done();
          });

          flushFakeToken();
        });
      });
    });

    describe('Password reset', () => {
      it('should call proper api url on request password reset', done => {
        service
          .requestPasswordReset({ email: 'TEST@MAIL.COM' })
          .subscribe(value => {
            expect(value).toBeTrue();
            done();
          });

        httpController
          .expectOne({
            method: 'POST',
            url: apiPaths.requestResetPassword,
          })
          .flush(null, { status: 202, statusText: '' });
      });

      it('should call proper api url on password reset', done => {
        service
          .resetPassword({
            email: 'TEST@MAIL.COM',
            password: 'TEST',
            token: 'TEST',
          })
          .subscribe(value => {
            expect(value).toBeTrue();
            done();
          });

        httpController
          .expectOne({
            method: 'POST',
            url: apiPaths.resetPassword,
          })
          .flush(null, { status: 204, statusText: '' });
      });
    });

    describe('User update', () => {
      it('should call proper api url on changePassword', done => {
        service
          .changePassword({ password: 'TEST', newPassword: 'TEST' })
          .subscribe(value => {
            expect(value).toBeTrue();
            done();
          });

        httpController
          .expectOne({
            method: 'POST',
            url: apiPaths.changePassword,
          })
          .flush(null, { status: 204, statusText: '' });
      });

      it('should call proper api url on updateUser', done => {
        const user: User = { name: 'TEST2', email: 'TEST@MAIL.COM', id: '' };

        service.updateUser({ name: user.name }).subscribe(value => {
          expect(value).toEqual(user);
          done();
        });

        httpController
          .expectOne({
            method: 'PUT',
            url: apiPaths.updateUser,
          })
          .flush(user);
      });
    });
  });
});
