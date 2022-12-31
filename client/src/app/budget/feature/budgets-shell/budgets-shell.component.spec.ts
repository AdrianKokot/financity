import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetsShellComponent } from './budgets-shell.component';

describe('BudgetsShellComponent', () => {
  let component: BudgetsShellComponent;
  let fixture: ComponentFixture<BudgetsShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BudgetsShellComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(BudgetsShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
