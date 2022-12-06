import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { map, Subject, take, takeUntil } from 'rxjs';
import { UserSettingsService } from '../../data-access/services/user-settings.service';

@Component({
  selector: 'app-app-settings',
  templateUrl: './app-settings.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppSettingsComponent implements OnInit, OnDestroy {
  form = this._fb.nonNullable.group({
    isDarkModeEnabled: [false, [Validators.required]],
  });

  private _destroyed$ = new Subject<boolean>();

  constructor(
    private _fb: FormBuilder,
    private _settings: UserSettingsService
  ) {}

  ngOnDestroy(): void {
    this._destroyed$.next(true);
    this._destroyed$.complete();
  }

  ngOnInit(): void {
    this._settings.settings$.pipe(take(1)).subscribe(val => {
      this.form.patchValue(val);
    });

    this.form.valueChanges
      .pipe(
        takeUntil(this._destroyed$),
        map(() => this.form.getRawValue())
      )
      .subscribe(settings => {
        this._settings.updateSettings(settings);
      });
  }
}
