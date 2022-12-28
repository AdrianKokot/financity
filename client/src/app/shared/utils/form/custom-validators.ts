import { AbstractControl, ValidationErrors } from '@angular/forms';
import { TuiValidationError } from '@taiga-ui/cdk';

const defaultPasswordValidation = {
  minLength: 8,
  requireDigit: true,
  requireUppercase: true,
  requireLowercase: true,
  requireNonAlphanumeric: true,
};

export const CustomValidators = {
  password:
    (passwordValidation = defaultPasswordValidation) =>
    (control: AbstractControl<string>): ValidationErrors | null => {
      const value = control.value;

      if (value.length < passwordValidation.minLength) {
        return new TuiValidationError(
          `Password must have at least ${passwordValidation.minLength} characters`
        );
      }

      if (passwordValidation.requireDigit && !/[0-9]/.test(value))
        return new TuiValidationError('Password must have at least 1 digit');

      if (passwordValidation.requireUppercase && !/[A-Z]/.test(value))
        return new TuiValidationError(
          'Password must have at least 1 uppercase letter'
        );

      if (passwordValidation.requireLowercase && !/[a-z]/.test(value))
        return new TuiValidationError(
          'Password must have at least 1 lowercase letter'
        );

      if (
        passwordValidation.requireNonAlphanumeric &&
        !/[^a-zA-Z0-9]/.test(value)
      )
        return new TuiValidationError(
          'Password must have at least 1 non alphanumeric symbol'
        );

      return null;
    },
};
