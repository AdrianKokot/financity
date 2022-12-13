import { distinctUntilChanged } from 'rxjs';

export const distinctUntilChangedObject = <T>() =>
  distinctUntilChanged<T>((a, b) => JSON.stringify(a) === JSON.stringify(b));
