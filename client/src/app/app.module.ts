import {
  TUI_DIALOGS_CLOSE,
  TUI_NOTHING_FOUND_MESSAGE,
  TUI_SANITIZER,
  TUI_SVG_SRC_PROCESSOR,
  TuiAlertModule,
  TuiDialogModule,
  tuiDialogOptionsProvider,
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
import { DATE_PIPE_DEFAULT_OPTIONS } from '@angular/common';
import { of, shareReplay } from 'rxjs';

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
    tuiDialogOptionsProvider({
      dismissible: false,
    }),
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
        email: 'This field must be a valid email address',
        minlength: ({ requiredLength }: { requiredLength: number }) =>
          `This field must be at least ${requiredLength} characters long`,
        maxlength: ({ requiredLength }: { requiredLength: number }) =>
          `This field may not be longer than ${requiredLength} characters`,
      },
    },
    {
      provide: TUI_SVG_SRC_PROCESSOR,
      useFactory: () => {
        return (src: string | null): string => {
          if (src === null) {
            return '';
          }

          if (src.startsWith('fa::')) {
            if (src.startsWith('fa::solid::')) {
              return `assets/icons/fa/solid/${src.replace(
                'fa::solid::',
                ''
              )}.svg`;
            }
            return `assets/icons/fa/${src.replace('fa::', '')}.svg`;
          }

          return src;
        };
      },
    },
    {
      provide: DATE_PIPE_DEFAULT_OPTIONS,
      useValue: { dateFormat: 'dd/MM/yyyy' },
    },
    {
      provide: TUI_NOTHING_FOUND_MESSAGE,
      useValue: of('No records found').pipe(shareReplay()),
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
