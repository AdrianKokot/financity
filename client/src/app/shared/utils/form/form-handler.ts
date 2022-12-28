import { AbstractControl, FormGroup } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { toFormSubmit } from '@shared/utils/rxjs/to-form-submit';

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
  effect: (result: TResult) => void;
}

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
