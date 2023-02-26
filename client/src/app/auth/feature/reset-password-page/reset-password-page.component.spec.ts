import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordPageComponent } from './reset-password-page.component';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import {
  TuiButtonModule,
  TuiErrorComponent,
  TuiErrorModule,
  TuiLabelModule,
  TuiLinkModule,
  TuiNotificationModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import {
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputPasswordModule,
} from '@taiga-ui/kit';
import { TuiAutoFocusModule } from '@taiga-ui/cdk';
import { RouterTestingModule } from '@angular/router/testing';
import { DebugElement } from '@angular/core';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {
  provideTestErrorMessages,
  TEST_ERROR_MESSAGES,
} from '../../../../test/helpers/error-messages';
import { AuthService } from '../../data-access/api/auth.service';
import { By } from '@angular/platform-browser';
import { fillInput } from '../../../../test/helpers/form';

describe('ResetPasswordPageComponent', () => {
  let component: ResetPasswordPageComponent;
  let fixture: ComponentFixture<ResetPasswordPageComponent>;
  let authService: AuthService;
  let httpController: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        ReactiveFormsModule,
        TuiNotificationModule,
        TuiInputModule,
        TuiTextfieldControllerModule,
        TuiAutoFocusModule,
        TuiLabelModule,
        TuiErrorModule,
        TuiFieldErrorPipeModule,
        TuiInputPasswordModule,
        TuiButtonModule,
        TuiLinkModule,
        RouterTestingModule,
        NoopAnimationsModule,
      ],
      providers: [provideTestErrorMessages()],
      declarations: [ResetPasswordPageComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ResetPasswordPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    authService = TestBed.inject(AuthService);
    httpController = TestBed.inject(HttpTestingController);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Form interaction', () => {
    let emailInput: DebugElement,
      passwordInput: DebugElement,
      submitButton: DebugElement;

    beforeEach(() => {
      emailInput = fixture.debugElement.query(By.css('input[type=email]'));
      passwordInput = fixture.debugElement.query(
        By.css('input[type=password]')
      );
      submitButton = fixture.debugElement.query(By.css('button[type=submit]'));
    });

    describe('Request reset password', () => {
      it('should have only email input', () => {
        expect(emailInput).not.toBeNull();
        expect(passwordInput).toBeNull();
      });

      it('should display required error on submit button click', () => {
        spyOn(authService, 'requestPasswordReset');

        submitButton.nativeElement.click();

        fixture.detectChanges();

        const errorMessages = fixture.debugElement
          .queryAll(By.directive(TuiErrorComponent))
          .map(x => x.nativeElement.textContent.trim());

        expect(errorMessages).toEqual([TEST_ERROR_MESSAGES.required]);
        expect(authService.requestPasswordReset).not.toHaveBeenCalled();
      });

      it('should display error on invalid email', () => {
        fillInput(emailInput, 'invalidmail');

        submitButton.nativeElement.click();
        fixture.detectChanges();

        const errorMessages = fixture.debugElement
          .queryAll(By.directive(TuiErrorComponent))
          .map(x => x.nativeElement.textContent.trim());

        expect(errorMessages).toContain(TEST_ERROR_MESSAGES.email);
      });

      it('should request password reset when form filled correctly', () => {
        fillInput(emailInput, 'test@test.com');

        submitButton.nativeElement.click();

        fixture.detectChanges();

        const req = httpController.expectOne({
          url: '/api/auth/request-reset-password',
          method: 'POST',
        });

        req.flush({});

        expect(req.request.body).toEqual({
          email: 'test@test.com',
          token: '',
          password: '',
        });
      });
    });
  });
});
