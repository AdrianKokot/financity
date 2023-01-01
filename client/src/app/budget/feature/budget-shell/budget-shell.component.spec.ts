import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetShellComponent } from './budget-shell.component';

describe('BudgetShellComponent', () => {
  let component: BudgetShellComponent;
  let fixture: ComponentFixture<BudgetShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BudgetShellComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BudgetShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
