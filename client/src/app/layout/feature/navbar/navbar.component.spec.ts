import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarComponent } from './navbar.component';
import { NavbarItemComponent } from '@layout/ui/navbar-item/navbar-item.component';
import { TuiSvgModule } from '@taiga-ui/core';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';
import {
  AppNavRoutes,
  UserRelatedRoutes,
} from '@layout/data-access/nav-routes';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  const allRoutes = [...AppNavRoutes, ...UserRelatedRoutes];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, TuiSvgModule],
      declarations: [NavbarComponent, NavbarItemComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have all ui links', () => {
    const links = fixture.debugElement.queryAll(
      By.directive(NavbarItemComponent)
    );

    expect(links.map(x => x.nativeElement.textContent.trim())).toEqual(
      allRoutes.map(x => x.label)
    );
  });
});
