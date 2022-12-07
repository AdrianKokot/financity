import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSettingsShellComponent } from './user-settings-shell.component';

describe('UserSettingsShellComponent', () => {
  let component: UserSettingsShellComponent;
  let fixture: ComponentFixture<UserSettingsShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserSettingsShellComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(UserSettingsShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
