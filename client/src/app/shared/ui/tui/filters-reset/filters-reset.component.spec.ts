import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FiltersResetComponent } from './filters-reset.component';

describe('FiltersResetComponent', () => {
  let component: FiltersResetComponent;
  let fixture: ComponentFixture<FiltersResetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FiltersResetComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FiltersResetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
