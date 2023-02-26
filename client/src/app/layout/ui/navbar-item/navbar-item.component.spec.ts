import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarItemComponent } from './navbar-item.component';
import { RouterTestingModule } from '@angular/router/testing';
import { TuiSvgModule } from '@taiga-ui/core';
import { By } from '@angular/platform-browser';
import { RouterLink } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';

describe('NavbarItemComponent', () => {
  let component: NavbarItemComponent;
  let fixture: ComponentFixture<NavbarItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, TuiSvgModule],
      declarations: [NavbarItemComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NavbarItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render provided route', async () => {
    component.route = { route: '/test', label: 'TEST', iconName: 'TEST' };

    fixture.debugElement.injector.get(ChangeDetectorRef).detectChanges();

    await fixture.whenStable();

    const link = fixture.debugElement.query(By.directive(RouterLink));

    expect(link).not.toBeNull();
    expect(link.nativeElement.textContent.trim()).toEqual('TEST');
    expect(link.attributes['href']).toBe('/test');
  });
});
