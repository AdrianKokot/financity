import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FooterComponent } from './ui/footer/footer.component';
import { NavbarComponent } from './ui/navbar/navbar.component';
import { NotFoundComponent } from './ui/not-found/not-found.component';
import { TuiButtonModule, TuiLinkModule } from '@taiga-ui/core';
import { RouterLinkWithHref } from '@angular/router';

const exportedComponents = [
  NavbarComponent,
  FooterComponent,
  NotFoundComponent,
];

@NgModule({
  declarations: [...exportedComponents],
  exports: [...exportedComponents],
  imports: [CommonModule, TuiLinkModule, TuiButtonModule, RouterLinkWithHref],
})
export class LayoutModule {}
