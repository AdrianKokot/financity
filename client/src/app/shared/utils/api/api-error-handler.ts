import { HttpErrorResponse } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { TuiValidationError } from '@taiga-ui/cdk';

export const handleValidationApiError = (
  form: FormGroup,
  error: HttpErrorResponse
) => {
  if (error.status !== 422) return;

  const errors: Record<string, string[]> = error.error.errors ?? {};

  for (const key in errors) {
    form.get(key)?.setErrors({
      apiError: new TuiValidationError(errors[key][0]),
    });
  }
};
