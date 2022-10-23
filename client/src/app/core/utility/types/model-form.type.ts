import { AbstractControl, FormControl } from '@angular/forms';

type ModelFormProperty<T> =
  | (T | null)
  | (T | null)[]
  | AbstractControl<T | null>
  | FormControl<T | null>;

export type ModelForm<TModel> = {
  [K in keyof TModel]: ModelFormProperty<TModel[K]>;
};
