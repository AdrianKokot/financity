import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletShellComponent } from './wallet-shell.component';

describe('WalletShellComponent', () => {
  let component: WalletShellComponent;
  let fixture: ComponentFixture<WalletShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WalletShellComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(WalletShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
