import { Injectable } from '@angular/core';
import { UserSettings } from '../models/user-settings';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserSettingsService {
  private _settings$ = new BehaviorSubject<UserSettings>(
    this._getSettingsFromStorage()
  );

  get settings$() {
    return this._settings$.asObservable();
  }

  get settingsSnapshot() {
    return this._settings$.value;
  }

  updateSettings(value: Partial<UserSettings>) {
    this._settings$.next({ ...this._settings$.value, ...value });
    this._saveSettingsToStorage();
  }

  private _getSettingsFromStorage(): UserSettings {
    const dataFromMemory = window.localStorage.getItem('userSettings');

    if (dataFromMemory !== null) {
      return JSON.parse(dataFromMemory);
    }

    return { isDarkModeEnabled: false, showSimplifiedWalletView: true };
  }

  private _saveSettingsToStorage(): void {
    window.localStorage.setItem(
      'userSettings',
      JSON.stringify(this._settings$.value)
    );
  }
}
