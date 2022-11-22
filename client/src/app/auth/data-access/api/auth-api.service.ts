import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthApiService {
  private _basePath = '/api/auth';

  constructor(private http: HttpClient) {}

  login(payload: { email: string; password: string }) {
    return this.http.post<{ token: string }>(
      `${this._basePath}/login`,
      payload
    );
  }

  register(payload: { email: string; password: string }) {
    return this.http.post<void>(`${this._basePath}/register`, payload);
  }
}
