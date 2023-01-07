import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';

export type ApiFilters = Record<
  string,
  string | string[] | null | number | number[] | undefined
>;

export interface ApiSort {
  orderBy?: string;
  direction?: 'asc' | 'desc';
}

export interface ApiPagination {
  page?: number;
  pageSize?: number;
}

export interface ApiParams extends ApiFilters, ApiSort, ApiPagination {}

export const toHttpParams = (pagination: ApiParams) => {
  const reducedFilters = (
    Object.keys(pagination) as (keyof typeof pagination)[]
  ).reduce((obj, key) => {
    const val = pagination[key];

    if (val !== undefined && val !== null) {
      obj[key] = val;
    }

    return obj;
  }, {} as Record<string, string | number | string[] | number[] | boolean>);

  return new HttpParams().appendAll(reducedFilters);
};

export abstract class GenericApiService<
  T extends { id: string },
  TListItem,
  TDetailsItem = T,
  TCreatePayload = T,
  TUpdatePayload = T
> {
  constructor(protected http: HttpClient, protected api: string) {}

  private _getListCache: Record<string, TListItem[]> = {};

  getAll(pagination: ApiParams) {
    const params = toHttpParams(pagination);

    const cacheKey = params.toString();

    // if (cacheKey in this._getListCache) {
    //   return of(this._getListCache[cacheKey]);
    // }

    return this.http.get<TListItem[]>(this.api, { params }).pipe(
      tap(data => {
        this._getListCache[cacheKey] = data;
      })
    );
  }

  get(id: T['id']) {
    return this.http.get<TDetailsItem>(`${this.api}/${id}`);
  }

  create(payload: TCreatePayload): Observable<TCreatePayload & Pick<T, 'id'>> {
    return this.http
      .post<TCreatePayload & Pick<T, 'id'>>(this.api, payload)
      .pipe(
        tap(() => (this._getListCache = {})),
        map(({ id }) => ({ id, ...payload }))
      );
  }

  update(payload: TUpdatePayload & { id: string }) {
    return this.http
      .put<Pick<T, 'id'>>(`${this.api}/${payload.id}`, payload)
      .pipe(tap(() => (this._getListCache = {})));
  }

  delete(id: T['id']): Observable<boolean> {
    return this.http.delete(`${this.api}/${id}`, { observe: 'response' }).pipe(
      map(res => res.status >= 200 && res.status < 300),
      tap(() => (this._getListCache = {}))
    );
  }
}
