import { Inject, Injectable, Injector, Type } from '@angular/core';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  constructor(
    @Inject(Injector) private _injector: Injector,
    private _dialog: TuiDialogService
  ) {}

  public open<T>(component: Type<T>) {
    return this._dialog.open(
      new PolymorpheusComponent(component, this._injector)
    );
  }
}
