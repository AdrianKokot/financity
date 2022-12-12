import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletShareManagementComponent } from './wallet-share-management.component';

describe('WalletShareManagementComponent', () => {
  let component: WalletShareManagementComponent;
  let fixture: ComponentFixture<WalletShareManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletShareManagementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WalletShareManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
