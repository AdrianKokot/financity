import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginPageComponent } from './login-page.component';
import { ReactiveFormsModule } from '@angular/forms';
import {
  TuiButtonModule,
  TuiErrorComponent,
  TuiErrorModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import {
  TUI_VALIDATION_ERRORS,
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputPasswordModule,
} from '@taiga-ui/kit';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';

describe('LoginFormComponent', () => {
  let component: LoginPageComponent;
  let fixture: ComponentFixture<LoginPageComponent>;

  const testErrorMessages = {
    required: 'REQUIRED_ERROR_MESSAGE',
    email: 'EMAIL_ERROR_MESSAGE',
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LoginPageComponent],
      imports: [
        RouterTestingModule,
        ReactiveFormsModule,
        TuiErrorModule,
        TuiInputModule,
        TuiInputPasswordModule,
        TuiFieldErrorPipeModule,
        TuiTextfieldControllerModule,
        TuiButtonModule,
        HttpClientTestingModule,
        NoopAnimationsModule,
      ],
      providers: [
        {
          provide: TUI_VALIDATION_ERRORS,
          useValue: testErrorMessages,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    spyOn(TestBed.inject(Router), 'navigate').and.returnValue(
      new Promise(resolve => resolve(true))
    );
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Form interaction', () => {
    const formValue = {
      email: 'test@test.mail',
      password: 'MatchingPassword1#!',
    };

    let emailInput: DebugElement,
      passwordInput: DebugElement,
      submitButton: DebugElement,
      httpController: HttpTestingController;

    const fillInput = (input: DebugElement, value: string) => {
      input.triggerEventHandler('input', { target: { value } });
    };

    beforeEach(() => {
      httpController = TestBed.inject(HttpTestingController);

      emailInput = fixture.debugElement.query(By.css('input[type=email]'));
      passwordInput = fixture.debugElement.query(
        By.css('input[type=password]')
      );
      submitButton = fixture.debugElement.query(By.css('button[type=submit]'));

      expect(emailInput).not.toBeNull();
      expect(passwordInput).not.toBeNull();
      expect(submitButton).not.toBeNull();
    });

    it('should login on valid form fill', () => {
      fillInput(emailInput, formValue.email);
      fillInput(passwordInput, formValue.password);

      submitButton.nativeElement.click();

      const req = httpController.expectOne({
        url: '/api/auth/login',
        method: 'POST',
      });

      req.flush({});

      expect(req.request.body).toEqual(formValue);
      expect(TestBed.inject(Router).navigate).toHaveBeenCalledWith([
        '/dashboard',
      ]);
    });

    describe('Error Messages', () => {
      const getErrorMessages = async () => {
        submitButton.nativeElement.click();

        fixture.detectChanges();
        await fixture.whenStable();

        return fixture.debugElement
          .queryAll(By.directive(TuiErrorComponent))
          .map(x => x.nativeElement.textContent.trim());
      };

      it('should display error message on empty fields', async () => {
        expect(await getErrorMessages()).toEqual([
          testErrorMessages.required,
          testErrorMessages.required,
        ]);
      });

      it('should display error message on wrong email', async () => {
        fillInput(emailInput, 'invalidmail');
        expect(await getErrorMessages()).toContain(testErrorMessages.email);
      });

      afterEach(() => {
        httpController.expectNone({
          url: '/api/auth/login',
          method: 'POST',
        });
      });
    });
  });
});
