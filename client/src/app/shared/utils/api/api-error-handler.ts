import { HttpErrorResponse } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { TuiValidationError } from '@taiga-ui/cdk';

export const handleValidationApiError = (
  form: FormGroup,
  error: HttpErrorResponse
) => {
  if (error.status !== 422) return;

  const errors: Record<string, string[]> = error.error.errors ?? {};

  for (const key of Object.keys(errors)) {
    const control = form.get(key);
    const errorMessage = new TuiValidationError(errors[key].join('. '));

    if (control !== null) {
      control.setErrors({
        apiError: errorMessage,
      });
    } else {
      form.setErrors({
        [key]: errorMessage,
      });
    }
  }
};
