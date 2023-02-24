import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthShellComponent } from './auth-shell.component';
import { TuiIslandModule } from '@taiga-ui/kit';
import { RouterTestingModule } from '@angular/router/testing';

describe('AuthShellComponent', () => {
  let component: AuthShellComponent;
  let fixture: ComponentFixture<AuthShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuthShellComponent],
      imports: [TuiIslandModule, RouterTestingModule],
    }).compileComponents();

    fixture = TestBed.createComponent(AuthShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
