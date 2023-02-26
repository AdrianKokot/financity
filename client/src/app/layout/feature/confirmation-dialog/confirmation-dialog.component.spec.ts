import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmationDialogComponent } from './confirmation-dialog.component';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import { By } from '@angular/platform-browser';
import { EMPTY_FUNCTION } from '@taiga-ui/cdk';

describe('ConfirmationDialogComponent', () => {
  let component: ConfirmationDialogComponent;
  let fixture: ComponentFixture<ConfirmationDialogComponent>;
  const confirmationDialogText = 'TEST_CONFIRMATION_DIALOG';

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        {
          provide: POLYMORPHEUS_CONTEXT,
          useValue: {
            data: { content: confirmationDialogText },
            completeWith: EMPTY_FUNCTION,
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ConfirmationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should generate cancel and confirm buttons', () => {
    const buttons = fixture.debugElement.queryAll(By.css('button'));

    expect(buttons.length).toBe(2);

    expect(buttons.map(x => x.nativeElement.textContent.trim())).toEqual([
      'Cancel',
      'Confirm',
    ]);
  });

  it('should render confirmation text', () => {
    expect(fixture.nativeElement.textContent.trim()).toContain(
      confirmationDialogText
    );
  });

  it('should send proper complete message on cancel', () => {
    const context = TestBed.inject(POLYMORPHEUS_CONTEXT);

    spyOn(context, 'completeWith');

    const cancelButton = fixture.debugElement.query(
      By.css('[data-test="cancel"]')
    );

    cancelButton.nativeElement.click();

    fixture.detectChanges();

    expect(context['completeWith']).toHaveBeenCalledOnceWith(false);
  });

  it('should send proper complete message on confirm', () => {
    const context = TestBed.inject(POLYMORPHEUS_CONTEXT);

    spyOn(context, 'completeWith');

    const cancelButton = fixture.debugElement.query(
      By.css('[data-test="confirm"]')
    );

    cancelButton.nativeElement.click();

    fixture.detectChanges();

    expect(context['completeWith']).toHaveBeenCalledOnceWith(true);
  });
});
