import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShareWalletComponent } from './share-wallet.component';

describe('ShareWalletComponent', () => {
  let component: ShareWalletComponent;
  let fixture: ComponentFixture<ShareWalletComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ShareWalletComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ShareWalletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
