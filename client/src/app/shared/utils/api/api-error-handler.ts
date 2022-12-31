import { HttpErrorResponse } from '@angular/common/http';
import { AbstractControl, FormGroup } from '@angular/forms';
import { TuiValidationError } from '@taiga-ui/cdk';

export const handleValidationApiError = <
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  }
>(
  form: FormGroup<TControl>,
  error: HttpErrorResponse,
  forceKey?: Extract<keyof TControl, string>
) => {
  if (error.status !== 422) return;

  const errors: Record<string, string[]> = error.error.errors ?? {};

  for (const key of Object.keys(errors)) {
    const control = form.get(forceKey ? forceKey : key);
    const errorMessage = new TuiValidationError(errors[key].join('. '));

    if (control !== null) {
      control.setErrors({
        apiError: errorMessage,
      });
      control.markAllAsTouched();
    } else {
      form.setErrors({
        [key]: errorMessage,
      });
    }
  }
};
