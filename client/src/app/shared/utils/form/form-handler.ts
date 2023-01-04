import { AbstractControl, FormGroup } from '@angular/forms';
import {
  catchError,
  exhaustMap,
  filter,
  map,
  Observable,
  of,
  startWith,
  Subject,
  switchMap,
  take,
  tap,
} from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { handleValidationApiError } from '@shared/utils/api/api-error-handler';

export interface FormHandlerFunctions<
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  },
  TResult
> {
  submit: (
    formValue: ReturnType<
      InstanceType<typeof FormGroup<TControl>>['getRawValue']
    >
  ) => Observable<TResult>;
  effect?: (result: TResult) => void;
  effect$?: (result: TResult) => Observable<unknown>;
}

const toFormSubmit = <
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  },
  TResult
>(
  form: FormGroup<TControl>,
  functions: FormHandlerFunctions<TControl, TResult>,
  forceErrorKeys?: Extract<keyof TControl, string>
) => {
  return function <TInput>(source: Observable<TInput>) {
    return source.pipe(
      tap(() => form.markAllAsTouched()),
      filter(() => form.valid),
      exhaustMap(() =>
        functions.submit(form.getRawValue()).pipe(
          startWith(null),
          catchError(err => {
            if (err instanceof HttpErrorResponse) {
              handleValidationApiError(form, err, forceErrorKeys);
            }
            return of(undefined);
          })
        )
      ),
      tap(result => {
        if (result !== null && result !== undefined && functions.effect) {
          functions.effect(result);
        }
      }),
      switchMap(result => {
        if (result !== null && result !== undefined && functions.effect$) {
          return functions.effect$(result).pipe(
            take(1),
            startWith(null),
            map(() => result)
          );
        }
        return of(result);
      }),
      map(x => x === null),
      startWith(false)
    );
  };
};

export class FormHandler<
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  },
  TResult
> {
  private _submit$ = new Subject<void>();
  public readonly submitButtonLoading$ = this._submit$.pipe(
    toFormSubmit(this.group, this._functions)
  );

  get statusChanges() {
    return this.group.statusChanges;
  }

  get errors() {
    return this.group.errors;
  }

  get controls() {
    return this.group.controls;
  }

  constructor(
    public readonly group: FormGroup<TControl>,
    private readonly _functions: FormHandlerFunctions<TControl, TResult>
  ) {}

  public submit() {
    this._submit$.next(undefined);
  }

  public patchValue(...args: Parameters<typeof this.group.patchValue>) {
    this.group.patchValue(...args);
  }
}
