import {
  catchError,
  exhaustMap,
  filter,
  map,
  Observable,
  of,
  startWith,
  tap,
} from 'rxjs';
import { AbstractControl, FormGroup } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { handleValidationApiError } from '@shared/utils/api/api-error-handler';

export const toFormSubmit = <
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  },
  TResult
>(
  form: FormGroup<TControl>,
  functions: {
    submit: (
      formValue: ReturnType<typeof form.getRawValue>
    ) => Observable<TResult>;
    effect: (result: TResult) => void;
  }
) => {
  return function <TInput>(source: Observable<TInput>) {
    return source.pipe(
      tap(() =>
        console.info(`[toFormSubmit::start]: form valid: ${form.valid}`)
      ),
      tap(() => form.markAllAsTouched()),
      filter(() => form.valid),
      exhaustMap(() =>
        functions.submit(form.getRawValue()).pipe(
          tap(r => console.info(`[toFormSubmit::submit]: ${r}`)),
          startWith(null),
          catchError(err => {
            if (err instanceof HttpErrorResponse) {
              handleValidationApiError(form, err);
            }
            return of(undefined);
          })
        )
      ),
      tap(result => {
        if (result !== null && result !== undefined) {
          console.info(`[toFormSubmit::effect]: ${result}`);
          functions.effect(result);
        }
      }),
      map(x => x === null),
      startWith(false)
    );
  };
};
