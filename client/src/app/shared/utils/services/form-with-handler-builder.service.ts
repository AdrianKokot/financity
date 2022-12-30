/* eslint-disable @typescript-eslint/ban-types */
import { Injectable } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup } from '@angular/forms';
import {
  FormHandler,
  FormHandlerFunctions,
} from '@shared/utils/form/form-handler';
import { FiltersForm } from '@shared/utils/form/filters-form';

function getGroup<T extends {}>() {
  return new FormBuilder().nonNullable.group<T>({} as T);
}

export type FormWithHandlerBuilderFunctions<
  T extends {},
  TResult
> = FormHandlerFunctions<
  ReturnType<typeof getGroup<T>> extends FormGroup<infer K> ? K : never,
  TResult
>;

@Injectable({
  providedIn: 'root',
})
export class FormWithHandlerBuilder extends FormBuilder {
  form<T extends {}, TResult>(
    controls: T,
    functions: FormWithHandlerBuilderFunctions<T, TResult>,
    options?: AbstractControlOptions | null
  ) {
    return new FormHandler(
      this.nonNullable.group<T>(controls, options),
      functions
    );
  }

  filters<T extends {}>(
    controls: T,
    config: {
      [K in keyof T]?: string;
    } = {},
    options?: AbstractControlOptions | null
  ) {
    return new FiltersForm<
      ReturnType<typeof this.nonNullable.group<T>> extends FormGroup<infer TK>
        ? TK
        : never,
      keyof T & string
    >(this.nonNullable.group<T>(controls, options), config);
  }
}
