import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { filter, merge, Subject, switchMap, tap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { CreateRecipientComponent } from '../../../recipient/feature/create-recipient/create-recipient.component';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { RecipientApiService } from '../../../core/api/recipient-api.service';
import { TuiDialogService } from '@taiga-ui/core';
import { Recipient } from '@shared/data-access/models/recipient.model';
import { UpdateRecipientComponent } from '../../../recipient/feature/update-recipient/update-recipient.component';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';
import { Label } from '@shared/data-access/models/label';

@Component({
  selector: 'app-wallet-recipients',
  templateUrl: './wallet-recipients.component.html',
  styleUrls: ['./wallet-recipients.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletRecipientsComponent {
  private readonly _walletId: Wallet['id'] =
    this._activatedRoute.snapshot.params['id'];

  readonly ui = {
    columns: ['name', 'actions'] as const,
    actions: {
      edit$: new Subject<Recipient['id']>(),
      delete$: new Subject<Recipient['id']>(),
      create$: new Subject<void>(),
    },
  };

  readonly filters = this._fb.filters({
    search: [''],
  });

  readonly data = new ApiDataHandler(
    this._recipientService.getList.bind(this._recipientService, this._walletId),
    this.filters
  );

  readonly dialogs$ = merge(
    this.ui.actions.edit$.pipe(
      switchMap(id =>
        this._dialog.open(
          new PolymorpheusComponent(UpdateRecipientComponent, this._injector),
          {
            label: 'Edit recipient',
            data: {
              id,
            },
          }
        )
      )
    ),
    this.ui.actions.create$.pipe(
      switchMap(() =>
        this._dialog.open(
          new PolymorpheusComponent(CreateRecipientComponent, this._injector),
          {
            label: 'Create recipient',
            data: {
              walletId: this._walletId,
            },
          }
        )
      )
    ),
    this.ui.actions.delete$.pipe(
      switchMap(id =>
        this._recipientService.delete(id).pipe(filter(success => success))
      )
    )
  ).pipe(tap(() => this.data.resetPage()));

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _recipientService: RecipientApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService
  ) {}

  trackById = (index: number, item: { id: Label['id'] }) => item.id;
}
