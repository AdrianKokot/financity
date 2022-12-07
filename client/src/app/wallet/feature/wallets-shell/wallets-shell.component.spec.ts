import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletsShellComponent } from './wallets-shell.component';

describe('WalletsShellComponent', () => {
  let component: WalletsShellComponent;
  let fixture: ComponentFixture<WalletsShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletsShellComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WalletsShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
