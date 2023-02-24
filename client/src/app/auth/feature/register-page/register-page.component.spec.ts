import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterPageComponent } from './register-page.component';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import {
  TUI_VALIDATION_ERRORS,
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputPasswordModule,
} from '@taiga-ui/kit';
import {
  TuiButtonModule,
  TuiErrorComponent,
  TuiErrorModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';
import { CustomValidators } from '@shared/utils/form/custom-validators';
import { Router } from '@angular/router';

describe('RegisterPageComponent', () => {
  let component: RegisterPageComponent;
  let fixture: ComponentFixture<RegisterPageComponent>;

  const testErrorMessages = {
    required: 'REQUIRED_ERROR_MESSAGE',
    email: 'EMAIL_ERROR_MESSAGE',
    maxlength: 'MAX_LENGTH_ERROR_MESSAGE',
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RegisterPageComponent],
      imports: [
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

    fixture = TestBed.createComponent(RegisterPageComponent);
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
      name: 'Test Test',
      email: 'test@test.mail',
      password: 'MatchingPassword1#!',
    };

    let nameInput: DebugElement,
      emailInput: DebugElement,
      passwordInput: DebugElement,
      submitButton: DebugElement,
      httpController: HttpTestingController;

    const fillInput = (input: DebugElement, value: string) => {
      input.triggerEventHandler('input', { target: { value } });
    };

    beforeEach(() => {
      httpController = TestBed.inject(HttpTestingController);

      nameInput = fixture.debugElement.query(By.css('input[type=text]'));
      emailInput = fixture.debugElement.query(By.css('input[type=email]'));
      passwordInput = fixture.debugElement.query(
        By.css('input[type=password]')
      );
      submitButton = fixture.debugElement.query(By.css('button[type=submit]'));

      expect(nameInput).not.toBeNull();
      expect(emailInput).not.toBeNull();
      expect(passwordInput).not.toBeNull();
      expect(submitButton).not.toBeNull();
    });

    it('should register on valid form fill', () => {
      fillInput(nameInput, formValue.name);
      fillInput(emailInput, formValue.email);
      fillInput(passwordInput, formValue.password);

      submitButton.nativeElement.click();

      const req = httpController.expectOne({
        url: '/api/auth/register',
        method: 'POST',
      });

      req.flush({});

      expect(req.request.body).toEqual(formValue);
      expect(TestBed.inject(Router).navigate).toHaveBeenCalledWith([
        '/auth/login',
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
          testErrorMessages.required,
        ]);
      });

      it('should display error message on wrong email', async () => {
        fillInput(emailInput, 'invalidmail');
        expect(await getErrorMessages()).toContain(testErrorMessages.email);
      });

      it('should display error message on invalid password', async () => {
        fillInput(passwordInput, 'invalidpassword');

        expect(await getErrorMessages()).toContain(
          CustomValidators.password()(component.form.group.controls.password)
            ?.message
        );
      });

      it('should display error message on too long name', async () => {
        fillInput(nameInput, new Array(129).fill('a').join(''));

        expect(await getErrorMessages()).toContain(testErrorMessages.maxlength);
      });

      it('should display error message on too long email', async () => {
        fillInput(nameInput, new Array(257).fill('a').join(''));

        expect(await getErrorMessages()).toContain(testErrorMessages.maxlength);
      });

      afterEach(() => {
        httpController.expectNone({
          url: '/api/auth/register',
          method: 'POST',
        });
      });
    });
  });
});
