import {
  catchError,
  exhaustMap,
  filter,
  map,
  Observable,
  of,
  startWith,
  switchMap,
  take,
  tap,
} from 'rxjs';
import { AbstractControl, FormGroup } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { handleValidationApiError } from '@shared/utils/api/api-error-handler';
import { FormHandlerFunctions } from '@shared/utils/form/form-handler';

export const toFormSubmit = <
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  },
  TResult
>(
  form: FormGroup<TControl>,
  functions: FormHandlerFunctions<TControl, TResult>
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
          tap(r =>
            console.info(`[toFormSubmit::submit]: ${JSON.stringify(r)}`)
          ),
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
        if (result !== null && result !== undefined && functions.effect) {
          console.info(`[toFormSubmit::effect]: ${JSON.stringify(result)}`);
          functions.effect(result);
        }
      }),
      switchMap(result => {
        if (result !== null && result !== undefined && functions.effect$) {
          console.info(`[toFormSubmit::effect$]: ${JSON.stringify(result)}`);
          return functions.effect$(result).pipe(
            take(1),
            startWith(null),
            map(() => result),
            tap(() => console.info('[toFormSubmit::effect$] emit'))
          );
        }
        return of(result);
      }),
      map(x => x === null),
      startWith(false)
    );
  };
};
