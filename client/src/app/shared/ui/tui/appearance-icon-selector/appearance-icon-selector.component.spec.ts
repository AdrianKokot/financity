import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppearanceIconSelectorComponent } from './appearance-icon-selector.component';

describe('AppearanceIconSelectorComponent', () => {
  let component: AppearanceIconSelectorComponent;
  let fixture: ComponentFixture<AppearanceIconSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ AppearanceIconSelectorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppearanceIconSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
