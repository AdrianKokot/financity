import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletRecipientsComponent } from './wallet-recipients.component';

describe('WalletRecipientsComponent', () => {
  let component: WalletRecipientsComponent;
  let fixture: ComponentFixture<WalletRecipientsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WalletRecipientsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(WalletRecipientsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
