import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FooterComponent } from './feature/footer/footer.component';
import { NavbarComponent } from './feature/navbar/navbar.component';
import { NotFoundComponent } from './feature/not-found/not-found.component';
import {
  TuiButtonModule,
  TuiLinkModule,
  TuiScrollbarModule,
  TuiSvgModule,
} from '@taiga-ui/core';
import { RouterLinkActive, RouterLinkWithHref } from '@angular/router';
import { NavbarItemComponent } from './ui/navbar-item/navbar-item.component';
import { TuiBadgeModule } from '@taiga-ui/kit';

const exportedComponents = [
  NavbarComponent,
  FooterComponent,
  NotFoundComponent,
];

@NgModule({
  declarations: [...exportedComponents, NavbarItemComponent],
  exports: [...exportedComponents],
  imports: [
    CommonModule,
    TuiLinkModule,
    TuiButtonModule,
    RouterLinkWithHref,
    TuiScrollbarModule,
    RouterLinkActive,
    TuiSvgModule,
    TuiBadgeModule,
  ],
})
export class LayoutModule {}
