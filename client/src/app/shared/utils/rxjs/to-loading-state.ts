import { map, Observable, share, startWith, take, tap } from 'rxjs';

export const toLoadingState = <T>(effect?: (result: T) => void) => {
  return function (source: Observable<T>) {
    return source.pipe(
      take(1),
      tap(data => effect && effect(data)),
      map(() => false),
      startWith(true),
      share()
    );
  };
};
