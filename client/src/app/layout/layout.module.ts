import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@shared/shared.module';
import { FooterComponent } from './components/footer/footer.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { NotFoundComponent } from './not-found/not-found.component';

const exportedComponents = [NavbarComponent, FooterComponent, NotFoundComponent];

@NgModule({
  declarations: [...exportedComponents],
  exports: [...exportedComponents],
  imports: [SharedModule, CommonModule, RouterModule.forChild([])],
})
export class LayoutModule {}
