import {
  TUI_DIALOGS_CLOSE,
  TUI_SANITIZER,
  TUI_SVG_SRC_PROCESSOR,
  TuiAlertModule,
  TuiDialogModule,
  TuiRootModule,
  TuiSvgService,
  TuiThemeNightModule,
} from '@taiga-ui/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Inject, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from '@layout/layout.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { JwtInterceptor } from '@shared/data-access/interceptors/jwt.interceptor';
import { ApiInterceptor } from '@shared/data-access/interceptors/api.interceptor';
import { NgDompurifySanitizer } from '@tinkoff/ng-dompurify';
import { TUI_VALIDATION_ERRORS } from '@taiga-ui/kit';
import { AuthService } from './auth/data-access/api/auth.service';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    TuiRootModule,
    TuiDialogModule,
    TuiAlertModule,
    TuiThemeNightModule,
    LayoutModule,
    HttpClientModule,
  ],
  providers: [
    {
      provide: TUI_DIALOGS_CLOSE,
      deps: [AuthService],
      useFactory: (authService: AuthService) => authService.loggedOut$,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ApiInterceptor,
      multi: true,
    },
    {
      provide: TUI_SANITIZER,
      useClass: NgDompurifySanitizer,
    },
    {
      provide: TUI_VALIDATION_ERRORS,
      useValue: {
        required: 'This field is required',
      },
    },
    {
      provide: TUI_SVG_SRC_PROCESSOR,
      useFactory: () => {
        return (src: string | null): string => {
          if (src === null) {
            return '';
          }
          const myCustomPrefix = 'fa::';

          return src.startsWith(myCustomPrefix)
            ? `assets/icons/fa/${src.replace(myCustomPrefix, '')}.svg`
            : src;
        };
      },
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(@Inject(TuiSvgService) svgService: TuiSvgService) {
    svgService.define({
      appHomeIcon: 'assets/icons/home-24px.svg#home-24px',
    });
  }
}
