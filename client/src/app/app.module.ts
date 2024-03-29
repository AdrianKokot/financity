import {
  TUI_DIALOGS_CLOSE,
  TUI_HINT_DEFAULT_OPTIONS,
  TUI_HINT_OPTIONS,
  TUI_NOTHING_FOUND_MESSAGE,
  TUI_SANITIZER,
  TuiAlertModule,
  TuiDialogModule,
  tuiDialogOptionsProvider,
  tuiFormatNumber,
  TuiRootModule,
  tuiSvgOptionsProvider,
  TuiThemeNightModule,
} from '@taiga-ui/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
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
import { TUI_DATE_SEPARATOR } from '@taiga-ui/cdk';
import { MissingWalletInterceptor } from './wallet/data-access/interceptors/missing-wallet.interceptor';

const largeIconsToIgnore = new Set([
  'tuiIconCheckLarge',
  'tuiIconCalendarLarge',
]);

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
      provide: HTTP_INTERCEPTORS,
      useClass: MissingWalletInterceptor,
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
        min: ({ min }: { min: number }) => {
          return `This field must be greater or equal to ${tuiFormatNumber(
            min
          )}`;
        },
        max: ({ max }: { max: number }) => {
          return `This field must be smaller than ${tuiFormatNumber(max)}`;
        },
      },
    },
    tuiSvgOptionsProvider({
      srcProcessor: src => {
        if (typeof src !== 'string') {
          return src;
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

        if (largeIconsToIgnore.has(src)) {
          src = src.replace('Large', '');
        }

        return src;
      },
    }),
    {
      provide: DATE_PIPE_DEFAULT_OPTIONS,
      useValue: { dateFormat: 'dd/MM/yyyy' },
    },
    { provide: TUI_DATE_SEPARATOR, useValue: '/' },
    {
      provide: TUI_NOTHING_FOUND_MESSAGE,
      useValue: of('No records found').pipe(shareReplay()),
    },
    {
      provide: TUI_HINT_OPTIONS,
      useValue: { ...TUI_HINT_DEFAULT_OPTIONS, appearance: 'onDark' },
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
