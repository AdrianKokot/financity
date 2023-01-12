import {
  Directive,
  EventEmitter,
  HostListener,
  Input,
  OnDestroy,
  Output,
} from '@angular/core';
import { TuiDialogService } from '@taiga-ui/core';
import { catchError, filter, of, Subject, take, takeUntil } from 'rxjs';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { ConfirmationDialogComponent } from '@layout/feature/confirmation-dialog/confirmation-dialog.component';

@Directive({
  selector: '[appRequireConfirmation]',
  standalone: true,
})
export class RequireConfirmationDirective implements OnDestroy {
  @Output() confirm = new EventEmitter<void>();
  @Input() confirmationMessage = 'Are you sure?';
  @Input() confirmationTitle = 'Are you sure?';

  private readonly _destroyed$ = new Subject<void>();

  constructor(private readonly _dialog: TuiDialogService) {}

  @HostListener('click')
  onHostClick() {
    this._dialog
      .open<boolean>(new PolymorpheusComponent(ConfirmationDialogComponent), {
        required: true,
        data: { content: this.confirmationMessage },
        label: this.confirmationTitle,
      })
      .pipe(
        takeUntil(this._destroyed$),
        catchError(() => of(false)),
        take(1),
        filter(confirmed => confirmed)
      )
      .subscribe(() => this.confirm.emit());
  }

  ngOnDestroy(): void {
    this._destroyed$.next();
    this._destroyed$.complete();
  }
}
