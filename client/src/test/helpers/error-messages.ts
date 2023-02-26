import { TUI_VALIDATION_ERRORS } from '@taiga-ui/kit';
import { Provider } from '@angular/core';

export const TEST_ERROR_MESSAGES = {
  required: 'REQUIRED_ERROR_MESSAGE',
  email: 'EMAIL_ERROR_MESSAGE',
  maxlength: 'MAX_LENGTH_ERROR_MESSAGE',
};

export const provideTestErrorMessages = (): Provider => {
  return {
    provide: TUI_VALIDATION_ERRORS,
    useValue: TEST_ERROR_MESSAGES,
  };
};
