import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';
import {
  TuiButtonModule,
  TuiDialogContext,
  TuiDialogService,
} from '@taiga-ui/core';
import { NgDompurifyModule } from '@tinkoff/ng-dompurify';
import { TuiAutoFocusModule } from '@taiga-ui/cdk';

@Component({
  standalone: true,
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TuiButtonModule, NgDompurifyModule, TuiAutoFocusModule],
})
export class ConfirmationDialogComponent {
  content = '';

  constructor(
    @Inject(TuiDialogService) private readonly _dialogService: TuiDialogService,
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly _context: TuiDialogContext<boolean, { content: string }>
  ) {
    this.content = _context.data.content;
  }

  cancel(): void {
    this._context.completeWith(false);
  }

  confirm(): void {
    this._context.completeWith(true);
  }
}
