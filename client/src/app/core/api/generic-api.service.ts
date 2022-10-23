import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GenericApiService {
  constructor(protected http: HttpClient) {}

  protected cachedRequest<T>(obs: Observable<T>) {
    const cached$ = obs; //.pipe(shareReplay(1));
    return () => cached$;
  }
}
