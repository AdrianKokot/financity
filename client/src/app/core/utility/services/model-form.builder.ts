import { Injectable } from '@angular/core';
import {
  AbstractControlOptions,
  ControlConfig,
  FormBuilder,
} from '@angular/forms';

type ModelControls<TModel> = {
  [K in keyof TModel]: (TModel[K] | null) | ControlConfig<TModel[K] | null>;
};

@Injectable({
  providedIn: 'root',
})
export class ModelFormBuilder extends FormBuilder {
  from<TModel extends Record<string, any>>(
    controls: ModelControls<TModel>,
    options?: AbstractControlOptions | null
  ) {
    return super.group<TModel>(controls as TModel, options);
  }
}
