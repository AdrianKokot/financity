import { AbstractControl, FormGroup } from '@angular/forms';
import { ParamMap } from '@angular/router';
import { TuiDay, TuiDayRange } from '@taiga-ui/cdk';

export const getFiltersFromParamMap = <
  TControl extends {
    [K in keyof TControl]: AbstractControl<unknown>;
  }
>(
  formGroup: FormGroup<TControl>,
  paramMap: ParamMap
) => {
  return (
    Object.keys(formGroup.controls) as (string extends keyof TControl
      ? string
      : never)[]
  ).reduce((obj, key) => {
    if (paramMap.has(key)) {
      const value = paramMap.getAll(key);

      if (formGroup.controls[key].value instanceof Array<string>) {
        obj[key] = value;
      } else if (
        formGroup.controls[key].value instanceof TuiDayRange &&
        value[0] !== 'null'
      ) {
        try {
          const days = value.map(x => TuiDay.jsonParse(x));

          obj[key] = new TuiDayRange(
            days[0],
            days.length > 1 ? days[1] : days[0]
          );
        } catch (e) {}
      } else {
        obj[key] = value[0] === 'null' ? null : value[0];
      }
    }

    return obj;
  }, {} as Record<string, unknown>);
};
