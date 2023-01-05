import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
} from '@angular/core';
import { WalletApiService } from '../../../core/api/wallet-api.service';
import { finalize, merge, Subject, switchMap, tap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import {
  TuiAlertService,
  TuiDialogService,
  TuiNotification,
} from '@taiga-ui/core';
import { User } from '../../../auth/data-access/models/user';
import { ShareWalletComponent } from '../../ui/share-wallet/share-wallet.component';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { Wallet } from '@shared/data-access/models/wallet.model';
import { Label } from '@shared/data-access/models/label';
import { ApiDataHandler } from '@shared/utils/api/api-data-handler';
import { FormWithHandlerBuilder } from '@shared/utils/services/form-with-handler-builder.service';

@Component({
  selector: 'app-wallet-share-management',
  templateUrl: './wallet-share-management.component.html',
  styleUrls: ['./wallet-share-management.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WalletShareManagementComponent {
  private readonly _walletId: Wallet['id'] =
    this._activatedRoute.snapshot.params['id'];

  readonly ui = {
    columns: ['name', 'email', 'actions'] as const,
    actions: {
      revoke$: new Subject<Pick<User, 'email'>>(),
      share$: new Subject<void>(),
    },
    deleteActionAt$: new Subject<User['email'] | null>(),
  };

  readonly filters = this._fb.filters({
    search: [''],
  });

  readonly data = new ApiDataHandler(
    this._walletService.getSharedToList.bind(
      this._walletService,
      this._walletId
    ),
    this.filters
  );

  readonly dialogs$ = merge(
    this.ui.actions.share$.pipe(
      switchMap(() =>
        this._dialog.open<boolean>(
          new PolymorpheusComponent(ShareWalletComponent, this._injector),
          {
            label: 'Share wallet',
            data: {
              walletId: this._walletId,
            },
          }
        )
      ),
      switchMap(success => {
        if (success) {
          return this._alertService.open('Invitation sent successfully.', {
            status: TuiNotification.Success,
            label: 'Success',
          });
        }
        return this._alertService.open(
          'Something went wrong. Please try again later.',
          {
            status: TuiNotification.Error,
            label: 'Error',
          }
        );
      })
    ),
    this.ui.actions.revoke$.pipe(
      tap(user => this.ui.deleteActionAt$.next(user.email)),
      switchMap(user =>
        this._walletService
          .revoke({
            userEmail: user.email,
            walletId: this._walletId,
          })
          .pipe(finalize(() => this.ui.deleteActionAt$.next(null)))
      )
    )
  ).pipe(tap(() => this.data.resetPage()));

  constructor(
    private readonly _fb: FormWithHandlerBuilder,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _walletService: WalletApiService,
    @Inject(Injector) private readonly _injector: Injector,
    @Inject(TuiDialogService) private readonly _dialog: TuiDialogService,
    @Inject(TuiAlertService) private readonly _alertService: TuiAlertService
  ) {}

  trackById = (index: number, item: { id: Label['id'] }) => item.id;
}
